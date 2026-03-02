# AI Manifest: NOTIFICATOSORUSATOR

> **Domain**: Physical World Interface (Audio/Notifications)
> **Status**: Active / Production

## 1. Purpose
This system acts as a **bridge** between the AI Agent (digital) and the User (physical).
It allows the AI to trigger sound alerts to catch the user's attention when tasks are completed or require input.

## 2. Architecture
- **Type**: Windows WPF Application (.NET 7)
- **Executable**: `./bin/Debug/net7.0-windows10.0.19041.0/Notificatosorusator.exe`
- **Behavior**: Runs in the background (Windowed with logs), polling for events.

## 3. Trigger Protocols

### Method A: Automatic (Preferred)
The application listens to **Windows Toast Notifications** emitted by the Agent (via `notify_user`).
**Constraint:** The notification source (App Name) MUST match an enabled source in the UI.
Default enabled sources are **"Antigravity"** and **"PowerShell"**.
It parses the text content of the notification to decide which sound to play.

**Audio Logic Rules:**
| Condition (Case-Insensitive) | Sound File | Meaning |
| :--- | :--- | :--- |
| Text contains `"command"` OR `"run"` | `3.mp3` | **Long Task Started** (Work in progress) |
| *Default / Any other text* | `1.mp3` | **Task Complete** (Success) |

**Usage for AI:**
Simply use the `notify_user` tool.
- To signal a start: `notify_user(message="Running command...")`
- To signal end: `notify_user(message="Task done.")`



## 4. Constraints
- The application must be running to play sounds.
- If `notify_user` is used, the system sound volume must be up.

## 5. Project Structure & File Map

| File / Directory | Description |
| :--- | :--- |
| **[Notificatosorusator.csproj](Notificatosorusator.csproj)** | Main Project File (WPF, .NET 7). Defines build settings and asset copying. |
| **[MainWindow.xaml.cs](MainWindow.xaml.cs)** | **Core Logic**. Contains the `UserNotificationListener` polling loop, source filtering (Antigravity/PowerShell), and Audio Playback logic. |
| **[Package.appxmanifest](Package.appxmanifest)** | Manifest declaring the Application Identity and the `userNotificationListener` capability (Required for API access). |
| **[REGISTER_APP.ps1](REGISTER_APP.ps1)** | PowerShell script to register the app identity on the machine (One-time setup). |
| **[Sounds/](Sounds/)** | Directory containing the `.mp3` audio assets (`1.mp3` to `4.mp3`). |
| **[bin/Debug/.../Notificatosorusator.exe](bin/Debug/net7.0-windows10.0.19041.0/Notificatosorusator.exe)** | Compiled entry point. Runs the application. |
