using Sanford.Multimedia.Midi;

namespace MidiReader
{
  public class MidiKeyboardListener
  {
    private InputDevice inputDevice;

    public void StartListening()
    {
      int deviceID = 2; // Идентификатор MIDI-устройства (может потребоваться изменение)
      inputDevice = new InputDevice(deviceID);
      inputDevice.ChannelMessageReceived += InputDevice_ChannelMessageReceived;
      inputDevice.StartRecording();

      Console.WriteLine("Слушаю MIDI-клавиатуру. Нажмите любую клавишу для выхода.");

      // Ожидание нажатия клавиши для выхода
      Console.ReadKey();

      // Остановка и освобождение ресурсов устройства
      inputDevice.StopRecording();
      inputDevice.Close();
    }

    private void InputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
    {
      // Обработка сообщения о нажатии клавиши
      if (e.Message.Command == ChannelCommand.NoteOn)
      {
        int noteNumber = e.Message.Data1;
        int velocity = e.Message.Data2;

        Console.WriteLine($"Нажата клавиша. Номер: {noteNumber}, Скорость: {velocity}");
      }
    }
  }
}
