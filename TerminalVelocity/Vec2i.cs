using System;
using System.Numerics;
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CA1050 // Declare types in namespaces

public class Vec2i
#pragma warning restore CA1050 // Declare types in namespaces
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
    public int x;
    public int y;
    public Vec2i(int _x = 0, int _y = 0)
    {
        this.x = _x;
        this.y = _y;
    }

    public static Vec2i operator +(Vec2i a, Vec2i b)
    {
        return new Vec2i(a.x + b.x,a.y + b.y);
    }

    public static Vec2i operator -(Vec2i a, Vec2i b)
    {
        return new Vec2i(a.x - b.x, a.y - b.y);
    }

    public static Vec2i operator -(Vec2i a, int b)
    {
        return new Vec2i(a.x - b, a.y - b);
    }


    public static Vec2i operator *(Vec2i a, int b)
    {
        return new Vec2i(a.x * b, a.y * b);
    }

    public static Vec2i operator *(Vec2i a, Vec2i b)
    {
        Vec2i result = new Vec2i();
        return new Vec2i(a.x * b.x, a.y * b.y);
    }
    public static bool operator ==(Vec2i a, Vec2i b)
    {
       return (a.x == b.x && a.y == b.y);
    }

    public static bool operator !=(Vec2i a, Vec2i b)
    {
        return !(a == b);
    }

    public static bool operator <(Vec2i a, Vec2i b)
    {
        return Math.Abs(a.x) < Math.Abs(b.x) && Math.Abs(a.y) < Math.Abs(b.y);
    }

    public static bool operator >(Vec2i a, Vec2i b)
    {
        return Math.Abs(a.x) > Math.Abs(b.x) && Math.Abs(a.y) > Math.Abs(b.y);
    }


    public override string ToString()
    {
        return $"[{x},{y}]";
    }

    public float dot(Vec2i other)
    {
        var dot_result = x * other.x + y * other.y;
        return dot_result;
    }

    public Vec2i distance_to(Vec2i other) {

        throw new NotImplementedException();
    }



    public static readonly Vec2i ZERO = new Vec2i(0, 0);
    public static readonly Vec2i ONE = new Vec2i(1, 1);

    public static readonly Vec2i UP = new Vec2i(0, -1);
    public static readonly Vec2i DOWN = new Vec2i(0, 1);
    public static readonly Vec2i LEFT = new Vec2i(-1, 0);
    public static readonly Vec2i RIGHT = new Vec2i(1, 0);
}
