using System;
using System.Windows.Input;
using System.ComponentModel;
using RejestratorGlosu.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace RejestratorGlosu.ViewModels
{
    /// <summary>
    /// Główny model widoku aplikacji.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AudioRecorder recorder;
        private AudioPlayer player;

        /// <summary>
        /// Kolekcja punktów reprezentujących dane audio.
        /// </summary>
        public ObservableCollection<Point> AudioDataPoints { get; } = new ObservableCollection<Point>();

        /// <summary>
        /// Komenda odpowiedzialna za rozpoczęcie nagrywania dźwięku.
        /// </summary>
        public ICommand StartRecordingCommand { get; }

        /// <summary>
        /// Komenda odpowiedzialna za zatrzymanie nagrywania dźwięku.
        /// </summary>
        public ICommand StopRecordingCommand { get; }

        /// <summary>
        /// Komenda odpowiedzialna za odtwarzanie nagranego dźwięku.
        /// </summary>
        public ICommand PlayCommand { get; }

        /// <summary>
        /// Komenda odpowiedzialna za zatrzymanie odtwarzania dźwięku.
        /// </summary>
        public ICommand StopPlayingCommand { get; }

        /// <summary>
        /// Komenda odpowiedzialna za zapisanie nagranego dźwięku do pliku.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Komenda odpowiedzialna za wyczyszczenie aktualnego nagrania.
        /// </summary>
        public ICommand ClearCommand { get; }


        /// <summary>
        /// Reprezentuje aktualny poziom głośności dźwięku.
        /// </summary>
        private float _volumeLevel;
        public float VolumeLevel
        {
            get { return _volumeLevel; }
            set
            {
                _volumeLevel = value;
                OnPropertyChanged(nameof(VolumeLevel));
            }
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy MainViewModel.
        /// </summary>
        public MainViewModel()
        {
            recorder = new AudioRecorder();
            recorder.VolumeUpdated += OnVolumeUpdated;
            recorder.ErrorOccurred += OnErrorOccurred;

            StartRecordingCommand = new RelayCommand(StartRecording);
            StopRecordingCommand = new RelayCommand(StopRecording);
            PlayCommand = new RelayCommand(Play);
            StopPlayingCommand = new RelayCommand(StopPlaying);
            SaveCommand = new RelayCommand(Save);
            ClearCommand = new RelayCommand(Clear);
        }

        /// <summary>
        /// Rozpoczyna proces nagrywania dźwięku.
        /// </summary>
        private void StartRecording()
        {
            StopPlaying();
            try
            {
                Clear();  
                recorder.StartRecording();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas nagrywania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zatrzymuje proces nagrywania dźwięku.
        /// </summary>
        private void StopRecording()
        {
            StopPlaying();
            try
            {
                recorder.StopRecording();
                var stream = recorder.GetRecordingStream();
                player = new AudioPlayer(stream);
                player.ErrorOccurred += OnErrorOccurred;  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zatrzymywania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odtwarza nagrany dźwięk.
        /// </summary>
        private void Play()
        {
            StopPlaying();
            try
            {
                player?.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odtwarzania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zatrzymuje odtwarzanie dźwięku.
        /// </summary>
        private void StopPlaying()
        {
            try
            {
                player?.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zatrzymywania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        /// <summary>
        /// Zapisuje nagrany dźwięk do pliku.
        /// </summary>
        private void Save()
        {
            StopPlaying();
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Wave Files (*.wav)|*.wav",
                    DefaultExt = ".wav"
                };

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string filename = saveFileDialog.FileName;
                    var audioData = recorder.GetRecordingStream();
                    if (audioData != null)
                    {
                        using (var fileStream = System.IO.File.Create(filename))
                        {
                            audioData.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Audio data is null.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Czyści aktualne nagranie i resetuje wszystkie parametry.
        /// </summary>
        private void Clear()
        {
            StopPlaying();
            try
            {
                if (player != null)
                {
                    player.ErrorOccurred -= OnErrorOccurred; 
                }
                recorder.Reset();
                player = null;
                AudioDataPoints.Clear();
                VolumeLevel = 0;  
                OnPropertyChanged(nameof(VolumeLevel));
                OnPropertyChanged(nameof(AudioDataPoints));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas czyszczenia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Obsługuje zdarzenie aktualizacji poziomu głośności.
        /// </summary>
        private void OnVolumeUpdated(float volume)
        {
            VolumeLevel = volume * 1000; 

            AudioDataPoints.Add(new Point(AudioDataPoints.Count, VolumeLevel));

            OnPropertyChanged(nameof(AudioDataPoints));
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Obsługuje zdarzenie wystąpienia błędu.
        /// </summary>
        private void OnErrorOccurred(Exception ex)
        {
            MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }

    /// <summary>
    /// Klasa reprezentująca komendę.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}


