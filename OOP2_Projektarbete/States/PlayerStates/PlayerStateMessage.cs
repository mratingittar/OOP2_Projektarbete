﻿using Skalm.Display;
using Skalm.GameObjects;
using Skalm.Input;
using Skalm.Structs;


namespace Skalm.States.PlayerStates
{
    internal class PlayerStateMessage : IPlayerState
    {
        private Player _player;
        private DisplayManager _displayManager;

        public PlayerStateMessage(Player player, DisplayManager displayManager)
        {
            _player = player;
            _displayManager = displayManager;
        }


        public void Enter()
        {
            ReadMessage();
        }

        public void Exit()
        {
            _displayManager.ClearMessageSection();
            Console.CursorVisible = false;
        }

        public void MoveInput(Vector2Int direction)
        {
            switch (direction)
            {
                default:
                    ReadMessage();
                    break;
            }
        }
        public void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    ReadMessage();
                    break;
                case InputCommands.Cancel:
                    ReadMessage();
                    break;
                case InputCommands.Interact:
                    ReadMessage();
                    break;
                case InputCommands.Inventory:
                    break;
            }
        }

        private void ReadMessage()
        {
            if (_displayManager.MessagesInQueue == 0)
                _player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
            else
                _displayManager.DisplayNextMessage();
        }
    }
}
