using hdsense;
using Newtonsoft.Json;
using sodoshot.Common;
using sodoshot.network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RePage : Page
    {
        private string xnumber;
        private GETNUMBERJSONClass deserializedProduct2;

        public RePage()
        {
            this.InitializeComponent(); dlrequest();
        }

        private void chongxinbtn_Click(object sender, RoutedEventArgs e)
        {
            regetNumber();
        }

        private void lijiebtn_Click(object sender, RoutedEventArgs e)
        {
            setUserNumber();
        }
        private void dlrequest()
        {
            HardwareToken hwToken = HardwareIdentification.GetPackageSpecificToken(null);
        
  IBuffer hwID = hwToken.Id;
     
    byte[] hwIDBytes = hwID.ToArray(); //WindowsRuntimeBufferExtensions.ToArray(hwID) 
          
   string deviceID = hwIDBytes.Select(b => b.ToString()).Aggregate((b, next) => b + "," + next);
      
  //sample id result : 3,0,188,97,3,0,76,128,3,0,250,138,5,0,220,130,6,0,1,0,4,0,20,164,1,0,204,49,2,0,226,37,9,0,22,72 
            Dictionary<string, string> dlDictionary = new Dictionary<string, string>();
            dlDictionary.Add(CommonString.reqkey, CommonString.MethName.dl.ToString());
            dlDictionary.Add(CommonString.appkey, CommonString.appValue);
            dlDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            dlDictionary.Add(CommonString.didkey, deviceID);
            dlDictionary.Add(CommonString.dtokey, CommonString.dtoValue);
            dlDictionary.Add(CommonString.arekey, "1");
            dlDictionary.Add(CommonString.cckey, "US");
            dlDictionary.Add(CommonString.langkey, "en");
            dlDictionary.Add(CommonString.dmkey, "wp8");
            dlDictionary.Add(CommonString.oskey, CommonString.osValue);
            dlDictionary.Add(CommonString.dtykey, "3");
            Random random = new Random();
            int nm = random.Next(1, 1000000);
            dlDictionary.Add(CommonString.nnkey, "业余爱好者" + nm.ToString());
            dlDictionary.Add(CommonString.formatkey, "pb");
            ReuestHelper.GetUserRequest(dlDictionary, responseCallback);
        }
        private void responseCallback(IAsyncResult result)
        {
            try
            {
                //获取异步操作返回的的信息
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                //结束对 Internet 资源的异步请求
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                //解析应答头
                //parseRecvHeader(response.Headers);
                //获取请求体信息长度
                long contentLength = response.ContentLength;
                //获取应答码
                int statusCode = (int)response.StatusCode;
                string str = response.ContentType;
                string statusText = response.StatusDescription;
                //应答头信息验证
                using (Stream stream = response.GetResponseStream())
                {
                    //byte[] bytes = StreamToBytes(stream);
                    DataQueryResponse newQuery = DataQueryResponse.ParseFrom(stream);
                    if (newQuery.ResultCode == 0)
                    {
                        PBGameUser one = newQuery.User;
                        Debug.WriteLine(one.UserId + "oneuserid");
                        App.User = newQuery.User;
                        App.userid = newQuery.User.UserId;
                        Debug.WriteLine(App.User.UserId + "App.User");
                        //var settings = IsolatedStorageSettings.ApplicationSettings;
                        //if (!settings.Contains("userid"))
                        //{
                        //    settings.Add("userid", App.User.UserId);
                        //    settings.Save();
                        //}
                        //else
                        //{
                        //    settings["userid"] = one.UserId;
                        //}
                        //if (!settings.Contains("nickname"))
                        //{
                        //    settings.Add("nickname", App.User.NickName);
                        //    settings.Save();
                        //}
                        //else
                        //{
                        //    settings["nickname"] = one.NickName;
                        //}
                        //获取请求信息

                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            getNewNumber();
                            return;
                        });
                    }
                }
            }
            catch (WebException e)
            {
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //progressBar.Visibility = Visibility.Collapsed;
                    //MessageBox.Show("网络有问题");
                });
            }
            catch (Exception e)
            {
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //progressBar.Visibility = Visibility.Collapsed;
                    //MessageBox.Show("不明错误");
                });

            }
        }
        private void getNewNumber()
        {
          //  progressBar.Visibility = Visibility.Visible;
            Dictionary<string, string> getNewNumberDictionary = new Dictionary<string, string>();
            getNewNumberDictionary.Add(CommonString.reqkey, CommonString.MethName.getNewNumber.ToString());
            getNewNumberDictionary.Add(CommonString.uidkey, App.User.UserId);
            getNewNumberDictionary.Add(CommonString.appkey, CommonString.appValue);
            getNewNumberDictionary.Add(CommonString.removeOldNumberkey, "1");
            getNewNumberDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            getNewNumberDictionary.Add(CommonString.tpkey, "2");
            getNewNumberDictionary.Add(CommonString.setUserNumberkey, "0");
            ReuestHelper.GetUserRequest(getNewNumberDictionary, (m) =>
            {
                try
                {
                    HttpWebRequest request2 = (HttpWebRequest)m.AsyncState;
                    //结束对 Internet 资源的异步请求
                    HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(m);
                    //解析应答头
                    //parseRecvHeader(response.Headers);
                    //获取请求体信息长度
                    long contentLength2 = response2.ContentLength;

                    //获取应答码
                    int statusCode2 = (int)response2.StatusCode;
                    string str2 = response2.ContentType;
                    string statusText2 = response2.StatusDescription;
                    //应答头信息验证
                    using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                    {
                        string streamstr = reader.ReadToEnd();
                        string[] chars = new string[] { ":", "}" };
                        string[] result = streamstr.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                        if (result[result.Length - 1] == "0")
                        {
                            //Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            //{
                                
                            //    idtb.Text = result[2].Replace("\"", "");
                            //    xnumber = result[2].Replace("\"", "");
                               
                                
                            //});
                            deserializedProduct2 = (GETNUMBERJSONClass)JsonConvert.DeserializeObject(streamstr, typeof(GETNUMBERJSONClass));
                            //string[] chars = new string[] { ":", "}" };
                            //string[] result = streamstr.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                            if (deserializedProduct2.ret == 0)
                            {
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                   // progressBar.Visibility = Visibility.Collapsed;
                                    //TextBlock idtb = (pCustomMessageBox.Content as RegisterpopControl).FindName("idtb") as TextBlock;
                                    if (idtb != null)
                                    {
                                        idtb.Text = deserializedProduct2.dat.xn;
                                        xnumber = deserializedProduct2.dat.xn;
                                    }
                                });
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch (WebException e)
                {
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        //progressBar.Visibility = Visibility.Collapsed;
                        //MessageBox.Show("网络异常");
                    });
                }
                catch (Exception)
                {
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        //progressBar.Visibility = Visibility.Collapsed;
                        //MessageBox.Show("获取号码失败");
                    });
                }
            });
        }
        private void setUserNumber()
        {
           // progressBar.Visibility = Visibility.Visible;
            Dictionary<string, string> setUserNumberDictionary = new Dictionary<string, string>();
            setUserNumberDictionary.Add(CommonString.reqkey, CommonString.MethName.setUserNumber.ToString());
            setUserNumberDictionary.Add(CommonString.uidkey, App.User.UserId);
            setUserNumberDictionary.Add(CommonString.appkey, CommonString.appValue);
            setUserNumberDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            //    string temp = result[2].Replace("\"", "");
            setUserNumberDictionary.Add(CommonString.xnkey, xnumber);
            ReuestHelper.GetUserRequest(setUserNumberDictionary, (mr) =>
            {
                try
                {
                    HttpWebRequest request3 = (HttpWebRequest)mr.AsyncState;
                    //结束对 Internet 资源的异步请求
                    HttpWebResponse response3 = (HttpWebResponse)request3.EndGetResponse(mr);
                    //解析应答头
                    //parseRecvHeader(response.Headers);
                    //获取请求体信息长度
                    long contentLength3 = response3.ContentLength;

                    //获取应答码
                    int statusCode3 = (int)response3.StatusCode;
                    string str3 = response3.ContentType;
                    string statusText3 = response3.StatusDescription;
                    //应答头信息验证
                    using (StreamReader reader2 = new StreamReader(response3.GetResponseStream()))
                    {
                        string streamstr2 = reader2.ReadToEnd();

                        //var settings = IsolatedStorageSettings.ApplicationSettings;
                        //if (!settings.Contains("userid"))
                        //{
                        //    settings.Add("userid", App.User.UserId);
                        //    settings.Save();
                        //}
                        //else
                        //{
                        //    settings["userid"] = App.User.UserId;
                        //}
                        //if (!settings.Contains("nickname"))
                        //{
                        //    settings.Add("nickname", App.User.NickName);
                        //    settings.Save();
                        //}
                        //else
                        //{
                        //    settings["nickname"] = App.User.NickName;
                        //}
                        //if (!settings.Contains("xn"))
                        //{
                        //    settings.Add("xn", xnumber);
                        //    settings.Save();
                        //    //Deployment.Current.Dispatcher.BeginInvoke(() => { this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute)); });
                        //}
                        //else
                        //{
                        //    settings["xn"] = xnumber;
                        //    settings.Save();
                        //}
                        ////if (settings.Contains("xn"))
                        ////{
                        ////    settings["xn"] = xnumber;
                        ////    settings.Save();
                        ////}
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.Frame.GoBack();
                            //pCustomMessageBox.Dismiss();
                            //progressBar.Visibility = Visibility.Collapsed;
                            ////IDtb.Text = App.User.XiaojiNumber;
                            //IDtb.Text = xnumber;
                            //loginst.Visibility = Visibility.Collapsed;
                            //GetIdBtn.Content = "马上完善我的信息";
                            //LoginBtn.Content = "立即开始";
                            //stveryfycode.Visibility = Visibility.Visible;
                            //codegrid.Margin = new Thickness(0, 0, 0, 12);
                        });
                        //  App.User.XiaojiNumber = temp;
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                       // MessageBox.Show(ex.Message);
                        //  throw;
                    });
                }

            });

        }
        private void regetNumber()
        {
           // progressBar.Visibility = Visibility.Visible;
            Dictionary<string, string> getNewNumberDictionary = new Dictionary<string, string>();
            getNewNumberDictionary.Add(CommonString.reqkey, CommonString.MethName.getNewNumber.ToString());
            getNewNumberDictionary.Add(CommonString.uidkey, App.User.UserId);
            getNewNumberDictionary.Add(CommonString.appkey, CommonString.appValue);
            getNewNumberDictionary.Add(CommonString.removeOldNumberkey, "1");
            getNewNumberDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            getNewNumberDictionary.Add(CommonString.tpkey, "2");
            getNewNumberDictionary.Add(CommonString.setUserNumberkey, "0");

            ReuestHelper.GetUserRequest(getNewNumberDictionary, (m) =>
            {
                HttpWebRequest request2 = (HttpWebRequest)m.AsyncState;
                //结束对 Internet 资源的异步请求
                HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(m);
                //解析应答头
                //parseRecvHeader(response.Headers);
                //获取请求体信息长度
                long contentLength2 = response2.ContentLength;
                //获取应答码
                int statusCode2 = (int)response2.StatusCode;
                string str2 = response2.ContentType;
                string statusText2 = response2.StatusDescription;
                //应答头信息验证
                using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                {
                    string streamstr = string.Empty;
                    streamstr = reader.ReadToEnd();
                    //string[] chars = new string[] { ":", "}" };
                    //string[] result = streamstr.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                    deserializedProduct2 = (GETNUMBERJSONClass)JsonConvert.DeserializeObject(streamstr, typeof(GETNUMBERJSONClass));
                    //string[] chars = new string[] { ":", "}" };
                    //string[] result = streamstr.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                    if (deserializedProduct2.ret == 0)
                    {
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            //progressBar.Visibility = Visibility.Collapsed;
                            //TextBlock idtb = (pCustomMessageBox.Content as RegisterpopControl).FindName("idtb") as TextBlock;
                            if (idtb != null)
                            {
                                idtb.Text = deserializedProduct2.dat.xn;
                                xnumber = deserializedProduct2.dat.xn;
                            }
                        });
                    }
                    else
                    {

                    }
                    //if (result[result.Length - 1] == "0")
                    //{

                    //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    //    {
                    //        //progressBar.Visibility = Visibility.Collapsed;
                    //        //TextBlock idtb = (pCustomMessageBox.Content as RegisterpopControl).FindName("idtb") as TextBlock;
                    //        //if (idtb != null)
                    //        //{
                    //            idtb.Text = result[2].Replace("\"", "");
                    //            xnumber = result[2].Replace("\"", "");
                    //        //}
                    //    });
                    //}
                    //else
                    //{

                    //}
                }
            });
        }
    }
}
