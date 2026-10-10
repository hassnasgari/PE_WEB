namespace PE
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    internal class ClsFile
    {
        public static int CountLine(string FileName)
        {
            int num = 0;
            string str = "";
            using (StreamReader reader = File.OpenText(FileName))
            {
                for (str = reader.ReadLine(); str != null; str = reader.ReadLine())
                {
                    if (str != "")
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public static void CreateFile(string strPath)
        {
            if (!File.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            if (!File.Exists(strPath))
            {
                using (File.CreateText(strPath))
                {
                }
            }
        }

        public static void DeleteFile(string strPath)
        {
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
            }
        }

        public static string FindLatestDbXcl(string sDir)
        {
            int[] array = new int[100];
            string str = "نامشخص";
            string[,] strArray = new string[100, 2];
            for (int i = 0; i < 100; i++)
            {
                array[i] = 0x1388;
            }
            List<string> list = new List<string>();
            int index = 0;
            try
            {
                foreach (string str2 in Directory.GetFiles(sDir))
                {
                    list.Add(str2);
                    if (str2.Contains("Database_") && str2.Contains(".xlsx"))
                    {
                        TimeSpan span = (TimeSpan) (MainForm.DateTimeSqlServer - File.GetLastWriteTime(str2));
                        int num4 = (int) Math.Ceiling(span.TotalMinutes);
                        strArray[index, 0] = str2;
                        strArray[index, 1] = num4.ToString();
                        array[index] = num4;
                        index++;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            Array.Sort<int>(array);
            int num5 = array[0];
            for (int j = 0; j < 100; j++)
            {
                if (strArray[j, 1] == num5.ToString())
                {
                    str = strArray[j, 0];
                }
            }
            return str;
        }

        public static string FindUpdatestXcl(string strPath)
        {
            List<DateTime> source = new List<DateTime>();
            foreach (string str in Directory.GetFiles(strPath))
            {
                if (str.Contains("EEI-") && str.Contains(".xlsx"))
                {
                    int num = str.LastIndexOf(".");
                    string str2 = str.Substring(num - 14, 14);
                    DateTime creationTime = File.GetCreationTime(str);
                    source.Add(creationTime);
                }
            }
            source.Sort();
            return source.Last<DateTime>().ToString();
        }

        public static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            DataTable table2;
            string str = isFirstRowHeader ? "Yes" : "No";
            string directoryName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);
            string cmdText = "SELECT * FROM [" + fileName + "]";
            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + directoryName + ";Extended Properties=\"Text;HDR=" + str + "\""))
            {
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable {
                            Locale = CultureInfo.CurrentCulture
                        };
                        adapter.Fill(dataTable);
                        table2 = dataTable;
                    }
                }
            }
            return table2;
        }

        public static string ReadFileConvertToString(string strPathFile)
        {
            using (StreamReader reader = new StreamReader(strPathFile, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static void WriteCheckedBscsToFile(string strPath, string StrCentersCheked)
        {
            StreamWriter writer;
            if (!File.Exists(strPath))
            {
                using (writer = File.CreateText(strPath))
                {
                    CreateFile(strPath);
                    writer.WriteLine(StrCentersCheked);
                }
            }
            else
            {
                File.Delete(strPath);
                using (writer = File.CreateText(strPath))
                {
                    CreateFile(strPath);
                    writer.WriteLine(StrCentersCheked);
                }
            }
        }

        public void WriteDataTableToFile(DataTable dt, string filePath)
        {
            int index = 0;
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath, false);
                foreach (DataRow row in dt.Rows)
                {
                    object[] itemArray = row.ItemArray;
                    index = 0;
                    while (index < (itemArray.Length - 1))
                    {
                        writer.Write(itemArray[index] + " | ");
                        index++;
                    }
                    writer.Write(itemArray[index].ToString());
                    writer.WriteLine();
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Invalid Operation : \n" + exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static void WriteFirstColumnDataTableToFile(DataTable dt, string filePath)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath, false);
                foreach (DataRow row in dt.Rows)
                {
                    writer.WriteLine(row.ItemArray[0]);
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Invalid Operation : \n" + exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static void writeNetIp(string strLog)
        {
            string path = @"d:\DtOptions.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(strLog);
                }
            }
            else
            {
                File.WriteAllText(@"d:\DtOptions.txt", string.Empty);
            }
        }

        public static void WriteStrToLogFile(string strForWrite, bool bolOverWrite)
        {
            //StreamWriter writer;
            //string[] logicalDrives = Environment.GetLogicalDrives();
            //string str = "C";
            //if (logicalDrives[1] != null)
            //{
            //    str = "D";
            //}
            //string path = str + @":\LogAlmView.txt";
            //string str3 = string.Concat(new object[] { ClsConvertDateHassan.convertDateAndDayToPersion(DateTime.Now), "  زمان ", MainForm.DateTimeSqlServer.Hour, ":", MainForm.DateTimeSqlServer.Minute, ":", MainForm.DateTimeSqlServer.Second });
            //strForWrite = strForWrite + "  " + str3;
            //if (File.Exists(path))
            //{
            //    if (bolOverWrite)
            //    {
            //        DeleteFile(path);
            //        CreateFile(path);
            //        using (writer = File.AppendText(path))
            //        {
            //            writer.WriteLine(strForWrite);
            //            writer.WriteLine("------------------------------------------------------");
            //        }
            //    }
            //    else
            //    {
            //        using (writer = File.AppendText(path))
            //        {
            //            writer.WriteLine(strForWrite);
            //            writer.WriteLine("------------------------------------------------------");
            //        }
            //    }
            //}
            //else
            //{
            //    CreateFile(path);
            //    using (writer = File.AppendText(path))
            //    {
            //        writer.WriteLine("---------------LogFile Of AlarmViewer--------------");
            //        writer.WriteLine("------------------------------------------------------");
            //        writer.WriteLine(strForWrite);
            //        writer.WriteLine("------------------------------------------------------");
            //    }
            //}
        }

        public static bool checkSizeOfPic(string strPicFileName,int intDesiredWidth,int intDesiredHeight)//چک کردن سایز یک فایل عکس
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(@"" + strPicFileName + "");
            bool bolIsOkSize = false;
            if (img.Width == intDesiredWidth && img.Height == intDesiredHeight)
                bolIsOkSize = true;
            else
                bolIsOkSize = false;
            return bolIsOkSize;
        }
        public static bool HasThisExtension(string filename,string strExt)// چک کردن پسوند فایل
        {
           
            return Path.GetExtension(filename).Equals(strExt, StringComparison.InvariantCultureIgnoreCase);
        }


    }
}

