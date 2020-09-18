using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Security.RightsManagement;
using System.Text;

namespace WPFArkanoid
{
    public enum Shape
    {
        RECT,
        ELLIPSE
    }

    public enum Colors 
    {
        WHITE,
        GREEN,
        ORANGE,
        RED,
        BLUE
    }

    public enum BallColisionSide 
    { 
        NONE,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }
    public class Position 
    {
        public Position(int x, int y) 
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Size 
    {
        public Size(int width, int height) 
        {
            Width = width;
            Height = height;
        }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Speed 
    {
        public Speed(int xspeed, int yspeed) 
        {
            XSpeed = xspeed;
            YSpeed = yspeed;
        }
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
    }
    public interface IColidableObject
    {
        bool IsColidable { get; set; }
        bool IsDestroyable { get; set; }
        bool IsActive { get; set; }
        Position Position { get; set; }
        Size Size { get; set; }
        Speed Speed { get; set; }
        int PointValue { get; set; }

        Colors Color { get; set; }
        Shape Shape { get; set; }

    }
}
