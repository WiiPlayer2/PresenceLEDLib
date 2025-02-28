﻿using System;

namespace DarkLink.AnimLED.Commands
{
    public abstract class Command
    {
        public abstract CommandType Type { get; }

        public abstract byte[] Serialize();
    }
}
