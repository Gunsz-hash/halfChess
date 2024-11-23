using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Square
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Square(int row, int col)
        {
            Row = row;
            Col = col;
        }

       

        // need to not forget to add tostring, equals, gethashcode, and so on...

    }
}
