using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Menu : Form
    {

        private const string PATH = "api/TblChessPlayers";

        public Menu(int id)
        {
            InitializeComponent();
            InitPlayerValues(id);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            //nothing here, keeping it for future upgrades of the code
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


        private async Task<Player> GetPlayerAsync(string path)
        {
            HttpResponseMessage response = await Program.client.GetAsync($"{path}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Player>();
            }
            return null;
        }

       
    }
}

