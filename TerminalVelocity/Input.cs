namespace TerminalVelocity;
public class Input 
{
    public static event OnKeyPressed? KeyPressed;
    public delegate void OnKeyPressed(ConsoleKey key);
    static Input()
    {
        
    }
    public static void get_input()
    {
        if(Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key != ConsoleKey.None)
            {
                KeyPressed?.Invoke(key);
            }
            while (Console.KeyAvailable) // flush input
                Console.ReadKey(true);
        }
    }

    public static void get_input(bool emit_event)
    {
        if( emit_event )
        {
            get_input();
        }
    }
}