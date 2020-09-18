using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFArkanoid
{
    public class Drawer
    {
        private WriteableBitmap render;
        public Drawer(Size dravableArea) 
        {
            DrawableArea = dravableArea;
            render = BitmapFactory.New(DrawableArea.Width, DrawableArea.Height);
            Clear();
        }

        public void Clear() 
        {
            render.Clear(Color.FromRgb(0, 0, 0));
        }

        public void Draw(IColidableObject obj) 
        {
            switch (obj.Shape)
            {
                case Shape.RECT:
                    render.FillRectangle(obj.Position.X, obj.Position.Y, obj.Position.X + obj.Size.Width, obj.Position.Y + obj.Size.Height, Color.FromRgb(255, 255, 255));
                    break;
                case Shape.ELLIPSE:
                    render.FillEllipse(obj.Position.X, obj.Position.Y, obj.Position.X + obj.Size.Width, obj.Position.Y + obj.Size.Height, Color.FromRgb(255, 255, 255));
                    break;
                default:
                    break;
            }
        }

        private Size DrawableArea { get; set; }
        public WriteableBitmap Render { get => render; }

        private Color setColor(Colors col) 
        {
            switch (col)
            {
                case Colors.WHITE:
                    return Color.FromRgb(236, 240, 241); //Clouds color
                case Colors.GREEN:
                    return Color.FromRgb(46, 204, 113); //Emeralds color;
                case Colors.ORANGE:
                    return Color.FromRgb(241, 48, 15); //Sun flower color;
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
