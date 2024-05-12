using System.Security.Cryptography;
using TerminalVelocity;

public class Level : SceneObject
{
    public Vec2i LevelSize = Vec2i.ONE * 100;
    public int RoomCount;
    private Vec2i[] RoomSizes = new Vec2i[]
    {
         Vec2i.New(16,8),
         Vec2i.New(15,17),
         Vec2i.New(18,17),
         Vec2i.New(16,8),
         Vec2i.New(12,6)
    };
    private Vec2i MinRoomSize = Vec2i.New(6, 3);
    private Vec2i RoomSizeVar = Vec2i.New(0, 3);
    private Vec2i RoomPosition = Vec2i.ZERO;
    public bool IsReady = false;
    public Level(Vec2i _size, int _roomCount)
    {
        RoomCount = _roomCount;
        LevelSize = _size;
        Visible = false;
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        var roomCoord = new Vec2i();
        for (int i = 0; i < RoomCount; i++)
        {
            var thisSize = RoomSizes[new Random().Next(RoomSizes.Length)];
            var r = new Room(thisSize);
            r.Position = roomCoord;
            AddChild(r);
            roomCoord = r.Box.End;
        }
    }

    private SceneObject GenerateRoom()
    {
        var thisSize = RoomSizes[new Random().Next(RoomSizes.Length)];
        var r = new Room(thisSize);
        return r;
    }
}
