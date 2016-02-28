using System;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading.Tasks;

namespace TextToSpeech
{
    public class SpeakIt
    {
        private const string PreferredVoice = "Susan";
        private const int BufferSize = 4096;
        private SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

        public SpeakIt() {
            SetPreferredVoice();
        }

        public static async void ReadText(string mytext) {
            // requires the using Windows.UI.Xaml.Controls namespace...
            var mediaPlayer = new MediaElement();

            using (var speech = new SpeechSynthesizer()) {
                speech.Voice = SpeechSynthesizer.AllVoices.First(voice => voice.Id.Contains(PreferredVoice));
                var stream = await speech.SynthesizeTextToStreamAsync(mytext);
                mediaPlayer.SetSource(stream, stream.ContentType);
                mediaPlayer.Play();
            }
        }
    
        public async void StoreText(string myText) {
            var synthesisStream = await _synthesizer.SynthesizeTextToStreamAsync(myText);
            var sf = await CreateLocalFile($"{Guid.NewGuid()}.wav");
            await SaveSpeechStreamToStorageFile(synthesisStream, sf);
        }

        private static async Task<StorageFile> CreateLocalFile(string fileName) {
            // https://msdn.microsoft.com/en-gb/library/windows/apps/br227251
            var sfo = ApplicationData.Current.LocalFolder;
            var sf = await sfo.CreateFileAsync(fileName); 
            return sf;
        }

        private static async Task SaveSpeechStreamToStorageFile(SpeechSynthesisStream synthesisStream, StorageFile sf) {
            var writeStream = await sf.OpenAsync(FileAccessMode.ReadWrite);
            var outputStream = writeStream.GetOutputStreamAt(0);
            var dataWriter = new DataWriter(outputStream);
            var buffer = new Windows.Storage.Streams.Buffer(BufferSize);

            while (synthesisStream.Position < synthesisStream.Size) {
                await synthesisStream.ReadAsync(buffer, BufferSize, InputStreamOptions.None);
                dataWriter.WriteBuffer(buffer);
            }
            dataWriter.StoreAsync().AsTask().Wait();
            outputStream.FlushAsync().AsTask().Wait();
            outputStream.Dispose();
            writeStream.Dispose();
        }

        private void SetPreferredVoice() {
            _synthesizer.Voice = SpeechSynthesizer.AllVoices.First(voice => voice.Id.Contains(PreferredVoice));
        }
    }
}


