using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;

namespace Model
{
    public abstract class APlayer
    {
        public List<UnitModel> units;
        public readonly PlayerType thisplayer;
        protected GameModel gameModel;

        private List<IPreEndTurnTask> tasks;

        public APlayer(PlayerType newplayer, GameModel gameModel)
        {
            this.gameModel = gameModel;
            thisplayer = newplayer;
            units = new List<UnitModel>();
        }
        //public abstract void EndTurn();
        public void StartTurn()
        {
            tasks = new List<IPreEndTurnTask>();
            foreach (var unit in units)
            {
                unit.StartTurn();
                tasks.Add(new PreEndTurnMovementTask(unit));
            }
        }

        // See if there are any tasks the player needs to complete before ending their turn
        public bool CanEndTurn()
        {
            bool canEndTurn = true;
            foreach (var task in tasks)
            {
                canEndTurn &= task.Complete();
            }
            return canEndTurn;
        }

        // Display the first task that hasn't been completed
        public void PreEndTurnTask()
        {
            bool foundTask = false;
            foreach (var task in tasks)
            {
                if (!foundTask && !task.Complete())
                {
                    task.Show();
                    foundTask = true;
                }
            }
        }

        internal EndTurnButtonResult TryEndTurn()
        {
            bool notThreading = true;

            foreach (var task in tasks)
            {
                notThreading &= task.TryComplete();
            }

            if (!notThreading)
            {
                return EndTurnButtonResult.Threading;
            }
            if (CanEndTurn())
            {
                return EndTurnButtonResult.Normal;
            }
            return EndTurnButtonResult.Show_Task;
        }
    }
}
