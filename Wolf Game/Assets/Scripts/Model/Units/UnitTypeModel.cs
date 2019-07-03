using System;
using Pathfinding;
namespace Model
{
    public class UnitTypeModel
    {
        public readonly UnitTypes ID; public readonly UnitKind iconKind;
        public int Attack { get; private set; }
        public int Defence { get; private set; }
        public int MaxHP { get; private set; }
        public int Movement { get; private set; }
        public int VisionRange { get; private set; }
        public int BaseHealAmount { get; private set; }
        public string Description { get; private set;}

        public IBlockLOS BlockLOS { get; private set; }
        public IMovementCost MovementCostDeterminer { get; private set; }


        public UnitTypeModel(UnitTypes id, UnitKind iconKind, int attack, int defence, int maxHP, int movementNumSpaces, int healAmount, int visionRange, string desc, IBlockLOS blockLOS, IMovementCost movementCostDeterminer)
        {
            ID = id; this.iconKind = iconKind;
            Attack = attack;
            Defence = defence;
            MaxHP = maxHP;
            Movement = movementNumSpaces * PathfindingDijkstras.ONE_SPACE; BaseHealAmount = healAmount; VisionRange = visionRange; BlockLOS = blockLOS; MovementCostDeterminer = movementCostDeterminer;
            Description = desc;
        }
        public enum UnitTypes
        {
            Wolf, BattleSpider, WorkerSpider, ScoutEagle
        }
        public enum UnitKind
        {
            Normal, Worker, Single
        }
    }
}