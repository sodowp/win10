using App1.Helper;
using hdsense;
using sodoshot.Common;
using sodoshot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class uploadPage : Page
    {
        public uploadPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.bmpimage = null;
            base.OnNavigatedFrom(e);
        }
        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           // loadimg.Source = App.bmpimage;
            base.OnNavigatedTo(e);
        }
        private void can_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            checkbox.Background = new SolidColorBrush(Color.FromArgb(255, 161, 203, 59));
        }

        private void can_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            checkbox.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        HttpMultformdatapartPostHelper httpMultformdatapartPostHelperpb;
        int phototype;
        PBOpus.Builder onebuilder;
        byte[] enbytes;
        byte[] imagedate;
        bool hasnotfinish;
        public void submitOpus()
        {
            if (hasnotfinish)
            {
                return;
            }
            hasnotfinish = true;
            Dictionary<string, string> submitOpusDictionary = new Dictionary<string, string>();
            submitOpusDictionary.Add(CommonString.reqkey, CommonString.MethName.submitOpus.ToString());
            submitOpusDictionary.Add(CommonString.uidkey, App.userid);
            submitOpusDictionary.Add(CommonString.appkey, CommonString.appValue);
            submitOpusDictionary.Add(CommonString.stpkey, CommonString.stpValue);
            submitOpusDictionary.Add(CommonString.sodotpKey, phototype.ToString());
            submitOpusDictionary.Add(CommonString.dataTypekey, "aac");
            //string info = DeviceStatus.DeviceManufacturer + ",Windows" + System.Environment.OSVersion.Version.ToString().Substring(0, 3);
            submitOpusDictionary.Add(CommonString.dmkey, "windows10");
            submitOpusDictionary.Add(CommonString.formatkey, "pb");
            if (onebuilder == null)
            {
                onebuilder = PBOpus.CreateBuilder();
            }
            onebuilder.SetOpusId(string.Empty);
            onebuilder.SetName(picphotoname.Text);
            onebuilder.SetCategory((PBOpusCategoryType)1);
            onebuilder.SetDesc(destbox.Text);
            onebuilder.SetSodoType((PBOpusSodoType)phototype);
            onebuilder.SetType((PBOpusType)1000);
            //onebuilder.Category = PBOpusCategoryType.SING_CATEGORY;          
            var parameters = new Dictionary<string, object>();
            PBOpus mytets = onebuilder.Build();
            byte[] opusbyteArray = mytets.ToByteArray();
            parameters.Add("meta_data", opusbyteArray);
            if (enbytes != null)
            {
                parameters.Add("data", enbytes);
            }
            else
            {
                //  parameters.Add("data", imagedate);
            }


            parameters.Add("image", imagedate);


            string Getstr = CommonString.ip2 + CommonFunction.GetDictionaryString(submitOpusDictionary);
            this.httpMultformdatapartPostHelperpb = new HttpMultformdatapartPostHelper(Getstr, parameters, this.pbPostCompleted, "pb");
            this.httpMultformdatapartPostHelperpb.OnError = this.OnpbPostError;
            this.httpMultformdatapartPostHelperpb.Execute();

        }

        private void OnpbPostError(string obj)
        {
            // throw new NotImplementedException();
        }

        private void pbPostCompleted(string obj)
        {
            //  throw new NotImplementedException();
        }
        private async void loadimg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            //openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            var fileList = await openPicker.PickSingleFileAsync();
            //if (fileList.Count > 0)
            //{
            //    foreach (var item in fileList)
            //    {
            //原图
            IRandomAccessStream randomAccessStream = await fileList.OpenReadAsync();
           

            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(randomAccessStream);


            loadimg.Source = bmp;
            Stream stream = randomAccessStream.AsStream();
            imagedate = new byte[stream.Length];
            stream.Read(imagedate, 0, (int)stream.Length);
            //ImageItem imageItemBig = new ImageItem();
            //imageItemBig.ImageBig.SetSource(randomAccessStream);
            //imageItemListBig.Add(imageItemBig);

            //    }
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            submitOpus();
        }
    }
}
