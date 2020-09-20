using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFArkanoid
{
    /// <summary>
    /// Rendering class
    /// </summary>
    public class Drawer
    {
        private WriteableBitmap render;
        public Drawer(Size dravableArea) 
        {
            DrawableArea = dravableArea;
            render = BitmapFactory.New(DrawableArea.Width, DrawableArea.Height);
            Clear();
        }

        /// <summary>
        /// Clear canvas with black color
        /// </summary>
        public void Clear() 
        {
            render.Clear(Color.FromRgb(0, 0, 0));
        }

        /// <summary>
        /// Draw object into canvas.
        /// </summary>
        /// <param name="obj"></param>
        public void Draw(IColidableObject obj) 
        {
            switch (obj.Shape)
            {
                case Shape.RECT:
                    render.FillRectangle(obj.Position.X, obj.Position.Y, obj.Position.X + obj.Size.Width, obj.Position.Y + obj.Size.Height, SetColor(obj.Color));
                    break;
                case Shape.ELLIPSE:
                    render.FillEllipse(obj.Position.X, obj.Position.Y, obj.Position.X + obj.Size.Height, obj.Position.Y + obj.Size.Height, SetColor(obj.Color));
                    break;
                default:
                    break;
            }
        }

        private Size DrawableArea { get; set; }
        public WriteableBitmap Render { get => render; }

        /// <summary>
        /// Get predefined color
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private Color SetColor(Colors col) 
        {
            switch (col)
            {
                case Colors.WHITE:
                    return Color.FromRgb(236, 240, 241); //Clouds color
                case Colors.GREEN:
                    return Color.FromRgb(46, 204, 113); //Emeralds color;
                case Colors.ORANGE:
                    return Color.FromRgb(241, 196, 15); //Sun flower color;
                case Colors.RED:
                    return Color.FromRgb(231, 76, 60); //Alizarin color;
                case Colors.BLUE:
                    return Color.FromRgb(52, 152, 219); //Peter River color;
                default:
                    return Color.FromRgb(0, 0, 0); //Errorous black;
            }
        }
    }
}
