using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Position
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool InBounds(int maxRows, int maxCols)
        {
            return Row >= 0 && Row < maxRows && Col >= 0 && Col < maxCols;
        }


        //add tostring, equals, gethashcode, and so on...

    }
}
