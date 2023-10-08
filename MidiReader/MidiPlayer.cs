
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using MidiReader;
using NAudio.Midi;
using System.Media;

namespace MidiReader
{
  #region Synth
  public class MidiPlayer
  {
    private string folder;
    private Dictionary<int, string> noteMappings;
    private MidiIn midiIn;
    //private WaveOutEvent player;
    //private float volume = 1.0f;
    private Dictionary<int, WaveOutEvent> players;
    private Dictionary<int, bool> keyStates; // для зашиты от быстрого нажатия
    private TaskCompletionSource<bool> playbackCompletionSource;
    private int deviceNumber;

    public void SetMessageText(string text)
    {
      folder = text; // Установить значение messageText
      //this.keyStates = new Dictionary<int, bool>();
    }

    public MidiPlayer(Dictionary<int, string> noteMappings, string folder, int deviceNumber)
    {
      this.noteMappings = noteMappings;
      this.midiIn = new MidiIn(deviceNumber);
      this.midiIn.MessageReceived += MidiIn_MessageReceived;
      this.folder = folder;
      this.players = new Dictionary<int, WaveOutEvent>();
      //this.player = new WaveOutEvent(); 
      this.keyStates = new Dictionary<int, bool>();
      this.playbackCompletionSource = new TaskCompletionSource<bool>();
    }

    //public MidiPlayer(string text)
    //{
    //  folder = text;
    //  this.players = new Dictionary<int, WaveOutEvent>();
    //}

    public void Start()
    {
      this.midiIn.Start();
      //Console.WriteLine("Нажмите любую клавишу для остановки.");
      Console.ReadKey();

      //this.midiIn.Stop();
      //foreach (var player in players.Values)
      //{
      //  player.Stop();
      //  player.Dispose();
      //}
      //players.Clear();
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
          PlayWavFileAsync(fileName);
          //PlayWavFileAsync(fileName);
        }
      }
    }

    //private async void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
    //{
    //  var midiMessage = e.MidiEvent;
    //  if (midiMessage.CommandCode == MidiCommandCode.NoteOn)
    //  {
    //    var noteOnEvent = (NoteOnEvent)midiMessage;
    //    int noteNumber = noteOnEvent.NoteNumber;
    //    if (noteMappings.ContainsKey(noteNumber))
    //    {
    //      folder = TelegramBot.SetFolder();
    //      string fileName = Path.Combine(folder, noteMappings[noteNumber]);
    //      Console.WriteLine(fileName);
    //      if (!keyStates.ContainsKey(noteNumber) || !keyStates[noteNumber])
    //      {
    //        keyStates[noteNumber] = true;
    //        await PlayWavFileAsync(fileName);
    //        keyStates[noteNumber] = false;
    //      }
    //    }
    //  }
    //}

    //public void StopWavFile(int noteNumber)
    //{
    //  if (players.ContainsKey(noteNumber))
    //  {
    //    var player = players[noteNumber];
    //    player.Stop();
    //    player.Dispose();
    //    players.Remove(noteNumber);
    //  }
    //}

    //public async Task PlayWavFileAsync(string fileName)
    //{
    //  Console.WriteLine("Зашёл в PlayWavFile");
    //  await Task.Run(async () =>
    //  {
    //    using (var audioFile = new AudioFileReader(fileName))
    //    {
    //      var player = new WaveOutEvent();
    //      player.Init(audioFile);
    //      try
    //      {
    //        Console.ForegroundColor = ConsoleColor.Green;
    //        Console.WriteLine("Зашёл в try");
    //        player.Play();
    //        Console.ResetColor();
    //      }
    //      catch
    //      {
    //        Console.ForegroundColor = ConsoleColor.Red;
    //        Console.WriteLine(new InvalidOperationException("Must call Init first"));
    //        Console.ResetColor();
    //      }
    //      await Task.Delay(4000);
    //    }
    //  });
    //}

    //public void PlayWavFileAsync(string fileName)
    //{
    //  Console.WriteLine("Зашёл в PlayWavFile");
    //  var sound = new CachedSound(fileName);
    //  AudioPlaybackEngine.Instance.PlaySound(fileName);
    //  //AudioPlaybackEngine.Instance.Dispose();
    //}

    public void PlayWavFileAsync(string fileName)
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
