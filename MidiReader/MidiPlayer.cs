using NAudio.Midi;

namespace MidiReader
{
  /// <summary>
  /// Класс MidiPlayer предоставляет функционал для воспроизведения MIDI-событий.
  /// </summary>
  internal sealed class MidiPlayer
  {
    #region Поля
    private string folder;
    private Dictionary<int, string> noteMappings;
    private MidiIn midiIn;
    #endregion

    #region Метод
    /// <summary>
    /// Запускает воспроизведение MIDI-событий.
    /// </summary>
    public void Start()
    {
      midiIn.Start();
      Console.ReadKey();
    }
    #endregion

    #region События
    private void MidiInMessageReceivedHandler(object sender, MidiInMessageEventArgs e)
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
    #endregion

    #region Констуктор
    /// <summary>
    /// Инициализирует новый экземпляр класса MidiPlayer с указанными настройками.
    /// </summary>
    /// <param name="noteMappings">Словарь, содержащий соответствие между номерами нот и именами файлов звуков.</param>
    /// <param name="folder">Путь к папке, содержащей звуковые файлы.</param>
    /// <param name="deviceId">Идентификатор MIDI-устройства.</param>
    public MidiPlayer(Dictionary<int, string> noteMappings, string folder, int deviceId)
    {
      this.noteMappings = noteMappings;
      this.midiIn = new MidiIn(deviceId);
      this.midiIn.MessageReceived += this.MidiInMessageReceivedHandler;
      this.folder = folder;
    }
    #endregion
  }
}
