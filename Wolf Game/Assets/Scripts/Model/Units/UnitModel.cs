using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Model
{
    public class UnitModel
    {
        public UnitModel(int attack, int defence, int maxHP, int movementNumSpaces, int healAmount, int visionRange,
            SpaceModel space, AbstractPlayer player, GameModel gameModel, IBlockLOS blockLOS, IMovementCost movementCostDeterminer)
        {
            Attack = attack;
            Defence = defence;
            MaxHP = maxHP;
            // Max movement equal to number of spaces times cost.
            Movement = movementNumSpaces * PathfindingDijkstras.ONE_SPACE;
            HealAmount = healAmount;
            VisionRange = visionRange;

            CurrentHP = maxHP;
            CurrentMovement = Movement;

            Space = space;
            space.OccupingUnit = this;
            this.player = player;
            controller = gameModel.AddUnit(this);
            this.gameModel = gameModel;
            this.blockLOS = blockLOS;
            this.MovementCostDeterminer = movementCostDeterminer;
        }

        private readonly IBlockLOS blockLOS;
        //private bool moving;
        private List<PathfindingNode> movementSpaces;

        private readonly AbstractPlayer player;
        private readonly UnitController controller;
        private readonly GameModel gameModel;

        public int Attack { get; private set; }
        public int Defence { get; private set; }
        public int MaxHP { get; private set; }
        public int Movement { get; private set; }
        // TODO Heal Amount might increase based on other factors
        private int healAmount;
        public int HealAmount
        {
            get => healAmount;
            private set => healAmount = value;
        }
        public int VisionRange { get; private set; }
        public int CurrentHP { get; private set; }
        public int CurrentMovement { get; private set; }
        public SpaceModel Space { get; private set; }
        public PreEndTurnMovementTask MovementTask { get; internal set; }
        public IMovementCost MovementCostDeterminer { get; }


        // View all Spaces in Range
        public void Explore()
        {
            foreach (var node in Pathfinding.PathfindingDijkstras.GetFieldOfView(Space, VisionRange,
                gameModel.map, blockLOS))
            {
                node.Space.Explore();
            }
        }

        public void StartTurn()
        {
            // If the unit didn't move last turn, and it is damaged, heals a bit.
            if (CurrentMovement == Movement && CurrentHP < MaxHP)
            {
                CurrentHP = Math.Min(CurrentHP + HealAmount, MaxHP);
            }
            CurrentMovement = Movement;
        }

        public Player GetPlayer()
        {
            return player.thisplayer;
        }

        public void StartMove()
        {
            //moving = true;
            movementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(
                Space, CurrentMovement, MovementCostDeterminer);


            foreach (var node in movementSpaces)
            {
                node.Space.Moveable();
                //node.GetSpace().controller.SetText(node.GetCost());
            }
            gameModel.Moving = true;
        }

        public void FinishMove(SpaceModel spaceModel)
        {
            foreach (var node in movementSpaces)
            {
                if (node.Space == spaceModel)
                {
                    CurrentMovement = Math.Max(0, CurrentMovement - node.Cost);
                    controller.MovePosition(node.Space);
                    Space.OccupingUnit = null;
                    Space = node.Space;
                    Space.OccupingUnit = this;
                }
                node.Space.Deselect();
            }
            movementSpaces = null;
            gameModel.SetSelectedUnit(null);
            Explore();

            if (CurrentMovement <= 0)
            {
                MovementTask.MarkComplete();
            }

            gameModel.Moving = false;
            gameModel.FinishedMovement();
        }
    }
}