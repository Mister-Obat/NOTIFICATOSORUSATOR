# NOTIFICATOSORUSATOR 🦖🔊

![WPF App](https://img.shields.io/badge/App-WPF-blue) ![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-lightgrey) ![Status](https://img.shields.io/badge/Status-Production-green)

**Le lien manquant entre ton agent IA et le monde physique.**

![App Screenshot](Images/screenshot.png)

---

## 🎯 Le Problème
Vos notifications Windows sont parfois silencieuses ou peu fiables.

Avec **Antigravity** / agents IA, ajoutez cette règle :
```markdown
# NOTIFICATIONS

**Toute tâche terminée (partielle ou complète) -> `notify_user` obligatoire.**
Aucune tâche ne peut se terminer sans appeler notify_user.
```

---

## ⚙️ Réglages Antigravity
Ajoutez ces lignes dans votre `settings.json` :
```json
  "antigravity.agent.notifications.desktop": "always",
  "antigravity.agent.notifications.sound": true,
  "antigravity.agent.notifyOnTaskCompletion": true,
  "antigravity.agent.notifyOnUserActionRequired": true,
  "java.showBuildStatusOnStart.enabled": "notification"
```

---

## 💡 La Solution
**Notificatosorusator** est un pont audio Windows (WPF/.NET 7) qui écoute les toasts Windows et joue des sons selon leur contenu.

### Fonctionnalités
- Écoute automatique des notifications Windows (`UserNotificationListener`)
- Règles audio:
  - contient `command` ou `run` -> `3.mp3`
  - sinon -> `1.mp3`
- Interface sombre avec sélection des sources actives + volume

---

## 🛠️ Installation et Utilisation
1. Prérequis : Windows 10/11, Mode développeur activé
2. Exécuter `REGISTER_APP.ps1` pour enregistrer l'identité app
3. Lancer `Notificatosorusator.exe` depuis `./bin/Debug/net7.0-windows10.0.19041.0/`
4. Autoriser l'accès notifications si demandé

---

## 🎵 Personnalisation
Remplacez les fichiers dans `Sounds/` :
- `1.mp3` : succès / défaut
- `3.mp3` : tâche longue

---

## 🔔 Addon (Astuce pour VS Code)
L'astuce `notify.ps1` est un **addon optionnel pour VS Code** (notifications toast + beep), hors runtime principal de l'app.

Script global recommandé :
`C:\Users\...\.codex\tools\notifications\notify.ps1`
`C:\Users\...\.claude\notify.ps1`

Copie d'exemple dans ce repo :
`addons/notifications/notify.ps1`

---

## 📜 License
Ce projet est distribué sous licence AGPL-3.0.

---
*Codé 100% par des IA, supervisé à l'arrache par Obat 😏*
