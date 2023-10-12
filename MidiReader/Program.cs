namespace MidiReader
{
  internal sealed class Program
  {
    private static string folder;
    private static int deviceId;
    private const string emptyWav = "Empty.wav";

    public static void Main()
    {
      AudioPlaybackEngine.Instance.PlaySound(emptyWav);  // Чтобы в начале игры не было щелчка, включаем микшер отправкой WAV-файла, содержащего тишину.
      var bot = new TelegramBot(BotToken.Token);
      bot.Start();
      var deviceLister = new MidiDeviceLister();
      deviceId = deviceLister.GetMidiDeviceId();
      var midiPlayer = new MidiPlayer(NoteMap.Notes, folder, deviceId);
      midiPlayer.Start();
    }
  }
}