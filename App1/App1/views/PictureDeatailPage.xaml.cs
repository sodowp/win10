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
                getCommentlist(fid);
            base.OnNavigatedTo(e);
            }
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
    }
}
