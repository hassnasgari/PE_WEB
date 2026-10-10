namespace PE
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class ClsNet
    {
        static ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public static string CheckNetOfSaServer()
        {
            Ping ping = new Ping();
            try
            {
                if (ping.Send("10.84.77.66").Status.ToString() == "Success")
                {
                    MainForm.strConnectedNetWorkSa = " * SA Net is TCE * ";
                    return "Sa Server Is : 10.84.77.66";
                }
            }
            catch
            {
            }
            try
            {
                if (ping.Send("172.16.3.17").Status.ToString() == "Success")
                {
                    MainForm.strConnectedNetWorkSa = " * SA Net is MCI * ";
                    return "Sa Server Is : 172.16.3.116";
                }

            }
            catch
            {
            }
            return "NET NOT FOUND";
        }
        public static string chooseNet()
        {
            
            string strOut = "NET NOT FOUND";
            string strConnect = string.Empty;
            
            if (MainForm.StrCurIp.Contains("10.84"))
            {
                MainForm.StrConnectedNetWork = " * Net is TCE * ";
                MainForm.StrSelectedRootPathInPeServer = @"\\10.84.224.74\PE_Share\PE";
                strOut = "10.84.224.74";
            }
            else
            {
                if (MainForm.StrCurIp.Contains("172.16"))
                {
                    MainForm.StrConnectedNetWork = " * Net is MCI * ";
                    MainForm.StrSelectedRootPathInPeServer = @"\\172.16.3.116\PE_Share\PE";
                    strOut = "172.16.3.116";
                }
            }
           
            return strOut ;
        }

        public static string IsAnyConnectedVpn()
        {
            string strConnectedVpn = "";
            if (NetworkInterface.GetIsNetworkAvailable())//first If
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface Interface in interfaces)
                {
                    
                        if (Interface.OperationalStatus == OperationalStatus.Up)

                        {


                        bool BolExistThisConnectionInDataBase = objClsSql.IsAnyRecord("[PE].[dbo].[tblExsitVpn]", "WHERE [VpnName]=N'" + Interface.Name + "'");

                        if (BolExistThisConnectionInDataBase)
                        {
                            return Interface.Name;
                        }
                        
                        }
                        else
                        {
                        strConnectedVpn= "";
                        }
               }

               
            }//fi
            return strConnectedVpn;

        }

        public static string GetIpAddress()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            string strIp = "";
            for(int i=0;i< addr.Length;i++)
            {
                if(addr[i].ToString().Contains("10.84"))
                {
                    strIp=addr[i].ToString();
                }
            }
            if(strIp=="")
            {
                for (int i = 0; i < addr.Length; i++)
                {
                    if (addr[i].ToString().Contains("172.16"))
                    {
                        strIp = addr[i].ToString();
                    }
                }
            }

            return strIp;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

        public static string GetMacFromIp(string ip)
        {
            IPAddress dst = IPAddress.Parse(ip); // the destination IP address

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            if (SendARP(BitConverter.ToInt32(dst.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                throw new InvalidOperationException("SendARP failed.");

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");

            return(string.Join(":", str));
        }


        public static string GetName()
        {
            string netBiosName = System.Environment.MachineName;

            string dnsName = System.Net.Dns.GetHostName();
            return dnsName;

        }

        public static void ExecuteBathFileOnRemoteSys(string strPathOfRemoteSys,string strNameOfBachFile)
        {
            try
            {
                string filePath =   strPathOfRemoteSys  + strNameOfBachFile;
                //v-nesing1 is the remote machine and monitoring is folder where a bat file is placed.
                // First check existance of file whether it exist on given path or not.
                if (File.Exists(filePath))
                {
                    // Start a new process which open a command prompt
                    ProcessStartInfo processInfo = new ProcessStartInfo("CMD.Exe");
                    // pass the .bat file path in arguments option with /c option which inforce only to execute .bat file process.
                    processInfo.Arguments = "/c " + filePath;
                    processInfo.UseShellExecute = false;
                    // CreateNoWindow means you want to do work silently without opening cmd.exe
                    processInfo.CreateNoWindow = true;
                    Process.Start(processInfo);

                }
            }
            catch ( Exception ex)
            {
                string strErr = ex.ToString();
            }
        }
    }
}

