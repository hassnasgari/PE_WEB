using System;
using System.Windows.Forms;
//Add MySql Library
//using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PE
{
    class ClsSql
    {
        private SqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public ClsSql()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            #region تشخیص آی پی شبکه کاندید برای ارتباط با سرور ترجیحا 10.84 و سپس 172.16
            //MainForm.StrCurIp = ClsNet.GetIpAddress();
            MainForm.StrCurIp = "172.16.3.116";
            #endregion

            #region تشخیص آی پی شبکه کاندید برای ارتباط با سرور ترجیحا 10.84 و سپس 172.16
            MainForm.StrConnectedNetWork = " * Net is MCI * ";
            MainForm.StrSelectedRootPathInPeServer = @"\\172.16.3.116\PE_Share\PE";
            server = "172.16.3.116";

            //server = MainForm.StrAccessNetIp = ClsNet.chooseNet();
            #endregion
           
            database = "master";
            uid = "asg";
            password = "Asg_8560@200";
            string connectionString;
            connectionString = "SERVER=" + server + ";"  + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new SqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            if(connection.State.ToString()=="Open")
            {
                return true;
            }
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                   default:
                        string strExceptionMsg = "- ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n OpenConnection()\n" + ex;
                        MessageBox.Show(strExceptionMsg);
                        ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                        MainForm.ActiveForm.Close();
                        break;
                }
                return false;
            }

        }

        //Close connection
        public bool CloseConnection()
        {
            if (connection.State.ToString() == "Close")
            {
                return true;
            }
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n CloseConnection()\n" + ex;
                MessageBox.Show(strExceptionMsg);
                ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                return false;
            }
        }

        //Insert statement
        public bool Insert(string tbl, string[] feild, object[] value) 
        {
            string query = String.Format("INSERT INTO {0}(", tbl);
            foreach (object itm in feild)
            {
                query += String.Format("{0},", itm);
            }
            query = query.Remove(query.Length - 1, 1) + ") Values (";

            foreach (object itm in value)
            {
                if (itm.GetType() == typeof(byte[]))
                {
                    query += "0x";
                    foreach (byte b in ((byte[])itm))
                        query += Convert.ToString(b, 16);
                    query += ",";
                }
                else if (itm is bool)
                    query += Convert.ToByte(itm).ToString() + ",";
                else if (itm.GetType() == typeof(string))
                    query += "N'" + itm.ToString().Replace("\0", string.Empty) + "',";
                else if (itm.GetType() == typeof(int) || itm.GetType() == typeof(float) ||
                    itm.GetType() == typeof(double) || itm.GetType() == typeof(decimal) ||
                itm.GetType() == typeof(byte))
                    query += itm.ToString() + ",";
                else
                    query += "'" + itm.ToString() + "',";

            }
            query = query.Remove(query.Length - 1, 1) + ")";

            //open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create command and assign the query and connection from the constructor
                    SqlCommand cmd = new SqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Insert()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool Insert(string tbl, string feild, string strValue)
        {

            //open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("insert into {0}({1}) values({2})", tbl, feild, strValue) };

                    //Execute command
                    sqlComDb.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Insert()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    return false;
                }
            }
            else
            {
                return false;
            }


           

        }
        public bool BulkInsertDataTable(string tbl, DataTable dt)
        {
            bool bolOk = false;
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    bulkCopy.DestinationTableName = tbl;
                    //  con.Open();

                    bulkCopy.WriteToServer(dt);
                    bolOk = true;

                    //close connection
                    this.CloseConnection();

                }
                catch (Exception ex)
                {
                    connection.Close();
                    bolOk = false;

                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n BulkInsertDataTable()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
                bolOk = false;
            }
            return bolOk;
        }
        public void GetAndSendToDataBase(string tbl, string strFieldName, string strValue)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("insert into {0}({1}) values(N'{2}')", tbl, strFieldName, strValue) };

                    sqlComDb.ExecuteNonQuery();
                    //close connection
                    this.CloseConnection();


                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n GetAndSendToDataBase()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                }
            }
            else
            {

            }

        }
        public bool GetAllAndSendToDataBase(string tbl, string strFieldsName, string strValues)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("insert into {0} ({1}) values({2})", tbl, strFieldsName, strValues) };

                    sqlComDb.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();

                    return true;

                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n GetAllAndSendToDataBase()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")).Replace("'","<>"));

                    return false;

                }
            }
            else
            {
                return false;
            }

        }
        public bool GetAndSendToDataBaseAndReturnResult(string tbl, string strFieldName, Int64 intValue)//نیاز به اصلاح نام
        {
            bool bolIsSucsess = false;
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("insert into {0}({1}) values('{2}')", tbl, strFieldName, intValue) };

                    sqlComDb.ExecuteNonQuery();
                    bolIsSucsess = true;

                    //close connection
                    this.CloseConnection();
                }
                catch(Exception ex)
                {
                    connection.Close();
                    bolIsSucsess = false;
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n GetAndSendToDataBaseAndReturnResault()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
                bolIsSucsess = false;
            }
            return bolIsSucsess;
        }


        //Update statement
        public bool Update(string tbl, string feild, string dblNewValue, string Where)
        {
            string query = String.Format("update {0} set {1}=N'{2}' {3}", tbl, feild, dblNewValue, Where);
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch(Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Update()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool Update(string tbl, string feild, bool dblNewValue, string Where)
        {
            string query = String.Format("update {0} set {1}=N'{2}' {3}", tbl, feild, dblNewValue, Where);
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch(Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Update()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool UpDate(string tbl, string[] feild, object[] value, string Where)
        {
            string query = String.Format("UPDATE {0} SET ", tbl);

            for (int i = 0; i < feild.Length; i++)
            {
                if (value[i].GetType() == typeof(string))
                    query += String.Format("{0}=N'{1}' , ", feild[i], value[i]);
                else if (value[i].GetType() == typeof(bool))
                    query += String.Format("{0}={1}, ", feild[i], Convert.ToInt16(value[i]));
                else
                    query += String.Format("{0}={1} , ", feild[i], value[i]);

            }

            query = String.Format("{0} WHERE {1}", query.Remove(query.Length - 2, 1), Where);

            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();

                    //Assign the query using CommandText
                    cmd.CommandText = query;

                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();

                    return true;
                }
                catch(Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Update()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
           
        }

        //Send Command 
        public bool SendCommand(string strCommand)
        {
            string query = strCommand;
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();

                    //Assign the query using CommandText
                    cmd.CommandText = query;

                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteScalar();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch(Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n SendCommand()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool DeleteWhereExistTableInDataBase(string fullDataBaseNameForDelete)
        {
            string query  = @"
             IF EXISTS(
             SELECT 1 FROM sys.objects
                WHERE object_id = object_id(N'"+ fullDataBaseNameForDelete + @"')

                    AND type in (N'U')
                    )
                    BEGIN
                      DROP TABLE[DB].[dbo].[tblDbAras]
            END;

            ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();

                    //Assign the query using CommandText
                    cmd.CommandText = query;

                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteScalar();

                    //close connection
                    this.CloseConnection();
                    return true;
                }
                catch(Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n DeleteWhereExsitTableInDataBase()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public string SendCommandGetStringResult(string strCommand)
        {
            string query = strCommand;
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql comman
                    SqlCommand cmd = new SqlCommand();

                    //Assign the query using CommandText
                    cmd.CommandText = query;

                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    object obj=cmd.ExecuteScalar();
                    return obj.ToString();

                    //close connection
                    CloseConnection();

                   
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n SendCommandGetStringResult()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                   ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return null;
                }
            }
            else
            {

                return null;
            }
        }
        public List<string> SendCommandGetListResult(string strCommand)
        {
            string query = strCommand;

           List<string> lst = new List<string>();

            if (this.OpenConnection() == true)
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                DataSet ds = new DataSet();
                int i = 0;
                try
                {
                    adapter.SelectCommand = new SqlCommand(strCommand, connection);

                    adapter.Fill(ds);

                    for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        lst.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }

                    //close connection
                    this.CloseConnection();

                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n SendCommandGetListResult()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
                MessageBox.Show("SendCommandGetListResult  اشکال در ارتباط با دیتابیس" );
            }

            return (lst);
        }

        //Delete statement
        public bool DeleteRecords(string tbl, string strWhere)
        {
            bool bolIsDelRecord = false;
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("delete {0} {1}", tbl, strWhere) };

                    sqlComDb.ExecuteNonQuery();
                    bolIsDelRecord = true;

                    //close Connection
                    this.CloseConnection();
                }

                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n DeleteRecords()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    bolIsDelRecord = false;
                }
            }
            else
            {

            }
            return bolIsDelRecord;

        }
        public bool DeleteAllRecords(string tbl)
        {
            bool bolIsDelRecord = false;
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = "delete " + tbl };

                    sqlComDb.ExecuteNonQuery();
                    bolIsDelRecord = true;

                    //close Connection
                    this.CloseConnection();
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n DeleteAllRecords()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    bolIsDelRecord = false;

                }
            }
            else
            {
                bolIsDelRecord = false;
            }
            return bolIsDelRecord;
        }

        //Count statement
        public int Count(string strTable, string strWhere)
        {
            string query = String.Format("select count(*) from {0}  {1}", strTable, strWhere);
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //Create Mysql Command
                    SqlCommand cmd = new SqlCommand(query, connection);

                    //ExecuteScalar will return one value
                    Count = int.Parse(cmd.ExecuteScalar() + "");

                    //close Connection
                    this.CloseConnection();

                    return Count;
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n Count()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    return -1;

                }
            }
            else
            {
                return -1;
            }
        }

        //Fill DataTable From Query
        public DataTable FillDataTableFromQuery(string strCommand)
        {

            string query = strCommand;
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(strCommand, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //close Connection
                    this.CloseConnection();
                    return dt;
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n FillDataTableFromQuery()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    return null;

                }
            }
            else
            {
                return null;
            }
        }

        //Create Command from datatable for create table
        public string CreateSqlCommandForCreateTableFromDataTable(string tbl, DataTable dt)
        {
           

            if (this.OpenConnection() == true)
            {
                try
                {

                    string MySqlsc;
                    MySqlsc = "CREATE TABLE " + tbl + "(";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        //MySqlsc += "\n " + table.Columns[i].ColumnName.Replace(" ", "_").Replace("-", "_").Replace("(", "_").Replace(")", "_").Replace("/", "_") + " ";
                        MySqlsc += "\n " + "`" + dt.Columns[i].ColumnName.Trim() + "`";
                        string columnType = dt.Columns[i].DataType.ToString();
                        switch (columnType)
                        {
                            case "System.Int32":
                                MySqlsc += " int ";
                                break;
                            case "System.Int64":
                                MySqlsc += " bigint ";
                                break;
                            case "System.Int16":
                                MySqlsc += " tinyint ";
                                break;
                            case "System.Decimal":
                                MySqlsc += " decimal ";
                                break;
                            case "System.DateTime":
                                MySqlsc += " datetime ";
                                break;
                            case "System.String":
                            default:
                                MySqlsc += string.Format(" TEXT({0}) ", dt.Columns[i].MaxLength == -1 ? "250" : dt.Columns[i].MaxLength.ToString());
                                break;
                        }
                        if (dt.Columns[i].AutoIncrement)
                            MySqlsc += " IDENTITY(" + dt.Columns[i].AutoIncrementSeed.ToString() + "," + dt.Columns[i].AutoIncrementStep.ToString() + ") ";
                        if (!dt.Columns[i].AllowDBNull)
                            MySqlsc += " NOT NULL ";
                        MySqlsc += ",";
                    }

                    MySqlsc = MySqlsc.Remove(MySqlsc.Length - 1, 1) + ")";

                    //close Connection
                    this.CloseConnection();
                    return MySqlsc;
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n CreateSqlCommandForCreateTableFromDataTable()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    return null;

                }
            }
            else
            {
                return null;
            }


        }
       
        //Is Any Record
        public bool IsAnyRecord(string tbl, string strFieldName, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("select {0} from {1}  {2}", strFieldName, tbl, strWhere) };
                    object obj = sqlComDb.ExecuteScalar();

                    if (obj == null || obj == DBNull.Value)
                    {

                        return (false);
                    }
                    else
                    {
                        return (true);

                    }
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n IsAnyRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                    return false;

                }
                //close Connection
                this.CloseConnection();
            }

            else
            {
                return false;
            }

        }

        public bool IsAnyRecord(string tbl, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("select count(*) from {0} {1}", tbl, strWhere) };

                    object obj = sqlComDb.ExecuteScalar();

                    //close Connection
                    this.CloseConnection();


                    if (int.Parse(obj.ToString()) == 0)
                    {

                        return (false);
                    }
                    else
                    {
                        return (true);
                    }
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n IsAnyRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return (false);

                }
            }
            else
            {
                return (false);
            }

        }

        //DateTime SqlServer
        public DateTime GetDateTimeSqlServer()
        {
            try
            {
                return DateTime.Parse(this.SendCommandGetStringResult("SELECT GETDATE()"));
            }
            catch (Exception ex)
            {
                connection.Close();
                string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n GetDatTimeSqlServer()\n" + ex;
                MessageBox.Show(strExceptionMsg);
                ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                return DateTime.Now;

            }

        }

        //SetDateForStatusBar();
        public string SetDateForStatusBar()
        {
            string strCurDateTime = "";

            try
            {
              //  string strDateTimeSql = this.SendCommandGetStringResault("SELECT GETDATE()");

                DateTime DateTimeSqlServer = GetDateTimeSqlServer();

                strCurDateTime =
                   string.Concat(
                       new object[] { " امروز ", ClsConvertDateHassan.convertDateAndDayToPersion(DateTimeSqlServer), "   ----------------  مطابق با  ", DateTimeSqlServer.ToString("dd-MM-yyyy   روز  dd  از ماه  MMM  سال  yyyy  میلادی ") + "                 " + MainForm.StrConnectedNetWork }
                                 );


                return strCurDateTime;
            }
            catch (Exception ex)
            {


                connection.Close();
                string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n SetDateForStatusBar()\n" + ex;
                MessageBox.Show(strExceptionMsg);
                ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                return strCurDateTime;

            }
        }

        //Find 
        public string FindFieldInLastRecord(string tbl, string strFieldName, string strFieldForSort)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("select top 1 {0} from {1} order by {2} desc", strFieldName, tbl, strFieldForSort) };

                    object obj = sqlComDb.ExecuteScalar();

                    return (obj.ToString());

                    //close Connection
                    CloseConnection();

                  
                }
                catch (Exception ex)
                {

                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n FindFieldInLastRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return ("نامشخص");

                }
            }
            else
            {
                return ("نامشخص");
            }

        }
        public string FindRecord(string tbl, string strFieldName, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("select {0} from {1} {2}", strFieldName, tbl, strWhere) };
                    object obj = sqlComDb.ExecuteScalar();

                   

                    if (obj == null || obj == DBNull.Value)
                    {

                        return (null);
                    }
                    else
                    {
                        return (obj.ToString());

                    }

                    //close Connection
                    this.CloseConnection();
                }
                catch (Exception ex)
                {


                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n FindRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return ("نامشخص");

                }
            }
            else
            {
                return (null);
            }
        }

        //Edit Statment
        public bool EditRecord(string tbl, string strFieldName, string strNewValue, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("update {0} set {1}=N'{2}' {3}", tbl, strFieldName, strNewValue, strWhere) };

                    sqlComDb.ExecuteNonQuery();
                    return true;

                    //close Connection
                    this.CloseConnection();

                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n EditRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));
                    return (false);

                }
            }
            else
            {
                return false;
            }
        }
        public void EditRecord(string tbl, string strFieldName, Boolean bolNewValue, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("update {0} set {1}='{2}' {3}", tbl, strFieldName, bolNewValue, strWhere) };

                    sqlComDb.ExecuteNonQuery();

                    //close Connection
                    this.CloseConnection();

                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n EditRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
            }
        }
        public void EditRecord(string tbl, string strFieldName, int intNewValue, string strWhere)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlComDb = new SqlCommand(tbl, connection) { CommandText = String.Format("update {0} set {1}='{2}' {3}", tbl, strFieldName, intNewValue, strWhere) };

                    sqlComDb.ExecuteNonQuery();

                    //close Connection
                    this.CloseConnection();

                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n EditRecord()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
            }
        }

        //Diff Statment
        public DataTable RemoveNullColumnFromDataTable(DataTable dt)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Rows[i][1].ToString() == null || dt.Rows[i][1].ToString() == "")
                            dt.Rows[i].Delete();
                    }

                    //close Connection
                    this.CloseConnection();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n RemoveNullColumnFromDataTable()\n" + ex;
                    MessageBox.Show(strExceptionMsg);
                    ClsDiff.writeLogOfAction((strExceptionMsg.Replace("'",":")));

                }
            }
            else
            {
                MessageBox.Show(" به دلیل عدم اتصال به سرور نال های دیتا تیبل حذف نشد");

            }
            return dt;
        }
    }


}
