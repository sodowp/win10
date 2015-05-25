using hdsense;
using sodoshot.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace App1.Helper
{
    public class HttpMultformdatapartPostHelper
    {
        private readonly string _boundary = "----------" + DateTime.Now.Ticks.ToString();

        private bool _isCancelled;
        private bool _isExecuted;
        private string _returnformat = string.Empty;
        public HttpMultformdatapartPostHelper(string url, Dictionary<string, object> parameters, Action<string> onCompleted, string returnformat)
        {
            this.Url = url;
            this.Parameters = parameters;
            this.OnCompleted = onCompleted;
            _returnformat = returnformat;
        }

        public string Url { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public Action<string> OnCompleted { get; private set; }

        public Action<string> OnError { get; set; }

        public void Execute()
        {
            if (this._isExecuted)
            {
                throw new NotSupportedException();
            }

            this._isExecuted = true;
            //业务相关
            DateTime s = new DateTime(1970, 1, 1);
            TimeSpan datetimespan = DateTime.Now - s;
            int totalseconds = (int)datetimespan.TotalSeconds;
            byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(totalseconds.ToString() + CommonString.md5requestkey);
            string base64 = Convert.ToBase64String(byteMD5Code);
            Debug.WriteLine(base64);
            Debug.WriteLine(WebUtility.UrlEncode(base64));
            Debug.WriteLine(this.Url + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + WebUtility.UrlEncode(base64));
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(this.Url + "&" + CommonString.istkey + "=" + totalseconds.ToString() + "&" + CommonString.cmikey + "=" + WebUtility.UrlEncode(base64));
            //以上业务相关
            //  HttpWebRequest httpWebRequest = WebRequest.CreateHttp(this.Url);
            // Debug.WriteLine(Url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = string.Format("multipart/form-data; boundary={0}", this._boundary);

            httpWebRequest.BeginGetRequestStream(new AsyncCallback(this.GetRequestStreamCallback), httpWebRequest);
        }

        public void Cancel()
        {
            this._isCancelled = true;
        }

        protected void InvokeOnErrorHandler(string message)
        {
            if (this.OnError != null)
            {
                this.InvokeInUiThread(() => this.OnError(message));
            }
        }

        protected void InvokeInUiThread(Action action)
        {
            if (!this._isCancelled)
            {
                DispatchedHandler handler = new Windows.UI.Core.DispatchedHandler(action);
                Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, handler);
                // Deployment.Current.Dispatcher.BeginInvoke(action);
            }
        }


        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            this.WriteMultipartObject(postStream, this.Parameters);
            postStream.Dispose();
            // postStream.Close();

            request.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // get the response
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.StartsWith("[net_WebHeaderInvalidControlChars]"))
                {
                    // not an exception, everything is ok
                    this.InvokeInUiThread(() => this.OnCompleted(null));
                }
                else
                {
                    this.InvokeOnErrorHandler("Unable to post data.");
                }

                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.InvokeOnErrorHandler((int)response.StatusCode + " " + response.StatusDescription);
                return;
            }

            // response stream
            using (Stream stream = response.GetResponseStream())
            {
                if (_returnformat == "pb")
                {
                    //DataQueryResponse  是自定义的类型 跟业务相关
                    DataQueryResponse dquery = DataQueryResponse.ParseFrom(stream);
                    if (dquery.ResultCode == 0)
                    {
                        this.InvokeInUiThread(() => this.OnCompleted("ok"));
                    }
                    else
                    {
                        this.InvokeInUiThread(() => this.OnCompleted("fail"));
                    }
                }
                if (_returnformat == "string")
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string str = reader.ReadToEnd();
                        this.InvokeInUiThread(() => this.OnCompleted(str));
                    }
                }

            }
        }

        private void WriteMultipartObject(Stream stream, Dictionary<string, object> data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data)
                {
                    this.WriteEntry(writer, entry.Key, entry.Value);
                }
            }

            writer.Write("--");
            writer.Write(this._boundary);
            writer.WriteLine("--");
            writer.Flush();
        }

        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(this._boundary);
                if (value is byte[])
                {
                    byte[] ba = value as byte[];


                    if (key == "pic")
                    {
                        writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "hfd.jpg");
                        writer.WriteLine(@"Content-Type:image/jpeg");
                    }
                    else if (key == "img")
                    {
                        writer.WriteLine(@"Content-Disposition:form-data;name=""{0}""; filename=""{1}""", key, "hfd.jpg");
                        writer.WriteLine(@"Content-Type:image/jpg");
                    }
                    else if (key == "image")
                    {
                        writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "ch.jpg");
                        writer.WriteLine(@"Content-Type:image/jpeg");
                    }

                    else
                    {
                        writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "data");
                        writer.WriteLine(@"Content-Type:application/octet-stream");
                    }
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;

                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }
    }
}
