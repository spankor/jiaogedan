using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace ClientSendTest.Util
{
    public class IPUtil
    {
        /// <summary>
        /// ��ȡ����IP��ַ��Ϣ
        /// </summary>
        public static string GetAddressIP()
        {
            ///��ȡ���ص�IP��ַ
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}
