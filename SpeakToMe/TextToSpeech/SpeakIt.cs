using System;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;
using System.Linq;

namespace TextToSpeech
{
    public static class SpeakIt
    {
        private const string PreferredVoice = "Susan";

        public static async void ReadText(string mytext) {
            MediaElement mediaPlayer = new MediaElement();

            using (var speech = new SpeechSynthesizer()) {
                speech.Voice = SpeechSynthesizer.AllVoices.First(voice => voice.Id.Contains(PreferredVoice));

                var x = SpeechSynthesizer.AllVoices.ToList();
                foreach (var item in x) {
                    var x1 = item.Description;
                    var x2 = item.DisplayName;
                    var x3 = item.Gender;
                    var x4 = item.Id;
                    var x5 = item.Language;
                }

                SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(mytext);
                mediaPlayer.SetSource(stream, stream.ContentType);
                mediaPlayer.Play();
            }
        }
    }
}


