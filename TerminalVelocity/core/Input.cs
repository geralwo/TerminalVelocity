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
    public static ConsoleKeyInfo LastKeyPressed;
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
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.None)
            {
                KeyPressed?.Invoke(keyInfo.Key);
                LastKeyPressed = keyInfo;
            }
            while (Console.KeyAvailable) // flush input
                Console.ReadKey(true);
        }
    }
}