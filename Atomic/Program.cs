﻿using System;

namespace Atomic
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new App())
                game.Run();
        }
    }
}