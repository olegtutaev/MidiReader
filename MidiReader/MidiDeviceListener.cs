using NAudio.Midi;

namespace MidiReader
{
  public class MidiDeviceLister
  {
    private static int deviceId;

    public int GetMidiDevice()
    {
      var numberOfAllDevices = MidiIn.NumberOfDevices; // Это количество не только подключюченных MIDI-устройств, но и тех, что встроены в Windows. 
      Dictionary<int, string> midiDevices = new Dictionary<int, string>();

      AddMidiDevices(numberOfAllDevices, midiDevices);

      if (midiDevices.Count == 0)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Подключите MIDI-устройство. Ожидаю подключения...");
        Console.ResetColor();

        while (midiDevices.Count == 0)
        {
          numberOfAllDevices = MidiIn.NumberOfDevices;  // Проверка подключения.
          AddMidiDevices(numberOfAllDevices, midiDevices);
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Появилось!");
        Console.ResetColor();
      }

      if (midiDevices.Count == 1)
      {
        Console.WriteLine($"Подключено MIDI-устройство {midiDevices.First().Value}");

        return midiDevices.First().Key;
      }

      Console.WriteLine("Список доступных MIDI-устройств:");
      Console.WriteLine();

      foreach(var device in midiDevices)
      {
        Console.WriteLine($"ID {device.Key}: {device.Value}");
      }

      Console.WriteLine();
      Console.WriteLine("Введите ID MIDI-устройства");

      bool success = false;
      while (success == false)
      {
        var userInput = Console.ReadLine();
        success = int.TryParse(userInput, out deviceId) && midiDevices.ContainsKey(deviceId);

        if (success)
        {
          string deviceName =  MidiIn.DeviceInfo(deviceId).ProductName;
          Console.WriteLine($"Подключено MIDI-устройство {deviceName}");
        }
        else
        {
          Console.WriteLine("Некорректный ввод. Введите ID одного из предложенных устройств.");
        }
      }

      return deviceId;
    }

    private static void AddMidiDevices(int numberOfAllDevices, Dictionary<int, string> midiDevices)
    {
      for (deviceId = 0; deviceId < numberOfAllDevices; deviceId++)
      {
        var deviceName = MidiIn.DeviceInfo(deviceId);

        if (deviceName.Manufacturer.ToString() != "Microsoft")
          midiDevices.Add(deviceId, deviceName.ProductName);
      }
    }
  }
}

#region DontUse
//все
//for (int deviceID = 0; deviceID < devices; deviceID++)
//{
//  var deviceName = MidiIn.DeviceInfo(deviceID);
//  Console.WriteLine($"ID {deviceID}: {deviceName.ProductName}");
//}
//
#endregion
