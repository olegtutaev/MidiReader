using NAudio.Midi;

namespace MidiReader
{
  /// <summary>
  /// Функционал для получения информации о подключенных MIDI-устройствах.
  /// </summary>
  internal sealed class MidiDeviceLister
  {
    #region Константы
    private const string microsoft = "Microsoft";
    #endregion

    #region Поля
    private static int deviceId;
    #endregion

    #region Методы
    /// <summary>
    /// Возвращает идентификатор подключенного MIDI-устройства.
    /// </summary>
    /// <returns>Идентификатор подключенного MIDI-устройства.</returns>
    public int GetMidiDeviceId()
    {
      var numberOfAllDevices = MidiIn.NumberOfDevices;
      var midiDevices = new Dictionary<int, string>();
      this.AddMidiDevices(numberOfAllDevices, midiDevices);

      if (midiDevices.Count == 0)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Подключите MIDI-устройство. Ожидаю подключения...");
        Console.ResetColor();

        while (midiDevices.Count == 0)
        {
          numberOfAllDevices = MidiIn.NumberOfDevices;
          this.AddMidiDevices(numberOfAllDevices, midiDevices);
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Появилось!");
        Console.ResetColor();
      }

      if (midiDevices.Count == 1)
      {
        Console.WriteLine($"Подключено MIDI-устройство {midiDevices.First().Value}");
        Console.WriteLine();

        return midiDevices.First().Key;
      }
      Console.WriteLine("Список доступных MIDI-устройств:");
      Console.WriteLine();

      foreach (var device in midiDevices)
        Console.WriteLine($"ID {device.Key}: {device.Value}");

      Console.WriteLine();
      Console.WriteLine("Введите ID MIDI-устройства");
      var isCorrectInput = false;

      while (isCorrectInput == false)
      {
        var userInput = Console.ReadLine();
        isCorrectInput = int.TryParse(userInput, out deviceId) && midiDevices.ContainsKey(deviceId);

        if (isCorrectInput)
        {
          string deviceName = MidiIn.DeviceInfo(deviceId).ProductName;
          Console.WriteLine($"Подключено MIDI-устройство {deviceName}");
          Console.WriteLine();
        }
        else
        {
          Console.WriteLine("Некорректный ввод. Введите ID одного из предложенных устройств.");
        }
      }
      return deviceId;
    }

    private void AddMidiDevices(int numberOfAllDevices, Dictionary<int, string> midiDevices)
    {
      for (deviceId = 0; deviceId < numberOfAllDevices; deviceId++)
      {
        var deviceName = MidiIn.DeviceInfo(deviceId);

        if (deviceName.Manufacturer.ToString() != microsoft)
          midiDevices.Add(deviceId, deviceName.ProductName);
      }
    }
    #endregion
  }
}
