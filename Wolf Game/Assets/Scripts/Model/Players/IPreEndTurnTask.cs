using System;
using Model;

namespace Pathfinding
{
    public interface IPreEndTurnTask
    {
        void Show();
        bool Complete();
        //void MarkComplete();
    }

    public class PreEndTurnMovementTask : IPreEndTurnTask
    {
        private readonly UnitModel unit;
        private bool complete;

        public PreEndTurnMovementTask(UnitModel unit)
        {
            complete = false;
            this.unit = unit;
            unit.MovementTask = this;
        }

        public void Show()
        {
            unit.Space.Clicked();
        }

        public bool Complete()
        {
            return complete;
        }

        public void MarkComplete()
        {
            complete = true;
        }
    }
}

