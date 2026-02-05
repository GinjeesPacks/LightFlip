# LightFlip

[![Latest Version](https://img.shields.io/github/v/release/GinjeesPacks/LightFlip?display_name=tag)](../../releases/latest)
[![Download Latest](https://img.shields.io/badge/Download-Latest%20Release-green)](../../releases/latest)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Discord](https://img.shields.io/badge/Discord-Join%20Server-5865F2?logo=discord&logoColor=white)](https://discord.gg/n8WnKWMzhJ)

LightFlip is a lightweight Windows tray utility that lets you **instantly switch display color/brightness profiles per game or app** using a global hotkey. It detects the active foreground application and applies the matching display gamma/color profile in real time — no monitor OSD or GPU panel tweaking required.

Built for fast runtime switching, per-game presets, and zero workflow interruption.

---

## ✨ Features

- 🎮 **Per-Game Profiles** — Create custom color/gamma profiles for specific executables
- ⚡ **Global Hotkey Toggle** — Switch profiles instantly while in-game or in-app
- 🖥 **Foreground App Detection** — Automatically matches the active window to a saved profile
- 🌙 **Gamma / Color Ramp Control** — Adjusts display output at the system level
- 📌 **System Tray App** — Runs quietly in the tray with quick access menu
- 🚀 **Start With Windows (Optional)** — Launch automatically on login
- 🗕 **Minimize to Tray** — No taskbar clutter
- 🔄 **Revert on Close (Per Game Option)** — Restore neutral display settings when the app closes

---

## 🧠 How It Works

LightFlip monitors the **currently focused window**. When that window belongs to a configured game/app profile, LightFlip can apply a custom display gamma/color ramp to your monitor.

When you press your configured **global hotkey**, LightFlip toggles between:

- Your saved profile for that game/app
- A neutral/default display profile

All changes happen live and system-wide using Windows display gamma APIs.

---

## ⌨️ Hotkey (Runtime Toggle)

You can configure a global hotkey inside **Settings**.

**At runtime:**

- Press your hotkey → toggles the profile for the active game/app
- Works even when the game is fullscreen (borderless/windowed works best)
- Hotkey is registered globally — no need to focus LightFlip

---

## ⚙️ Setup & Usage

### 1️⃣ Run LightFlip

Launch `LightFlip.exe`. The app starts in the system tray.

Double-click the tray icon or right-click → **Settings**.

---

### 2️⃣ Add a Game/App Profile

In Settings:

1. Click **Browse**
2. Select the game/app `.exe`
3. Adjust the color/gamma profile values
4. Save the profile

Options available per profile:

- Revert on close
- Custom color profile
- Toggle behavior

---

### 3️⃣ Configure App Behavior (Optional)

In Settings menu:

- ✅ Start with Windows
- ✅ Start minimized
- ✅ Minimize to tray

---

### 4️⃣ Tray Controls

Right-click the tray icon:

- **Toggle** — Toggle active profile immediately
- **Settings** — Open configuration window
- **Exit** — Close LightFlip

---

## 🖥 Multi-Monitor Behavior

LightFlip attempts to apply the profile to:

- The monitor containing the active window
- Falls back to all active displays if needed

---

## 🔒 Safety Notes

- LightFlip changes **display gamma ramps only**
- No driver or hardware modification
- Startup entry is optional and user-controlled
- Neutral profile can always be restored

---

## 👤 Credits

Created by **Caleb Cook**

If you redistribute or modify this project, please keep the original credit.

---

## 💬 Join the Discord

[![Discord](https://img.shields.io/badge/Discord-Join%20Server-5865F2?logo=discord&logoColor=white)](https://discord.gg/n8WnKWMzhJ)

Join the GinjeesPacks Discord for support, updates, and releases.

---

## License

LightFlip source code is licensed under the MIT License.

The LightFlip name, icon, and branding assets are not covered by the MIT License and may not be reused without permission.  
Copyright © 2026 Caleb Cook.




