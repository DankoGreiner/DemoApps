using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoApps.TestniMatch
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTermin();
                LoadTesters();
                LoadSelectedPlayers();
            }
        }

        private void LoadTermin()
        {
            var today = DateTime.Today;
            var nextTuesday = today.AddDays(((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7);
            //if (nextTuesday == today)
            //    nextTuesday = nextTuesday.AddDays(7);

            txtTermin.Text = string.Format("{0}-{1}-{2}", nextTuesday.Year, nextTuesday.Month, nextTuesday.Day);
        }

        protected void gvPlayersSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvPlayersSelected.SelectedRow;

            lblSelectedPlayerId.Text = row.Cells[1].Text;
            lblSelectedPlayerName.Text = row.Cells[3].Text;
            txtRedosljed.Text = row.Cells[5].Text;

            txtRedosljed.Focus();
            ScriptManager.RegisterStartupScript(
            this,
            GetType(),
            "SelectText",
            "setTimeout(function(){ var t = document.getElementById('" + txtRedosljed.ClientID + "'); if(t) t.select(); }, 100);",
            true
        );

            panSelectedPlayer.Visible = true;
        }

        protected void gvPlayersAvalilable_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvPlayersAvalilable.SelectedRow;

            // Example: get values from GridView columns
            int igracId = Convert.ToInt32(row.Cells[1].Text);
            string imePrezime = row.Cells[2].Text;
            string opis = row.Cells[4].Text.Replace("&nbsp;", string.Empty);
            int redosljed = 1; // or set dynamically
            string termin = txtTermin.Text; // or get from your UI

            string sql = @"
        INSERT INTO [dbo].[TLOdabraniIgrac]
        ([IgracId], [ImePrezime], [Opis], [Redosljed], [Termin])
        VALUES (@IgracId, @ImePrezime, @Opis, @Redosljed, @Termin);";

            using (SqlConnection con = new SqlConnection(DataAccess.DBConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IgracId", igracId);
                cmd.Parameters.AddWithValue("@ImePrezime", imePrezime);
                cmd.Parameters.AddWithValue("@Opis", opis);
                cmd.Parameters.AddWithValue("@Redosljed", redosljed);
                cmd.Parameters.AddWithValue("@Termin", termin);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadSelectedPlayers();

        }

        private void LoadTesters()
        {
            string selectStr = string.Format("SELECT TOP 1 SezonaId FROM Sezona WHERE GetDate() BETWEEN DatumOd AND DATEADD(day, 1, DatumDo) ORDER BY DatumOd DESC");
            int sezonaId = DataAccess.GetInt(DataAccess.TenisLigaConnectionString, selectStr);


            selectStr = String.Format(@"SELECT
                                    --vitNovi.NastupaSljedeceSezone
                                    --, ti.*
                                     vitNovi.IgracId, vitNovi.ImePrezime AS NoviIgrac, CONVERT(varchar, vitNovi.DateInserted, 104) AS DatumPrijave, vitNovi.Trenutci
                                    , vitNovi.NastupaSljedeceSezone AS NSS
                                    --, vi.ImePrezime AS Tester
                                    FROM TestIgrac ti
                                    INNER JOIN vIgrac vitNovi ON vitNovi.IgracId = ti.IgracId
                                    LEFT OUTER JOIN vIgrac vi ON ti.TesterIgracId = vi.IgracId
                                    WHERE ti.SezonaId = {0}
                                    AND TestStatus <> 'Procjenjen'
                                    ORDER BY ti.IgracId DESC
                                    ", sezonaId);

            DataTable dt = DataAccess.GetDataTable(DataAccess.TenisLigaConnectionString, selectStr);

            DataRow row = dt.NewRow();
            row["NoviIgrac"] = "Danko Greiner";
            row["IgracId"] = 6;
            dt.Rows.Add(row);

            gvPlayersAvalilable.DataSource = dt;
            gvPlayersAvalilable.DataBind();
        }

       

        private void LoadSelectedPlayers()
        {
            string selectStr = string.Format("SELECT OdabraniIgracId AS Id, IgracId, ImePrezime, Opis, Redosljed AS R, Termin, '' AS Vrijeme FROM TLOdabraniIgrac WHERE Termin = '{0}' ORDER BY Redosljed", txtTermin.Text);
            DataTable dt = DataAccess.GetDataTable(DataAccess.DBConnectionString, selectStr);

            gvPlayersSelected.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                ViewState["SelectedIgracIds"] = string.Empty;
                lblMails.Text = string.Empty;

                int totalRows = dt.Rows.Count;  
                DateTime start = DateTime.Today.AddHours(20); // 20:00
                DateTime end = DateTime.Today.AddHours(22); // 22:00
                TimeSpan window = end - start;

                long slotTicks = totalRows > 0
                    ? window.Ticks / totalRows
                    : 0;

                ViewState["TotalRows"] = totalRows;
                ViewState["StartTicks"] = start.Ticks;
                ViewState["SlotTicks"] = slotTicks;

                gvPlayersSelected.DataBind();
                
            }
        }

        protected void btnChangeRedosljed_Click(object sender, EventArgs e)
        {
            string selectStr = string.Format("UPDATE TLOdabraniIgrac SET Redosljed = {0} WHERE OdabraniIgracId = {1}", Convert.ToInt16(txtRedosljed.Text), lblSelectedPlayerId.Text);
            DataAccess.ExecuteSql(DataAccess.DBConnectionString, selectStr);

            panSelectedPlayer.Visible = false;
            LoadSelectedPlayers();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string selectStr = string.Format("DELETE TLOdabraniIgrac WHERE OdabraniIgracId = {0}", lblSelectedPlayerId.Text);
            DataAccess.ExecuteSql(DataAccess.DBConnectionString, selectStr);

            lblSelectedPlayerName.Text = string.Empty;
            lblSelectedPlayerId.Text = string.Empty;
            txtRedosljed.Text = string.Empty;

            panSelectedPlayer.Visible = false;

            LoadSelectedPlayers();
        }

        protected void gvPlayersSelected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            int totalRows = (int)(ViewState["TotalRows"] ?? 0);
            long startTicks = (long)(ViewState["StartTicks"] ?? 0L);
            long slotTicks = (long)(ViewState["SlotTicks"] ?? 0L);

            if (totalRows <= 0 || slotTicks <= 0) return;

            int i = e.Row.RowIndex;

            DateTime start = new DateTime(startTicks);
            TimeSpan slot = TimeSpan.FromTicks(slotTicks);

            DateTime slotStart = start.AddTicks(slot.Ticks * i);
            // Make the last slot end exactly at 22:00
            DateTime slotEnd = (i == totalRows - 1)
                ? DateTime.Today.AddHours(22)
                : slotStart.Add(slot);

            // last column
            int lastCellIndex = e.Row.Cells.Count - 1;
            e.Row.Cells[lastCellIndex].Text = $"{slotStart:HH\\:mm} - {slotEnd:HH\\:mm}";
        }

        protected void gvPlayersSelected_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if (gv.HeaderRow == null)
                return;

            int imePrezimeIndex = -1;

            for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
            {
                string headerText = gv.HeaderRow.Cells[i].Text.Trim();

                if (string.Equals(headerText, "ImePrezime", StringComparison.OrdinalIgnoreCase))
                {
                    imePrezimeIndex = i;
                    break;
                }
            }

            for (int i = 0; i < gvPlayersSelected.Rows.Count ; i++)
            {
                GridViewRow currentRow = gvPlayersSelected.Rows[i];
                GridViewRow previousRow;

                if (i == 0)
                {
                    previousRow = gvPlayersSelected.Rows[gvPlayersSelected.Rows.Count - 1];
                }
                else
                {
                    previousRow = gvPlayersSelected.Rows[i - 1];
                }

                //Igrači
                currentRow.Cells[imePrezimeIndex].Text = string.Format("<b>{0}</b>", currentRow.Cells[imePrezimeIndex].Text) + " - " + previousRow.Cells[imePrezimeIndex].Text.Split('-')[0].Trim().Replace("<b>", string.Empty).Replace("</b>", string.Empty);

                ViewState["SelectedIgracIds"] = ViewState["SelectedIgracIds"].ToString() + gvPlayersSelected.Rows[i].Cells[2].Text + ",";
            }
            ViewState["SelectedIgracIds"] = ViewState["SelectedIgracIds"].ToString() + gvPlayersSelected.Rows[0].Cells[2].Text + ",";
            ViewState["SelectedIgracIds"] = ViewState["SelectedIgracIds"].ToString().Trim(',');


            //Mails
            if (ViewState["SelectedIgracIds"].ToString() != string.Empty)
            {
                string selectStr = string.Format("SELECT Mail FROM Igrac WHERE IgracId IN ({0})", ViewState["SelectedIgracIds"]);
                DataTable dt = DataAccess.GetDataTable(DataAccess.TenisLigaConnectionString, selectStr);
                gvMails.DataSource = dt;
                gvMails.DataBind();

            }

        }

        protected void btnRaspored_Click(object sender, EventArgs e)
        {
            //0,1,2,4,5,6
            gvPlayersSelected.Columns[0].Visible = false;
            gvPlayersSelected.Columns[1].Visible = false;
            gvPlayersSelected.Columns[2].Visible = false;
            gvPlayersSelected.Columns[4].Visible = false;
            gvPlayersSelected.Columns[5].Visible = false;
            gvPlayersSelected.Columns[6].Visible = false;
            //gvPlayersSelected.Columns[0].Visible = false;
        }

     
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSelectedPlayers();
        }

       
    }
}