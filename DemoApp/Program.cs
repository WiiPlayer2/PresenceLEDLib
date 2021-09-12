﻿using System;
using System.IO.Ports;
using System.Threading.Tasks;
using PresenceLEDLib;
using PresenceLEDLib.Commands;
using PresenceLEDLib.Formats;
using PresenceLEDLib.Types;

namespace DemoApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var serialPort = new SerialPort("COM6", 115200)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 2000,
            };
            serialPort.Open();

            var metaData = new PaletteMetaData(new ColorR8G8B8[]
            {
                new(0x00, 0x00, 0x00),
                new(0xFF, 0x00, 0xFF),
            });
            var imageData = new PaletteImageData(8, 8, metaData, 4, 1);
            imageData.Frames[0][0, 0] = 1;
            imageData.Frames[1][7, 0] = 1;
            imageData.Frames[2][7, 7] = 1;
            imageData.Frames[3][0, 7] = 1;

            var device = new Device(serialPort.BaseStream);
            await device.Call(new Update
            {
                ColorFormat = imageData.Format,
                Frames = (byte) imageData.Frames.Count,
                Delay = 100,
                Loop = true,
            });
            await device.SendPacket(imageData.Serialize());
        }
    }
}
