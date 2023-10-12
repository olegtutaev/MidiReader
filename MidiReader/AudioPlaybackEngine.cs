using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MidiReader
{
  /// <summary>
  /// Класс AudioPlaybackEngine представляет движок воспроизведения аудио.
  /// </summary>
  internal sealed class AudioPlaybackEngine
  {
    private readonly MixingSampleProvider mixer;
    WaveOutEvent outputDevice = new WaveOutEvent();
    private const int sampleRate = 44100;
    private const int channelCount = 2;
    public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(sampleRate, channelCount);
    private const int latency = 50;
    private const int numberOfBuffers = 4;

    /// <summary>
    /// Создает новый экземпляр класса AudioPlaybackEngine с заданными параметрами.
    /// </summary>
    /// <param name="sampleRate">Частота дискретизации аудио.</param>
    /// <param name="channelCount">Количество каналов аудио.</param>
    public AudioPlaybackEngine(int sampleRate, int channelCount)
    {     
      this.outputDevice.DesiredLatency = latency;
      this.outputDevice.NumberOfBuffers = numberOfBuffers;
      this.mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
      this.mixer.ReadFully = true;
      this.outputDevice.Init(mixer);
      this.outputDevice.Play();
    }

    /// <summary>
    /// Воспроизводит звуковой файл.
    /// </summary>
    /// <param name="fileName">Путь к звуковому файлу.</param>
    public void PlaySound(string fileName)
    {
      ISampleProvider input = new AudioFileReader(fileName);
      mixer.AddMixerInput(input);
    }
  }
}