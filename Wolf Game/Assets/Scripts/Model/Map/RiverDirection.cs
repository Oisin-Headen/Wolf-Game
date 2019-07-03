using System;
public class RiverDirection
{
    private int direction;

    public RiverDirection(int direction)
    {
        this.direction = direction;
    }

    public void Increase()
    {
        direction = GetAbove();
    }

    public void Decrease()
    {
        direction = GetBelow();
    }

    public int GetAbove()
    {
        if(direction + 1 < 0)
        {
            return 5;
        }
        return direction + 1;
    }

    public int GetBelow()
    {
        if (direction - 1 > 5)
        {
            return 0;
        }
        return direction - 1;
    }

    public int GetDirectionIndex()
    {
        return direction;
    }
}
