using Sanford.Multimedia.Midi;

namespace MidiReader
{
  public class MidiDeviceLister
  {
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
  }
}
