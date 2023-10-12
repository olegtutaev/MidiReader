using NAudio.Midi;

namespace MidiReader
{
  public class MidiPlayer
  {
    private string folder;
    private Dictionary<int, string> noteMappings;
    private MidiIn midiIn;

    public MidiPlayer(Dictionary<int, string> noteMappings, string folder, int deviceNumber)
    {
      this.noteMappings = noteMappings;
      midiIn = new MidiIn(deviceNumber);
      midiIn.MessageReceived += PlaySound;
      this.folder = folder;
    }

    public void Start()
    {
      midiIn.Start();
      Console.ReadKey();
    }

    private async void PlaySound(object sender, MidiInMessageEventArgs e) 
    {
      var midiMessage = e.MidiEvent;
      if (midiMessage.CommandCode == MidiCommandCode.NoteOn)
      {
        var noteOnEvent = (NoteOnEvent)midiMessage;
        var noteNumber = noteOnEvent.NoteNumber;

        if (noteMappings.ContainsKey(noteNumber))
        {

          folder = TelegramBot.SetFolder();
          if (folder != null)
          {
            
            var fileName = Path.Combine(folder, noteMappings[noteNumber]);
            AudioPlaybackEngine.Instance.PlaySound(fileName);
          }
          else
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Загрузка голоса в сэмплер. Пожалуйста, подождите...");
            Console.ResetColor();
          }
        }
      }
    }
  }
}

#region DontUse
//public void PlayWavFile(string fileName)
//{
//  AudioPlaybackEngine.Instance.PlaySound(fileName);

//  //AudioPlaybackEngine.Instance.Dispose();


//  //Console.WriteLine("Зашёл в PlayWavFile");
//  //using (var stream = new FileStream(fileName, FileMode.Open))
//  //{
//  //  //var sound = new CachedSound(stream);
//  //  AudioPlaybackEngine.Instance.PlaySound(sound);
//  //}
//}
#endregion
