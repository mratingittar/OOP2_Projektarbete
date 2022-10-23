﻿using Skalm.GameObjects;
using Skalm.Input;
using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.States
{
    internal class PlayerStateMove : IPlayerState
    {
        public static event Action? OnPauseMenuRequested;
        private Player _player;

        public PlayerStateMove(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void MoveInput(Vector2Int direction)
        {
            _player.Move(direction);
        }
        public void CommandInput(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.Default:
                    break;
                case InputCommands.Confirm:
                    break;
                case InputCommands.Cancel:
                    OnPauseMenuRequested?.Invoke();
                    break;
                case InputCommands.Interact:
                    _player.playerStateMachine.ChangeState(PlayerStates.PlayerStateInteract);
                    break;
                case InputCommands.Inventory:
                    _player.playerStateMachine.ChangeState(PlayerStates.PlayerStateMenu);
                    break;
            }
        }
    }
}
