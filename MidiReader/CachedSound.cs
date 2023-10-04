using NAudio.Wave;

//Класс CachedSound используется в классе AudioPlaybackEngine для предварительной загрузки и кэширования звуковых данных.
//Класс CachedSound использует библиотеку NAudio для работы с звуковыми данными.
//При создании объекта CachedSound, звуковые данные из указанного потока звукового файла загружаются в память и сохраняются в массиве типа float для последующего использования.

namespace MidiReader
{
  class CachedSound
  {
    /// <summary>
    /// Класс CachedSound представляет звуковые данные, которые были предварительно загружены в память для последующего использования.
    /// </summary>
    public float[] AudioData { get; private set; }
    public WaveFormat WaveFormat { get; private set; }

    public CachedSound(Stream sound)
    {
      using (var audioFileReader = new WaveFileReader(sound))
      {
        WaveFormat = audioFileReader.WaveFormat;
        var sp = audioFileReader.ToSampleProvider();
        var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
        var sourceSamples = (int)(audioFileReader.Length / (audioFileReader.WaveFormat.BitsPerSample / 8));
        var sampleData = new float[sourceSamples];
        int samplesread;

        while ((samplesread = sp.Read(sampleData, 0, sourceSamples)) > 0)
        {
          wholeFile.AddRange(sampleData.Take(samplesread));
        }
        AudioData = wholeFile.ToArray();
      }
    }
  }
}
