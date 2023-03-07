using System.IO;
using System.Speech.Synthesis;

namespace TelegramBot.Classes.VoiceControllers
{
    public class MakeVoiceController
    {
        public static void SaveVoice(string text, string voiceName, string filePath)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                string.IsNullOrWhiteSpace(voiceName) ||
                string.IsNullOrWhiteSpace(filePath)) return;

            var synth = new SpeechSynthesizer();
            synth.SetOutputToWaveFile(filePath);

            synth.SelectVoice(voiceName);
            synth.Speak(text);
        }

        public static void SaveVoice(string text, string voiceName, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                string.IsNullOrWhiteSpace(voiceName) ||
                stream == null) return;

            var synth = new SpeechSynthesizer();
            synth.SetOutputToWaveStream(stream);

            synth.SelectVoice(voiceName);
            synth.Speak(text);
        }

        public static void PlayVoice(string text, string voiceName)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                string.IsNullOrWhiteSpace(voiceName)) return;

            var synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            synth.SelectVoice(voiceName);
            synth.Speak(text);
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<InstalledVoice> GetAllVoices() =>
            new SpeechSynthesizer().GetInstalledVoices();

        public static InstalledVoice FindVoiceByID(string ID)
        {
            var voices = GetAllVoices();
            foreach (var voice in voices)
                if (voice.VoiceInfo.Id == ID)
                    return voice;
            return null;
        }
    }
}
