using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel : IBlockLOS
{
    public UnitModel(int attack, int defence, int maxHP, int movement, int healAmount, int visionRange,
        SpaceModel space, AbstractPlayer player, GameModel gameModel, IBlockLOS blockLOS)
    {
        Attack = attack;
        Defence = defence;
        MaxHP = maxHP;
        Movement = movement;
        HealAmount = healAmount;
        VisionRange = visionRange;

        CurrentHP = maxHP;
        CurrentMovement = movement;

        Space = space;
        space.OccupingUnit = this;
        this.player = player;
        controller = gameModel.AddUnit(this);
        this.gameModel = gameModel;
        this.blockLOS = blockLOS;
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

    // View all Spaces in Range
    public void Explore()
    {
        foreach(var node in PathfindingDijkstras.GetFieldOfView(Space, VisionRange, 
            gameModel.map, blockLOS))
        {
            node.GetSpace().Explore();
        }
    }

    public int CurrentMovement { get; private set; }
    public SpaceModel Space { get; private set; }

    public PreEndTurnTask MovementTask { get; internal set; }



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
            Space, CurrentMovement);


        foreach(var node in movementSpaces)
        {
            node.GetSpace().Moveable();
            //node.GetSpace().controller.SetText(node.GetCost());
        }
    }

    public void FinishMove(SpaceModel spaceModel)
    {
        foreach (var node in movementSpaces)
        {
            if(node.GetSpace() == spaceModel)
            {
                Space = node.GetSpace();
                CurrentMovement = Math.Max(0, CurrentMovement - node.GetCost());
                controller.MovePosition(node);
                Space.OccupingUnit = null;
                node.GetSpace().OccupingUnit = this;
            }
            node.GetSpace().Deselect();
        }
        movementSpaces = null;
        gameModel.SetSelectedUnit(null);
        Explore();

        MovementTask.Complete = true;
    }

    public bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space)
    {
        return blockLOS.BlocksLOS(elevation, space);
    }
}
