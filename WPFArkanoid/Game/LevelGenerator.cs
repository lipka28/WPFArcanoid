using System.Collections.Generic;

namespace WPFArkanoid
{
    public static class LevelGenerator
    {
        private const int START_X = 50;
        private const int START_Y = 10;

        private const int BRICK_COUNT_X = 10;
        private const int BRICK_COUNT_Y = 5;

        private const int BRICK_WIDTH = 68;
        private const int BRICK_HEIGHT = 28;

        private const int STRIDE_X = BRICK_WIDTH + 2;
        private const int STRIDE_Y = BRICK_HEIGHT + 2;
        public static IColidableObject[] GenerateLevel(char[] level)
        {
            var generatedLevel = new List<Brick>();

            int xPos = START_X;
            int yPos = START_Y;

            for (int i = 0; i < BRICK_COUNT_X * BRICK_COUNT_Y; i++)
            {
                var brickType = level[i];

                if (i % 10 == 0)
                {
                    xPos = START_X;
                    yPos += STRIDE_Y;
                }
                else 
                {
                    xPos += STRIDE_X;
                }

                generatedLevel.Add(new Brick(new Position(xPos, yPos), new Size(BRICK_WIDTH, BRICK_HEIGHT), brickType));
            }

            return generatedLevel.ToArray();
        }
    }
}
