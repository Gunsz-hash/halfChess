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
       

        //details
        public static Game currGame;       
        public static Player player;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            client.BaseAddress = new Uri(PATH);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            welcomeForm = new WelcomeForm();
            player = new Player();
            Application.Run(welcomeForm);// - problem with form


            //maybe?:
           // Application.Run(new ChessForm());
        }
    }
}
