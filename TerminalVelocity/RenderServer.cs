public class RenderServer
{
    private RenderServer? renderServer;
    public RenderServer Instance
    {
        get {
            if(renderServer == null)
                renderServer = new RenderServer(); 
            return renderServer; }
    }
    public RenderServer()
    {}

    public static string Hello()
    {
        return "HELLO";
    }

    public static void DrawBuffer()
    {
        Console.WriteLine(RenderServer.Hello());
    }
}