using System;
using System.Collections.Generic;
using System.Text;

namespace WPFArkanoid.Objects
{
    class Brick : IColidableObject
    {
        public Brick(Position pos, char type) 
        {
            Position = pos;
            Size = new Size(120, 45);

            IsColidable = true;
            IsDestroyable = false;
            IsActive = true;

            Color = Colors.WHITE;
            Shape = Shape.RECT;
        }

        public bool IsColidable { get; set; }
        public bool IsDestroyable { get; set; }
        public bool IsActive { get; set; }
        public Speed Speed { get; set; }

        public Position Position { get; set; }
        public Size Size { get; set; }
        public int PointValue { get; set; }

        public Colors Color { get; set; }
        public Shape Shape { get; set; }
    }
}
