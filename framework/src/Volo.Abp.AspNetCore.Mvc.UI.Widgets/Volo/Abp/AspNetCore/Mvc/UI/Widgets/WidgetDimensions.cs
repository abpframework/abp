namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetDimensions
    {
        public int Width { get; }

        public int Height { get; }

        public WidgetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}