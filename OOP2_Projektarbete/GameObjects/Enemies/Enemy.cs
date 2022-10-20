﻿using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.States;
using Skalm.Structs;
using System.ComponentModel.DataAnnotations;

namespace Skalm.GameObjects.Enemies
{
    internal class Enemy : Actor, IDamageable
    {
        // MANAGERS
        private SceneManager _sceneManager;
        private MapManager _mapManager;

        private EnemyStateMachine stateMachine;

        // COMPONENTS
        private IMoveBehaviour moveBehaviour;
        private IAttackComponent attackBehaviour;

        // STATS
        public ActorStatsObject statsObject { get; set; }

        // CONSTRUCTOR I
        public Enemy(MapManager mapManager, SceneManager sceneManager, IMoveBehaviour moveBehaviour, IAttackComponent attackBehaviour, Vector2Int gridPosition, char sprite, ConsoleColor color) 
            : base(gridPosition, sprite, color)
        {
            _mapManager = mapManager;
            _sceneManager = sceneManager;
            this.moveBehaviour = moveBehaviour;
            this.attackBehaviour = attackBehaviour;
            stateMachine = new EnemyStateMachine(this, EnemyStates.EnemyStateIdle);

            // STATS
            statsObject = new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), "Monster");

            Player.playerTurn += MoveEnemy;
            stateMachine.ChangeState(EnemyStates.EnemyStateSearching);
        }

        // MOVEMENT
        public void MoveEnemy()
        {
            Move(moveBehaviour.MoveDirection(GridPosition));
        }

        // MOVE
        public override void Move(Vector2Int direction)
        {
            Vector2Int newPosition = GridPosition.Add(direction);

            if (newPosition.Equals(GridPosition))
                return;

            if (!_mapManager.TileGrid.TryGetGridObject(newPosition, out var tile))
                return;

            if (tile is ICollider collider && collider.ColliderIsActive)
            {
                collider.OnCollision();
                return;
            }

            if (tile is IOccupiable occupiable && occupiable.ActorPresent)
            {
                var obj = occupiable.ObjectsOnTile.Where(o => o is IDamageable).FirstOrDefault() as IDamageable;
                //obj?.TakeDamage(attackBehaviour.Attack());

                return;
            }


            ExecuteMove(newPosition, GridPosition);
        }

        // SET BEHAVIOUR
        public void SetMoveBehaviour(EnemyMoveBehaviours behaviour)
        {
            switch (behaviour)
            {
                case EnemyMoveBehaviours.Idle:
                    moveBehaviour = new MoveIdle();
                    break;
                case EnemyMoveBehaviours.Random:
                    moveBehaviour = new MoveRandom();
                    break;
                case EnemyMoveBehaviours.Pathfinding:
                    moveBehaviour = new MovePathfinding(_mapManager, _sceneManager);
                    break;
            }
        }

        // TAKE DAMAGE
        public void TakeDamage(DoDamage damage)
        {
            statsObject.TakeDamage(damage);
        }
    }

    // ENUM ENEMY BEHAVIOURS
    internal enum EnemyMoveBehaviours
    {
        Idle,
        Random,
        Pathfinding
    }
}

