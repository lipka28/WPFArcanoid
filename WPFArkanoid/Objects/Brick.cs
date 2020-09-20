
namespace WPFArkanoid
{
    class Brick : IColidableObject
    {
        public Brick(Position pos, Size size, char type) 
        {
            Position = pos;
            Size = size;

            IsColidable = true;
            IsDestroyable = true;
            IsActive = true;

            switch (type)
            {
                case '1':
                    Color = Colors.WHITE;
                    PointValue = 1;
                    break;
                case '2':
                    Color = Colors.GREEN;
                    PointValue = 2;
                    break;
                case '3':
                    Color = Colors.ORANGE;
                    PointValue = 3;
                    break;
                case '4':
                    Color = Colors.RED;
                    PointValue = 4;
                    break;
                case '5':
                    Color = Colors.BLUE;
                    PointValue = 5;
                    break;
                default:
                    IsActive = false;
                    PointValue = 0;
                    break;
            }

            Shape = Shape.RECT;
        }

        public void Break() 
        {
            switch (PointValue)
            {
                case 5:
                    PointValue = 4;
                    Color = Colors.RED;
                    break;
                case 4:
                    PointValue = 3;
                    Color = Colors.ORANGE;
                    break;
                case 3:
                    PointValue = 2;
                    Color = Colors.GREEN;
                    break;
                case 2:
                    PointValue = 1;
                    Color = Colors.WHITE;
                    break;
                case 1:
                default:
                    IsActive = false;
                    break;
            }
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
