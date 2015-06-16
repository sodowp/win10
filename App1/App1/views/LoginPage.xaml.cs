using hdsense;
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
using Windows.UI.Popups;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void phoneloginbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        private void forgetbtn_Click(object sender, RoutedEventArgs e)
        {

        }
        string email;
        private void login()
        {
            progressBar.Visibility = Visibility.Visible;
            Dictionary<string, string> loginNumberDictionary = new Dictionary<string, string>();
            loginNumberDictionary.Add(CommonString.reqkey, CommonString.MethName.loginNumber.ToString());
            if (App.User != null)
            {
                loginNumberDictionary.Add(CommonString.uidkey, App.User.UserId);
            }
            else
            {

            }
            loginNumberDictionary.Add(CommonString.appkey, CommonString.appValue);
            loginNumberDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            //loginNumberDictionary.Add(CommonString.dtokey, CommonString.dtoValue);
            // loginNumberDictionary.Add(CommonString.oskey, CommonString.osValue);
            //loginNumberDictionary.Add(CommonString.didkey,string.Empty);
            //loginNumberDictionary.Add(CommonString.dmkey,string.Empty);
            loginNumberDictionary.Add(CommonString.formatkey, CommonString.formatValue);
            //loginNumberDictionary.Add(CommonString.dtykey, CommonString.dtyValue);       
            loginNumberDictionary.Add(CommonString.xnkey, userxntxt.Text);
            // string tempmd5 = HMACMD5.HMAC_MD5(passwordbox.Text, CommonString.md5key);
            //// string tempmd5 = MD5.GetMd5String(passwordbox.Text + CommonString.md5key);         

            // Debug.WriteLine(passwordbox.Text + CommonString.md5key);
            //Debug.WriteLine(tempmd5);

            //string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(tempmd5));
            //Debug.WriteLine(base64);
            byte[] byteMD5Code = sodoshot.Helper.MD5.GetMd5ByteArray(passwordbox.Text + CommonString.md5key);
            string base64 = Convert.ToBase64String(byteMD5Code);
            Debug.WriteLine(base64);
            Debug.WriteLine(WebUtility.UrlEncode(base64));
            string[] resutls = base64.Split('=');
            string tem = WebUtility.UrlEncode("==");
            loginNumberDictionary.Add(CommonString.pwdkey, WebUtility.UrlEncode(resutls[0]) + tem.ToUpper());

            try
            {

                ReuestHelper.GetUserRequest(loginNumberDictionary, (asynic) =>
                {
                    try
                    {
                        HttpWebRequest request2 = (HttpWebRequest)asynic.AsyncState;
                        HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(asynic);
                        int statusCode2 = (int)response2.StatusCode;
                        using (Stream stream = response2.GetResponseStream())
                        {

                            DataQueryResponse newQuery = DataQueryResponse.ParseFrom(stream);
                            if (newQuery.ResultCode == 20010)
                            {
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    //progressBar.Visibility = Visibility.Collapsed;
                                    //MessageBox.Show("密码错误");
                                });
                            }
                            else if (newQuery.ResultCode == 0)
                            {
                                PBGameUser pbuser = newQuery.User;
                                App.User = pbuser;
                                App.userid = pbuser.UserId;
                                //var settings = IsolatedStorageSettings.ApplicationSettings;
                                //if (!settings.Contains("xn"))
                                //{
                                //    settings.Add("xn", pbuser.XiaojiNumber);
                                //    settings.Save();
                                //}
                                //else
                                //{
                                //    settings["xn"] = pbuser.XiaojiNumber;
                                //    settings.Save();
                                //}
                                //if (!settings.Contains("userid"))
                                //{
                                //    settings.Add("userid", pbuser.UserId);
                                //    settings.Save();
                                //}
                                //else
                                //{
                                //    settings["userid"] = pbuser.UserId;
                                //    settings.Save();
                                //}
                                //if (!settings.Contains("nickname"))
                                //{
                                //    settings.Add("nickname", pbuser.NickName);
                                //    settings.Save();
                                //}
                                //else
                                //{
                                //    settings["nickname"] = pbuser.NickName;
                                //    settings.Save();
                                //}
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    MessageDialog msg = new MessageDialog("登录成功");
                                    msg.Title = "提示";
                                    //  msg.
                                    var msginfo = msg.ShowAsync();
                                    this.Frame.GoBack();
                                    //progressBar.Visibility = Visibility.Collapsed;
                                    //App.RootFrame.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                                });
                            }
                            else
                            {
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    //progressBar.Visibility = Visibility.Collapsed;
                                    //MessageBox.Show("登陆失败");
                                });
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            //progressBar.Visibility = Visibility.Collapsed;
                            //MessageBox.Show("网络异常");
                            MessageDialog msg = new MessageDialog("登录失败");
                            msg.Title = "提示";
                          //  msg.
                            var msginfo =  msg.ShowAsync();
                        });
                    }
                    catch (Exception ex2)
                    {
                    }
                });
            }

            catch (Exception e)
            {

            }
        }
    }
}
