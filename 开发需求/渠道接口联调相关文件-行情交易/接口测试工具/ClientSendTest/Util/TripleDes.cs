using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using ClientSendTest.Util;
using ClientSendTest.Net;

/**
 * 对称加密
 * 三种des加密算法
 * 密钥为3个8字节字符串
 */

namespace ClientSendTest.Util
{
    public class TripleDes
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string sServerCertPath, byte[] sSrcMsgBuff)
        {
            RSACryptoServiceProvider rsaReceive = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsaSend = new RSACryptoServiceProvider();

            X509Certificate2 cert = new X509Certificate2(sServerCertPath);

            //生成XML格式的公钥
            String publicKey = cert.PublicKey.Key.ToXmlString(false);

            //发送方用公钥加密数据
            rsaSend.FromXmlString(publicKey);

            //每100个字节加密一次
            MemoryStream msOutBuff = new MemoryStream();
            for (int i = 0; i < sSrcMsgBuff.Length; i = i + 100)
            {
                byte[] tmp = CommUtil.ArrayCopy(sSrcMsgBuff, i, 100);
                //第二个参数指示是否使用OAEP, 如果使用, 则程序必须运行在Windows XP 及以上版本的
                //系统中. 无论true 或false, 解密时必须跟加密时的选择相同. 
                byte[] cryp = rsaSend.Encrypt(tmp, false);

                msOutBuff.Write(cryp, 0, cryp.Length);
            }
            return msOutBuff.ToArray();
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string sServerCertPath, byte[] recvMsgBody)
        {
            RSACryptoServiceProvider rsaReceive = new RSACryptoServiceProvider();
            string CreatePwd = "t7Bt2DPDSiE=";
            X509Certificate2 cert = new X509Certificate2(sServerCertPath, CreatePwd, X509KeyStorageFlags.Exportable);
            string privatekey = cert.PublicKey.Key.ToXmlString(false);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            MemoryStream msOutBuff = new MemoryStream();
            for (int i = 0; i < recvMsgBody.Length; i = i + 128)
            {
                byte[] tmpBuff = CommUtil.ArrayCopy(recvMsgBody, i, 128);
                byte[] decryptBuff = rsaReceive.Decrypt(tmpBuff, false);
                msOutBuff.Write(decryptBuff, 0, decryptBuff.Length);
            }

            return CommUtil.ConvertString(msOutBuff.ToArray());
        }


        /// <summary>
        /// 使用3Des算法对报文进行加密
        /// </summary>
        /// <param name="key">加密密钥，定长24位</param>
        /// <param name="ivByte">向量，定长8位</param>
        /// <param name="value">需要进行加密的报文字节数组</param>
        /// <returns></returns>
        public static byte[] encrypt(byte[] key, byte[] ivByte, byte[] value)
        {
            MemoryStream mStream = null;
            CryptoStream cStream = null;
            try
            {
                mStream = new MemoryStream();

                cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(key, ivByte),
                    CryptoStreamMode.Write);

                cStream.Write(value, 0, value.Length);
                cStream.FlushFinalBlock();

                return mStream.ToArray();
            }
            catch (CryptographicException e)
            {
            }
            finally
            {
                if (mStream != null)
                    mStream.Close();
                if (cStream != null)
                    cStream.Close();
            }
            return null;
        }

        //key-24字节的密钥， ivByte-8字节向量，value-需解密的数据
        public static byte[] decrypt(byte[] key, byte[] ivByte, byte[] value)
        {
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            try
            {
                msDecrypt = new MemoryStream(value);

                csDecrypt = new CryptoStream(msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(key, ivByte),
                    CryptoStreamMode.Read);

                byte[] fromEncrypt = new byte[value.Length];

                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
            }
            finally
            {
                if (msDecrypt != null)
                    msDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
            }

            return null;
        }

        /// <summary>
        /// 加密原报文
        /// </summary>
        /// <param name="iEncryptMode"></param>
        /// <param name="bSrcMsgBuff"></param>
        /// <returns></returns>
        public static byte[] encryptMsg(int iEncryptMode, byte[] bSrcMsgBuff)
        {
            byte[] arrSession = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEYS, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrDefault = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEY_DEFAULT, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrIv = CommUtil.ConvertBytes(Constant.IV_DEFAULT);
            switch (iEncryptMode)
            {
                //不加密，原报文返回
                case 0:
                    break;
                //RSA算法加密
                case 1:
                    bSrcMsgBuff = RSAEncrypt(Constant.CCIEPATH, bSrcMsgBuff);

                    break;
                //3DES加密（会话密钥）
                case 2:
                    bSrcMsgBuff = TripleDes.encrypt(arrSession, arrIv, bSrcMsgBuff);

                    break;
                //3DES加密（默认密钥）
                case 3:
                    bSrcMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bSrcMsgBuff);
                    break;

                //ZIP压缩
                case 4:
                    bSrcMsgBuff = ZipUtil.Compress(bSrcMsgBuff);
                    
                    break;
                //先ZIP压缩后再3DES加密(会话密钥)
                case 5:
                    bSrcMsgBuff = ZipUtil.Compress(bSrcMsgBuff);
                    bSrcMsgBuff = TripleDes.encrypt(arrSession, arrIv, bSrcMsgBuff);
                    break;
                //先ZIP压缩后再3DES加密(默认密密钥)
                case 6:
                    bSrcMsgBuff = ZipUtil.Compress(bSrcMsgBuff);
                    bSrcMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bSrcMsgBuff);
                    break;
            }
            bSrcMsgBuff = AddLenByte(iEncryptMode, Constant.SESSION_CODE, Constant.SESSION_ID, bSrcMsgBuff);
            return bSrcMsgBuff;

        }

        public static byte[] AddLenByte(int iEncryptMode, string Code, string ID, byte[] bCryptBuff)
        {
            //完整的待发送报文
            byte[] bFullBuff = new byte[Constant.MSG_LEN + Constant.ENCRYPT_MODEL_LEN + Constant.CODE_LEN + Constant.SESSION_LEN + bCryptBuff.Length];

            //填充报文长度
            int index = 0;
            byte[] bMsgLen = CommUtil.ConvertBytes(CommUtil.Fill("" + (bFullBuff.Length - Constant.MSG_LEN), '0', Constant.MSG_LEN, 'L'));
            Array.Copy(bMsgLen, 0, bFullBuff, index, Constant.MSG_LEN);

            //填充加密模式
            index += Constant.MSG_LEN;
            bMsgLen = CommUtil.ConvertBytes(CommUtil.Fill(iEncryptMode.ToString(), ' ', Constant.ENCRYPT_MODEL_LEN, 'R'));
            Array.Copy(bMsgLen, 0, bFullBuff, index, Constant.ENCRYPT_MODEL_LEN);

            //bFullBuff[index] = (byte)iEncryptMode;

            //填充证书编码
            index += Constant.ENCRYPT_MODEL_LEN;
            byte[] bEncryptMode = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_CODE, ' ', Constant.SESSION_LEN, 'R'));
            Array.Copy(bEncryptMode, 0, bFullBuff, index, Constant.CODE_LEN);

            //填充SessionID
            index += Constant.CODE_LEN;
            byte[] bsessionMode = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_ID, ' ', Constant.SESSION_LEN, 'R'));
            Array.Copy(bsessionMode, 0, bFullBuff, index, Constant.SESSION_LEN);



            //填充原报文加密后的内容
            index += Constant.SESSION_LEN;
            Array.Copy(bCryptBuff, 0, bFullBuff, index, bCryptBuff.Length);


            //返回加密后的完整报文
            return bFullBuff;

        }



        /// <summary>
        /// 解密接收报文，不包括通讯头的长度
        /// </summary>
        /// <param name="bEncryptMsgBuff">报文内容</param>
        /// <returns></returns>
        public static byte[] decryptMsg(byte[] bDecryptMsgBuff)
        {
            //从报文中获得加密模式
            byte decryptmodel = bDecryptMsgBuff[Constant.ENCRYPT_MODEL_LEN - 1];
            byte[] arrSession = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEYS, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrDefault = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEY_DEFAULT, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrIv = CommUtil.ConvertBytes(Constant.IV_DEFAULT);

            switch (decryptmodel)
            {
                //不加密，原报文返回
                case 0:
                    break;
                //RSA算法加密
                case 1:
                    bDecryptMsgBuff = RSAEncrypt(Constant.CCIEPATH, bDecryptMsgBuff);
                    break;
                //3DES加密（会话密钥）
                case 2:

                    bDecryptMsgBuff = TripleDes.encrypt(arrSession, arrIv, bDecryptMsgBuff);
                    break;
                //3DES加密（默认密钥）
                case 3:
                    bDecryptMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bDecryptMsgBuff);
                    break;

                //ZIP压缩
                case 4:
                    bDecryptMsgBuff = ByteUtil.Decompress(bDecryptMsgBuff);
                    break;
                //先ZIP压缩后再3DES加密(会话密钥)
                case 5:
                    bDecryptMsgBuff = ZipUtil.zip(bDecryptMsgBuff);
                    bDecryptMsgBuff = TripleDes.encrypt(bDecryptMsgBuff, arrIv, bDecryptMsgBuff);
                    break;
                //先ZIP压缩后再3DES加密(默认密密钥)
                case 6:
                    bDecryptMsgBuff = ZipUtil.unzip(bDecryptMsgBuff);
                    bDecryptMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bDecryptMsgBuff);
                    break;
            }

            return bDecryptMsgBuff;
        }
    }
}
