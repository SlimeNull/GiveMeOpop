using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace GenshinStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string? GenshinPath;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (GetGenshinPath() is string genshinPath)
            {
                GenshinPath = genshinPath;

                MessageBox.Show("我超, 原!\n这就帮你打开原神, 如果你关了, 你电脑就会蓝屏.", "我超, 原!", MessageBoxButton.OK, MessageBoxImage.Information);

                var deviceEnumerator = new MMDeviceEnumerator();
                var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

                WasapiCapture wasapiCapture = new WasapiCapture(defaultDevice);
                WaveInProvider waveIn = new WaveInProvider(wasapiCapture);

                foreach (var device in devices)
                {
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = 1;
                    device.AudioEndpointVolume.Mute = false;

                    if (device.ID == defaultDevice.ID)
                        continue;

                    WasapiOut wout = new WasapiOut();

                    wout.Init(waveIn);
                    wout.Play();
                }

                new MainWindow().Show();
            }
            else
            {
                MessageBox.Show("我超, 不是原!\n那没事了, 你, Safe!", "我超, 不是原!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            base.OnStartup(e);
        }

        public static string? GetGenshinPath()
        {
            var uninstall = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
            var subkeyNames = uninstall.GetSubKeyNames();

            try
            {
                foreach (var subkey in subkeyNames)
                {
                    if (subkey == "原神")
                    {
                        var genshin = uninstall.OpenSubKey(subkey);
                        var installLocation = genshin.GetValue("InstallPath");
                        return installLocation.ToString();
                    }
                }
            }
            catch { }

            return null;
        }

        public static Process? StartGenshinGame(string? genshinPath)
        {
            if (genshinPath == null)
                return null;

            var startExe = Path.Combine(genshinPath, @"Genshin Impact Game\YuanShen.exe");
            if (!File.Exists(startExe))
                return null;

            return Process.Start(
                new ProcessStartInfo()
                {
                    FileName = startExe,
                    UseShellExecute = true,
                });
        }
    }
}
