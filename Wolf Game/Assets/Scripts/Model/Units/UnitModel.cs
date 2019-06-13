using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Model
{
    public class UnitModel
    {
        public readonly UnitTypeModel UnitType;

        //private bool moving;
        private List<PathfindingNode> movementSpaces;

        private readonly UnitController controller;
        private readonly GameModel gameModel;


        public int HealAmount
        {
            get => UnitType.BaseHealAmount; // TODO Affected by surroundings
        }
        public int CurrentHP { get; private set; }
        public int CurrentMovement { get; private set; }
        public SpaceModel Space { get; private set; }
        public PreEndTurnMovementTask MovementTask { get; internal set; }

        public readonly APlayer Player;


        public UnitModel(UnitTypeModel unitType, SpaceModel space, APlayer player,  GameModel gameModel)
        {
            UnitType = unitType;

            CurrentHP = unitType.MaxHP;
            CurrentMovement = unitType.Movement;

            Space = space;
            space.OccupingUnit = this;

            Player = player;

            controller = gameModel.AddUnit(this);
            this.gameModel = gameModel;
        }



        // View all Spaces in Range
        public void Explore()
        {
            foreach (var node in PathfindingDijkstras.GetFieldOfView(Space, UnitType.VisionRange,
                gameModel.map, UnitType.BlockLOS))
            {
                node.Space.Explore();
            }
        }

        public void StartTurn()
        {
            // If the unit didn't move last turn, and it is damaged, heals a bit.
            if (CurrentMovement == UnitType.Movement && CurrentHP < UnitType.MaxHP)
            {
                CurrentHP = Math.Min(CurrentHP + HealAmount, UnitType.MaxHP);
            }
            CurrentMovement = UnitType.Movement;
        }

        public PlayerType GetPlayer()
        {
            return Player.thisplayer;
        }

        public void StartMove()
        {
            //moving = true;
            movementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(
                Space, CurrentMovement, UnitType.MovementCostDeterminer);


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