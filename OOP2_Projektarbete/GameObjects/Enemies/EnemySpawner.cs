﻿using Skalm.GameObjects.Interfaces;
using Skalm.Maps;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm.GameObjects.Enemies
{
    internal class EnemySpawner: ISpawner<Enemy>, IScalable
    {
        public float ScalingMultiplier { get; set ; }
        private float _scaledModifier => Calculations.SpawningScalingEquation(_baseModifier, ScalingMultiplier);
        private float _baseModifier;
        private Player _player;
        private MonsterGen _monsterGen;
        private MapManager _mapManager;
        private char _monsterSprite;
        private ConsoleColor _monsterColor;

        public EnemySpawner(float baseModifier, char monsterSprite, ConsoleColor monsterColor, MapManager mapManager, Player player, MonsterGen monsterGen)
        {
            _baseModifier = baseModifier;
            _mapManager = mapManager;
            _player = player;
            _monsterGen = monsterGen;
            _monsterSprite = monsterSprite;
            _monsterColor = monsterColor;
        }

        public Enemy Spawn(Vector2Int gridPosition, float scalingMod = 1)
        {
            return new Enemy(_mapManager, _player, new MovePathfinding(_mapManager, _player), new AttackNormal(), 
                _monsterGen.GetWeightedRandom(_scaledModifier), gridPosition, _monsterSprite, _monsterColor);
        }
    }
}
