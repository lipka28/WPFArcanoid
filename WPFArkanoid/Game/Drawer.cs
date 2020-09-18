using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFArkanoid
{
    public class Drawer
    {
        public static DrawingVisual dv;
        public Drawer(Size dravableArea) 
        {
            DrawableArea = dravableArea;
            dv = new DrawingVisual();
            Clear();
        }

        public void Clear() 
        {
            using (DrawingContext dc = dv.RenderOpen()) 
            {
                dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, DrawableArea.Width, DrawableArea.Height));
            }
        }

        public void Draw(IColidableObject obj) 
        {
            using (DrawingContext dc = dv.RenderOpen())
            {
                switch (obj.Shape)
                {
                    case Shape.RECT:
                        dc.DrawRectangle(getBrush(obj.Color), null, new Rect(obj.Position.X, obj.Position.Y, 
                                                                             obj.Size.Width, obj.Size.Height));
                        break;
                    case Shape.ELLIPSE:
                        dc.DrawEllipse(getBrush(obj.Color), null, new Point(obj.Position.X, obj.Position.Y), obj.Size.Height, obj.Size.Height);
                        break;
                    default:
                        break;
                }
            }
        }

        public Size DrawableArea { get; set; }
        public DrawingVisual Drawing { get => dv; }

        private Brush getBrush(Colors col) 
        {
            switch (col)
            {
                case Colors.WHITE:
                    return Brushes.SeaShell;
                case Colors.GREEN:
                    return Brushes.SeaGreen;
                case Colors.ORANGE:
                    return Brushes.Orange;
                case Colors.RED:
                    return Brushes.Tomato;
                case Colors.BLUE:
                    return Brushes.SteelBlue;
                default:
                    return Brushes.White;
            }
        }
    }
}
