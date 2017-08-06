using System;

using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Today.Model;
using Today.Services;
using System.Diagnostics;

namespace Today.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemInfoPage : ContentPage
    {
        IBingSpeechService bingSpeechService;
        bool isRecording = false;

        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create("Item", typeof(Item), typeof(ItemInfoPage), null);

        public Item Item
        {
            get { return (Item)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }


        public ItemInfoPage()
        {
            Debug.WriteLine("ItemInfoPage");
            InitializeComponent();
            Debug.WriteLine("ItemInfoPage");
            bingSpeechService = new BingSpeechService(new AuthenticationService(Constants.BingSpeechApiKey), Device.OS.ToString());
        }

        public static readonly BindableProperty IsProcessingProperty =
            BindableProperty.Create("IsProcessing", typeof(bool), typeof(ItemInfoPage), false);

        public bool IsProcessing
        {
            get { return (bool)GetValue(IsProcessingProperty); }
            set { SetValue(IsProcessingProperty, value); }
        }

        async void OnRecognizeSpeechButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var audioRecordingService = DependencyService.Get<IAudioRecorderService>();
                if (!isRecording)
                {
                    audioRecordingService.StartRecording();

                    ((Button)sender).Image = "recording.png";
                    IsProcessing = true;
                }
                else
                {
                    audioRecordingService.StopRecording();
                }

                

                isRecording = !isRecording;
                if (!isRecording)
                {
                    using (var scope = new ActivityIndicatorScope(syncIndicator, true))
                    {
                        Debug.WriteLine("Before Send audio: ");
                        var speechResult = await bingSpeechService.RecognizeSpeechAsync(Constants.AudioFilename);
                        Debug.WriteLine("After Send audio: ");
                        Debug.WriteLine("Name: " + speechResult.Name);
                        Debug.WriteLine("Confidence: " + speechResult.Confidence);

                        if (!string.IsNullOrWhiteSpace(speechResult.Name))
                        {
                            Debug.WriteLine("BindingContext Name: " + Item.Name);
                            Item.Name = char.ToUpper(speechResult.Name[0]) + speechResult.Name.Substring(1);
                            OnPropertyChanged("Item");
                            Debug.WriteLine("BindingContext Name: " + Item.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (!isRecording)
                {
                    ((Button)sender).Image = "record.png";
                    IsProcessing = false;
                }
            }
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            await App.Database.SaveAsync(Item);
            await Navigation.PopAsync();
        }

        async void OnDoneClicked(object sender, EventArgs e)
        {
            await App.Database.DeleteAsync(Item);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}