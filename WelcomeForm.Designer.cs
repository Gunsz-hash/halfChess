namespace FinalProject
{
    partial class WelcomeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.HalfChess = new System.Windows.Forms.Label();
            this.Submitters = new System.Windows.Forms.Label();

            this.UserName = new System.Windows.Forms.TextBox();
            /*this.Phone = new System.Windows.Forms.NumericUpDown();*/
            this.UserID = new System.Windows.Forms.NumericUpDown();
            /*this.Country = new System.Windows.Forms.ComboBox();*/

            this.Connect = new System.Windows.Forms.Button();

            this.SignUpLBL = new System.Windows.Forms.Label();  
            this.SignUp = new System.Windows.Forms.LinkLabel();

            //((System.ComponentModel.ISupportInitialize)(this.Phone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserID)).BeginInit();

            this.SuspendLayout();


            //HalfChess
            this.HalfChess.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.HalfChess.AutoSize = true;
            this.HalfChess.Font = new System.Drawing.Font("Maiandra GD", 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HalfChess.Location = new System.Drawing.Point(234, 34);
            this.HalfChess.Name = "HalfChess";
            this.HalfChess.Size = new System.Drawing.Size(318, 81);
            this.HalfChess.TabIndex = 0;
            this.HalfChess.Text = "HalfChess";
            this.HalfChess.ForeColor = System.Drawing.Color.Black;

            // submitters
            this.Submitters.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Submitters.AutoSize = true;
            this.Submitters.Font = new System.Drawing.Font("Maiandra GD", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Submitters.ForeColor = System.Drawing.Color.RosyBrown;  // Changed to RosyBrown
            this.Submitters.Location = new System.Drawing.Point(240, 126);
            this.Submitters.Name = "submitters";
            this.Submitters.Size = new System.Drawing.Size(171, 48);
            this.Submitters.TabIndex = 0; 
            this.Submitters.Text = "Itamar and Amit";

            //username
            this.UserName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserName.Font = new System.Drawing.Font("Maiandra GD", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName.Location = new System.Drawing.Point(262, 209);
            this.UserName.Multiline = true;
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(259, 26);
            this.UserName.TabIndex = 1;
            this.UserName.Text = "Name";
            this.UserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UserName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserName_MouseClick);
            this.UserName.TextChanged += new System.EventHandler(this.UserName_TextChanged);
            this.UserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserName_KeyDown);

            /*//phone
            this.Phone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Phone.Font = new System.Drawing.Font("Maiandra GD", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Phone.Location = new System.Drawing.Point(262, 250);
            this.Phone.Maximum = new decimal(new int[] { -1, 2328, 0, 0 });
            this.Phone.Minimum = 0;
            this.Phone.Name = "Phone";
            this.Phone.Size = new System.Drawing.Size(259, 23);
            this.Phone.TabIndex = 2;
            this.Phone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Phone.ThousandsSeparator = false;
            this.Phone.Value = 0;
            this.Phone.DecimalPlaces = 0;
            this.Phone.ValueChanged += new System.EventHandler(this.Phone_ValueChanged);
            this.Phone.Enter += new System.EventHandler(this.Phone_Enter);
            this.Phone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Phone_KeyDown);*/

            //user id
            this.UserID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserID.Font = new System.Drawing.Font("Maiandra GD", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserID.Location = new System.Drawing.Point(262, 250);
            this.UserID.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.UserID.Minimum = 0;
            this.UserID.Name = "UserID";
            this.UserID.Size = new System.Drawing.Size(259, 23);
            this.UserID.TabIndex = 3;
            this.UserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UserID.Value = 0;
            this.UserID.ValueChanged += new System.EventHandler(this.UserID_ValueChanged);
            this.UserID.Enter += new System.EventHandler(this.UserID_Enter);
            this.UserID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserID_KeyDown);

           /* //Country ComboBox
            this.Country.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Country.Font = new System.Drawing.Font("Maiandra GD", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Country.Location = new System.Drawing.Point(262, 326);
            this.Country.Name = "Country";
            this.Country.Size = new System.Drawing.Size(259, 23);
            this.Country.TabIndex = 4;
            this.Country.Text = "Select Country";
            this.Country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Country.FormattingEnabled = true;
            this.Country.Items.AddRange(new object[] {
    "Israel", "USA", "England", "France", "Australia", "Jamaica", "Norway", "Spain",
});
            this.Country.SelectedIndexChanged += new System.EventHandler(this.Country_SelectedIndexChanged);
            this.Country.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Country_KeyDown);
*/
            // Connect
            this.Connect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Connect.BackColor = System.Drawing.Color.Violet;
            this.Connect.Enabled = false;
            this.Connect.FlatAppearance.BorderColor = System.Drawing.Color.Purple;
            this.Connect.FlatAppearance.BorderSize = 5;
            this.Connect.Font = new System.Drawing.Font("Maiandra GD", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connect.ForeColor = System.Drawing.Color.White;
            this.Connect.Location = new System.Drawing.Point(331, 320);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(121, 25);
            this.Connect.TabIndex = 5;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = false;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);

            // SignUpLBL
            this.SignUpLBL.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SignUpLBL.AutoSize = true;
            this.SignUpLBL.Font = new System.Drawing.Font("Maiandra GD", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUpLBL.Location = new System.Drawing.Point(336, 350);
            this.SignUpLBL.Name = "SignUpLBL";
            this.SignUpLBL.Size = new System.Drawing.Size(122, 28);
            this.SignUpLBL.TabIndex = 6;
            this.SignUpLBL.Text = " First time?\r\n Click here to join:";
            this.SignUpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // SignUp
            this.SignUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SignUp.AutoSize = true;
            this.SignUp.Font = new System.Drawing.Font("Maiandra GD", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUp.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.SignUp.Location = new System.Drawing.Point(360, 380);
            this.SignUp.Name = "SignUp";
            this.SignUp.Size = new System.Drawing.Size(39, 14);
            this.SignUp.TabIndex = 7;
            this.SignUp.TabStop = true;
            this.SignUp.Text = "Sign Up";
            this.SignUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SignUp_LinkClicked);



            //creating the welcome form

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);

            this.Controls.Add(this.HalfChess);
            this.Controls.Add(this.Submitters);
            this.Controls.Add(this.UserName);
           /* this.Controls.Add(this.Phone);*/
            this.Controls.Add(this.UserID);
           /* this.Controls.Add(this.Country);*/
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.SignUpLBL);
            this.Controls.Add(this.SignUp);

            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WelcomeForm";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            //((System.ComponentModel.ISupportInitialize)(this.Phone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        //titles
        private System.Windows.Forms.Label HalfChess;
        private System.Windows.Forms.Label Submitters;

        //user details
        private System.Windows.Forms.TextBox UserName;
        /*private System.Windows.Forms.NumericUpDown Phone;*/
        private System.Windows.Forms.NumericUpDown UserID;
        /*private System.Windows.Forms.ComboBox Country;  */

        //connecting button
        private System.Windows.Forms.Button Connect;

        //sign up label
        private System.Windows.Forms.Label SignUpLBL;
        private System.Windows.Forms.LinkLabel SignUp;





        

    }
}