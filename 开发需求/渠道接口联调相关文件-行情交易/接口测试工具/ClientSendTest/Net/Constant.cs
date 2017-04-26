using System;
using System.Collections.Generic;
using System.Text;

namespace ClientSendTest.Net
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class Constant
    {


        /// <summary>
        /// 域分隔符
        /// </summary>
        public const char SEPARATOR_SEGMENT = '#';
        public static string FC_ORDER_STATE_SENDED = "";
        public static string FC_PRICE_MODE_LIMIT = "2";
        public static string FC_PRICE_MODE_LAST = "3";
        /// <summary>
        /// 广播包段分隔符 #
        /// </summary>
        public const char GSI_EOREC = '#';

        /// <summary>
        /// 广播包域分隔符 ^
        /// </summary>
        public const char GSI_EOFLD = '#';

        /// <summary>
        /// 多级记录分隔符
        /// </summary>
        public static char[] SEPARATOR_RECORD = new char[] { '∧', '｜', 'ˇ', '¨', };


        /// <summary>
        /// 服务器证书路径
        /// </summary>
        public const string SERVER_CERT_PATH = "servercert_path";

        /// <summary>
        /// 客户端证书路径
        /// </summary>
        public const string CLIENT_CERT_PATH = "clientcert_path";

        /// <summary>
        /// 服务器证书名
        /// </summary>
        public const string SERVER_CERT_NAME = "servercert_name";

        /// <summary>
        /// 证书密码
        /// </summary>
        public const string CERT_PWD = "cert_password";



        /// <summary>
        /// 报文体长度说明的长度
        /// </summary>
        public const int MSG_LEN = 8;

        /// <summary>
        /// 加密标识+证书编号+会话ID长度
        /// </summary>
        public const int MSG_LEN2 = 15;
      
       
        /// 响应报文头截取长度
        /// </summary>
        public const int GSI_RSP_LEN = 44;
        /// <summary>
 
        /// <summary>
        /// 加密模式：0：不加密
        /// </summary>
        public const int ENCRYPT_MODEL_0 = 0;

        /// <summary>
        /// 加密模式：1：RSA算法加密
        /// </summary>
        public const int ENCRYPT_MODEL_1 = 1;

        /// <summary>
        /// 加密模式：2：3DES加密（会话密钥）
        /// </summary>
        public const int ENCRYPT_MODEL_2 = 2;

        /// <summary>
        /// 加密模式：3：3DES加密（默认密钥）
        /// </summary>
        public const int ENCRYPT_MODEL_3 = 3;

        /// <summary>
        /// 加密模式：4：ZIP压缩
        /// </summary>
        public const int ENCRYPT_MODEL_4 = 4;

        /// <summary>
        /// 加密模式：5：先ZIP压缩后再3DES加密(会话密钥)
        /// </summary>
        public const int ENCRYPT_MODEL_5 = 5;

        /// <summary>
        /// 加密模式：6：先ZIP压缩后再3DES加密(默认密密钥)
        /// </summary>
        public const int ENCRYPT_MODEL_6 = 6;

        /// <summary>
        /// 默认3DES密钥（24位）
        /// </summary>
        public static string SESSION_KEY_DEFAULT = "240262447423713749922240";
        /// <summary>
        /// 密钥长度
        /// </summary>
        public static int SESSION_KEY_DEFAULT_Len = 24;
        /// <summary>
        /// 加密向量
        /// </summary>
        public static string IV_DEFAULT = "12345678";

        /// <summary>
        /// 加密模式长度
        /// </summary>
        public static int ENCRYPT_MODEL_LEN = 1;

        /// <summary>
        /// 会话ID长度
        /// </summary>
        public static int SESSION_LEN = 10;

        /// <summary>
        /// 证书编码长度
        /// </summary>
        public static int CODE_LEN = 4;

        /// <summary>
        /// 会话ID
        /// </summary>
        public static string SESSION_ID = "";
        /// <summary>
        /// 会话密钥
        /// </summary>
        public static string SESSION_KEYS = "";
        /// <summary>
        /// 认证临时密钥
        /// </summary>
        public static string SESSION_KEY = "";

        /// <summary>
        /// 证书编码
        /// </summary>
        public static string SESSION_CODE = "C000";

        /// <summary>
        /// 数字证书
        /// </summary>
        public static string CCIEPATH = "";

        /// <summary>
        /// 设置socket链接时间
        /// </summary>
        public static int SOCKETTIME = 20000;

    }
}

