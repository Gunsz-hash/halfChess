using FinalProject.Models;
using System.Windows.Forms;
using System;

namespace FinalProject
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        Player playerDet;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuLBL = new System.Windows.Forms.Label();
            this.Play = new System.Windows.Forms.Button();
            this.Logout = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.UName = new System.Windows.Forms.Label();
            this.UCountry = new System.Windows.Forms.Label();
            this.UPhone = new System.Windows.Forms.Label();
            this.UGames = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MenuLBL
            // 
            this.MenuLBL.AutoSize = true;
            this.MenuLBL.BackColor = System.Drawing.Color.RosyBrown;
            this.MenuLBL.Font = new System.Drawing.Font("Monotype Corsiva", 72F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuLBL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuLBL.Location = new System.Drawing.Point(282, 9);
            this.MenuLBL.Name = "MenuLBL";
            this.MenuLBL.Size = new System.Drawing.Size(256, 117);
            this.MenuLBL.TabIndex = 0;
            this.MenuLBL.Text = "Menu";
            // 
            // Play
            // 
            this.Play.BackColor = System.Drawing.Color.SaddleBrown;
            this.Play.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play.Location = new System.Drawing.Point(280, 315);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(258, 100);
            this.Play.TabIndex = 1;
            this.Play.Text = "Let\'s Play Chess";
            this.Play.UseVisualStyleBackColor = false;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // Logout
            // 
            this.Logout.BackColor = System.Drawing.Color.RosyBrown;
            this.Logout.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Logout.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Logout.Location = new System.Drawing.Point(560, 315);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(206, 100);
            this.Logout.TabIndex = 2;
            this.Logout.Text = "Log Out";
            this.Logout.UseVisualStyleBackColor = false;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.Color.RosyBrown;
            this.Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exit.Location = new System.Drawing.Point(42, 315);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(216, 100);
            this.Exit.TabIndex = 3;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = false;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            
            
            // 
            // UName
            // 
            this.UName.AutoSize = true;
            this.UName.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UName.Location = new System.Drawing.Point(273, 136);
            this.UName.Name = "UName";
            this.UName.Size = new System.Drawing.Size(110, 39);
            this.UName.TabIndex = 4;
            this.UName.Text = "Name: ";
            // 
            // UCountry
            // 
            this.UCountry.AutoSize = true;
            this.UCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UCountry.Location = new System.Drawing.Point(275, 189);
            this.UCountry.Name = "UCountry";
            this.UCountry.Size = new System.Drawing.Size(95, 29);
            this.UCountry.TabIndex = 5;
            this.UCountry.Text = "Country: ";
            // 
            // UPhone
            // 
            this.UPhone.AutoSize = true;
            this.UPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UPhone.Location = new System.Drawing.Point(275, 228);
            this.UPhone.Name = "UPhone";
            this.UPhone.Size = new System.Drawing.Size(83, 29);
            this.UPhone.TabIndex = 6;
            this.UPhone.Text = "Phone: ";
            // 
            // UGames
            // 
            this.UGames.AutoSize = true;
            this.UGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UGames.Location = new System.Drawing.Point(277, 267);
            this.UGames.Name = "label4";
            this.UGames.Size = new System.Drawing.Size(141, 29);
            this.UGames.TabIndex = 7;
            this.UGames.Text = "Games Played: ";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UGames);
            this.Controls.Add(this.UPhone);
            this.Controls.Add(this.UCountry);
            this.Controls.Add(this.UName);


            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Logout);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.MenuLBL);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private async void InitPlayerValues(int id)
        {
            string path = "api/TblChessPlayers/" + id;
            

            try
            {
                var player = await GetPlayerAsync(path);
                if (player != null)  // check if we got a player back
                {
                    playerDet = player;  // store the single player
                    UpdatePlayerLabels();
                }
                else
                {
                    
                    string message = "Failed to retrieve details";
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

        private void UpdatePlayerLabels()
        {
            if (playerDet != null)
            {
                UName.Text = "Name: " + playerDet.Name;
                UCountry.Text = "Country: " + playerDet.Country;
                UPhone.Text = "Phone: " + playerDet.Phone;
                UGames.Text = "Games Played: " + playerDet.NumOfGames;
            }
        }



        #endregion

        private System.Windows.Forms.Label MenuLBL;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.Button Logout;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Label UName;
        private System.Windows.Forms.Label UCountry;
        private System.Windows.Forms.Label UPhone;
        private System.Windows.Forms.Label UGames;
        
    }
}