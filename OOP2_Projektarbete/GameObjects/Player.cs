﻿using Skalm.Display;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Maps;
using Skalm.Maps.Tiles;
using Skalm.States.PlayerStates;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects
{
    internal class Player : Actor
    {
        // PROPERTIES
        internal PlayerStateMachine PlayerStateMachine { get => _playerStateMachine; }
        public EquipmentManager EquipmentManager { get => _equipmentManager; }

        // EVENTS
        public event Action? OnPlayerTurn;
        public static event Action<ActorStatsObject, int>? OnPlayerStatsUpdated;
        public static event Action<EquipmentManager>? OnPlayerEquipmentUpdated;
        public static event Action<EquipmentManager>? OnPlayerInventoryUpdated;

        // FIELDS
        private PlayerStateMachine _playerStateMachine;
        private EquipmentManager _equipmentManager;
        private int _currentFloor;

        private bool _statUpdateWaiting;
        private bool _equipUpdateWaiting;
        private bool _inventoryUpdateWaiting;

        // CONSTRUCTOR I
        public Player(MapManager mapManager, DisplayManager displayManager, IAttackComponent attack, ActorStatsObject statsObject, string name, Vector2Int gridPosition, char sprite = '@', ConsoleColor color = ConsoleColor.White) 
            : base(mapManager, attack, statsObject, gridPosition, sprite, color)
        {
            _playerStateMachine = new PlayerStateMachine(this, displayManager, mapManager, EPlayerStates.PlayerStateIdle);

            // INVENTORY
            _equipmentManager = new EquipmentManager();
            _equipmentManager.inventory.OnInventoryChanged += UpdateInventoryDisplay;

            // INITIALIZE PLAYER
            InitializePlayer(gridPosition, name, sprite, color);
            statsObject.OnStatsChanged += UpdateStatDisplay;
        }

        // INITIALIZE PLAYER
        public void InitializePlayer(Vector2Int gridPosition, string playerName, char sprite, ConsoleColor color)
        {
            // SET POSITION & MAP CHARACTERISTICS
            SetPlayerPosition(gridPosition);
            _sprite = sprite;
            _color = color;

            // SET STATS OBJECT & INVENTORY
            statsObject = GeneratePlayerStats(playerName, 15);
            statsObject.ResetHP();
            statsObject.name = playerName;
            _equipmentManager.ResetInventory();
            _equipmentManager.inventory.Keys = 0;

            statsObject.OnStatsChanged += UpdateStatDisplay;

            // RESET PROGRESS
            _currentFloor = 0;
        }

        public void SetPlayerPosition(Vector2Int gridPosition) => GridPosition = gridPosition;
        public void NextFloor() => _currentFloor++;
        public void AddItemToInventory(Item item) => _equipmentManager.AddItemToInventory(item);

        // UPDATE STATS DISPLAY
        public void UpdateStatDisplay() => _statUpdateWaiting = true;

        // UPDATE EQUIPMENT DISPLAY
        public void UpdateEquipmentDisplay() => _equipUpdateWaiting = true;

        // UPDATE INVENTORY DISPLAY
        public void UpdateInventoryDisplay() => _inventoryUpdateWaiting = true;

        // SEND STATS TO DISPLAY
        public void UpdateAllDisplays()
        {
            UpdateStatDisplay();
            UpdateEquipmentDisplay();
            UpdateInventoryDisplay();
        }

        public override void UpdateMain()
        {
            if (_statUpdateWaiting)
            {
                OnPlayerStatsUpdated?.Invoke(statsObject, _currentFloor);
                _statUpdateWaiting = false;
            }

            if (_equipUpdateWaiting)
            {
                OnPlayerEquipmentUpdated?.Invoke(_equipmentManager);
                _equipUpdateWaiting = false;
            }

            if (_inventoryUpdateWaiting)
            {
                OnPlayerInventoryUpdated?.Invoke(_equipmentManager);
                _inventoryUpdateWaiting = false;
            }
        }

        // MOVE METHOD
        public override void Move(Vector2Int direction)
        {
            base.Move(direction);
            OnPlayerTurn?.Invoke();
        }

        // INTERACT WITH NEIGHBOURS
        public void InteractWithNeighbor(BaseTile neighbor)
        {
            // CHECK FOR OCCUPIABLE TILE
            if ((neighbor is IOccupiable occupiable)
                && (occupiable.ObjectsOnTile.Count > 0))
            {
                // IF PICKUP ITEM, PICK IT UP
                foreach (var item in occupiable.ObjectsOnTile)
                {
                    if (item is ItemPickup i)
                    {
                        i.Interact(this);
                        break;
                    }
                }
                OnPlayerTurn?.Invoke();
                return;
            }

            // CHECK FOR INTERACTABLE TILE
            if (neighbor is IInteractable interactable)
            {
                interactable.Interact(this);
                _mapManager.MapPrinter.DrawSingleTile(neighbor.GridPosition);
                OnPlayerTurn?.Invoke();
            }
        }

        // GENERATE RANDOM PLAYER STATS OBJECT
        private ActorStatsObject GeneratePlayerStats(string name, int startingHP, int statPoints = 40, int statMinimum = 3)
        {
            // PLAYER STATS OBJECT
            StatsObject stats = new StatsObject(statMinimum, statMinimum, statMinimum, statMinimum, statMinimum, startingHP, 1, 1);

            // RANDOMIZE STATS
            int statTmp;
            statPoints -= (statMinimum * 5);
            for (int i = 0; i < statPoints; i++)
            {
                statTmp = Dice.rng.Next(0, 6);
                stats.statsArr[statTmp].AddValue(1);
            }

            // CREATE & RETURN ACTOR STATS OBJECT
            return new ActorStatsObject(stats, name, 0);
        }
    }
}
