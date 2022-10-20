﻿using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;

namespace Skalm
{
    internal class SceneManager
    {
        public string playerName = "";

        private MapManager _mapManager;
        private EnemySpawner _enemySpawner;
        private ItemSpawner _itemSpawner;

        public List<GameObject> GameObjectsInScene { get; }
        public List<Actor> ActorsInScene { get; }
        public Player Player { get; }

        // CONSTRUCTOR I
        public SceneManager(MapManager mapManager)
        {
            _mapManager = mapManager;
            Player = new Player(mapManager, Vector2Int.Zero, new PlayerAttackComponent(), "Nameless");
            ActorsInScene = new List<Actor>();
            GameObjectsInScene = new List<GameObject>();

            _enemySpawner = new EnemySpawner(mapManager, this);
            _itemSpawner = new ItemSpawner();
        }

        // INITIALIZE SCENE
        public void InitializeScene()
        {
            Vector2Int spawnPos = _mapManager.GetRandomSpawnPosition();
            if (playerName.Length == 0)
                playerName = "Nameless";

            Player.InitializePlayer(spawnPos, playerName, 'P', ConsoleColor.Cyan);

            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);

            // ADD ENEMIES
            Enemy enemy = _enemySpawner.Spawn(_mapManager.GetRandomSpawnPosition(), 'E', ConsoleColor.Red);
            GameObjectsInScene.Add(enemy);
            ActorsInScene.Add(enemy);

            // ADD ITEMS
            var freeTiles = _mapManager.mapGenerator.freeTiles;
            var itemXY = freeTiles.ElementAt(Dice.rng.Next(0, freeTiles.Count));

            ItemEquippable item1 = new ItemEquippable("Helmet of misfortune", (int)EEqSlots.Head, new StatsObject(0, 0, 2, 0, 1, 5, 0, 2));

            GameObjectsInScene.Add(_itemSpawner.Spawn(itemXY, 'o', ConsoleColor.Yellow, item1));

            AddObjectsToMap();
        }

        // ADD OBJECTS TO MAP
        private void AddObjectsToMap()
        {
            foreach (GameObject go in GameObjectsInScene)
            {
                _mapManager.TileGrid.TryGetGridObject(go.GridPosition, out BaseTile tile);
                ((IOccupiable)tile).ObjectsOnTile.Add(go);
            }

            foreach (Actor actor in ActorsInScene)
            {
                _mapManager.TileGrid.TryGetGridObject(actor.GridPosition, out BaseTile tile);
                ((IOccupiable)tile).ActorPresent = true;
            }
        }

        // RESET OBJECTS IN SCENE
        public void ResetObjectsInScene()
        {
            foreach (var go in GameObjectsInScene)
            {
                _mapManager.TileGrid.TryGetGridObject(go.GridPosition, out BaseTile tile);
                ((IOccupiable)tile).ObjectsOnTile.Clear();
            }

            foreach (Actor actor in ActorsInScene)
            {
                _mapManager.TileGrid.TryGetGridObject(actor.GridPosition, out BaseTile tile);
                ((IOccupiable)tile).ActorPresent = false;                
            }

            ActorsInScene.Clear();
            GameObjectsInScene.Clear();
        }

        // ENEMY FACTORY

        // ITEM FACTORY
    }
}
