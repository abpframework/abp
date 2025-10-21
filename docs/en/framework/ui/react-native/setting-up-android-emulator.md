```json
//[doc-seo]
{
    "Description": "Learn how to set up an Android emulator without Android Studio using command line tools on Windows, macOS, and Linux."
}
```

# Setting Up Android Emulator Without Android Studio (Windows, macOS, Linux)

This guide explains how to install and run an Android emulator **without Android Studio**, using only **Command Line Tools**.

---

## 1. Download Required Tools

Go to: [https://developer.android.com/studio#command-tools](https://developer.android.com/studio#command-tools)  
Download the "Command line tools only" package for your OS:

- **Windows:** `commandlinetools-win-*.zip`
- **macOS:** `commandlinetools-mac-*.zip`
- **Linux:** `commandlinetools-linux-*.zip`

---

## 2. Create the Required Directory Structure

### Windows:
```
C:\Android\
└── cmdline-tools\
    └── latest\
        └── [extract all files from the zip here]
```

### macOS / Linux:
```
~/Android/
└── cmdline-tools/
    └── latest/
        └── [extract all files from the zip here]
```

> You need to create the `latest` folder manually.

---

## 3. Set Environment Variables

### Windows (temporary for CMD session):
```cmd
set PATH=C:\Android\cmdline-tools\latest\bin;C:\Android\platform-tools;C:\Android\emulator;%PATH%
```

### macOS / Linux:
Add the following to your `.bashrc`, `.zshrc`, or `.bash_profile` file:

```bash
export ANDROID_HOME=$HOME/Android
export PATH=$PATH:$ANDROID_HOME/cmdline-tools/latest/bin
export PATH=$PATH:$ANDROID_HOME/platform-tools
export PATH=$PATH:$ANDROID_HOME/emulator
```

> Apply the changes:
```bash
source ~/.zshrc  # or ~/.bashrc if you're using bash
```

---

## 4. Install SDK Components

Install platform tools, emulator, and a system image:

```bash
sdkmanager --sdk_root=$ANDROID_HOME "platform-tools" "platforms;android-34" "system-images;android-34;google_apis;x86_64" "emulator"
```

> On Windows, replace `$ANDROID_HOME` with `--sdk_root=C:\Android`.

---

## 5. Create an AVD (Android Virtual Device)

### List available devices:
```bash
avdmanager list devices
```

### Create your AVD:
```bash
avdmanager create avd -n myEmu -k "system-images;android-34;google_apis;x86_64" --device "pixel"
```

---

## 6. Start the Emulator

```bash
emulator -avd myEmu
```

The emulator window should open 

---

## Extra Tools and Commands

### List connected devices with ADB:
```bash
adb devices
```

### Install an APK:
```bash
adb install myApp.apk
```

---

## How to Enable Fast Refresh in React Native

React Native uses a hot reload system called **Fast Refresh**.  
It is enabled by default in development mode, but you can manually enable or disable it via the Developer Menu.

### To open the Developer Menu on Android emulator:

```bash
adb shell input keyevent 82
```

This command simulates the hardware menu button and opens the Developer Menu inside the emulator.

### From the Developer Menu:

- Look for the option: **Enable Fast Refresh**
- If it's unchecked, tap to enable it
- If it's already checked, Fast Refresh is already active

### Alternative (if adb doesn't work):

Focus the emulator window and press:

- **Ctrl + M** (Windows/Linux)
- **Cmd + M** (Mac)

---

## Troubleshooting

| Problem | Explanation |
|--------|-------------|
| `sdkmanager not found` | Make sure `PATH` includes the `latest/bin` directory |
| `x86_64 system image not found` | Make sure you've downloaded it using `sdkmanager` |
| `emulator not found` | Add the `emulator` directory to `PATH` |
| `setx` truncates path (Windows) | Use GUI to update environment variables manually |

---

## Summary

You can now run an Android emulator without installing Android Studio, entirely through the command line. This emulator can be used for React Native or any mobile development framework.
