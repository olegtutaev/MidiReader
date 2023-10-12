namespace MidiReader
{
  public class Program
  {
    private static string folder;
    private static int deviceNumber;

    public static void Main()
    {
      #region Experiments     
      //var a = Math.Pow(2, 6/12);
      //Console.WriteLine(a); 
      #endregion Experiments

      AudioPlaybackEngine.Instance.PlaySound("Empty.wav");  // Чтобы в начале игры не было щелчка, включаем микшер отправкой WAV-файла, содержащего тишину.

      var bot = new TelegramBot(BotToken.Token);
      bot.Start();

      MidiDeviceLister deviceLister = new MidiDeviceLister();
      deviceNumber = deviceLister.GetMidiDevice();

      // чекнуть нажатия
      //MidiKeyboardListener keyboardListener = new MidiKeyboardListener();
      //keyboardListener.StartListening();

      MidiPlayer midiPlayer = new MidiPlayer(NoteMap.Notes, folder, deviceNumber);
      midiPlayer.Start();

      Console.ReadLine();
    }
  }
}

