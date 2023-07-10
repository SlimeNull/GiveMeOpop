using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GiveMeOpopNormal.Properties;
using NAudio.CoreAudioApi;
using NAudio.Utils;
using NAudio.Wave;

namespace GiveMeOpopNormal
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 设备枚举器
            var deviceEnumerator = new MMDeviceEnumerator();

            // 获取所有设备
            IEnumerable<MMDevice> devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            // 每一个设备都播放一个 O 泡果奶
            foreach (var device in devices)
            {
                // 从资源里面拿到流
                MemoryStream memoryStream = new MemoryStream(Resources.Opop);

                // 创建一个音频流
                WaveStream waveStream = new Mp3FileReader(memoryStream);

                // 根据当前设备创建一个输出
                WasapiOut wout = new WasapiOut(device, AudioClientShareMode.Shared, true, 200);
                wout.Init(waveStream);
                wout.Play();

                // 播放停止的时候, 重新开始播放
                wout.PlaybackStopped += (s, e) =>
                {
                    waveStream.Seek(0, SeekOrigin.Begin); // reset stream
                    wout.Play();
                };
            }

            // 循环音量最大化以及取消静音
            while (true)
            {
                foreach (var device in devices)
                {
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = 1f;
                    device.AudioEndpointVolume.Mute = false;
                }

                await Task.Delay(10);
            }
        }
    }
}