using System;
using System.Threading.Tasks;
using Model;

namespace Model
{
    public interface IPreEndTurnTask
    {
        void Show();
        bool Complete();
        bool TryComplete();
        //void MarkComplete();
    }

    public class PreEndTurnMovementTask : IPreEndTurnTask
    {
        private UnitMovementOverseer overseer;
        private readonly UnitModel unit;
        private bool complete;

        public PreEndTurnMovementTask(UnitModel unit)
        {
            complete = false;
            this.unit = unit;
            unit.MovementOverseer.MovementTask = this;
            overseer = unit.MovementOverseer;
        }

        public void Show()
        {
            unit.Space.Clicked();
        }

        public bool TryComplete()
        {
            return overseer.TryEndTurn();
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

