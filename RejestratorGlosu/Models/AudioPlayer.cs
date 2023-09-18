using System;
using System.IO;
using NAudio.Wave;

namespace RejestratorGlosu.Models
{
    /// <summary>
    /// Klasa odpowiedzialna za odtwarzanie dźwięku.
    /// </summary>
    public class AudioPlayer
    {
        private WaveOutEvent waveOut;
        private WaveStream waveStream;

        /// <summary>
        /// Zdarzenie zgłaszane, gdy wystąpi błąd.
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        /// <summary>
        /// Inicjalizuje nową instancję klasy AudioPlayer.
        /// </summary>
        public AudioPlayer(Stream audioStream)
        {
            try
            {
                waveStream = new WaveFileReader(audioStream);
                waveOut = new WaveOutEvent();
                waveOut.Init(waveStream);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Rozpoczyna odtwarzanie dźwięku.
        /// </summary>
        public void Play()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.Play();
                }
                else
                {
                    throw new InvalidOperationException("WaveOut is null.");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }

        /// <summary>
        /// Zatrzymuje odtwarzanie dźwięku.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                }
                else
                {
                    throw new InvalidOperationException("WaveOut is null.");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex);
            }
        }
    }
}




