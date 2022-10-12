﻿using Skalm.Actors.Tile;
using Skalm.Display;
using Skalm.Grid;
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
        private List<IGameState> gameStates;
        private int updateFrequency = Globals.G_UPDATE_FREQUENCY;

        // MANAGERS
        private InputManager inputManager;
        private SoundManager soundManager;
        private MapManager mapManager;
        private DisplayManager displayManager;
        private MenuManager menuManager;

        private List<char> animationTest;
        private int animationFrame;
        #endregion

        // CONSTRUCTOR I
        public GameManager()
        {
            //mapManager = new MapManager(32, 32, Vector2Int.Zero);
            displayManager = new DisplayManager(new ConsoleWindowPrinter(ConsoleColor.White, ConsoleColor.Black), new ConsoleWindowEraser(), new ConsoleWindowInfo());

            GameState = new GameStateInitializing(displayManager);
            GameState.Enter();

            mapManager = new MapManager(new Grid2D<BaseTile>(displayManager.gridMapRect.Width, displayManager.gridMapRect.Height, 2, 1, displayManager.pixelGridController.cellsInSections["MapSection"].First().planePositions.First(), (x,y, gridPosition) => new VoidTile(new Vector2Int(x, y))), displayManager);
            
            soundManager = new SoundManager(new ConsoleSoundPlayer(Globals.G_SOUNDS_FOLDER_PATH));

            inputManager = new InputManager(new MoveInputArrowKeys(), new CommandInputKeyboard());
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            menuManager = new MenuManager(inputManager, displayManager, soundManager);
            menuManager.mainMenu.onMenuExecution += MenuExecution;
            menuManager.pauseMenu.onMenuExecution += MenuExecution;

            gameStates = new List<IGameState>
            {
                new GameStateInitializing(displayManager),
                new GameStateMainMenu(menuManager),
                new GameStatePaused(menuManager),
                new GameStatePlaying(displayManager, mapManager)
            };

            animationTest = new List<char> { ' ', '░', '▒', '▓', '█', '▓', '▒', '░'};
            animationFrame = 0;
        }

        // METHOD START STATE
        public void Start()
        {
            ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);           
            soundManager.player.Play(soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));
            Update();
        }

        // METHOD ANIMATE
        private void Animate() 
        {
            if (animationFrame == animationTest.Count)
                animationFrame = 0;

            displayManager.printer.PrintAtPosition(animationTest[animationFrame], 10, 10);
            animationFrame++;
        }

        // METHOD UPDATE GAME
        private void Update()
        {
            while (true)
            {
                inputManager.GetInput();

                Thread.Sleep(1000 / updateFrequency);
            }
        }

        // METHOD CHANGE STATE
        public void ChangeGameState(IGameState gameState)
        {
            GameState.Exit();
            GameState = gameState;
            GameState.Enter();
        }

        // METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            if (GameState is GameStateMainMenu or GameStatePaused)
            {
                menuManager.TraverseMenu(direction);
            }
            else if (GameState is GameStatePlaying)
            {

            }
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            if (GameState is GameStateMainMenu or GameStatePaused)
            {
                menuManager.ExecuteMenu(command);
            }
            else if (GameState is GameStatePlaying)
            {
                if (command == InputCommands.Cancel)
                    ChangeGameState(gameStates.Find(state => state is GameStatePaused)!);
            }
        }

        // METHOD EXECUTE MENU INDEX
        private void MenuExecution(string menuPage, string item)
        {
            if (GameState is not GameStateMainMenu and not GameStatePaused)
                return;

            switch (menuPage)
            {
                case "MAIN MENU":
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;
                case "NEW GAME":
                    if (item == "Start New Game")
                        ChangeGameState(new GameStatePlaying(displayManager, mapManager));
                    break;
                case "OPTIONS":
                    if (item == "Toggle Beep")
                        soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
                    break;
                case "MUSIC":
                    soundManager.player.Play(soundManager.Tracks.Find(sound => sound.soundName == item));
                    break;
                case "INPUT METHOD":
                    inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    break;
                case "PAUSE MENU":
                    if (item == "Resume")
                        ChangeGameState(gameStates.Find(state => state is GameStatePlaying)!);
                    else if (item == "Exit")
                        ChangeGameState(gameStates.Find(state => state is GameStateMainMenu)!);
                    break;
            }
        }
    }
}