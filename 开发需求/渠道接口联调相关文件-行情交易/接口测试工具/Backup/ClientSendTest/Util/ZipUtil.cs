using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Compression;
using System.IO;
using ClientSendTest.Util;
using System.Threading;

namespace ClientSendTest.Util
{
	public class ZipUtil
	{
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
    
        public static byte[] zip(byte[] bytes)
        {
            MemoryStream ms = null;
            GZipStream zipStream = null;
            try
            {
                ms = new MemoryStream();
                zipStream = new GZipStream(ms, CompressionMode.Compress, true);
                zipStream.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms.ToArray();
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (zipStream != null)
                    zipStream.Close();
                if (ms != null)
                    ms.Close();
            }
            return bytes;
        }

        /// <summary>
        /// 解压字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] unzip(byte[] bytes)
        {
            MemoryStream msIn = null;
            MemoryStream msOut = null;
            GZipStream zipStream = null;
            try
            {
                msIn = new MemoryStream(bytes);
                
                msOut = new MemoryStream();
                zipStream = new GZipStream(msIn, CompressionMode.Decompress,true);

                byte[] buff = new byte[4096];
                
                while (  true )
                {
                    int n = zipStream.Read(buff, 0, buff.Length);
                    if ( n == 0 && msOut.Length > 0 )
                        break;
                    msOut.Write(buff,0,n);
                }

                msOut.Position = 0;
                return msOut.ToArray();
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (zipStream != null)
                    zipStream.Close();
                if (msIn != null)
                    msIn.Close();
                if (msOut != null)
                    msOut.Close();


            }
            return bytes;
        }


        /// <summary>
        /// 压缩二进制流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream compressionStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    compressionStream.Write(data, 0, data.Length);
                    compressionStream.Flush();
                }
                //必须先关了compressionStream后才能取得正确的压缩流
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Decompress(byte[] data)
        {
            string commonString = "";
            MemoryStream mstream = new MemoryStream(data);
            GZipStream cstream = new GZipStream(mstream, CompressionMode.Decompress);
            StreamReader reader = new StreamReader(cstream);
            commonString = reader.ReadToEnd();
            mstream.Close();
            cstream.Close();
            reader.Close();
            return commonString;
        }
	}
}
