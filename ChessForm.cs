using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
  
    public partial class ChessForm : Form
    {

        private Button[,] boardButton;
        private Game game;


        public ChessForm()
        {
            InitializeComponent();
        }
    }
}
