# NOTIFICATOSORUSATOR 🦖🔊

![WPF App](https://img.shields.io/badge/App-WPF-blue) ![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-lightgrey) ![Status](https://img.shields.io/badge/Status-Production-green)

**The Missing Link Between Your AI Agent & The Physical World.**

> *Scroll down for French version / Voir plus bas pour la version française* 🇫🇷

---

## 🇺🇸 ENGLISH

### 🎯 The Problem
**Also, are your Windows notifications bugged and silent?**
If you work with **Antigravity**, add this to your AI rules to ensure it actually triggers the notification:
```markdown
## NOTIFICATIONS
No matter what, as soon as a task is finished (partial or complete) THE AI MUST IMMEDIATELY call notify_user
No task can end without calling notify_user
```
Then run this app, and your AI will finally have a voice!

### 💡 The Solution
**Notificatosorusator** is a lightweight **Audio Bridge** for Windows. It allows AI Agents (like those running in VS Code, Cursor, or terminals) to trigger **real sound effects** in your physical environment, simply by sending a standard Windows Notification.

### 🚀 Features
- **👂 Automatic Listening**: Uses Windows `UserNotificationListener` API to detect toasts instantly.
- **🧠 Smart Logic**: Parses notification text to choose the right sound:
  - Contains **"command"** or **"run"** -> 🎵 Plays "Long Task" sound (`3.mp3`).
  - Any other text -> 🎵 Plays "Success/Default" sound (`1.mp3`).
- **⚡ Lightweight**: Built as a native WPF app (.NET 7).

### 🛠️ Installation & Usage
1. **Prerequisites**: Windows 10/11, Developer Mode enabled.
2. **Setup**: Run `REGISTER_APP.ps1` with PowerShell to register the app identity.
3. **Run**: Launch `Notificatosorusator.exe` from `./bin/Debug/net7.0-windows10.0.19041.0/`.
4. **Grant Access**: Allow "Notification Access" when prompted.



### ❤️ Support
If this tool helps your workflow, consider supporting usage!

[![Donate](https://img.shields.io/badge/Donate-PayPal-blue.svg)](https://www.paypal.com/paypalme/creaprisme)

---

## 🇫🇷 FRANÇAIS

### 🎯 Le Problème
**Vous aussi vos notifications Windows sont buguées et ne veulent pas émettre de son ?**
Pas de soucis ! Si vous travaillez sur **Antigravity**, ajoutez ceci dans les règles de l'IA :
```markdown
## NOTIFICATIONS
Quoi qu’il arrive, dès qu’une tâche est terminée (partielle ou complète) L’IA DOIT IMMÉDIATEMENT appeler notify_user
Aucune tâche ne peut se terminer sans appeler notify_user
```
Activez ce programme, et l'IA vous préviendra par un son dès qu'elle a terminé ou qu'elle vous attend !

### 💡 La Solution
**Notificatosorusator** est un **Pont Audio** léger pour Windows. Il permet aux agents IA de déclencher de **vrais effets sonores** dans votre environnement physique, simplement en envoyant une notification Windows standard.

### 🚀 Fonctionnalités
- **👂 Écoute Automatique** : Utilise l'API Windows `UserNotificationListener` pour détecter les notifications.
- **🧠 Logique Intelligente** : Analyse le texte pour choisir le bon son :
  - Contient **"command"** ou **"run"** -> 🎵 Son "Tâche Longue" (`3.mp3`).
  - Tout autre texte -> 🎵 Son "Succès/Défaut" (`1.mp3`).
- **⚡ Léger** : Application native WPF (.NET 7).

### 🛠️ Installation et Utilisation
1. **Prérequis** : Windows 10/11, Mode Développeur activé.
2. **Installation** : Exécutez `REGISTER_APP.ps1` avec PowerShell pour enregistrer l'identité de l'application.
3. **Lancement** : Lancez `Notificatosorusator.exe` depuis `./bin/Debug/net7.0-windows10.0.19041.0/`.
4. **Accès** : Autorisez l'"Accès aux notifications" à la demande.



### ❤️ Soutien
Si cet outil vous aide, pensez à faire un don !

[![Faire un don](https://img.shields.io/badge/Faire%20un%20don-PayPal-blue.svg)](https://www.paypal.com/paypalme/creaprisme)

---

## 🎵 Customization / Personnalisation
Replace files in `Sounds/` folder / Remplacez les fichiers dans le dossier `Sounds/` :
- `1.mp3` : Success / Succès
- `3.mp3` : Long Task / Tâche Longue

---

## 📜 License
Ce projet est distribué sous la licence AGPL-3.0.

---
*Codé avec l'assistance de l'IA*
