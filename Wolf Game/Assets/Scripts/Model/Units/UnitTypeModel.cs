﻿using System;

{
    public class UnitTypeModel
        public int Attack { get; private set; }
        public int Defence { get; private set; }
        public int MaxHP { get; private set; }
        public int Movement { get; private set; }
        public int VisionRange { get; private set; }
        public int BaseHealAmount { get; private set; }

        public IBlockLOS BlockLOS { get; private set;  }
        public IMovementCost MovementCostDeterminer { get; private set; }


        public UnitTypeModel(UnitTypes id, UnitKind iconKind, int attack, int defence, int maxHP, int movementNumSpaces, 
        {
            Attack = attack;
            Defence = defence;
            MaxHP = maxHP;
            Movement = movementNumSpaces * PathfindingDijkstras.ONE_SPACE;
        }
        {
            Wolf, BattleSpider, WorkerSpider
        }
        {
            Normal, Worker
        }
    }
}