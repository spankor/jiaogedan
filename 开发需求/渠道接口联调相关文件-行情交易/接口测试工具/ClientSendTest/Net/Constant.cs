using System;
using System.Collections.Generic;
using System.Text;

namespace ClientSendTest.Net
{
    /// <summary>
    /// ��������
    /// </summary>
    public class Constant
    {


        /// <summary>
        /// ��ָ���
        /// </summary>
        public const char SEPARATOR_SEGMENT = '#';
        public static string FC_ORDER_STATE_SENDED = "";
        public static string FC_PRICE_MODE_LIMIT = "2";
        public static string FC_PRICE_MODE_LAST = "3";
        /// <summary>
        /// �㲥���ηָ��� #
        /// </summary>
        public const char GSI_EOREC = '#';

        /// <summary>
        /// �㲥����ָ��� ^
        /// </summary>
        public const char GSI_EOFLD = '#';

        /// <summary>
        /// �༶��¼�ָ���
        /// </summary>
        public static char[] SEPARATOR_RECORD = new char[] { '��', '��', '��', '��', };


        /// <summary>
        /// ������֤��·��
        /// </summary>
        public const string SERVER_CERT_PATH = "servercert_path";

        /// <summary>
        /// �ͻ���֤��·��
        /// </summary>
        public const string CLIENT_CERT_PATH = "clientcert_path";

        /// <summary>
        /// ������֤����
        /// </summary>
        public const string SERVER_CERT_NAME = "servercert_name";

        /// <summary>
        /// ֤������
        /// </summary>
        public const string CERT_PWD = "cert_password";



        /// <summary>
        /// �����峤��˵���ĳ���
        /// </summary>
        public const int MSG_LEN = 8;

        /// <summary>
        /// ���ܱ�ʶ+֤����+�ỰID����
        /// </summary>
        public const int MSG_LEN2 = 15;
      
       
        /// ��Ӧ����ͷ��ȡ����
        /// </summary>
        public const int GSI_RSP_LEN = 44;
        /// <summary>
 
        /// <summary>
        /// ����ģʽ��0��������
        /// </summary>
        public const int ENCRYPT_MODEL_0 = 0;

        /// <summary>
        /// ����ģʽ��1��RSA�㷨����
        /// </summary>
        public const int ENCRYPT_MODEL_1 = 1;

        /// <summary>
        /// ����ģʽ��2��3DES���ܣ��Ự��Կ��
        /// </summary>
        public const int ENCRYPT_MODEL_2 = 2;

        /// <summary>
        /// ����ģʽ��3��3DES���ܣ�Ĭ����Կ��
        /// </summary>
        public const int ENCRYPT_MODEL_3 = 3;

        /// <summary>
        /// ����ģʽ��4��ZIPѹ��
        /// </summary>
        public const int ENCRYPT_MODEL_4 = 4;

        /// <summary>
        /// ����ģʽ��5����ZIPѹ������3DES����(�Ự��Կ)
        /// </summary>
        public const int ENCRYPT_MODEL_5 = 5;

        /// <summary>
        /// ����ģʽ��6����ZIPѹ������3DES����(Ĭ������Կ)
        /// </summary>
        public const int ENCRYPT_MODEL_6 = 6;

        /// <summary>
        /// Ĭ��3DES��Կ��24λ��
        /// </summary>
        public static string SESSION_KEY_DEFAULT = "240262447423713749922240";
        /// <summary>
        /// ��Կ����
        /// </summary>
        public static int SESSION_KEY_DEFAULT_Len = 24;
        /// <summary>
        /// ��������
        /// </summary>
        public static string IV_DEFAULT = "12345678";

        /// <summary>
        /// ����ģʽ����
        /// </summary>
        public static int ENCRYPT_MODEL_LEN = 1;

        /// <summary>
        /// �ỰID����
        /// </summary>
        public static int SESSION_LEN = 10;

        /// <summary>
        /// ֤����볤��
        /// </summary>
        public static int CODE_LEN = 4;

        /// <summary>
        /// �ỰID
        /// </summary>
        public static string SESSION_ID = "";
        /// <summary>
        /// �Ự��Կ
        /// </summary>
        public static string SESSION_KEYS = "";
        /// <summary>
        /// ��֤��ʱ��Կ
        /// </summary>
        public static string SESSION_KEY = "";

        /// <summary>
        /// ֤�����
        /// </summary>
        public static string SESSION_CODE = "C000";

        /// <summary>
        /// ����֤��
        /// </summary>
        public static string CCIEPATH = "";

        /// <summary>
        /// ����socket����ʱ��
        /// </summary>
        public static int SOCKETTIME = 20000;

    }
}

