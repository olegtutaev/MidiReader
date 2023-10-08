//using NAudio.Wave;

////Класс CachedSoundSampleProvider используется в классе AudioPlaybackEngine для предоставления звуковых данных для воспроизведения.
////Класс CachedSoundSampleProvider использует библиотеку NAudio для работы с звуковыми данными.

//namespace MidiReader
//{
//  /// <summary>
//  /// Класс CachedSoundSampleProvider реализует интерфейс ISampleProvider и предоставляет функциональность для чтения звуковых данных из объекта CachedSound.
//  /// </summary>
//  class CachedSoundSampleProvider : ISampleProvider
//  {
//    private readonly CachedSound cachedSound;
//    private long position;
//    public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }

//    public CachedSoundSampleProvider(CachedSound cachedSound)
//    {
//      this.cachedSound = cachedSound;
//    }

//    /// <summary>
//    /// Читает звуковые данные из объекта CachedSound и записывает их в указанный буфер.
//    /// Принимает параметры buffer типа float[], offset типа int и count типа int, указывающие на буфер, смещение и количество элементов, которые нужно прочитать.
//    /// Возвращает количество успешно прочитанных элементов.
//    /// </summary>
//    /// <param name="buffer"></param>
//    /// <param name="offset"></param>
//    /// <param name="count"></param>
//    /// <returns></returns>
//    public int Read(float[] buffer, int offset, int count)
//    {
//      var availableSamples = cachedSound.AudioData.Length - position;
//      var samplesToCopy = Math.Min(availableSamples, count);
//      Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
//      position += samplesToCopy;

//      return (int)samplesToCopy;
//    }
//  }
//}
