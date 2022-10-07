﻿using Skalm.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class GameStatePlaying : IGameState
    {
        private readonly DisplayManager _displayManager;

        public GameStatePlaying(DisplayManager displayManager)
        {
            _displayManager = displayManager;
        }

        public void Enter()
        {
            DisplayManager.Print("Starting new game", 0, Console.WindowHeight / 2, true);
            Thread.Sleep(500);
            Console.Clear();
            _displayManager.DisplayHUD();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

    }
}
