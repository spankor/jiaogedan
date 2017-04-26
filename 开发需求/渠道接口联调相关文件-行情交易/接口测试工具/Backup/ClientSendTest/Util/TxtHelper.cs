using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClientSendTest.Util
{
    /// <summary>
    /// 读取写入文件
    /// </summary>
    public class TxtHelper
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read(string path)
        {
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            return string.Empty;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="strConn"></param>
        public static void Write(string path, string strConn)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
            sw.Write(strConn);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
