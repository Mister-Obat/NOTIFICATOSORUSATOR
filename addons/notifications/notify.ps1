[CmdletBinding()]
param(
    [ValidateSet("Stop", "Notification", "PermissionRequest", "Manual")]
    [string]$Event = "Notification",
    [string]$Title = "Codex",
    [string]$Message = "",
    [switch]$NoBeep
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($Message)) {
    switch ($Event) {
        "Stop" { $Message = "Tache terminee." }
        "Notification" { $Message = "En attente de ton attention..." }
        "PermissionRequest" { $Message = "Permission requise pour continuer." }
        default { $Message = "En attente de ton attention..." }
    }
}

[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] | Out-Null
[Windows.Data.Xml.Dom.XmlDocument, Windows.Data.Xml.Dom.XmlDocument, ContentType = WindowsRuntime] | Out-Null

$xml = New-Object Windows.Data.Xml.Dom.XmlDocument
$xml.LoadXml("<toast><visual><binding template=""ToastGeneric""><text>$Title</text><text>$Message</text></binding></visual></toast>")

$toast = [Windows.UI.Notifications.ToastNotification]::new($xml)
$appId = "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}\WindowsPowerShell\v1.0\powershell.exe"
[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier($appId).Show($toast)

if (-not $NoBeep) {
    [System.Media.SystemSounds]::Beep.Play()
}
