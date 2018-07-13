using System;
using System.Collections.Generic;

namespace MazeLibrary
{
    public class MazeSolver
    {
        private int[,] matrix;
        private readonly int startX;
        private readonly int startY;

        public MazeSolver(int[,] mazeModel, int startX, int startY)
        {
            if (mazeModel == null)
            {
                throw new ArgumentNullException($"{nameof(mazeModel)} is null");
            }
            
            int rows = mazeModel.GetUpperBound(0) + 1;
            int columns = mazeModel.Length / rows;

            if (startX >= rows || startX < 0)
            {
                throw new ArgumentException($"{nameof(startX)} is out of range");
            }

            if (startY >= columns || startY < 0)
            {
                throw new ArgumentException($"{nameof(startY)} is out of range");
            }
            
            ValidateEntriesAndExits(mazeModel);

            if (mazeModel[startX, startY] != 0)
            {
                throw new ArgumentException($"{nameof(mazeModel)} is not a start");
            }

            this.matrix = new int[rows, columns];
            matrix = (int[,]) mazeModel.Clone();
            this.startY = startY;
            this.startX = startX;
        }

        public int[,] MazeWithPass() => this.matrix;

        public void PassMaze()
        {
            MarkVertexes();
        }

        private void MarkVertexes()
        {
            int rows = matrix.GetUpperBound(0) + 1;
            int columns = matrix.Length / rows;

            bool[,] bools = new bool[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (matrix[i, j] == 0) bools[i, j] = true;
                    else bools[i, j] = false;
                }
            }
            
            bool Find(int x, int y, int acc)
            {
                if (!(x == startX && y == startY))
                {
                    if (x < 0 || y < 0) return true;
                    if (y >= columns || x >= rows) return true;
                    if (!bools[x, y]) return false;
                }

                bools[x, y] = false;
                bool a = Find(x + 1, y, acc + 1);
                bool b = Find(x - 1, y, acc + 1);
                bool c = Find(x, y + 1, acc + 1);
                bool d = Find(x, y - 1, acc + 1);

                bool res = (a || b || c || d);
                if (res)
                    matrix[x, y] = acc;
                return res;
            }

            Find(startX, startY, 1);
        }

        private void ValidateEntriesAndExits(int[,] m)
        {
            int count = 0;
            int rows = m.GetUpperBound(0) + 1;
            int columns = m.Length / rows;

            for (int i = 0; i < rows; i++)
            {
                if (m[i, 0] == 0)
                {
                    count++;
                }

                if (m[i, columns - 1] == 0)
                {
                    count++;
                }

                if (m[0, i] == 0)
                {
                    count++;
                }

                if (m[rows - 1, i] == 0)
                {
                    count++;
                }

                if (count > 2)
                {
                    throw new ArgumentException($"{nameof(m)} has more than 1 exits");
                }
            }

            if (count != 2)
            {
                throw new ArgumentException($"{nameof(m)} has less than 1 exits");
            }
        }
    }
}
