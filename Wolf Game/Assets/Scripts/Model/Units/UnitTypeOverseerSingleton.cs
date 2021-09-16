using System;
using Pathfinding;
namespace Model
{
    public class UnitTypeOverseerSingleton
    {
        private static UnitTypeOverseerSingleton instance;
        private UnitTypeOverseerSingleton()
        {
            Wolf = new UnitTypeModel(
                UnitTypeModel.UnitTypes.Wolf,
                UnitTypeModel.UnitKind.Normal,
                5, // attack
                5, // defence
                5, // Max HP
                3, // movement
                1, // heal amount
                3, // Vision range
                "Wolf",
                new IgnoreForestLOS(),
                new OrdinaryMovementCost()
                );

            BattleSpider = new UnitTypeModel(
                UnitTypeModel.UnitTypes.BattleSpider,
                UnitTypeModel.UnitKind.Normal,
                7, // attack
                5, // defence
                7, // Max HP
                2, // movement
                2, // heal amount
                2, // Vision range
                "Battle Spider",
                new NormalLOS(),
                new DeepForestAtHalfMovementCostForestAtOne()
                );
            WorkerSpider = new UnitTypeModel(
                UnitTypeModel.UnitTypes.WorkerSpider,
                UnitTypeModel.UnitKind.Worker,
                1, // attack
                2, // defence
                3, // Max HP
                1, // movement
                1, // heal amount
                2, // Vision range
                "Worker Spider",
                new NormalLOS(),
                new DeepForestAtHalfMovementCostForestAtOne()
                );

            ScoutEagle = new UnitTypeModel(
                UnitTypeModel.UnitTypes.ScoutEagle,
                UnitTypeModel.UnitKind.Normal,
                2, // attack
                2, // defence
                3, // Max HP
                3, // movement
                1, // heal amount
                2, // Vision range
                "Scout Eagle",
                new NormalLOS(),
                new OneCostMovement()
                );


            // TODO test unit I'm going to leave around the map.
            Juggernaut = new UnitTypeModel(
                UnitTypeModel.UnitTypes.ScoutEagle,
                UnitTypeModel.UnitKind.Normal,
                10, // attack
                10, // defence
                30, // Max HP
                0, // movement
                3, // heal amount
                2, // Vision range
                "Juggernaut",
                new IgnoreForestLOS(),
                new OrdinaryMovementCost()
                );
        }

        // Unit Types
        public readonly UnitTypeModel Wolf;
        public readonly UnitTypeModel BattleSpider;
        public readonly UnitTypeModel WorkerSpider;
        public readonly UnitTypeModel ScoutEagle;

        //TODO: test target. 
        public readonly UnitTypeModel Juggernaut;

        public static UnitTypeOverseerSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new UnitTypeOverseerSingleton();
            }
            return instance;
        }
    }
}