using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GessMonitoring.Util
{
    class TimeHelper
    {
        /// <summary>
        /// 判断是不是节假日,节假日返回true 
        /// </summary>
        /// <param name="date">日期格式：yyyyMMdd</param>
        /// <returns></returns>
        public static bool IsHolidayByDate(string date)
        {
            bool isHoliday = false;
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
            PostVars.Add("d", date);//参数
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(@"http://www.easybots.cn/api/holiday.php", "POST", PostVars);//请求地址,传参方式,参数集合
                string sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);//获取返回值

                string result = JObject.Parse(sRemoteInfo)[date].ToString();
                if (result == "0")
                {
                    isHoliday = false;
                }
                else if (result == "1" || result == "2")
                {
                    isHoliday = true;
                }
            }
            catch
            {
                isHoliday = false;
            }
            return isHoliday;
        }
    }
}
