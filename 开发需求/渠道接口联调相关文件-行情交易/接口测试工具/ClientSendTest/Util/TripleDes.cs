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
 * �ԳƼ���
 * ����des�����㷨
 * ��ԿΪ3��8�ֽ��ַ���
 */

namespace ClientSendTest.Util
{
    public class TripleDes
    {
        /// <summary>
        /// RSA����
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string sServerCertPath, byte[] sSrcMsgBuff)
        {
            RSACryptoServiceProvider rsaReceive = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsaSend = new RSACryptoServiceProvider();

            X509Certificate2 cert = new X509Certificate2(sServerCertPath);

            //����XML��ʽ�Ĺ�Կ
            String publicKey = cert.PublicKey.Key.ToXmlString(false);

            //���ͷ��ù�Կ��������
            rsaSend.FromXmlString(publicKey);

            //ÿ100���ֽڼ���һ��
            MemoryStream msOutBuff = new MemoryStream();
            for (int i = 0; i < sSrcMsgBuff.Length; i = i + 100)
            {
                byte[] tmp = CommUtil.ArrayCopy(sSrcMsgBuff, i, 100);
                //�ڶ�������ָʾ�Ƿ�ʹ��OAEP, ���ʹ��, ��������������Windows XP �����ϰ汾��
                //ϵͳ��. ����true ��false, ����ʱ���������ʱ��ѡ����ͬ. 
                byte[] cryp = rsaSend.Encrypt(tmp, false);

                msOutBuff.Write(cryp, 0, cryp.Length);
            }
            return msOutBuff.ToArray();
        }

        /// <summary>
        /// RSA����
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
        /// ʹ��3Des�㷨�Ա��Ľ��м���
        /// </summary>
        /// <param name="key">������Կ������24λ</param>
        /// <param name="ivByte">����������8λ</param>
        /// <param name="value">��Ҫ���м��ܵı����ֽ�����</param>
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

        //key-24�ֽڵ���Կ�� ivByte-8�ֽ�������value-����ܵ�����
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
        /// ����ԭ����
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
                //�����ܣ�ԭ���ķ���
                case 0:
                    break;
                //RSA�㷨����
                case 1:
                    bSrcMsgBuff = RSAEncrypt(Constant.CCIEPATH, bSrcMsgBuff);

                    break;
                //3DES���ܣ��Ự��Կ��
                case 2:
                    bSrcMsgBuff = TripleDes.encrypt(arrSession, arrIv, bSrcMsgBuff);

                    break;
                //3DES���ܣ�Ĭ����Կ��
                case 3:
                    bSrcMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bSrcMsgBuff);
                    break;

                //ZIPѹ��
                case 4:
                    bSrcMsgBuff = ZipUtil.Compress(bSrcMsgBuff);
                    
                    break;
                //��ZIPѹ������3DES����(�Ự��Կ)
                case 5:
                    bSrcMsgBuff = ZipUtil.Compress(bSrcMsgBuff);
                    bSrcMsgBuff = TripleDes.encrypt(arrSession, arrIv, bSrcMsgBuff);
                    break;
                //��ZIPѹ������3DES����(Ĭ������Կ)
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
            //�����Ĵ����ͱ���
            byte[] bFullBuff = new byte[Constant.MSG_LEN + Constant.ENCRYPT_MODEL_LEN + Constant.CODE_LEN + Constant.SESSION_LEN + bCryptBuff.Length];

            //��䱨�ĳ���
            int index = 0;
            byte[] bMsgLen = CommUtil.ConvertBytes(CommUtil.Fill("" + (bFullBuff.Length - Constant.MSG_LEN), '0', Constant.MSG_LEN, 'L'));
            Array.Copy(bMsgLen, 0, bFullBuff, index, Constant.MSG_LEN);

            //������ģʽ
            index += Constant.MSG_LEN;
            bMsgLen = CommUtil.ConvertBytes(CommUtil.Fill(iEncryptMode.ToString(), ' ', Constant.ENCRYPT_MODEL_LEN, 'R'));
            Array.Copy(bMsgLen, 0, bFullBuff, index, Constant.ENCRYPT_MODEL_LEN);

            //bFullBuff[index] = (byte)iEncryptMode;

            //���֤�����
            index += Constant.ENCRYPT_MODEL_LEN;
            byte[] bEncryptMode = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_CODE, ' ', Constant.SESSION_LEN, 'R'));
            Array.Copy(bEncryptMode, 0, bFullBuff, index, Constant.CODE_LEN);

            //���SessionID
            index += Constant.CODE_LEN;
            byte[] bsessionMode = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_ID, ' ', Constant.SESSION_LEN, 'R'));
            Array.Copy(bsessionMode, 0, bFullBuff, index, Constant.SESSION_LEN);



            //���ԭ���ļ��ܺ������
            index += Constant.SESSION_LEN;
            Array.Copy(bCryptBuff, 0, bFullBuff, index, bCryptBuff.Length);


            //���ؼ��ܺ����������
            return bFullBuff;

        }



        /// <summary>
        /// ���ܽ��ձ��ģ�������ͨѶͷ�ĳ���
        /// </summary>
        /// <param name="bEncryptMsgBuff">��������</param>
        /// <returns></returns>
        public static byte[] decryptMsg(byte[] bDecryptMsgBuff)
        {
            //�ӱ����л�ü���ģʽ
            byte decryptmodel = bDecryptMsgBuff[Constant.ENCRYPT_MODEL_LEN - 1];
            byte[] arrSession = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEYS, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrDefault = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEY_DEFAULT, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrIv = CommUtil.ConvertBytes(Constant.IV_DEFAULT);

            switch (decryptmodel)
            {
                //�����ܣ�ԭ���ķ���
                case 0:
                    break;
                //RSA�㷨����
                case 1:
                    bDecryptMsgBuff = RSAEncrypt(Constant.CCIEPATH, bDecryptMsgBuff);
                    break;
                //3DES���ܣ��Ự��Կ��
                case 2:

                    bDecryptMsgBuff = TripleDes.encrypt(arrSession, arrIv, bDecryptMsgBuff);
                    break;
                //3DES���ܣ�Ĭ����Կ��
                case 3:
                    bDecryptMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bDecryptMsgBuff);
                    break;

                //ZIPѹ��
                case 4:
                    bDecryptMsgBuff = ByteUtil.Decompress(bDecryptMsgBuff);
                    break;
                //��ZIPѹ������3DES����(�Ự��Կ)
                case 5:
                    bDecryptMsgBuff = ZipUtil.zip(bDecryptMsgBuff);
                    bDecryptMsgBuff = TripleDes.encrypt(bDecryptMsgBuff, arrIv, bDecryptMsgBuff);
                    break;
                //��ZIPѹ������3DES����(Ĭ������Կ)
                case 6:
                    bDecryptMsgBuff = ZipUtil.unzip(bDecryptMsgBuff);
                    bDecryptMsgBuff = TripleDes.encrypt(arrDefault, arrIv, bDecryptMsgBuff);
                    break;
            }

            return bDecryptMsgBuff;
        }
    }
}
