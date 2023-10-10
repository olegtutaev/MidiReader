//using Sanford.Multimedia.Midi;
using NAudio.Midi;

namespace MidiReader
{
  public class MidiDeviceLister
  {
    private static int deviceNumber;

    public int ChooseDevice()
    {
      var numberOfAllDevices = MidiIn.NumberOfDevices; // Это количество не только подключюченных MIDI-устройств, но и тех, что встроены в Windows. 
      Dictionary<int, string> midiDevices = new Dictionary<int, string>();

      for (int deviceID = 0; deviceID < numberOfAllDevices; deviceID++)
      {
        var deviceName = MidiIn.DeviceInfo(deviceID);
        
        if(deviceName.Manufacturer.ToString() != "Microsoft") // 
        {
          midiDevices.Add(deviceID, deviceName.ProductName);
        }
      }

      if (midiDevices.Count == 0)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Подключите MIDI-устройство. Ожидаю подключения...");
        Console.ResetColor();

        while (midiDevices.Count == 0)
        {
          numberOfAllDevices = MidiIn.NumberOfDevices;

          for(int deviceID = 0; deviceID < numberOfAllDevices; deviceID++)
          {
            var deviceName = MidiIn.DeviceInfo(deviceID);

            if (deviceName.Manufacturer.ToString() != "Microsoft")
              midiDevices.Add(deviceID, deviceName.ProductName);
          }
          
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

      //все
      //for (int deviceID = 0; deviceID < devices; deviceID++)
      //{
      //  var deviceName = MidiIn.DeviceInfo(deviceID);
      //  Console.WriteLine($"ID {deviceID}: {deviceName.ProductName}");
      //}
      //



      Console.WriteLine();
      Console.WriteLine("Введите ID MIDI-устройства");
      bool success = false;
      while (success == false)
      {
        string userInput = Console.ReadLine();
        success = int.TryParse(userInput, out deviceNumber) && midiDevices.ContainsKey(deviceNumber);

        if (success)
        {
          string deviceName =  MidiIn.DeviceInfo(deviceNumber).ProductName;
          Console.WriteLine($"Подключено MIDI-устройство {deviceNumber}: {deviceName}");
        }
        else
        {
          Console.WriteLine("Некорректный ввод. Введите номер одного из предложенных устройств.");
        }
      }

      return deviceNumber;
    }
  }
}
