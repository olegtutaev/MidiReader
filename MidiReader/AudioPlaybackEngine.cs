using NAudio.Wave;
using NAudio.Wave.SampleProviders;

// Класс AudioPlaybackEngine использует библиотеку NAudio для воспроизведения звуковых файлов.
//Воспроизводимые звуковые файлы могут быть в формате WAV или MP3.
//Поддерживается воспроизведение звука с различными частотами дискретизации и количеством каналов.

namespace MidiReader
{
  /// <summary>
  /// Класс AudioPlaybackEngine предоставляет функциональность для воспроизведения звуковых файлов.
  /// </summary>
  class AudioPlaybackEngine : IDisposable
  {
    private readonly MixingSampleProvider mixer;
    WaveOutEvent outputDevice = new WaveOutEvent();
    public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);

    public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
    {     
      outputDevice.DesiredLatency = 50;
      outputDevice.NumberOfBuffers = 4;
      mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
      mixer.ReadFully = true;
      outputDevice.Init(mixer);
      outputDevice.Play();
    }

    /// <summary>
    /// Воспроизводит звуковой файл с указанным именем и принимает строковый параметр fileName, содержащий путь к звуковому файлу.
    /// </summary>
    /// <param name="fileName"></param>
    public void PlaySound(string fileName)
    {
      var input = new AudioFileReader(fileName);
      AddMixerInput(new AutoDisposeFileReader(input));
    }

    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
    {
      if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
      {
        return input;
      }
      if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
      {
        return new MonoToStereoSampleProvider(input);
      }
      throw new NotImplementedException("Еще не реализовано это преобразование количества каналов");
    }

    /// <summary>
    /// Воспроизводит звуковой файл из объекта CachedSound. Принимает параметр sound типа CachedSound, содержащий звуковые данные.
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(CachedSound sound)
    {
      AddMixerInput(new CachedSoundSampleProvider(sound));
    }

    private void AddMixerInput(ISampleProvider input)
    {
      mixer.AddMixerInput(ConvertToRightChannelCount(input));
    }

    /// <summary>
    /// Освобождает ресурсы, используемые объектом AudioPlaybackEngine.
    /// </summary>
    public void Dispose()
    {
      outputDevice.Dispose();
    }

    //public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
  }
}
