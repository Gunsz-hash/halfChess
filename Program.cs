using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    internal static class Program
    {

        //paths and clients
        public static HttpClient client = new HttpClient();
        private const string PATH = "https://localhost:7179";

        //forms
        public static WelcomeForm welcomeForm;
        //public static Form PreviousPage;

        //details
        public static Game currGame;       
        public static Player player;

        //DB
        //public static MyDataClassesDataContext database;???????????????????



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //example for a friend;
            //SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mark0\\source\\repos\\FinalProj_Noa_Mark\\FinalProj_Noa_Mark\\MyPrivateDB.mdf;Integrated Security=True");
            //database = new MyDataClassesDataContext(con);

            client.BaseAddress = new Uri(PATH);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            welcomeForm = new WelcomeForm();
            player = new Player();
            Application.Run(welcomeForm);



           // Application.Run(new ChessForm());
        }
    }
}
