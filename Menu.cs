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
    public partial class Menu : Form
    {

        private const string PATH = "api/TblChessPlayers";

        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
/*            Labels_Load(); // player name / data
*/
        }

        private void Play_Click(object sender, EventArgs e)
        {
            ChessForm chessform = new ChessForm();

            Game game = new Game (chessform.UpdateBoardUI);

            chessform.Show();
            this.Hide();
        }

        private void Logout_Click(object sender, EventArgs e) 
        {
            Program.welcomeForm.Show();
            this.Close();
        }
        
        private void Exit_Click(object sender, EventArgs e) 
        {
            Application.Exit();
        }
    }
}
