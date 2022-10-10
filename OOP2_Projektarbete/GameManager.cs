﻿using Skalm.Display;
using Skalm.Input;
using Skalm.Map;
using Skalm.Menu;
using Skalm.Sounds;
using Skalm.States;
using Skalm.Structs;

namespace Skalm
{
    internal class GameManager
    {

        #region FIELDS
        public IGameState GameState;
        private int updateFrequency = Globals.G_UPDATE_FREQUENCY;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        public MapManager mapManager;
        public DisplayManager displayManager; 
        private MenuManager menuManager;
        #endregion

        public GameManager()
        {
            mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new DisplayManager(mapManager, new ConsoleWindowPrinter(ConsoleColor.White, ConsoleColor.Black), new ConsoleWindowEraser());
            
            GameState = new GameStateInitializing(displayManager);
            GameState.Enter();

            soundManager = new SoundManager();
            soundManager.PlayMusic(soundManager.defaultSound);

            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            menuManager = new MenuManager(inputManager, displayManager);
            menuManager.mainMenu.onMenuExecution += MainMenuExecution;
        }



        public void Start()
        {
            ChangeGameState(new GameStateMainMenu(displayManager, menuManager));
            Update();
        }

        private void Update()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    inputManager.GetInput(); // INPUTS ARE QUEUEING UP, NEEDS FIXING.
                }


                else if (GameState is GameStatePlaying)
                {

                }

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }

        private void MainMenuExecution(string item)
        {
            if (GameState is not GameStateMainMenu)
                return;

            switch (item)
            {
                case "Start New Game":
                    ChangeGameState(new GameStatePlaying(displayManager));
                    break;
                case "Continue":
                    break;
                case "Toggle Beep":
                    soundManager.beepsEnabled = !soundManager.beepsEnabled;
                    break;
                default:
                    break;
            }
        }
    }
}