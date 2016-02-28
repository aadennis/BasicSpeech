using Windows.UI.Xaml.Controls;
using TextToSpeech;

namespace App1 {

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var si = new SpeakIt();
            var textToSpeak = " I have a high respect for your nerves";
            SpeakIt.ReadText(textToSpeak);
            si.StoreText(textToSpeak);
        }
    }
}
