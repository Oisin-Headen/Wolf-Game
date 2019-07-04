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
        public readonly UnitController controller;

        private readonly GameModel gameModel;


        public int HealAmount
        {
            get => UnitType.BaseHealAmount; // TODO Affected by surroundings
        }
        public int CurrentHP { get; private set; }
        public int CurrentMovement { get; private set; }
        public SpaceModel Space { get; private set; }

        public readonly APlayer Player;

        public readonly UnitMovementOverseer MovementOverseer;


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

            MovementOverseer = new UnitMovementOverseer(this, gameModel);
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

        // does not check that this space is empty;
        internal void Enter(SpaceModel spaceModel)
        {
            Space.OccupingUnit = null;
            spaceModel.OccupingUnit = this;
            Space = spaceModel;
            foreach(var node in PathfindingDijkstras.GetFieldOfView(spaceModel, UnitType.VisionRange, 
                   spaceModel.map, UnitType.BlockLOS))
            {
                node.Space.Explore();
            }
            CurrentMovement = Math.Max(0, CurrentMovement - UnitType.MovementCostDeterminer.GetMovementCost(spaceModel));
            controller.MovePosition(spaceModel);
        }
    }
}