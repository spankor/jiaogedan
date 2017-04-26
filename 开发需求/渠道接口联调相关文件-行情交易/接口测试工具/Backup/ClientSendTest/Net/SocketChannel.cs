using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ClientSendTest.Util;
using System.Text.RegularExpressions;


namespace ClientSendTest.Net
{
    /// <summary>
    /// ����ͨ���࣬�����ͺͽ��շ��ϻƽ��׶���ϵͳ���Ľӿڹ淶�ı���
    /// </summary>
    public class SocketChannel
    {
        /// <summary>
        /// ����Socket
        /// </summary>
        public Socket m_Socket = null;
        private int m_TimeOut = 0;
        private static byte[] m_byBuff = new byte[256];	// Recieved data buffer
        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        public SocketChannel()
        {

        }

        public SocketChannel(int v_iTimeOut)
        {
            m_TimeOut = v_iTimeOut;
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (m_Socket == null)
                    return false;
                else
                    return m_Socket.Connected;
            }
        }


        /// <summary>
        /// �����������ڲ���װ�˴���
        /// </summary>
        /// <param name="v_sHost">������ַ</param>
        /// <param name="v_iPort">�����˿�</param>
        /// <returns></returns>
        public bool Connect(string v_sHost, int v_iPort)
        {
            CloseSocket();
            m_Socket = null;
            m_Socket = ConnectByNoProxy(v_sHost, v_iPort);
            if (m_Socket != null)
            {
                if (m_TimeOut > 0)
                    SetScoketTimeout(m_TimeOut);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ����socket���ղ�ʱ
        /// </summary>
        /// <param name="v_iTimeout">��ʱʱ�䣨���룩</param>
        private void SetScoketTimeout(int v_iTimeout)
        {
            m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, v_iTimeout);
        }

        /// <summary>
        /// ���ͷ��ϻƽ��׶���ϵͳ�ӿڹ淶�ı���
        /// </summary>
        /// <param name="v_sMsg"></param>
        public void SendGoldMsg(string v_sMsg, int encryptType)
        {
            if (v_sMsg == null || v_sMsg.Length <= 0 || m_Socket == null)
                return;

            byte[] sSrcMsg = CommUtil.ConvertBytes(v_sMsg);
            byte[] bSendMsg = TripleDes.encryptMsg(encryptType, sSrcMsg);

            m_Socket.Send(bSendMsg, SocketFlags.None);
        }

        /// <summary>
        /// ֱ������ָ���������˿�
        /// </summary>
        /// <param name="v_sHost">������������IP��ַ</param>
        /// <param name="v_iPort">�������ŵĶ˿�</param>
        private Socket DirectConnect(string v_sHost, int v_iPort)
        {
            try
            {
                IPEndPoint iep = null;
                if (Regex.IsMatch(v_sHost, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                {
                    iep = new IPEndPoint(IPAddress.Parse(v_sHost), v_iPort);
                }
                else
                {
                    IPHostEntry iphe = Dns.GetHostEntry(v_sHost);
                    iep = new IPEndPoint(iphe.AddressList[0], v_iPort);
                }


                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if (!Connect(iep, Constant.SOCKETTIME))
                {

                    return null;
                }
                if (m_Socket.RemoteEndPoint.ToString() != iep.ToString())
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                string msg = "�������� [ " + v_sHost + " , " + v_iPort + " ]ʧ��.\n\n";
                msg += CommUtil.GetExceptionMsg(e);

                return null;
            }

            return m_Socket;

        }

        /// <summary>
        /// ��������ͨѶ���ã����ӵ����׷�����
        /// </summary>
        /// <returns></returns>
        public bool ConnectTransServer(string host, string port)
        {
            if (Connect(host, int.Parse(port)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Socket�����������ó�ʱʱ��
        /// </summary>
        /// <param name="remoteEndPoint">����˵�</param>
        /// <param name="timeoutMSec">��ʱʱ��</param>
        public bool Connect(IPEndPoint remoteEndPoint, int timeoutMSec)
        {
            TimeoutObject.Reset();
            m_Socket.BeginConnect(remoteEndPoint, CallBackMethod, m_Socket);
            //������ǰ�߳�

            if (TimeoutObject.WaitOne(timeoutMSec, true))
            {
                return true;

            }

            else
            {
                return false;

            }
        }

        private void CallBackMethod(IAsyncResult asyncresult)
        {

            //ʹ�������̼߳���
            Socket socket = asyncresult.AsyncState as Socket;

            if (socket.Connected)
            {
                socket.EndConnect(asyncresult);

            }

            TimeoutObject.Set();

        }

        /// <summary>
        /// �޴�������Զ������
        /// </summary>
        /// <param name="v_sHost">������������IP��ַ</param>
        /// <param name="v_iPort">�������ŵĶ˿�</param>
        /// <returns></returns>
        private Socket ConnectByNoProxy(string v_sHost, int v_iPort)
        {
            return DirectConnect(v_sHost, v_iPort);
        }

        /// <summary>
        /// ������ʽ���շ��ϻƽ��׶���ϵͳ�ӿڹ淶�ı���
        /// </summary>
        /// <returns></returns>
        public string RecvGoldMsg()
        {
            if (m_Socket == null)
                return "";

            byte[] bLens = RecvByLen(Constant.MSG_LEN);

            int len = Int32.Parse(CommUtil.ConvertString(bLens));
            byte[] bKeys = RecvByLen(1);
            byte[] bCcieKey = RecvByLen(4);
            byte[] bSessionID = RecvByLen(10);

            byte[] arrSession = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEYS, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrDefault = CommUtil.ConvertBytes(CommUtil.Fill(Constant.SESSION_KEY_DEFAULT, ' ', Constant.SESSION_KEY_DEFAULT_Len, 'R'));
            byte[] arrIv = CommUtil.ConvertBytes(Constant.IV_DEFAULT);
      

            byte[] bMsgs = RecvByLen(len - 1 - 4 - 10);
            int iType = Int32.Parse(CommUtil.ConvertString(bKeys));
            string sRecvMsg = "";
            switch (iType)
            {
                //�����ܣ�ԭ���ķ���
                case 0:
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;
                //RSA�㷨����
                case 1:
                    sRecvMsg = TripleDes.RSADecrypt(Constant.CCIEPATH, bMsgs);
                    break;
                //3DES���ܣ��Ự��Կ��
                case 2:
                    bMsgs = TripleDes.decrypt(arrSession, arrIv, bMsgs);
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;
                //3DES���ܣ�Ĭ����Կ��
                case 3:
                    bMsgs = TripleDes.decrypt(arrDefault, arrIv, bMsgs);
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;

                //ZIPѹ��
                case 4:
                    bMsgs = ZipUtil.unzip(bMsgs);
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;
                //��ZIPѹ������3DES����(�Ự��Կ)
                case 5:
                    bMsgs = TripleDes.decrypt(arrSession, arrIv, bMsgs);
                    bMsgs = ZipUtil.unzip(bMsgs);
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;
                //��ZIPѹ������3DES����(Ĭ������Կ)
                case 6:
                    bMsgs = TripleDes.decrypt(arrDefault, arrIv, bMsgs);
                    bMsgs = ZipUtil.unzip(bMsgs);
                    sRecvMsg = CommUtil.ConvertString(bMsgs);
                    break;
            }

            return sRecvMsg;
        }

        /// <summary>
        /// ��ѹ��ȡ���ı��ģ�û��ͨѶͷ��
        /// </summary>
        /// <param name="vReadBytes"></param>
        /// <returns></returns>
        private byte[] unzipReadBytes(byte[] vReadBytes)
        {
            if (vReadBytes.Length > 1
                    && vReadBytes[0] == 0x01)
            {
                byte[] zipBuff = new byte[vReadBytes.Length - 1];
                for (int i = 1; i < vReadBytes.Length; i++)
                {
                    zipBuff[i - 1] = vReadBytes[i];
                }

                byte[] unzipBuff = ZipUtil.unzip(zipBuff);
                byte[] tmp = new byte[unzipBuff.Length - Constant.MSG_LEN];
                for (int i = Constant.MSG_LEN; i < unzipBuff.Length; i++)
                    tmp[i - Constant.MSG_LEN] = unzipBuff[i];
                return tmp;
            }
            else
            {
                return vReadBytes;
            }
        }

        /// <summary>
        /// ���չ̶����ȵ�����
        /// </summary>
        /// <param name="v_iRecvLen"></param>
        /// <returns></returns>
        private byte[] RecvByLen(int v_iRecvLen)
        {
            byte[] recvBuff = new byte[v_iRecvLen];
            int iRecvIndex = 0;

            while (iRecvIndex < v_iRecvLen)
            {
                int iCanReadLen = v_iRecvLen - iRecvIndex;
                if (iCanReadLen > 1024)
                    iCanReadLen = 1024;

                byte[] tempBuff = new byte[iCanReadLen];

                try
                {
                    int iLen = m_Socket.Receive(tempBuff, iCanReadLen, SocketFlags.None);
                    if (iLen > 0)
                    {
                        for (int i = 0; i < iLen; i++)
                        {
                            recvBuff[iRecvIndex + i] = tempBuff[i];
                        }
                        iRecvIndex += iLen;
                    }
                    else if (iLen == 0)
                    {
                        throw new Exception("���ӱ�Զ������ǿ�ƹرգ�");
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return recvBuff;
        }

        /// <summary>
        /// �ر�Socket
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                if (m_Socket != null)
                    m_Socket.Close();
                m_Socket = null;
            }
            catch (Exception)
            {
            }
        }

    }
}
