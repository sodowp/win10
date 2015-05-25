using sodoshot.Common;
using sodoshot.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace sodoshot.network
{
  public  class ReuestHelper
    {
        public static void GetRequest(Dictionary<string, string> dic, System.AsyncCallback responseCallbacky)
        {
            DateTime s = new DateTime(1970, 1, 1);
            s = s.ToLocalTime();
            // s = s.AddSeconds(k);//(DateTime 是1个值类型,你只用add方法,s的值是没有改变的.
            //string str = s.ToString("yyyy-MMdd-HH:mm:ss");
            TimeSpan datetimespan = DateTime.Now - s;

            int totalseconds = (int)datetimespan.TotalSeconds;

            byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(totalseconds.ToString() + CommonString.md5requestkey);
            string base64 = Convert.ToBase64String(byteMD5Code);
            Debug.WriteLine(base64);
            //Debug.WriteLine(HttpUtility.UrlEncode(base64));
            string Getstr = CommonString.ip2 + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&cmi=" + WebUtility.UrlEncode(base64);
            HttpWebRequest request = null;
            request = HttpWebRequest.CreateHttp(new Uri(Getstr));
            //request.Headers["Cache-Control"] = "no-cache";
            //request.Headers["Pragma"] = "no-cache";

            if (request.Headers == null)
            {
                request.Headers = new WebHeaderCollection();
            }
            // request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            Debug.WriteLine(Getstr);
            //设置请求方式GET POST
            request.Method = "GET";

            //返回应答请求异步操作的状态
            request.BeginGetResponse(responseCallbacky, request);
        }
        public static void GetUserRequest(Dictionary<string, string> dic, System.AsyncCallback responseCallbacky)
        {
            DateTime s = new DateTime(1970, 1, 1);
            s = s.ToLocalTime();
            // s = s.AddSeconds(k);//(DateTime 是1个值类型,你只用add方法,s的值是没有改变的.
            //string str = s.ToString("yyyy-MMdd-HH:mm:ss");
            TimeSpan datetimespan = DateTime.Now - s;
            int totalseconds = (int)datetimespan.TotalSeconds;
            byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(totalseconds.ToString() + CommonString.md5requestkey);
            Debug.WriteLine(totalseconds.ToString() + CommonString.md5requestkey);
            string base64 = Convert.ToBase64String(byteMD5Code);
            Debug.WriteLine(base64);
            //Debug.WriteLine(HttpUtility.UrlEncode(base64));
            string[] resutls = base64.Split('=');
            HttpFormUrlEncodedContent posdata = new HttpFormUrlEncodedContent(dic);


            //string tem = HttpUtility.UrlEncode("==");
            //  uuDictionary.Add(CommonString.pwdkey, );
            //string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + HttpUtility.UrlEncode(resutls[0]) + tem.ToUpper();
            string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + WebUtility.UrlEncode(base64);
            //   string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic);
            // Debug.WriteLine(Getstr);
            //string encuodeurl=  HttpUtility.UrlEncode(Getstr);
            //Debug.WriteLine(encuodeurl);
            HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(Getstr));

            //设置请求方式GET POST
            request.Method = "GET";
            //if (request.Headers == null)
            //{
            //    request.Headers = new WebHeaderCollection();
            //}
            //request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            Debug.WriteLine(Getstr);
            //返回应答请求异步操作的状态
            request.BeginGetResponse(responseCallbacky, request);
        }
      //  public static void GetRequest(Dictionary<string, string> dic, System.AsyncCallback responseCallbacky)
      //{
      //    try
      //    {
      //        DateTime s = new DateTime(1970, 1, 1);
      //        s = s.ToLocalTime();
      //        // s = s.AddSeconds(k);//(DateTime 是1个值类型,你只用add方法,s的值是没有改变的.
      //        //string str = s.ToString("yyyy-MMdd-HH:mm:ss");
      //        TimeSpan datetimespan = DateTime.Now - s;

      //        int totalseconds = (int)datetimespan.TotalSeconds;

      //        byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(totalseconds.ToString() + CommonString.md5requestkey);
      //        string base64 = Convert.ToBase64String(byteMD5Code);
      //        Debug.WriteLine(base64);
      //        Debug.WriteLine(HttpUtility.UrlEncode(base64));
      //        string Getstr = CommonString.ip2 + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&cmi=" + HttpUtility.UrlEncode(base64);
      //        HttpWebRequest request = null;
      //        request = HttpWebRequest.CreateHttp(new Uri(Getstr));
      //        //request.Headers["Cache-Control"] = "no-cache";
      //        //request.Headers["Pragma"] = "no-cache";

      //        if (request.Headers == null)
      //        {
      //            request.Headers = new WebHeaderCollection();
      //        }
      //        request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
      //        Debug.WriteLine(Getstr);
      //        //设置请求方式GET POST
      //        request.Method = "GET";

      //        //返回应答请求异步操作的状态
      //        request.BeginGetResponse(responseCallbacky, request);
      //    }
      //    catch (Exception)
      //    {
              
      //        throw;
      //    }
         
      //}
      //public static void GetUserRequest(Dictionary<string, string> dic, System.AsyncCallback responseCallbacky)
      //{
      //    DateTime s = new DateTime(1970, 1, 1);
      //    s = s.ToLocalTime();
      //    // s = s.AddSeconds(k);//(DateTime 是1个值类型,你只用add方法,s的值是没有改变的.
      //    //string str = s.ToString("yyyy-MMdd-HH:mm:ss");
      //    TimeSpan datetimespan = DateTime.Now - s;
      //    int totalseconds = (int)datetimespan.TotalSeconds;
      //    byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(totalseconds.ToString() + CommonString.md5requestkey);
      //    Debug.WriteLine(totalseconds.ToString() + CommonString.md5requestkey);
      //    string base64 = Convert.ToBase64String(byteMD5Code);
      //    Debug.WriteLine(base64);
      //    Debug.WriteLine(HttpUtility.UrlEncode(base64));
      //    string[] resutls = base64.Split('=');
      //    string tem = HttpUtility.UrlEncode("==");
      //  //  uuDictionary.Add(CommonString.pwdkey, );
      //    //string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + HttpUtility.UrlEncode(resutls[0]) + tem.ToUpper();
      //    string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic) + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + HttpUtility.UrlEncode(base64);
      //   //   string Getstr = CommonString.ip + CommonFunction.GetDictionaryString(dic);
      //   // Debug.WriteLine(Getstr);
      //    //string encuodeurl=  HttpUtility.UrlEncode(Getstr);
      //    //Debug.WriteLine(encuodeurl);
      //    HttpWebRequest request = HttpWebRequest.CreateHttp(new Uri(Getstr));
      //    //设置请求方式GET POST
      //    request.Method = "GET";
      //    //if (request.Headers == null)
      //    //{
      //    //    request.Headers = new WebHeaderCollection();
      //    //}
      //    //request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
      //    Debug.WriteLine(Getstr);
      //    //返回应答请求异步操作的状态
      //    request.BeginGetResponse(responseCallbacky, request);
      //}
    }
}
