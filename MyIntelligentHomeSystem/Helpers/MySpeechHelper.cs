using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace MyIntelligentHomeSystem.Helpers
{

    public  class MySpeechHelper
    {
        public static async void SaySomething(string TTS,MediaElement mediaElement)
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            //speechSynthesizer.Voice = SpeechSynthesizer.DefaultVoice;

            speechSynthesizer.Voice = SpeechSynthesizer.AllVoices[3]; //0-8,可以从设置里看到
            SpeechSynthesisStream speechSynthesisStream = await speechSynthesizer.SynthesizeTextToStreamAsync(TTS);
            await mediaElement.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                mediaElement.SetSource(speechSynthesisStream, speechSynthesisStream.ContentType);
                mediaElement.Play();
            }));
            speechSynthesisStream = null;
        }
    }
}
