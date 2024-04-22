namespace TerminalVelocity;
public class Input 
{
    public static event OnKeyPressed? KeyPressed;
    public delegate void OnKeyPressed(ConsoleKey key);

    public static ConsoleKey LastKeyPressed;
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
                LastKeyPressed = key;
            }
            while (Console.KeyAvailable) // flush input
                Console.ReadKey(true);
        }
    }
}