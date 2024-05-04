namespace TerminalVelocity.UI;
public class SelectBox : SceneObject
{
    public Vec2i Gap = Vec2i.ZERO;
    public Vec2i FlowDirection = Vec2i.DOWN;
    public ConsoleColor HighlightForegroundColor = ConsoleColor.Black;
    public ConsoleColor HighlightBackgroundColor = ConsoleColor.White;

    public ConsoleColor DefaultForegroundColor = ConsoleColor.White;
    public ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

    private int longest_str = 0;

    public SelectBox()
    {
        Visible = false;
        ProcessEnabled = true;
    }

    private int menuIndex = 0;

    public override void OnProcess()
    {
        for (int i = 0; i < Children.Count; i++)
        {
            if (i == menuIndex)
            {
                Children[i].Color = HighlightForegroundColor;
                Children[i].BackgroundColor = HighlightBackgroundColor;
            }
            else
            {
                Children[i].Color = DefaultForegroundColor;
                Children[i].BackgroundColor = DefaultBackgroundColor;
            }
        }
    }

    public void AddChild(SceneObject obj, int? index = null)
    {
        if (index == null || index >= Children.Count)
        {
            // Default behavior: append to the end
            index = Children.Count;
        }

        // Insert at the specified index
        Children.Insert(index.Value, obj);

        // Reflow all items to ensure correct positioning
        for (int i = 0; i < Children.Count; i++)
        {
            Children[i].Position = this.Position + FlowDirection * i + Gap * i;
        }
    }

    public void Next()
    {
        if (menuIndex < Children.Count - 1)
        {
            menuIndex++;
        }
    }

    public void Previous()
    {
        if (menuIndex > 0)
        {
            menuIndex--;
        }
    }

    public void Select()
    {
        Children[menuIndex].ProcessAction?.Invoke();
    }

    public void PadAndRecenter()
    {
        // Pad and recenter items
        int longest_str = 0;
        Children.ForEach(
            child =>
            {
                if (child.Display.Length > longest_str)
                    longest_str = child.Display.Length;
            }
        );

        Children.ForEach(
            child =>
            {
                child.Display = child.Display.PadRight(longest_str);
            }
        );

        center_xy();
        Position += Vec2i.LEFT * (longest_str / 2);
    }
}
