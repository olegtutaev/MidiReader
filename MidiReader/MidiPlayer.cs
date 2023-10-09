using NAudio.Midi;

namespace MidiReader
{
  #region Synth
  public class MidiPlayer
  {
    private string folder;
    private Dictionary<int, string> noteMappings;
    private MidiIn midiIn;

    public MidiPlayer(Dictionary<int, string> noteMappings, string folder, int deviceNumber)
    {
      this.noteMappings = noteMappings;
      this.midiIn = new MidiIn(deviceNumber);
      this.midiIn.MessageReceived += MidiIn_MessageReceived;
      this.folder = folder;
    }

    public void Start()
    {
      this.midiIn.Start();
      //Console.WriteLine("Нажмите любую клавишу для остановки.");
      Console.ReadKey();
    }

    private async void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e) 
    {

      var midiMessage = e.MidiEvent;
      if (midiMessage.CommandCode == MidiCommandCode.NoteOn)
      {
        var noteOnEvent = (NoteOnEvent)midiMessage;
        int noteNumber = noteOnEvent.NoteNumber;
        if (noteMappings.ContainsKey(noteNumber))
        {

          folder = TelegramBot.SetFolder();

          string fileName = Path.Combine(folder, noteMappings[noteNumber]);
          Console.WriteLine(fileName);
          PlayWavFile(fileName);
        }
      }
    }
    
    public void PlayWavFile(string fileName)
    {
      AudioPlaybackEngine.Instance.PlaySound(fileName);
      //Console.WriteLine("Зашёл в PlayWavFile");
      //using (var stream = new FileStream(fileName, FileMode.Open))
      //{
      //  var sound = new CachedSound(stream);
      //  AudioPlaybackEngine.Instance.PlaySound(sound);
      //}
    }
  }
  #endregion
}
