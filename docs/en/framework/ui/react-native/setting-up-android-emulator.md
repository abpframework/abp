```json
//[doc-seo]
{
    "Description": "Learn how to set up an Android emulator without Android Studio using command line tools on Windows, macOS, and Linux."
}
```

# Setting Up Android Emulator Without Android Studio (Windows, macOS, Linux)

This guide walks you through installing and running an Android emulator **without Android Studio**, using only the **Android Command Line Tools**.

---

## 1. Download Required Tools

Visit the [Android Command Line Tools download page](https://developer.android.com/studio#command-line-tools-only) and download the "Command line tools only" package for your operating system:

- **Windows:** `commandlinetools-win-*.zip`
- **macOS:** `commandlinetools-mac-*.zip`
- **Linux:** `commandlinetools-linux-*.zip`

> **Alternative for Windows:** If you prefer using Windows Package Manager, you can install Android Studio (which includes the command-line tools) using:
> ```powershell
> winget install --id=Google.AndroidStudio -e
> ```
> However, this guide focuses on installing only the command-line tools without the full IDE.

---

## 2. Create Directory Structure and Extract Files

The Android SDK tools require a specific directory structure. Follow the steps below for your operating system.

### Windows:

1. **Create the directory structure:**
   ```
   C:\Android\
   └── cmdline-tools\
       └── latest\
   ```

2. **Extract the downloaded zip file.** The archive contains a `cmdline-tools` folder with `bin`, `lib`, and other files.

3. **Move all contents** from the extracted `cmdline-tools` folder into `C:\Android\cmdline-tools\latest\`

   Your final directory structure should look like this:
   ```
   C:\Android\
   └── cmdline-tools\
       └── latest\
           ├── bin\
           ├── lib\
           └── [other files]
   ```

### macOS / Linux:

1. **Create the directory structure:**
   ```bash
   mkdir -p ~/Android/cmdline-tools
   ```

2. **Extract the downloaded zip file:**
   ```bash
   cd ~/Downloads
   unzip commandlinetools-*.zip
   ```

3. **Move the extracted folder to the correct location:**
   ```bash
   mv cmdline-tools ~/Android/cmdline-tools/latest
   ```

   Your final directory structure should look like this:
   ```
   ~/Android/
   └── cmdline-tools/
       └── latest/
           ├── bin/
           ├── lib/
           └── [other files]
   ```

> **Important:** The `latest` folder must be created manually (Windows) or by renaming the extracted folder (macOS/Linux). The Android SDK tools require this exact directory structure to function properly.

---

## 3. Configure Environment Variables

Set up environment variables so your system can locate the Android SDK tools.

### Windows (PowerShell - permanent):

Run these commands in PowerShell to set environment variables permanently:

```powershell
[System.Environment]::SetEnvironmentVariable('ANDROID_HOME', 'C:\Android', 'User')
$currentPath = [System.Environment]::GetEnvironmentVariable('Path', 'User')
[System.Environment]::SetEnvironmentVariable('Path', "$currentPath;C:\Android\cmdline-tools\latest\bin;C:\Android\platform-tools;C:\Android\emulator", 'User')
```

> **Note:** You may need to restart your terminal or PowerShell session for the changes to take effect.

### Windows (CMD - temporary for current session):

If you only need the environment variables for the current session, use these commands:

```cmd
set ANDROID_HOME=C:\Android
set PATH=%PATH%;C:\Android\cmdline-tools\latest\bin;C:\Android\platform-tools;C:\Android\emulator
```

> **Note:** These settings will be lost when you close the command prompt window.

### macOS / Linux:

Add the following environment variables to your shell configuration file (`.bashrc`, `.zshrc`, or `.bash_profile`):

```bash
export ANDROID_HOME=$HOME/Android
export PATH=$PATH:$ANDROID_HOME/cmdline-tools/latest/bin
export PATH=$PATH:$ANDROID_HOME/platform-tools
export PATH=$PATH:$ANDROID_HOME/emulator
```

After saving the file, reload your shell configuration:

```bash
source ~/.zshrc  # or ~/.bashrc if you're using bash
```

**Verify Environment Variables:**

After reloading, verify the variables are set correctly:

```bash
echo $ANDROID_HOME
which sdkmanager
```

---

## 4. Accept Android SDK Licenses (macOS/Linux)

**macOS / Linux users only:** Before installing SDK components, you must accept the Android SDK licenses:

```bash
yes | sdkmanager --licenses
```

This command automatically accepts all licenses. Without this step, the installation will fail.

> **Note:** Windows users will be prompted to accept licenses during the installation in the next step.

---

## 5. Install SDK Components

Use `sdkmanager` to install the required Android SDK components: platform tools, an Android platform, a system image, and the emulator.

### Windows:

```cmd
sdkmanager --sdk_root=C:\Android "platform-tools" "platforms;android-35" "system-images;android-35;google_apis;x86_64" "emulator"
```

### macOS / Linux:

**For Apple Silicon Macs (M1/M2/M3/M4):**
```bash
sdkmanager "platform-tools" "platforms;android-35" "system-images;android-35;google_apis;arm64-v8a" "emulator"
```

**For Intel-based Macs and Linux:**
```bash
sdkmanager "platform-tools" "platforms;android-35" "system-images;android-35;google_apis;x86_64" "emulator"
```

> **Note:** 
> - This command installs Android 15 (API level 35), which is the current stable version required by Google Play as of 2025.
> - To see all available versions, run `sdkmanager --list` and replace `android-35` with your preferred API level if needed.
> - Use `arm64-v8a` for Apple Silicon Macs (M1/M2/M3/M4) and `x86_64` for Intel-based Macs, Windows, and most Linux systems. The architecture must match your system's processor.

---

## 6. Create an Android Virtual Device (AVD)

An AVD is a device configuration that defines the characteristics of an Android device you want to simulate.

### List Available Device Profiles (Optional):

Before creating an AVD, you can view all available device profiles (e.g., Pixel, Nexus) to choose from:

```bash
avdmanager list devices
```

### Create Your AVD:

Create a new AVD with the following command:

**Windows:**
```cmd
avdmanager create avd -n myEmu -k "system-images;android-35;google_apis;x86_64" --device "pixel_8"
```

**macOS / Linux (Apple Silicon):**
```bash
avdmanager create avd -n myEmu -k "system-images;android-35;google_apis;arm64-v8a" --device "pixel_8"
```

**macOS / Linux (Intel-based):**
```bash
avdmanager create avd -n myEmu -k "system-images;android-35;google_apis;x86_64" --device "pixel_8"
```

When prompted "Do you wish to create a custom hardware profile?" type `no` and press Enter.

> **Note:** 
> - Replace `myEmu` with your preferred AVD name
> - Replace `pixel_8` with a device profile from the list above if you want a different device configuration
> - The `-k` parameter specifies the system image you installed in the previous step. Make sure it matches the architecture you installed (x86_64 for Windows and Intel-based systems, arm64-v8a for Apple Silicon Macs)

---

## 7. Start the Emulator

Launch your emulator using the AVD name you created:

```bash
emulator -avd myEmu
```

Replace `myEmu` with the name you used when creating your AVD. The emulator window should open and boot up the Android system.

### Common Startup Issues (macOS/Linux):

**If you get "command not found":**
```bash
$ANDROID_HOME/emulator/emulator -avd myEmu
```

**For better performance on Apple Silicon:**
```bash
emulator -avd myEmu -gpu host
```

**For headless mode (no GUI window):**
```bash
emulator -avd myEmu -no-window
```

---

## 8. Additional Tools and Commands

### List All AVDs:

View all created virtual devices:

```bash
avdmanager list avd
```

### List Connected Devices:

Use ADB (Android Debug Bridge) to view all connected Android devices and running emulators:

```bash
adb devices
```

This is useful for verifying that your emulator is running and properly connected.

### Install an APK:

Install an Android application package (APK) to your emulator or connected device:

```bash
adb install myApp.apk
```

Replace `myApp.apk` with the path to your APK file.

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

## 9. Troubleshooting

Common issues and their solutions:

| Problem | Solution |
|--------|----------|
| `sdkmanager not found` | **Windows:** Verify that your `PATH` environment variable includes `C:\Android\cmdline-tools\latest\bin`. **macOS/Linux:** Verify `PATH` includes `$ANDROID_HOME/cmdline-tools/latest/bin`. Run `source ~/.zshrc` or `source ~/.bashrc` to reload environment variables |
| `Warning: Could not create settings` | **macOS/Linux:** Run `yes \| sdkmanager --licenses` to accept all Android SDK licenses. **Windows:** Accept licenses when prompted during installation |
| System image not found | Ensure you've installed the system image using `sdkmanager` in step 5. The architecture must match: `x86_64` for Windows and Intel-based systems, `arm64-v8a` for Apple Silicon Macs. Run `sdkmanager --list` to verify available images, then run the installation command again if needed |
| `emulator not found` | **Windows:** Add the `emulator` directory to your `PATH`: `C:\Android\emulator`. **macOS/Linux:** Use full path: `$ANDROID_HOME/emulator/emulator -avd myEmu` or verify `$ANDROID_HOME/emulator` is in your PATH |
| `setx` truncates path (Windows) | If `setx` truncates your PATH variable, use the PowerShell method from step 3, or update environment variables manually through the Windows GUI (System Properties → Environment Variables) |
| Emulator won't start | **Windows:** Ensure hardware acceleration is enabled. Enable Hyper-V or use Intel HAXM. **macOS (Apple Silicon):** Try `emulator -avd myEmu -gpu host`. **Linux:** Ensure KVM is enabled: `sudo apt install qemu-kvm` and add user to kvm group |
| Permission denied errors (macOS/Linux) | Run `chmod +x $ANDROID_HOME/cmdline-tools/latest/bin/*` to make tools executable |
| Emulator extremely slow | **Windows:** Enable Hyper-V or Intel HAXM. **Linux:** Install and enable KVM (see below). **Apple Silicon:** Ensure you're using `arm64-v8a` system images for optimal performance |

### Enable Hardware Acceleration (Linux):

For better performance on Linux, install and enable KVM:

```bash
sudo apt install qemu-kvm libvirt-daemon-system libvirt-clients bridge-utils
sudo adduser $USER kvm
```

Log out and log back in for group changes to take effect.

---

## 10. Summary

You've successfully set up an Android emulator without installing Android Studio, using only the command-line tools. This emulator can be used for React Native development or any other mobile development framework that requires Android emulation.

The emulator is now ready to use. You can start it anytime with `emulator -avd myEmu` (using your AVD name) and begin developing and testing your Android applications.

### Quick Reference Commands:

**Start the emulator:**
```bash
emulator -avd myEmu
```

**List running devices:**
```bash
adb devices
```

**Install an app:**
```bash
adb install myApp.apk
```

**List all AVDs:**
```bash
avdmanager list avd
```

---

## Additional Notes

- **Android API Updates:** As of January 2026, Android 15 (API 35) is the current requirement for Google Play Store submissions. This guide uses API 35, but you can install older versions if needed for legacy app testing.
- **Storage Location:** All Android SDK files are stored in `C:\Android\` (Windows) or `~/Android/` (macOS/Linux) and can consume several GB of disk space.
- **Updating SDK Components:** Run `sdkmanager --update` periodically to keep your SDK tools up to date.