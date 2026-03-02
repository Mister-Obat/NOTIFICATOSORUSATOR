using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.Foundation.Metadata;

namespace Notificatosorusator
{
    public partial class MainWindow : Window
    {
        private volatile bool _allowAntigravity = true;
        private volatile bool _allowPowerShell = true;
        private volatile int _audioVolumePercent = 100;
        private bool _isUiReady;


        public MainWindow()
        {
            InitializeComponent();
            _isUiReady = true;
            ApplyUiSettings();
            Log("Initialized.");
            Task.Run(() => RunPollingLoop());
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            TryEnableDarkTitleBar();
        }

        private void TryEnableDarkTitleBar()
        {
            try
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                if (hwnd == IntPtr.Zero)
                {
                    return;
                }

                const int DwmwaUseImmersiveDarkMode = 20;
                const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
                int enabled = 1;

                _ = DwmSetWindowAttribute(hwnd, DwmwaUseImmersiveDarkMode, ref enabled, sizeof(int));
                _ = DwmSetWindowAttribute(hwnd, DwmwaUseImmersiveDarkModeBefore20H1, ref enabled, sizeof(int));
            }
            catch
            {
                // Ignore if unsupported by OS.
            }
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogOutput.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
                LogOutput.ScrollToEnd();
            });
        }

        private void ApplyUiSettings()
        {
            if (!_isUiReady
                || SourceAntigravityCheckbox == null
                || SourcePowerShellCheckbox == null
                || VolumeSlider == null
                || VolumeValueText == null)
            {
                return;
            }

            _allowAntigravity = SourceAntigravityCheckbox.IsChecked == true;
            _allowPowerShell = SourcePowerShellCheckbox.IsChecked == true;

            double sliderValue = VolumeSlider.Value;
            VolumeValueText.Text = $"{Math.Round(sliderValue):0}%";
            _audioVolumePercent = (int)Math.Clamp(Math.Round(sliderValue), 0, 100);
        }

        private void SourceCheckboxChanged(object sender, RoutedEventArgs e)
        {
            ApplyUiSettings();
            if (_isUiReady)
            {
                Log($"[Sources] Antigravity={_allowAntigravity} | PowerShell={_allowPowerShell}");
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplyUiSettings();
            if (_isUiReady && VolumeSlider != null)
            {
                Log($"[Audio] Volume set to {Math.Round(VolumeSlider.Value):0}%");
            }
        }



        private async Task RunPollingLoop()
        {
            Log("[Polling] Starting notification monitor...");

            if (!ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                Log("[Polling] API Not Supported.");
                return;
            }

            UserNotificationListener listener = UserNotificationListener.Current;

            try
            {
                var access = await listener.RequestAccessAsync();
                if (access != UserNotificationListenerAccessStatus.Allowed)
                {
                    Log("[Polling] Access Denied. Launching settings...");
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("ms-settings:privacy-notifications") { UseShellExecute = true });
                    return;
                }
            }
            catch (Exception ex)
            {
                Log($"[Polling] Access Check Error: {ex.Message}");
                return;
            }

            Log("[Polling] Access Granted. Monitoring...");

            uint lastId = 0;

            // Initial check to set baseline
            try
            {
                var initial = await listener.GetNotificationsAsync(NotificationKinds.Toast);
                var newest = initial.OrderByDescending(n => n.CreationTime).FirstOrDefault();
                if (newest != null) lastId = newest.Id;
            }
            catch { }

            while (true)
            {
                try
                {
                    var notifications = await listener.GetNotificationsAsync(NotificationKinds.Toast);
                    var latest = notifications.OrderByDescending(n => n.CreationTime).FirstOrDefault();

                    if (latest != null && latest.Id != lastId)
                    {
                        lastId = latest.Id;

                        string fullText = "";
                        try
                        {
                            var binding = latest.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);
                            if (binding != null)
                            {
                                var textElements = binding.GetTextElements();
                                foreach (var t in textElements)
                                {
                                    fullText += t.Text + " ";
                                }
                            }
                        }
                        catch { }

                        fullText = fullText.Trim();
                        string appName = latest.AppInfo.DisplayInfo.DisplayName;

                        Log($"[New Toast] App: {appName} | Content: {fullText}");

                        // FILTER: Allowed sources are controlled from UI checkboxes.
                        bool fromAntigravity = appName.Contains("Antigravity", StringComparison.OrdinalIgnoreCase);
                        bool fromPowerShell = appName.Contains("PowerShell", StringComparison.OrdinalIgnoreCase);
                        bool isAllowed = (fromAntigravity && _allowAntigravity)
                                      || (fromPowerShell && _allowPowerShell);
                        if (!isAllowed)
                        {
                            Log($"[Ignored] Source disabled or unknown: {appName}");
                            continue;
                        }

                        // LOGIC RULES
                        string lowerText = fullText.ToLowerInvariant();

                        if (lowerText.Contains("command") || lowerText.Contains("run"))
                        {
                            Log("=> Rule Match: 'command'/'run' -> Sound 3");
                            PlaySoundById("3");
                        }
                        else
                        {
                            Log("=> Rule Default: -> Sound 1");
                            PlaySoundById("1");
                        }
                    }
                }
                catch
                {
                    // Ignore transient errors
                }

                await Task.Delay(1000);
            }
        }

        private void PlaySoundById(string id)
        {
            string filename = $"Sounds\\{id}.mp3";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            if (File.Exists(path))
            {
                Log($"[Audio] Playing {id}...");
                try
                {
                    var player = new Windows.Media.Playback.MediaPlayer();
                    player.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(path));
                    player.Volume = _audioVolumePercent / 100.0;
                    player.Play();
                }
                catch (Exception ex)
                {
                    Log($"[Audio] Error: {ex.Message}");
                }
            }
            else
            {
                Log($"[Audio] File not found: {filename}");
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int valueSize);
    }
}
