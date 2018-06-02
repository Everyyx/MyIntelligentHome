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

using Windows.Media.Core;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.System.Display;
using Windows.Graphics.Display;
using MyIntelligentHomeSystem.Helpers;
using Windows.UI.Core;
using Windows.ApplicationModel;
using System.Net.Http;
using System.Net.Http.Headers;
using MyIntelligentHomeSystem.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyIntelligentHomeSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FaceDetectPage : Page
    {
        const string subscriptionKey = "58517156548544b18120161caebff53f";
        const string uriFace = "https://api.cognitive.azure.cn/face/v1.0/";
        const string groupId = "group1";
        private const double confidenceThreshold = 0.5;

        private MediaCapture captureManager;
        private bool _isPreviewing;
        DisplayRequest displayRequest = new DisplayRequest();
        private List<MyDetectFace> detectFaces;

        private List<PersonsInFaceGroup> allperson;

        private List<MyIdentifyFace> identifyFaces;

        private Dictionary<string, string> FaceDictionary = new Dictionary<string, string>();
        public ObservableCollection<Visitor> visitors { get; set; }
        public FaceDetectPage()
        {
            this.InitializeComponent();

            Application.Current.Suspending += Application_Suspending;

            Application.Current.Resuming += Application_Resuming;

            //FaceDictionary.Add("dd130dd0-9e4d-4ae0-bef3-5869465bddfa", "安文瑞");
            visitors = new ObservableCollection<Visitor>();
            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BingWallPaper/Face.jpg"))
            };
            Facepage_grid.Background = imageBrush;
        }



        private async void DoorBell_Click(object sender, RoutedEventArgs e)
        {
            DoorBell.IsEnabled = false;
            ImageEncodingProperties imageFormat = ImageEncodingProperties.CreateJpeg();
            StorageFile file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync($"{DateTime.Now.ToFileTime()}.jpg", CreationCollisionOption.GenerateUniqueName);
            await captureManager.CapturePhotoToStorageFileAsync(imageFormat, file);

            Stream stream = await file.OpenStreamForReadAsync();
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] imagedata = binaryReader.ReadBytes((int)stream.Length);

            ByteArrayContent byteArrayContent = new ByteArrayContent(imagedata);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestuir = uriFace + "detect?returnFaceId=true";// &returnFaceLandmarks=true&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
        
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var responseMessage = await client.PostAsync(requestuir, byteArrayContent);
            if (responseMessage.StatusCode.ToString()=="OK")
            {
                string detectjson = await responseMessage.Content.ReadAsStringAsync();
                detectFaces = await Myjson.ToObjectAsync<List<MyDetectFace>>(detectjson);
                if (detectFaces.Count>0)
                {
                    BitmapImage image = new BitmapImage(new Uri(file.Path));
                    //visitors.Add(new Visitor { VisitDate = System.DateTime.Now.ToString(), VisitorImage = image });

                    var identifyclient = new HttpClient();
                    string identifyUri = uriFace + "identify";
                    identifyclient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                    string requestjson = "{\"personGroupId\":\"" + groupId + "\",\"faceIds\":[\"" + detectFaces[0].faceId + "\"],\"maxNumOfCandidatesReturned\": 1,\"confidenceThreshold\": 0.5}";

                    HttpContent httpContent = new StringContent(requestjson);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage httpResponseMessage = await client.PostAsync(identifyUri, httpContent);
                    if (httpResponseMessage.StatusCode.ToString() == "OK")
                    {
                        string identifyjson = await httpResponseMessage.Content.ReadAsStringAsync();
                        identifyFaces = await Myjson.ToObjectAsync<List<MyIdentifyFace>>(identifyjson);

                        if (identifyFaces[0].candidates.Count>0)
                        {
                            //树莓派崩了的点                       
                            if (identifyFaces[0].candidates[0].confidence > confidenceThreshold)
                            {
                                DebugText.Text = identifyFaces[0].candidates[0].confidence.ToString();
                                if (FaceDictionary.ContainsKey(identifyFaces[0].candidates[0].personId))
                                {
                                    DebugText.Text = FaceDictionary.ContainsKey(identifyFaces[0].candidates[0].personId).ToString();
                                    PersonsInFaceGroup person = allperson.Find(x => x.personId.Contains(identifyFaces[0].candidates[0].personId));
                                    string words = $"欢迎回来，{person.userData}{FaceDictionary[identifyFaces[0].candidates[0].personId]}";
                                    visitors.Add(new Visitor { VisitDate = System.DateTime.Now.ToString(), VisitorImage = image, Name = FaceDictionary[identifyFaces[0].candidates[0].personId]});

                                    MySpeechHelper.SaySomething(words, myFaceDetectMediaElement);
                                    MyNotificationHelper.SendNotification(file.Path,FaceDictionary[identifyFaces[0].candidates[0].personId],person.userData);

                                }
                            }
                        }
                        else
                        {
                            string words = "对不起！我好像不认识你哦！";
                            visitors.Add(new Visitor { VisitDate = System.DateTime.Now.ToString(), VisitorImage = image, Name ="陌生人"});

                            MySpeechHelper.SaySomething(words, myFaceDetectMediaElement);
                            MyNotificationHelper.SendNotification(file.Path);
                        }
                    }
                }
                else
                {
                    string words = "你是不是逗我玩呢？能看着镜头吗？再试一次，我就不信了！";
                    MySpeechHelper.SaySomething(words, myFaceDetectMediaElement);
                }
            }
            DoorBell.IsEnabled = true;
        }

        private async Task StartPreViewAsync()
        {
            try
            {
                captureManager = new MediaCapture();
                await captureManager.InitializeAsync();

                displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
                
            }
            catch(UnauthorizedAccessException)
            {
                MyMessageDialogHelper.ShowWebcamErrorMessageAsync();
                return;
            }

            try
            {
                myCaptureElement.Source = captureManager;
                await captureManager.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (System.IO.FileLoadException)
            {
                captureManager.CaptureDeviceExclusiveControlStatusChanged +=_mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
            }

        }

        private async void _mediaCapture_CaptureDeviceExclusiveControlStatusChanged(MediaCapture sender, MediaCaptureDeviceExclusiveControlStatusChangedEventArgs args)
        {
            if (args.Status == MediaCaptureDeviceExclusiveControlStatus.SharedReadOnlyAvailable)
            {

                MyMessageDialogHelper.ShowWebcamErrorMessageAsync("摄像头初始化失败，摄像头正在被其他应用使用");
            }
            else if (args.Status == MediaCaptureDeviceExclusiveControlStatus.ExclusiveControlAvailable && !_isPreviewing)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await StartPreViewAsync();
                });
            }
        }

        private async Task CleanupCameraAsync()
        {
            if (captureManager != null)
            {
                if (_isPreviewing)
                {
                    await captureManager.StopPreviewAsync();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    myCaptureElement.Source = null;
                    if (displayRequest != null)
                    {
                        displayRequest.RequestRelease();
                    }

                    captureManager.Dispose();
                    captureManager = null;
                });
            }
        }

        private async Task ListAllPersonInGroup()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",subscriptionKey);
            string listallpersonIngrouprequest = uriFace + "/persongroups/group1/persons";
            var response = await client.GetAsync(listallpersonIngrouprequest);
            string json =await response.Content.ReadAsStringAsync();
            allperson= await Myjson.ToObjectAsync<List<PersonsInFaceGroup>>(json);
            if (allperson.Count>0)
            {
                foreach (var item in allperson)
                {
                    if(!FaceDictionary.ContainsKey(item.personId))
                    {
                        FaceDictionary.Add(item.personId, item.name);
                    }
                }
            }

        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanupCameraAsync();
                deferral.Complete();
            }
        }

        private async void Application_Resuming(object sender, object e)
        {
            await StartPreViewAsync();
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanupCameraAsync();
            base.OnNavigatedFrom(e);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            myFaceDetectMediaElement.AutoPlay = false;
            await StartPreViewAsync();
            await ListAllPersonInGroup();
            base.OnNavigatedTo(e);
            myFaceDetectMediaElement.AutoPlay = true;
        }


    }
}
