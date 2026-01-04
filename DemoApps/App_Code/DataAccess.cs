using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace DemoApps
{
    public class DataAccess
    {
        public static string TenisLigaConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["TenisLigaConnectionString"].ConnectionString;
                //return RPAConnectionString;
            }
        }


        public static string DBConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            }
        }

        public static string ManticoreConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ManticoreConnectionString"].ConnectionString;
            }
        }

        public static string NonSapConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NonSapConnectionString"].ConnectionString;
            }
        }

        public static string NonSapConnectionStringMT
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NonSapConnectionStringMT"].ConnectionString;
            }
        }

        public static string NonSapUserRolesConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NonSapUserRolesConnectionString"].ConnectionString;
            }
        }




        public static string ClearStringForSQL(string param)
        {
            param = param.Replace("'", "''");
            //param = param.Replace("[", "[[");
            //param = param.Replace("]", "]]");
            //param = param.Replace("_", "[_]");
            //param = param.Replace("ESCAPE", "");
            return param;
        }

        public static void FillColumnWithValues(DataTable dt, string columnName, string value)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[columnName] = value;
            }
        }

        public static bool RecordExists(string _connectionString, string tableName, string columnName, string value)
        {
            string selectStr = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'", tableName, columnName, value);
            int i = GetInt(_connectionString, selectStr);
            if (i > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Vraća DataTable koji je rezultat SELECT upita
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string connectionString, string selectStr)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(selectStr, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return dt;
        }

        public static DataTable ConvertDateTimeToString(DataTable dt)
        {
            // Clone the structure of the original DataTable
            DataTable convertedTable = dt.Clone();

            // Update column types to string for DateTime columns
            foreach (DataColumn column in convertedTable.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    column.DataType = typeof(string);
                }
            }

            // Copy rows, converting DateTime values to formatted strings
            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = convertedTable.NewRow();
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.DataType == typeof(DateTime) && row[column] != DBNull.Value)
                    {
                        newRow[column.ColumnName] = ((DateTime)row[column]).ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        newRow[column.ColumnName] = row[column];
                    }
                }
                convertedTable.Rows.Add(newRow);
            }

            return convertedTable;
        }

        public static DataTable GetDataTableSchema(string connectionString, string selectStr)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(selectStr, conn);
                adapter.FillSchema(dt, SchemaType.Mapped);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Vraća prvi redak DataTable-a koji je rezultat SELECT upita
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="selectStr"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(string connectionString, string selectStr)
        {
            DataTable dt = GetDataTable(connectionString, selectStr);
            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public static DataTable AddBlankRow(DataTable dt)
        {
            dt.Rows.InsertAt(dt.NewRow(), 0);
            return dt;
        }

        /// <summary>
        /// Vraća prvi redak DataTable-a koji je rezultat SELECT upita
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="selectStr"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(DataTable dt, string selectStr)
        {
            DataRow[] rows = dt.Select(selectStr);
            if (rows.Length > 0)
            {
                return rows[0];
            }
            else return null;
        }

        /// <summary>
        /// Puni DataTable dt sa retulratima SELECT upita
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <param name="dt">DataTable koji će se napuniti</param>
        /// <returns>DataTable napunjen</returns>
        public static DataTable FillTable(string connectionString, string selectStr, DataTable dt)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(selectStr, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// Vraća ExecuteScalar SELECT-a
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <returns>Object - rezultat SELECT-a</returns>
        public static object GetValue(string connectionString, string selectStr)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            object o;
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(selectStr, conn);
                o = comm.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return o;
        }

        /// <summary>
        /// Vraća Convert.ToInt32(ExecuteScalar) SELECT-a, ako ne uspije vraća uint.MinValue
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <returns>inVraća Convert.ToInt32(ExecuteScalar) SELECT-a, ako ne uspije vraća int.MinValue</returns>
        public static int GetInt(string connectionString, string selectStr)
        {
            object o = GetValue(connectionString, selectStr);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o);
            }
            else
            {
                return int.MinValue;
            }
        }

        public static int GetInt(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && !row.IsNull(columnName))
            {
                return Convert.ToInt32(row[columnName]);
            }
            else
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// Vraća Convert.ToString(ExecuteScalar) SELECT-a
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <returns>int - Convert.ToInt32(rezultat SELECT-a)</returns>
        public static string GetString(string connectionString, string selectStr)
        {
            object o = GetValue(connectionString, selectStr);
            if (o != null)
            {
                return Convert.ToString(o);
            }
            else
            {
                return null;
            }
        }

        public static string GetString(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && !row.IsNull(columnName))
            {
                return row[columnName].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Vraća Convert.ToDateTime(ExecuteScalar) SELECT-a
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="selectStr">SELECT query</param>
        /// <returns>int - Convert.ToInt32(rezultat SELECT-a)</returns>
        public static DateTime GetDateTime(string connectionString, string selectStr)
        {
            object o = GetValue(connectionString, selectStr);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToDateTime(o);
            }
            else
            {
                return new DateTime(0);
            }
        }

        public static DateTime GetDateTime(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && !row.IsNull(columnName))
            {
                return Convert.ToDateTime(row[columnName]);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandSql"></param>
        /// <returns></returns>
        public static int ExecuteSql(string connectionString, string commandSql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(connectionString);
            cmd.CommandText = commandSql;
            int returnValue = -1;
            try
            {
                cmd.Connection.Open();
                SqlCommand comm = new SqlCommand(commandSql, cmd.Connection);
                returnValue = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        #region SetTransaction
        /// <summary>
        ///     //Setup connection
        ///     conn = new SqlConnection(Properties.Settings.Default.NorthwindConnectionString);
        ///     conn.Open();
        ///     //Setup transaction
        ///     transaction = conn.BeginTransaction();
        ///     Data.SqlClient.SetTransaction(ordersTA, transaction);
        ///     Data.SqlClient.SetTransaction(orderDetailsTA, transaction);
        ///     //Update Orders table
        ///     ordersTA.Update(ordersDT);
        ///     //Update OrderDetails table
        ///     orderDetailsTA.Update(ordersDetailsDT);
        ///     //Commit transaction
        ///     transaction.Commit();
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="trans"></param>
        public static void SetTransaction(object tableAdapter, SqlTransaction trans)
        {
            SqlDataAdapter adapter = GetAdapter(tableAdapter);
            SetTransaction(adapter, trans);
        }

        private static void SetTransaction(SqlDataAdapter adapter, SqlTransaction trans)
        {
            adapter.InsertCommand.Connection = trans.Connection;
            adapter.InsertCommand.Transaction = trans;
            adapter.UpdateCommand.Connection = trans.Connection;
            adapter.UpdateCommand.Transaction = trans;
            adapter.DeleteCommand.Connection = trans.Connection;
            adapter.DeleteCommand.Transaction = trans;
        }

        public static SqlDataAdapter GetAdapter(object tableAdapter)
        {
            Type tableAdapterType = tableAdapter.GetType();
            SqlDataAdapter adapter = (SqlDataAdapter)tableAdapterType.GetProperty("Adapter", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tableAdapter, null);
            return adapter;
        }
        #endregion

        public static string DateForSQL(DateTime date)
        {
            return string.Format("{0}-{1}-{2}", date.Year, date.Month, date.Day);
        }

        public static string DateForSQL(DateTime date, string delimiter)
        {
            return string.Format("{0}{1}{2}{1}{3}", date.Year, delimiter, date.Month, date.Day);
        }

        public static string DateTimeForSQL(DateTime date)
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public static string DecimalForSQL(decimal d)
        {
            return d.ToString().Replace(',', '.');
        }


        #region SQLParameters

        protected static void AddParametar(SqlCommand comm, string paramName, bool paramValue)
        {
            comm.Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        protected static void AddParametar(SqlCommand comm, string paramName, DBNull dbNullValue)
        {
            comm.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
        }

        protected static void AddParametar(SqlCommand comm, string paramName, bool? paramValue)
        {
            if (paramValue.HasValue)
                comm.Parameters.Add(new SqlParameter(paramName, paramValue.Value));
            else
                comm.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
        }

        protected static void AddParametar(SqlCommand comm, string paramName, int? paramValue)
        {
            if (paramValue.HasValue)
                comm.Parameters.Add(new SqlParameter(paramName, paramValue.Value));
            else
                comm.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
        }

        protected static void AddParametar(SqlCommand comm, string paramName, DateTime? paramValue)
        {
            if (paramValue.HasValue)
                comm.Parameters.Add(new SqlParameter(paramName, paramValue.Value));
            else
                comm.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
        }

        protected static void AddParametar(SqlCommand comm, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
                comm.Parameters.Add(new SqlParameter(paramName, paramValue));
            else
                comm.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
        }

        #endregion

        public static void LogMailMessage(MailMessage mm, string mailType)
        {
            //SqlConnection conn = new SqlConnection(DataAccess.DBConnectionString);
            //SqlCommand comm = new SqlCommand("SMTPLogInsert", conn);
            //comm.CommandType = CommandType.StoredProcedure;

            //AddParametar(comm, "@MailFrom", mm.From.ToString());
            //AddParametar(comm, "@MailTo", mm.To.ToString());
            //AddParametar(comm, "@MailCC", mm.CC.ToString());
            //AddParametar(comm, "@MailBCC", mm.Bcc.ToString());
            //AddParametar(comm, "@MailSubject", mm.Subject.ToString());
            //AddParametar(comm, "@MailBody", mm.Body.ToString());
            //AddParametar(comm, "@SendMailEnabled", Convert.ToInt32(SettingsHelper.SendMailEnabled));

            //conn.Open();
            //comm.ExecuteNonQuery();

            //conn.Close();
        }

    }

}