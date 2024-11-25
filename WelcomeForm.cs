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
    public partial class WelcomeForm : Form
    {
        private const string PATH = "https://localhost:7179";


        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            UserID.Size = new Size(259, 26);
            UserID.Value = 0;
           
        }

        

        private void UserID_ValueChanged(object sender, EventArgs e)
        {

        }

        private void UserID_Enter(object sender, EventArgs e)
        {
            if (UserID != null)
            {
                UserID.Select(0, UserID.Text.Length);
            }
        }

        private void UserID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Connect_Click(sender, e);
        }


        private void SignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PATH);
        }


        
        static async Task<IEnumerable<Player>> GetPlayerAsync(string path, int id, string name)
        {
            IEnumerable<Player> player = null;
            HttpResponseMessage response = await Program.client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<IEnumerable<Player>>();
            }
            return player;
        }




        private void UserName_TextChanged(object sender, EventArgs e)
        {
            if (UserName.Text.Length > 1)
            {
                Connect.Enabled = true;
            }
        }


        private void UserName_MouseClick(object sender, MouseEventArgs e)
        {
            //UserName.Clear();  //if we want to enable a "reseting" field, helped us for checking a lot of users
        }

        private void UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Connect_Click(sender, e);

        }


        
        private async void Connect_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(UserID.Value);
            string name = UserName.Text;
            string path = "api/TblChessPlayers/getbyid/" + id + "/" + name;
            try
            {
                var res = await GetPlayerAsync(path, id, name);
                if (res.Count() == 1)
                {
                    Program.player = res.First();
                    this.Hide();
                    new Menu(id).ShowDialog();
                }
                else
                {
                    UserID.ResetText();
                    //Connect.Enabled = false; irrelvant for now, because the fields arnt reseting, if reseting enable
                    UserID.Refresh();

                    string message = "Failed to Log in";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;
                    DialogResult result = MessageBox.Show(message, "Error", buttons, icon);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
