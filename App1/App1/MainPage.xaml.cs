using App1.views;
using hdsense;
using sodoshot.Common;
using sodoshot.network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private byte[] imagedate;

        public MainPage()
        {
            this.InitializeComponent();
        }
   
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void kindgrid_Tap(object sender, TappedRoutedEventArgs e)
        {
            Grid OBJ = sender as Grid;
            PBFeed onepb = (PBFeed)OBJ.DataContext;
            App.transferfeed = onepb;
        }
        private void GetLatestItems(string m)
        {
          //  ProgressBarVisibility = true;
            Dictionary<string, string> gflDictionary = new Dictionary<string, string>();
            gflDictionary.Add(CommonString.reqkey, CommonString.MethName.gfl.ToString());
            //if (App.User == null)
            //{
            //    PBGameUser.Builder bulider = PBGameUser.CreateBuilder();
               
            //        bulider.UserId = "888888888888888888888888";
               
            //        bulider.NickName = "sodo";
               
            //    App.User = bulider.Build();
            //}
            ////if (string.IsNullOrEmpty(App.User.UserId))
            ////{
            ////    // App.User.UserId = "666666";
            ////}
            //else
            //{
            //    gflDictionary.Add(CommonString.uidkey, App.User.UserId);
            //}
            gflDictionary.Add(CommonString.uidkey, App.userid);
            gflDictionary.Add(CommonString.pgokey, CommonString.pgoValue);
            gflDictionary.Add(CommonString.pgtkey, "19");
            gflDictionary.Add(CommonString.tpkey, ((int)sodoshot.Common.CommonString.FeedListType.FeedListTypeRecommend).ToString());
            gflDictionary.Add(CommonString.langkey, CommonString.langValue);
            gflDictionary.Add(CommonString.formatkey, CommonString.formatValue);
            gflDictionary.Add(CommonString.appkey, CommonString.appValue);
            gflDictionary.Add(CommonString.imgkey, "1");
            ReuestHelper.GetRequest(gflDictionary, (asynic) =>
            {
                try
                {

                    HttpWebRequest request2 = (HttpWebRequest)asynic.AsyncState;
                    HttpWebResponse response2 = (HttpWebResponse)request2.EndGetResponse(asynic);
                    int statusCode2 = (int)response2.StatusCode;
                    using (Stream stream = response2.GetResponseStream())
                    {
                        try
                        {
                          
                            DataQueryResponse newQuery = DataQueryResponse.ParseFrom(stream);
                            IList<PBFeed> pbfeed = newQuery.FeedList;
                            
                            if (pbfeed.Count > 0)
                            {
                                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    listbox.ItemsSource = pbfeed;
                                });
                                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    //{
                                    //    ProgressBarVisibility = false;
                                    //    LatestItems.Clear();
                                    //    foreach (var a in pbfeed)
                                    //    {

                                    //        Debug.WriteLine(a.AlbumNumber);
                                    //        Debug.WriteLine(a.AlbumsCount);
                                    //        Debug.WriteLine(a.AlbumsList.Count);
                                    //        //  Debug.WriteLine(a.);
                                    //        this.LatestItems.Add(a);
                                    //    }
                                    //});

                                }
                        }
                        catch (Exception e)
                        {
                            // GetLatestItems(string.Empty);
                        }
                    }
                }
                catch (WebException ex)
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    //    // MessageBox.Show(AppResources.neterror);
                    //    var toast = new ToastPrompt { Message = AppResources.neterror, }; toast.Show();
                    //});
                }
                catch (Exception)
                {


                }
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetLatestItems(string.Empty);
        }

        private void addeffectbtn_Click(object sender, RoutedEventArgs e)
        {
            getpicfile();
            this.Frame.Navigate(typeof(addeffectPage));
        }

        private void tuwenbtn_Click(object sender, RoutedEventArgs e)
        {
            getpicfile();
            this.Frame.Navigate(typeof(tuwenPage));
        }

        private void uploadbtn_Click(object sender, RoutedEventArgs e)
        {
         //   getpicfile();
            this.Frame.Navigate(typeof(uploadPage));
        }
        private async void getpicfile()
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();

                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".gif");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".bmp");

                var fileList = await openPicker.PickSingleFileAsync();
                //if (fileList.Count > 0)
                //{
                //    foreach (var item in fileList)
                //    {
                //原图
                IRandomAccessStream randomAccessStream = await fileList.OpenReadAsync();
                Stream stream = randomAccessStream.AsStream();
                imagedate = new byte[stream.Length];
                stream.Read(imagedate, 0, (int)stream.Length);

                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(randomAccessStream);
                App.bmpimage = bmp;
            }
            catch (Exception)
            {

              
            }
            
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image senderimg = sender as Image;
            App.transfersource = senderimg.Source;
            App.transferfeed = (senderimg.DataContext as PBFeed);
            this.Frame.Navigate(typeof(PictureDeatailPage));
        }

        private void rebtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
