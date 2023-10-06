//using NAudio.Wave;

////Класс AutoDisposeFileReader используется в классе AudioPlaybackEngine для обеспечения автоматического освобождения ресурсов после окончания воспроизведения звукового файла.
////Класс AutoDisposeFileReader использует библиотеку NAudio для работы с звуковыми данными.

//namespace MidiReader
//{
//  /// <summary>
//  /// Класс AutoDisposeFileReader реализует интерфейс ISampleProvider и предоставляет функциональность для чтения звуковых данных из объекта AudioFileReader и автоматического
//  /// освобождения ресурсов после окончания чтения.
//  /// </summary>
//  class AutoDisposeFileReader : ISampleProvider
//  {
//    private readonly AudioFileReader reader;
//    private bool isDisposed;
//    public WaveFormat WaveFormat { get; private set; }

//    public AutoDisposeFileReader(AudioFileReader reader)
//    {
//      this.reader = reader;
//      this.WaveFormat = reader.WaveFormat;
//    }

//    /// <summary>
//    /// Читает звуковые данные из объекта AudioFileReader и записывает их в указанный буфер.
//    /// Принимает параметры buffer типа float[], offset типа int и count типа int, указывающие на буфер, смещение и количество элементов, которые нужно прочитать.
//    /// Возвращает количество успешно прочитанных элементов.
//    /// </summary>
//    /// <param name="buffer"></param>
//    /// <param name="offset"></param>
//    /// <param name="count"></param>
//    /// <returns></returns>
//    public int Read(float[] buffer, int offset, int count)
//    {
//      if (isDisposed)
//        return 0;

//      int read = reader.Read(buffer, offset, count);

//      if (read == 0)
//      {
//        reader.Dispose();
//        isDisposed = true;
//      }
//      return read;
//    }
//  }
//}
