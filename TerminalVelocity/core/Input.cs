namespace TerminalVelocity;
public class Input 
{
    /// <summary>
    /// The event that gets fired when a key is pressed
    /// </summary>
    public static event OnKeyPressed? KeyPressed;
    public delegate void OnKeyPressed(ConsoleKey key);
    /// <summary>
    /// The last key that was pressed. Lags behind by one input
    /// </summary>
    public static ConsoleKey LastKeyPressed;
    static Input()
    {
        
    }
    /// <summary>
    /// If a key is pressed the KeyPressed event is fired and the pressed key is passed as 
    /// </summary>
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