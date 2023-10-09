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

      AudioPlaybackEngine.Instance.PlaySound("Empty.wav");

      var bot = new TelegramBot(BotToken.Token);

      bot.Start();

      MidiDeviceLister deviceLister = new MidiDeviceLister();
      deviceLister.ListDevices();
      deviceNumber = deviceLister.ChooseDevice();

      // чекнуть нажатия
      //MidiKeyboardListener keyboardListener = new MidiKeyboardListener();
      //keyboardListener.StartListening();

      MidiPlayer midiPlayer = new MidiPlayer(NoteMap.Notes, folder, deviceNumber);
      midiPlayer.Start();

      Console.ReadLine();
    }
  }
}

