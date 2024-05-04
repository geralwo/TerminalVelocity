namespace TerminalVelocity;

public class Path2D
{
    public Vec2i Start;
    public Vec2i End;
    public Vec2i[] WayPoints;
    public int wayPointIndex = -1;

    public Path2D(Vec2i _start, Vec2i _end)
    {
        Start = _start;
        End = _end;
        WayPoints = Vec2i.GetLine(Start, End).ToArray();
    }

    public Vec2i GetNextPoint()
    {
        if (wayPointIndex < WayPoints.Length - 1)
            wayPointIndex++;
        return WayPoints[wayPointIndex];
    }
}
