using System.Security.Cryptography;
using TerminalVelocity;
public class Level : SceneObject {
    public Vec2i size = new Vec2i(8, 8);
    public Vec2i player_spawn = Vec2i.ZERO;
    public Vec2i door_spawn = Vec2i.ZERO;
    public Vec2i key_spawn = Vec2i.ZERO;
    public Guid key_guid = Guid.Empty;
    public Guid door_guid = Guid.Empty;

    public PhysicsObject door;
    public SceneObject key;

    public bool ready = false;
    private Random rng = new Random();
    public Level(int x, int y)
    {
        size = new Vec2i(x,y);
        generate_level();
        Visible = false;
        name = "level_constr";
    }
    public void generate_level()
    {
        key = new SceneObject();
        key.Display = "K";
        key.Position = get_random_cell_in_bounds();
        key.ZIndex = 1;
        add_child(key);
        // place walls around
        bool door_not_placed = true;
        for (int y = 0; y < size.y; y++)
        {
            int east = rng.Next(5);
            int west = rng.Next(5);
            for (int x = 0; x < size.x; x++)
            {
                rng = new Random();
                
                int north = rng.Next(5);
                int south = rng.Next(5);
                Vec2i pos = new Vec2i(x, y);
                if (y == 0 || x == 0 || x == size.x - 1|| y == size.y - 1)
                {
                    PhysicsObject wall_piece;
                    if (east > west && north > south && door_not_placed && x != 0 && y != 0)
                    {
                        wall_piece = new PhysicsObject(pos,"D");
                        door = wall_piece;
                        door_spawn = wall_piece.Position;
                        wall_piece.name = "theoneandonly";
                        key.ForegroundColor = ConsoleColor.Red;
                        key.BackgroundColor = ConsoleColor.Green;
                        key.Visible = true;
                        key_guid = key.id;
                        door_guid = wall_piece.id;
                        add_child(key);
                        door_not_placed = false;
                    }
                    else
                    {
                        wall_piece = new PhysicsObject(pos, "█");
                        wall_piece.mass = 1000;
                        wall_piece.BackgroundColor = ConsoleColor.Blue;
                        wall_piece.ForegroundColor = ConsoleColor.Blue;

                    }
                    wall_piece.Visible = true;
                    add_child(wall_piece);
                }
            }
        }
        place_player_spawn();
    }

    private Vec2i get_random_cell_in_bounds()
    {
        Vec2i random_cell = new Vec2i(RandomNumberGenerator.GetInt32(1,size.x-1),RandomNumberGenerator.GetInt32(1,size.y-1));
        return random_cell;
    }

    private void place_player_spawn()
    {
        player_spawn = get_random_cell_in_bounds();
        ready = true;
    }


    public Vec2i get_random_cell()
    {
        return new Vec2i(rng.Next(0, size.x), rng.Next(0, size.y));
    }

    public bool Is_inside_bounds(int x, int y)
    {
        return x > 1 || x < size.x - 1 && y > 1 || y < size.y - 1;
    }

    public bool Is_wall(int x, int y)
    {
        return !Is_inside_bounds(x,y);
    }
}