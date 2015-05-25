using sodoshot.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Windows.Media.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace sodoshot.Helper
{
  public  class CommonFunction
    {

       public static string  GetDictionaryString( Dictionary<string, string> dic)
      {
                 string result = string.Empty;
          foreach (KeyValuePair<string, string> kvp in dic)
{
    result = result + kvp.Key + "=" + kvp.Value + "&";
//Console.WriteLine(result);
}
       result=   result.Remove(result.Length - 1, 1);
          return result;
      }
      public static string GetchangedFormmatTime(int value)
       {
           string timestr = "一分钟前";
           string temptimestr = "分钟前";
           string temphourstr = "时前";
           string tempdaystr = "天前";
           string tempdmonthstr = "月前";
           if (value != null)
           {
               int k = (int)value;            
               DateTime s = new DateTime(1970, 1, 1);
               s = s.ToLocalTime();
              // DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
               s = s.AddSeconds(k);//(DateTime 是1个值类型,你只用add方法,s的值是没有改变的.
               Debug.WriteLine(s);
               Debug.WriteLine( s.ToString("yyyy-MMdd-HH:mm:ss"));
               //string str = s.ToString("yyyy-MMdd-HH:mm:ss");
               TimeSpan datetimespan =  DateTime.Now-s;
               int totalminutes = (int)datetimespan.TotalMinutes;
               Debug.WriteLine(totalminutes);
               //  double modenumber = totalminutes / 60;
               if (totalminutes < 1)
               {
                   return timestr;
               }
               else if (totalminutes > 1 && totalminutes < 60)
               {
                   return totalminutes.ToString() + temptimestr;
               }
               else if (totalminutes > 60 && totalminutes < 1400)
               {
                   int temhour = totalminutes / 60;
                   return temhour.ToString() + temphourstr;
               }
               else if (totalminutes > 1400 && totalminutes < 84000)
               {
                   int temday = totalminutes / 60 / 24;
                   return temday.ToString() + tempdaystr;
               }
               else if (totalminutes > 84000 && totalminutes < 2520000)
               {
                   int temmonth = totalminutes / 60 / 24 / 30;
                   return temmonth.ToString() + tempdmonthstr;
               }
               else
               {
                   string str = s.ToString("yyyy-MMdd-HH:mm");
                   return str;
               }

           }
           else
           {
               return string.Empty;
           }
       }
       //public static bool checkiscacheimg(string file,string path)
       // {
       //     //using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
       //     //{
       //     //    string tempath = path + "\\" + file;
       //     //    if (iso.FileExists(tempath))
       //     //    {
       //     //        return true;
       //     //    }
       //     //}
       //     //return false;
       // }
       /// <summary>
       /// 判断输入的字符串是否是一个合法的手机号
       /// </summary>
       /// <param name="input"></param>
       /// <returns></returns>
       public static bool IsMobilePhone(string input)
       {
           Regex regex = new Regex("^1\\d{10}$");
         
           return regex.IsMatch(input);

       }
       //public static BitmapImage readexistfile(string file, string path)
       // {
       //     BitmapImage onebitmap = new BitmapImage();
       //     try
       //     {
               
       //         using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
       //         {
       //             string tempath = path + "\\" + file;

       //             using (IsolatedStorageFileStream fileStream = iso.OpenFile(tempath, FileMode.Open, FileAccess.Read))
       //             {
       //                 onebitmap.SetSource(fileStream);
                                           
       //              //   fileStream.Close();
       //             }
       //            // iso.Dispose();
       //         }
             
       //     }
       //     catch (Exception ex)
       //     {
       //         onebitmap.UriSource = new Uri("/Image/mxfb.jpg",UriKind.RelativeOrAbsolute);

       //         return onebitmap;
       //     }
       //           return onebitmap;
         
            
           
       // }
       //public static void Cacheimg(string filename, BitmapImage bmp,string path)
       //{
       //    using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
       //    {
       //        try
       //        {
       //            if (!iso.DirectoryExists(path))
       //            {
       //                iso.CreateDirectory(path);
       //                string tempath = path + "\\" + filename;
       //                if (!iso.FileExists(tempath))
       //                {
       //                    using (IsolatedStorageFileStream fs = iso.CreateFile(tempath))
       //                    {                              
       //                        WriteableBitmap wb = new WriteableBitmap(bmp);
       //                        Extensions.SaveJpeg(wb, fs, wb.PixelWidth, wb.PixelHeight, 0, 100);
       //                        fs.Dispose();
       //                    }
       //                }
       //                else
       //                {
       //                }
       //            }
       //            else
       //            {
       //                string tempath = path + "\\" + filename;
       //                if (!iso.FileExists(tempath))
       //                {
       //                    using (IsolatedStorageFileStream fs = iso.CreateFile(tempath))
       //                    {                              
       //                        WriteableBitmap wb = new WriteableBitmap(bmp);
       //                        Extensions.SaveJpeg(wb, fs, wb.PixelWidth, wb.PixelHeight, 0, 100); 
       //                    }
       //                }
       //                else
       //                {
       //                }
       //            }
       //        }
       //        catch (Exception ex)
       //        {
       //        }

       //    }
       //}
    
    }
}
