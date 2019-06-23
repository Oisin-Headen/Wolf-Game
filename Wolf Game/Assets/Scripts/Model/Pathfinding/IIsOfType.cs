using Model;

namespace Pathfinding
{
    public interface IIsOfType
    {
        bool IsType(SpaceModel space);
    }

    public class UnexploredType : IIsOfType
    {
        public bool IsType(SpaceModel space)
        {
            return !space.Explored;
        }
    }
}