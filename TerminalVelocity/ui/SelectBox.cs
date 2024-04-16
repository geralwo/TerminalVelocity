namespace TerminalVelocity;
using TerminalVelocity;

public class SelectBox : SceneObject
{
    public Vec2i Gap = Vec2i.DOWN;
    public Vec2i FlowDirection = Vec2i.DOWN;
    public ConsoleColor HighlightForegroundColor = ConsoleColor.Black;
    public ConsoleColor HighlightBackgroundColor = ConsoleColor.White;

    public ConsoleColor DefaultForegroundColor = ConsoleColor.White;
    public ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

    public SelectBox()
    {
        Visible = false;
        ProcessEnabled = true;
    }

    int menuIndex = 0;

    public override void OnProcess()
    {
			for(int i=0;i < Children.Count;i++)
			{
				if (i == menuIndex)
				{
					Children[i].ForegroundColor = HighlightForegroundColor;
					Children[i].BackgroundColor = HighlightBackgroundColor;
				}
				else
				{
                    Children[i].ForegroundColor = DefaultForegroundColor;
                    Children[i].BackgroundColor = DefaultBackgroundColor;
                }
			}
    }

    public new void add_child(SceneObject obj)
    {
        obj.Position += this.Position;
        obj.Position += FlowDirection * Children.Count;
        if(Children.Count > 0)
            obj.Position += Gap * Children.Count;
        Children.Add(obj);
    }

    public void next()
    {
        if(menuIndex < Children.Count - 1)
        {
            menuIndex++;
        }
    }

    public void previous()
    {
        if(menuIndex > 0)
            menuIndex--;
    }

    public void select()
    {
        Children[menuIndex].ProcessAction?.Invoke();
    }

    public void pad_and_recenter()
    {
        // pad and recenter items
        int longest_str = 0;
        Children.ForEach(
            child => {
                if(child.Display.Length > longest_str)
                    longest_str = child.Display.Length;
            }
        );
        Children.ForEach(
            child => {
                child.Display = child.Display.PadRight(longest_str);
            }
        );
        center_xy();
    }


}