using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SamplePost.Resources;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;


namespace SamplePost
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("bilgi"))
            {
                string denemem = (NavigationContext.QueryString["bilgi"]).ToString();
                MessageBox.Show(denemem);

            }
            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.URL = "Burası cevap döndüren web site http:li adres";
            webBrowserTask.Show();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cloud-api.yandex.net/v1/disk/");
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cloud-api.yandex.net/v1/disk/resources?path=/");
                if (request.Headers == null)
                {
                    request.Headers = new WebHeaderCollection();
                }
                request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
                request.Method = HttpMethod.Get;
                //request.Accept = "application/json;odata=verbose";
                request.Headers["Authorization"] = "OAuth burayakodgelir";
                request.BeginGetResponse(GetSizeOfSpace, request);
                
            }
            catch (Exception ex)
            {
               
            }
        }
        void GetSizeOfSpace(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    Dispatcher.BeginInvoke(() =>
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string text = reader.ReadToEnd();
                        string Total = "";
                        JObject jsonObject = JObject.Parse(text);

                        long used = jsonObject.Value<long>("used_space");
                        long totalsize = jsonObject.Value<long>("total_space");
                        double usedMB = (used / 1024f) / 1024f;
                        double totalMB = (totalsize / 1024f) / 1024f;

                        double usedGB = usedMB / 1024.0;
                        double totalGB = totalMB / 1024.0;

                        Total = Total + " Used : " + usedMB + " Total Size: " + totalMB + " ----- ";
                        Total = Total + " Used : " + usedGB + " Total Size: " + totalGB + " ----- ";
                        ekranmetin.Text = Total;

                    });

                }
                catch (WebException e)
                {
                    return;
                }
            }
        }

        void GetResourceOnDisk(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            RootObject parsedResponse;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    Dispatcher.BeginInvoke(() =>
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string text = reader.ReadToEnd();
                        string Total = "";

                        parsedResponse = JsonConvert.DeserializeObject<RootObject>(text);

                        int i = 1;
                        Total = Total + "Klasörler \n -----------------------------\n";
                        foreach (var item in parsedResponse._embedded.items)
                        {
                            if (item.type =="dir")
                            {
                                Total = Total + i + "-)" + item.name + "- " + item.type + "\n";
                                i++; 
                            }
                            
                        }
                        int j = 1;
                        Total = Total + "\n Dosyalar \n -----------------------------\n";
                        foreach (var item in parsedResponse._embedded.items)
                        {
                            if (item.type == "file")
                            {
                                Total = Total + j + "-)" + item.name + "- " + item.type + "\n";
                                j++;
                            }
                        }

                        ekranmetin.Text = Total +"\n Toplam :" + i + "Klasör" + j + "Dosya";

                    });

                    
                    
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }



        private void MetaInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cloud-api.yandex.net/v1/disk/resources?path=/&limit=25");
                if (request.Headers ==null)
                {
                    request.Headers = new WebHeaderCollection();
                }
                request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
                request.Method = HttpMethod.Get;
                //request.Accept = "application/json;odata=verbose";
                request.Headers["Authorization"] = "OAuth ";
                request.Headers["Date"] = DateTime.UtcNow.ToString();
                request.BeginGetResponse(GetResourceOnDisk, request);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cloud-api.yandex.net/v1/disk/resources/upload?path=deneme.txt");
            if (request.Headers == null)
            {
                request.Headers = new WebHeaderCollection();
            }
            request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            request.Method = HttpMethod.Get;
            //request.Accept = "application/json;odata=verbose";
            request.Headers["Authorization"] = "OAuth";
            request.BeginGetResponse(UploadTo, request);
        }


        void UploadTo(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            string href = "";
            string method;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    Dispatcher.BeginInvoke(() =>
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string text = reader.ReadToEnd();
                        string Total = "";
                        JObject jsonObject = JObject.Parse(text);

                        href = jsonObject.Value<string>("href");
                        method = jsonObject.Value<string>("method");
                        Uplo(href);
                    });

                    
                  

                }
                catch (WebException e)
                {
                    return;
                }
            }
        }

        private static void Uplo(string href)
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(href);
            if (_request.Headers == null)
            {
                _request.Headers = new WebHeaderCollection();
            }
            _request.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            _request.Method = HttpMethod.Put;
            _request.ContentType = "application/x-www-form-urlencoded";
            _request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), _request);
        }
        private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);
            string postData = "vasoo";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            string responseString = streamRead.ReadToEnd();
            //Debug.WriteLine(responseString);
            streamResponse.Close();
            streamRead.Close();
            response.Close();
        }




    }
}