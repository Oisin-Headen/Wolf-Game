using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel
{
    private int healAmount;
    private readonly AbstractPlayer player;
    private readonly UnitController controller;

    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int MaxHP { get; private set; }
    public int Movement { get; private set; }

    // TODO Heal Amount might increase based on other factors
    public int HealAmount 
    { 
        get => healAmount; 
        private set => healAmount = value; 
    }

    public int VisionRange { get; private set; }


    public int CurrentHP { get; private set; }
    public int CurrentMovement { get; private set; }
    public SpaceModel Space { get; private set; }

    public UnitModel(int attack, int defence, int maxHP, int movement, int healAmount, int visionRange,
        SpaceModel space, AbstractPlayer player, GameModel gameModel)
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

    public void Move()
    {
        var movementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(
            Space, CurrentMovement);

        Debug.Log(CurrentMovement);

        foreach(var node in movementSpaces)
        {
            node.GetSpace().Moveable();
        }
    }
}
