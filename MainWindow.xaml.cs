using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.Foundation.Metadata;

namespace Notificatosorusator
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            Log("Initialized.");
            Task.Run(() => RunPollingLoop());
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogOutput.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
                LogOutput.ScrollToEnd();
            });
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
    }
}
