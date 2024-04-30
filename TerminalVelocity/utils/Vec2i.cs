#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CA1050 // Declare types in namespaces
namespace TerminalVelocity;
public struct Vec2i
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
        return new Vec2i(a.x + b.x, a.y + b.y);
    }

    public static Vec2i operator -(Vec2i a, Vec2i b)
    {
        return new Vec2i(a.x - b.x, a.y - b.y);
    }

    public static Vec2i operator -(Vec2i a, int b)
    {
        return new Vec2i(a.x - b, a.y - b);
    }

    public static Vec2i operator -(Vec2i a)
    {
        return new Vec2i(-a.x, -a.y);
    }


    public static Vec2i operator *(Vec2i a, int b)
    {
        return new Vec2i(a.x * b, a.y * b);
    }

    public static Vec2i operator /(Vec2i a, int b)
    {
        return new Vec2i(a.x / b, a.y / b);
    }

    public static Vec2i operator *(Vec2i a, Vec2i b)
    {
        return new Vec2i(a.x * b.x, a.y * b.y);
    }
    public static bool operator ==(Vec2i a, Vec2i b)
    {
        return a.x == b.x && a.y == b.y;
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
    
    public static bool operator <(Vec2i a, int b)
    {
        return a.x < b && a.y < b;
    }

    public static bool operator >(Vec2i a, int b)
    {
        return Math.Abs(a.x) > b && Math.Abs(a.y) > b;
    }

    public static Vec2i operator <<(Vec2i a, int b)
    {
        return new Vec2i(a.x << b, a.y << b);
    }

    public static Vec2i operator >>(Vec2i a, int b)
    {
        return new Vec2i(a.x >> b, a.y >> b);
    }

    public static Vec2i operator &(Vec2i a, int b)
    {
        return new Vec2i(a.x & b, a.y & b);
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

    public Vec2i distance_to(Vec2i other)
    {
        throw new NotImplementedException();
    }

    public double magnitude()
    {
        return Math.Sqrt(x * x + y * y);
    }
    public bool IsInBoundsOf(AABB _aabb)
    {
        return _aabb.Contains(this);
    }

    public bool IsInBoundsOf(Vec2i _size)
    {
        return this.x <= _size.x && this.y <= _size.y && this.x >= 0 && this.y >= 0;
    }

    public static readonly Vec2i[] CardinalDirections = [Vec2i.DOWN, Vec2i.UP, Vec2i.LEFT, Vec2i.RIGHT];
    public static Vec2i RandomCardinalDirection
    {
        get { return CardinalDirections[new Random().Next(0, CardinalDirections.Length)]; }
    }


    public static readonly Vec2i ZERO = new Vec2i(0, 0);
    public static readonly Vec2i ONE = new Vec2i(1, 1);

    public static readonly Vec2i UP = new Vec2i(0, -1);
    public static readonly Vec2i DOWN = new Vec2i(0, 1);
    public static readonly Vec2i LEFT = new Vec2i(-1, 0);
    public static readonly Vec2i RIGHT = new Vec2i(1, 0);

    public static Vec2i Random(int _maxExclusive)
    {
        var rng = new Random();
        return new Vec2i(rng.Next(_maxExclusive),rng.Next(_maxExclusive));
    }
}
