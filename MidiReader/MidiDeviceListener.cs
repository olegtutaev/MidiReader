using Sanford.Multimedia.Midi;

namespace MidiReader
{
  public class MidiDeviceLister
  {
    private static int deviceNumber;

    public void ListDevices()
    {
      int deviceCount = InputDevice.DeviceCount;

      if (deviceCount == 0)
      {
        Console.WriteLine("Нет доступных MIDI-устройств.");

        return;
      }

      Console.WriteLine("Список доступных MIDI-устройств:");

      for (int deviceID = 0; deviceID < deviceCount; deviceID++)
      {
        string deviceName = InputDevice.GetDeviceCapabilities(deviceID).name;
        Console.WriteLine($"Устройство {deviceID}: {deviceName}");
      }
    }

    public int ChooseDevice()
    {
      Console.WriteLine();
      Console.WriteLine("Введите номер MIDI-устройства");
      bool success = false;
      while (success == false)
      {
        string userInput = Console.ReadLine();
        success = int.TryParse(userInput, out deviceNumber) && (deviceNumber <= (InputDevice.DeviceCount - 1)) && (deviceNumber >= 0);

        if (success)
        {
          string deviceName = InputDevice.GetDeviceCapabilities(deviceNumber).name;
          Console.WriteLine($"Выборано устройство {deviceNumber}: {deviceName}");
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
