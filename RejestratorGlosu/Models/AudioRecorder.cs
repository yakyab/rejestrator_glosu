using NAudio.Wave;
using System;
using System.IO;

namespace RejestratorGlosu.Models
{
    /// <summary>
    /// Klasa odpowiedzialna za nagrywanie dźwięku.
    /// </summary>
    public class AudioRecorder
    {
        private WaveInEvent waveIn;
        private MemoryStream memoryStream;
        private WaveFileWriter writer;

        /// <summary>
        /// Zdarzenie zgłaszane, gdy poziom głośności zostanie zaktualizowany.
        /// </summary>
        public event Action<float> VolumeUpdated;

        /// <summary>
        /// Zdarzenie zgłaszane, gdy wystąpi błąd.
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        /// <summary>
        /// Inicjalizuje nową instancję klasy AudioRecorder.
        /// </summary>
        public AudioRecorder()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Inicjalizuje nagrywanie dźwięku.
        /// </summary>
        private void Initialize()
        {
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            memoryStream = new MemoryStream();
            writer = new WaveFileWriter(memoryStream, waveIn.WaveFormat);
            waveIn.DataAvailable += OnDataAvailable;
        }

        /// <summary>
        /// Obsługuje zdarzenie dostępności danych.
        /// </summary>
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (writer != null)
                {
                    writer.Write(e.Buffer, 0, e.BytesRecorded);
                    writer.Flush();
                }
                else
                {
                    throw new InvalidOperationException("Writer is null.");
                }

                float volume = 0;
                for (int index = 0; index < e.BytesRecorded; index += 2)
                {
                    short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index]);
                    float sample32 = sample / 32768.0f;
                    volume += Math.Abs(sample32);
                }
                volume /= e.BytesRecorded / 2;

                VolumeUpdated?.Invoke(volume);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Rozpoczyna nagrywanie dźwięku.
        /// </summary>
        public void StartRecording()
        {
            try
            {
                if (waveIn == null || writer == null)
                {
                    memoryStream = new MemoryStream();
                    Initialize();
                }
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Zatrzymuje nagrywanie dźwięku.
        /// </summary>
        public void StopRecording()
        {
            try
            {
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                }

                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Pobiera strumień z nagraniem.
        /// </summary>
        public Stream GetRecordingStream()
        {
            try
            {
                if (memoryStream != null)
                {
                    return new MemoryStream(memoryStream.ToArray());
                }
                else
                {
                    throw new InvalidOperationException("MemoryStream is null.");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
                return null;
            }
        }

        /// <summary>
        /// Resetuje nagrywanie dźwięku.
        /// </summary>
        public void Reset()
        {
            try
            {
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                    waveIn.Dispose();
                    waveIn = null;
                }

                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }
    }
}






