using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using hdsense;
using sodoshot.Common;
using sodoshot.network;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PictureDeatailPage : Page
    {
        public PictureDeatailPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //App.bmpimage = null;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                picdetail.Source = App.transfersource;
                fid = App.transferfeed.FeedId;
                OnepbFeed = App.transferfeed;
                getCommentlist(fid);
                setActionOnOpus("106");
                if (!string.IsNullOrEmpty(OnepbFeed.OpusImage))
                {
                    //int intpicindex = OnepbFeed.OpusImage.LastIndexOf('/');
                    //string smallpicname = OnepbFeed.OpusImage.Substring(intpicindex + 1, OnepbFeed.OpusImage.Length - intpicindex - 1);
                    //if (CommonFunction.checkiscacheimg(smallpicname, CommonString.smallcacheimgsirectory))
                    //{
                    //    Debug.WriteLine("offlinepic");
                    //    //pic.Source = CommonFunction.readexistfile(smallpicname, CommonString.smallcacheimgsirectory);
                    //}
                    //else
                    //{
                    //    Debug.WriteLine("onlinepic");
                    //    picbitmap = new BitmapImage(new Uri(OnepbFeed.OpusImage, UriKind.Absolute));
                    //    //pic.Source = picbitmap;
                    //}
                    if (OnepbFeed.OpusImage.Contains("_m.jpg"))
                    {
                        if (OnepbFeed.IsWithAppImage)
                        {
                            datuurl = OnepbFeed.OpusImage.Substring(0, OnepbFeed.OpusImage.Length - 6) + "_app.jpg";
                        }
                        else
                        {
                            datuurl = OnepbFeed.OpusImage.Substring(0, OnepbFeed.OpusImage.Length - 6) + ".jpg";
                        }

                    }
                    else
                    {
                        if (OnepbFeed.IsWithAppImage)
                        {
                            datuurl = OnepbFeed.OpusImage.Substring(0, OnepbFeed.OpusImage.Length - 4) + "_app.jpg";
                        }
                        else
                        {
                            datuurl = OnepbFeed.OpusImage;
                        }
                    }
                    int datuint = datuurl.LastIndexOf('/');
                    datutempname = datuurl.Substring(datuint + 1, datuurl.Length - datuint - 1);

                    Debug.WriteLine(datuurl + "dataurl");
                    Debug.WriteLine(OnepbFeed.OpusImage);
                }
                Debug.WriteLine(OnepbFeed.OpusImage);
                Debug.WriteLine(OnepbFeed);

                if (OnepbFeed.AlbumNumber > 0)
                {
                    GetOpusByID(fid);
                }

                IList<PBFeedTimes> feedtimes = OnepbFeed.FeedTimesList;
                foreach (PBFeedTimes onefeed in feedtimes)
                {
                    if (onefeed.Type == 10)
                    {
                       // viewcounttb.Text = "" + onefeed.Value.ToString();
                    }
                    if (onefeed.Type == 5 && onefeed.Value > 0)
                    {
                      //  getZanlist(fid);
                    }
                    if (onefeed.Type == 4 && onefeed.Value > 0)
                    {
                        getCommentlist(fid);
                    }
                }
                base.OnNavigatedTo(e);
            }
        }
        public void GetOpusByID(string opusid)
        {

            Dictionary<string, string> goiDictionary = new Dictionary<string, string>();
            goiDictionary.Add(CommonString.reqkey, CommonString.MethName.goi.ToString());
            goiDictionary.Add(CommonString.fidkey, opusid);
            goiDictionary.Add(CommonString.uidkey, App.User.UserId);
            goiDictionary.Add(CommonString.appkey, CommonString.appValue);
            goiDictionary.Add(CommonString.formatkey, CommonString.formatValue);
            goiDictionary.Add(CommonString.rdmkey, "1");
            goiDictionary.Add(CommonString.rcdkey, "0");
            ReuestHelper.GetRequest(goiDictionary, (asynic) =>
            {
                try
                {
                    HttpWebRequest request2 = (HttpWebRequest)asynic.AsyncState;
                    HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(asynic);
                    int statusCode2 = (int)response2.StatusCode;
                    using (Stream stream = response2.GetResponseStream())
                    {
                        DataQueryResponse newQuery = DataQueryResponse.ParseFrom(stream);
                        IList<PBFeed> pbfeed = newQuery.FeedList;
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            if (pbfeed != null && pbfeed.Count > 0)
                            {
                                PBFeed feed = pbfeed[0];
                                Debug.WriteLine(feed.AlbumNumber);
                                if (feed.AlbumNumber > 0)
                                {
                                    //alburmlist.ItemsSource = feed.AlbumsList;
                                    ////  btnalburm.Visibility = Visibility.Visible;
                                    //duotuimage.Visibility = Visibility.Visible;
                                    //numbertb.Text = feed.AlbumNumber.ToString() + "张";
                                    //numbertb.Visibility = Visibility.Visible;

                                }
                                if (feed.UserId == App.User.UserId)
                                {
                                  //  addduotuimage.Visibility = Visibility.Visible;
                                }

                                // img.Source = new BitmapImage(new Uri(pbfeed[0].OpusImage, UriKind.Absolute));
                            }
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
        private void Image_Tap(object sender, TappedRoutedEventArgs e)
        {

        }

        private void replygrid_Tap(object sender, TappedRoutedEventArgs e)
        {

        }
        string fid = string.Empty;
        PBFeed OnepbFeed;
        BitmapImage picbitmap;
        PBFeed pbFeedcomment;
        string datuurl = string.Empty;
        IList<PBFeed> resultlist;
        List<string> smallcacheimglist = new List<string>();
        List<string> cacheurlimgs = new List<string>();
       // IsolatedStorageSettings smallsettings;
        ObservableCollection<PBFeed> tempfeed = new ObservableCollection<PBFeed>();
        string tempname;//头像图缓存名称
        bool iscommentadd = false;
        private string datutempname;

        private void getCommentlist(string fid)
        {
            Dictionary<string, string> gfcDictionary = new Dictionary<string, string>();
            gfcDictionary.Add(CommonString.reqkey, CommonString.MethName.gfc.ToString());
            gfcDictionary.Add(CommonString.opidkey, fid);
            gfcDictionary.Add(CommonString.pgokey, CommonString.pgoValue);
            gfcDictionary.Add(CommonString.pgtkey, "200");
            gfcDictionary.Add(CommonString.formatkey, CommonString.formatValue);
            gfcDictionary.Add(CommonString.appkey, CommonString.appValue);
            gfcDictionary.Add(CommonString.rikey, "1");
            gfcDictionary.Add(CommonString.tpkey, "3");
            ReuestHelper.GetRequest(gfcDictionary, (asynic) =>
            {
                HttpWebRequest request2 = null;
                HttpWebResponse response2 = null;
                try
                {

                    request2 = (HttpWebRequest)asynic.AsyncState;

                    response2 = (HttpWebResponse)request2.EndGetResponse(asynic);
                    int statusCode2 = (int)response2.StatusCode;

                    //using (Stream stream = response2.GetResponseStream())
                    //{
                    Stream stream = null;
                    stream = response2.GetResponseStream();
                    //
                    DataQueryResponse pbDataQueryResponse = null;
                    pbDataQueryResponse = DataQueryResponse.ParseFrom(stream);
                    resultlist = null;
                    resultlist = pbDataQueryResponse.FeedList;
                    // stream.Close();
                    //getZanlist(fid);  
                    // DataQueryResponse newQuery = DataQueryResponse.ParseFrom(stream);
                    //PBFeed pbfeed = newQuery.FeedList[0];
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if (iscommentadd)
                        {
                            if (resultlist.Count > 0)
                            {
                                List<PBFeed> newpbbspost = new List<PBFeed>();
                                IList<PBFeed> bbspostlists = commentlistbox.ItemsSource as IList<PBFeed>;
                                foreach (var b in bbspostlists)
                                {
                                    newpbbspost.Add(b);
                                }
                                foreach (var a in resultlist)
                                {
                                    newpbbspost.Add(a);
                                }
                                commentlistbox.ItemsSource = newpbbspost;
                            }

                        }
                        else
                        {
                            commentlistbox.ItemsSource = resultlist;
                            //  progressBar.Visibility = Visibility.Collapsed;
                            //indicator.Text = "请求完成...";
                            //indicator.IsIndeterminate = false;
                            //indicator.IsVisible = false;
                        }

                    });


                    //}
                }
                catch (Exception ex)
                {
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                     //   MessageBox.Show(ex.Message);
                    });
                }
                finally
                {
                    //if (request2 != null)
                    //{
                    //    request2.Abort();

                    //}

                    //if (response2 != null) response2.Close();
                }
            });
        }
        public void setActionOnOpus(string acttype)
        {
            // progressBar.Visibility = Visibility.Visible;
            //indicator.Text = "请求中...";
            //indicator.IsIndeterminate = true;
            //indicator.IsVisible = true;
            Dictionary<string, string> aooDictionary = new Dictionary<string, string>();
            aooDictionary.Add(CommonString.reqkey, CommonString.MethName.aoo.ToString());
            aooDictionary.Add(CommonString.appkey, CommonString.appValue);
            aooDictionary.Add(CommonString.uidkey, App.userid);
            if (App.User == null)
            {
                aooDictionary.Add(CommonString.nnkey,"游客");
                aooDictionary.Add(CommonString.avkey, "http://sodoshot.com/static/images/LOGO.png");
            }
            else
            {
                aooDictionary.Add(CommonString.nnkey, App.User.NickName);
                aooDictionary.Add(CommonString.avkey, App.User.Avatar);
            }
            aooDictionary.Add(CommonString.gekey, CommonString.geValue);
            aooDictionary.Add(CommonString.opidkey, fid);



            aooDictionary.Add(CommonString.opckey, OnepbFeed.UserId);
            aooDictionary.Add(CommonString.comckey, CommonString.comcValue);
            aooDictionary.Add(CommonString.catekey, CommonString.cateValue);
            aooDictionary.Add(CommonString.vipkey, CommonString.vipValue);
            aooDictionary.Add(CommonString.cmtkey, CommonString.cmtValue);
            aooDictionary.Add(CommonString.cmidkey, fid);
            aooDictionary.Add(CommonString.cmsmkey, CommonString.cmsmValue);
            aooDictionary.Add(CommonString.cmuidkey, CommonString.cmuidValue);
            aooDictionary.Add(CommonString.cmnnkey, CommonString.cmnnValue);
            aooDictionary.Add(CommonString.cidkey, CommonString.cidValue);
            if (acttype == "100")
            {
                aooDictionary.Add(CommonString.ankey, "save_times");
            }
            if (acttype == "106")
            {
                aooDictionary.Add(CommonString.ankey, "play_times");
            }
            aooDictionary.Add(CommonString.actkey, acttype);
            ReuestHelper.GetRequest(aooDictionary, (asynic) =>
            {
                try
                {
                    HttpWebRequest request2 = (HttpWebRequest)asynic.AsyncState;
                    HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(asynic);
                    int statusCode2 = (int)response2.StatusCode;
                    using (Stream stream = response2.GetResponseStream())
                    {
                        StreamReader preader = new StreamReader(stream);
                        string str = preader.ReadToEnd();
                        string[] results = str.Split(':');
                        string resultcode = results[results.Length - 1].Split('}')[0];

                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            //  progressBar.Visibility = Visibility.Collapsed;
                            //indicator.Text = "请求完成...";
                            //indicator.IsIndeterminate = false;
                            //indicator.IsVisible = false;
                            if (acttype == "106")
                            {
                                //  viewcounttb.Text = string.Empty;
                            }
                            if (acttype == "100")
                            {
                                if (resultcode == "0")
                                {
                                    //Coding4Fun.Toolkit.Controls.
                                    //ToastPrompt toast = new ToastPrompt { Title = "", Message = "收藏成功", }; toast.Show();
                                    // getZanlist(fid);
                                }
                                else
                                {
                                   // var toast = new ToastPrompt { Title = "", Message = "收藏失败", }; toast.Show();
                                }
                            }
                            if (acttype == "6")
                            {
                                if (resultcode == "0")
                                {
                                 //   var toast = new ToastPrompt { Title = "", Message = "点赞成功", }; toast.Show();
                                 //   getZanlist(fid);
                                }
                                else
                                {
                                   // var toast = new ToastPrompt { Title = "", Message = "点赞失败", }; toast.Show();
                                }
                            }
                        });
                    }

                }
                catch (Exception)
                {


                }
            });
        }
    }
}
