# LightFlip

[![Latest Version](https://img.shields.io/github/v/release/GinjeesPacks/LightFlip?display_name=tag)](../../releases/latest)
[![Download Latest](https://img.shields.io/badge/Download-Latest%20Release-green)](../../releases/latest)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Discord](https://img.shields.io/badge/Discord-Join%20Server-5865F2?logo=discord&logoColor=white)](https://discord.gg/n8WnKWMzhJ)

LightFlip is a lightweight Windows tray utility that lets you **instantly switch display gamma, brightness, contrast, and color temperature profiles per game or app** using a global or per-game hotkey.

It detects the active foreground application and applies the matching display gamma ramp in real time — no monitor OSD or GPU control panel tweaking required.

Built for fast runtime switching, per-game presets, and zero workflow interruption.

---

## ✨ Features

- 🎮 **Per-Game Profile Catalog** — Create custom gamma / brightness / contrast / temperature profiles per executable
- ⚡ **Global Hotkey Toggle** — Switch profiles instantly while in-game or in-app
- 🎯 **Per-Game Hotkey Override** — Assign a custom hotkey for specific games that overrides the global hotkey while that game is active
- 🖥 **Foreground App Detection** — Matches the active window to a saved profile using executable path with process-name fallback for protected games
- 🌙 **Gamma Ramp Control** — Applies gamma ramp changes at the Windows display LUT level
- 🎚 **Brightness / Contrast / Temperature Adjustments** — Incorporated into the generated gamma ramp
- 📌 **System Tray App** — Runs quietly in the tray with quick access menu
- 🚀 **Start With Windows (Optional)** — Launch automatically on login via user startup entry
- 🗕 **Minimize to Tray** — No taskbar clutter during normal use
- 🔄 **Revert When Inactive (Per Profile Option)** — Restores captured baseline display ramp when a profile deactivates or the app exits
- 🖥 **Multi-Monitor Aware** — Attempts to target the display containing the active window, with fallback to all displays if needed

---

## 🧠 How It Works

LightFlip continuously monitors the **currently focused window**.

When that window belongs to a configured game/app profile, LightFlip applies that profile’s gamma ramp settings.

Each profile contains two runtime states:

- **Normal profile**
- **Bright profile**

When you press the active hotkey, LightFlip toggles between the Normal and Bright profiles for the active game/app.

If **Revert When Inactive** is enabled for that profile, LightFlip restores your captured baseline gamma ramp when the game loses focus, the profile deactivates, or the app exits.

All changes are applied live using the Windows `SetDeviceGammaRamp` API.

---

## ⌨️ Hotkeys (Runtime Toggle)

You can configure:

- A **global hotkey**
- Optional **per-game hotkey overrides**

Runtime behavior:

- If no per-game hotkey is set → the global hotkey is used
- If a per-game hotkey is set → it is automatically registered while that game is active
- Hotkey toggles between the profile’s Normal and Bright states
- Hotkeys are registered globally — LightFlip does not need focus
- Borderless/windowed modes are most reliable for runtime switching

---

## ⚙️ Setup & Usage

### 1️⃣ Run LightFlip

Launch `LightFlip.exe`.  
The app starts in the system tray.

Double-click the tray icon or right-click → **Settings**.

---

### 2️⃣ Add a Game/App Profile

In Settings:

1. Click **Browse**
2. Select the game/app `.exe`
3. Adjust gamma / brightness / contrast / temperature
4. Configure Normal and Bright profile values
5. Optionally set a per-game hotkey override
6. Save the profile

Per-profile options include:

- Revert when inactive
- Per-game hotkey override
- Normal vs Bright profile tuning

---

### 3️⃣ Configure App Behavior (Optional)

In Settings:

- ✅ Start with Windows
- ✅ Start minimized
- ✅ Minimize to tray

---

### 4️⃣ Tray Controls

Right-click the tray icon:

- **Toggle** — Toggle active profile immediately
- **Settings** — Open configuration window
- **Exit** — Close LightFlip and restore baseline gamma ramp

---

## 🖥 Multi-Monitor Behavior

LightFlip attempts to apply the gamma ramp to:

- The monitor containing the active window
- Falls back to all active displays if a specific monitor device context cannot be obtained

---

## 🔒 Safety Notes

- LightFlip changes **display gamma ramps only**
- No monitor hardware settings are modified
- No driver configuration is changed
- Startup entry is optional and user-controlled
- Baseline gamma ramp is captured and restored when profiles deactivate or the app exits

---

## ⚠️ Compatibility Notes

- Borderless and windowed fullscreen modes are most reliable
- Some exclusive fullscreen games may ignore gamma ramp changes
- Windows HDR mode can reduce or override gamma ramp effects
- GPU driver color controls may override gamma ramp behavior

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
