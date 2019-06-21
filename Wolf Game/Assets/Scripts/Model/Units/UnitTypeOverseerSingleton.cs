﻿using System;
    public class UnitTypeOverseerSingleton
    {
        private static UnitTypeOverseerSingleton instance;
        private UnitTypeOverseerSingleton()
        {
        }

        // Unit Types
        public readonly UnitTypeModel Wolf;
        public readonly UnitTypeModel WorkerSpider;
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