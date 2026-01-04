using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoApps.Acquire.Pages
{
    public partial class DefaultBS : System.Web.UI.Page
    {

        int ACQGameId = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGameList();
            }
        }

        private void LoadGameList()
        {
            using (var context = new DemoAppEntities())
            {
                var games = context.ACQGames.ToList();
                gvGameList.DataSource = games;
                gvGameList.DataBind();
            }
        }

        private void LoadSackson()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Sackson, Convert.ToInt32(txtSacksonCount.Text));
            if (shi != null)
            {
                lblSacksonPrice.Text = shi.StockPrice.ToString();
                lblSacksonHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblSacksonPrice.Text = "0";
                lblSacksonHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        private void LoadTower()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Tower, Convert.ToInt32(txtTowerCount.Text));
            if (shi != null)
            {
                lblTowerPrice.Text = shi.StockPrice.ToString();
                lblTowerHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";

            }
            else
            {
                lblTowerPrice.Text = "0";
                lblTowerHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }



        private ACQStockholderInfo LoadHotelChainData(ACQHotelChain hc, int buildingCount)
        {
            ACQStockholderInfo shiRet = new ACQStockholderInfo();

            using (var context = new DemoAppEntities())
            {
                var shi = context.ACQStockholderInfoes.FirstOrDefault(p => p.ACQHotelChainId == (int)hc && (buildingCount >= p.SizeFrom && buildingCount <= p.SizeTo));
                if (shiRet != null)
                {
                    shiRet = shi;
                }
            }

            return shiRet;
        }

        protected void btnStartSackson_Click(object sender, EventArgs e)
        {
            txtSacksonCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Sackson);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadSackson();
        }

        protected void btnAddSackson_Click(object sender, EventArgs e)
        {
            txtSacksonCount.Text = (Convert.ToInt32(txtSacksonCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Sackson);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtSacksonCount.Text);
                }
                context.SaveChanges();
            }

            LoadSackson();
        }

        protected void btnResetSackson_Click(object sender, EventArgs e)
        {
            txtSacksonCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Sackson);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtSacksonCount.Text);
                }
                context.SaveChanges();
            }

            LoadSackson();
        }


        protected void btnStartTower_Click(object sender, EventArgs e)
        {
            txtTowerCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Tower);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadTower();
        }

        protected void btnAddTower_Click(object sender, EventArgs e)
        {
            txtTowerCount.Text = (Convert.ToInt32(txtTowerCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Tower);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtTowerCount.Text);
                }
                context.SaveChanges();
            }

            LoadTower();
        }

        protected void btnResetTower_Click(object sender, EventArgs e)
        {
            txtTowerCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Tower);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtTowerCount.Text);
                }
                context.SaveChanges();
            }

            LoadTower();
        }

        protected void btnStartAmerican_Click(object sender, EventArgs e)
        {
            txtAmericanCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.American);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadAmerican();
        }

        private void LoadAmerican()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.American, Convert.ToInt32(txtAmericanCount.Text));
            if (shi != null)
            {
                lblAmericanPrice.Text = shi.StockPrice.ToString();
                lblAmericanHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblAmericanPrice.Text = "0";
                lblAmericanHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        protected void btnAddAmerican_Click(object sender, EventArgs e)
        {
            txtAmericanCount.Text = (Convert.ToInt32(txtAmericanCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.American);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtAmericanCount.Text);
                }
                context.SaveChanges();
            }

            LoadAmerican();
        }

        protected void btnResetAmerican_Click(object sender, EventArgs e)
        {
            txtAmericanCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.American);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtAmericanCount.Text);
                }
                context.SaveChanges();
            }

            LoadAmerican();
        }



        protected void btnStartFestival_Click(object sender, EventArgs e)
        {
            txtFestivalCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Festival);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadFestival();
        }

        protected void btnAddFestival_Click(object sender, EventArgs e)
        {
            txtFestivalCount.Text = (Convert.ToInt32(txtFestivalCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Festival);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtFestivalCount.Text);
                }
                context.SaveChanges();
            }

            LoadFestival();
        }

        private void LoadFestival()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Festival, Convert.ToInt32(txtFestivalCount.Text));
            if (shi != null)
            {
                lblFestivalPrice.Text = shi.StockPrice.ToString();
                lblFestivalHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblFestivalPrice.Text = "0";
                lblFestivalHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        protected void btnResetFestival_Click(object sender, EventArgs e)
        {
            txtFestivalCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Festival);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtFestivalCount.Text);
                }
                context.SaveChanges();
            }

            LoadFestival();
        }

        protected void btnStartWorldwide_Click(object sender, EventArgs e)
        {
            txtWorldwideCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Worldwide);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadWorldwide();
        }

        protected void btnAddWorldwide_Click(object sender, EventArgs e)
        {
            txtWorldwideCount.Text = (Convert.ToInt32(txtWorldwideCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Worldwide);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtWorldwideCount.Text);
                }
                context.SaveChanges();
            }

            LoadWorldwide();
        }

        private void LoadWorldwide()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Worldwide, Convert.ToInt32(txtWorldwideCount.Text));
            if (shi != null)
            {
                lblWorldwidePrice.Text = shi.StockPrice.ToString();
                lbllWorldwideHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblWorldwidePrice.Text = "0";
                lbllWorldwideHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        protected void btnResetWorldwide_Click(object sender, EventArgs e)
        {
            txtWorldwideCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Worldwide);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtWorldwideCount.Text);
                }
                context.SaveChanges();
            }

            LoadWorldwide();
        }


        protected void btnStartContinental_Click(object sender, EventArgs e)
        {
            txtContinentalCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Continental);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadContinental();
        }

        protected void btnAddContinental_Click(object sender, EventArgs e)
        {
            txtContinentalCount.Text = (Convert.ToInt32(txtContinentalCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Continental);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtContinentalCount.Text);
                }
                context.SaveChanges();
            }

            LoadContinental();
        }

        private void LoadContinental()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Continental, Convert.ToInt32(txtContinentalCount.Text));
            if (shi != null)
            {
                lblContinentalPrice.Text = shi.StockPrice.ToString();
                lblContinentalHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblContinentalPrice.Text = "0";
                lblContinentalHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        protected void btnResetContinental_Click(object sender, EventArgs e)
        {
            txtContinentalCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Continental);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtContinentalCount.Text);
                }
                context.SaveChanges();
            }

            LoadContinental();
        }


        protected void btnStartImperial_Click(object sender, EventArgs e)
        {
            txtImperialCount.Text = "2";

            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Imperial);

                if (bc != null)
                {
                    bc.BuildingCount = 2;
                }
                context.SaveChanges();
            }

            LoadImperial();
        }

        protected void btnAddImperial_Click(object sender, EventArgs e)
        {
            txtImperialCount.Text = (Convert.ToInt32(txtImperialCount.Text) + 1).ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Imperial);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtImperialCount.Text);
                }
                context.SaveChanges();
            }

            LoadImperial();
        }

        private void LoadImperial()
        {
            ACQStockholderInfo shi = LoadHotelChainData(ACQHotelChain.Imperial, Convert.ToInt32(txtImperialCount.Text));
            if (shi != null)
            {
                lblImperialPrice.Text = shi.StockPrice.ToString();
                lblImperialHolderValues.Text = $"{shi.PrimaryHolder:N0} - {shi.SecondaryHolder:N0} - {shi.TertiaryHolder:N0}";
            }
            else
            {
                lblImperialPrice.Text = "0";
                lblImperialHolderValues.Text = string.Empty;
            }

            updMain.Update();
        }

        protected void btnResetImperial_Click(object sender, EventArgs e)
        {
            txtImperialCount.Text = 0.ToString();
            using (var context = new DemoAppEntities())
            {
                var bc = context.ACQBuildingCounts.FirstOrDefault(b => b.ACQGameId == ACQGameId && b.ACQHotelChainId == (int)ACQHotelChain.Imperial);

                if (bc != null)
                {
                    bc.BuildingCount = Convert.ToInt32(txtImperialCount.Text);
                }
                context.SaveChanges();
            }

            LoadImperial();
        }


    }

   
}