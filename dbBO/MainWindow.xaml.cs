using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dbBO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
<<<<<<< HEAD
    {
        vhod a = new vhod();
        string dbFilePath;
        string connectionString;
        SQLiteConnection connection;
=======
    {/*
        vhod a = new vhod();
        string dbFilePath;
        string connectionString;
        SQLiteConnection connection;*/
>>>>>>> 1fd1d163ea52cc6462765f6337e9ddb09e681274

        SQLiteCommand cmd;
        public MainWindow()
        {
            InitializeComponent();
<<<<<<< HEAD
            dbFilePath = "C:\\dbBO\\dbBO\\dbG.db";
=======
           /* dbFilePath = "C:\\dbBO\\dbBO\\dbG.db";
>>>>>>> 1fd1d163ea52cc6462765f6337e9ddb09e681274
            connectionString = $"Data Source={dbFilePath};Version=3;";
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            cmd = new SQLiteCommand("select count (*) from Nomera", connection);
            int ks = Convert.ToInt32(cmd.ExecuteScalar());

            if (ks < 1)
            {
                Close();
                a.Show();

            }
<<<<<<< HEAD
            connection.Close();
=======
            connection.Close();*/
>>>>>>> 1fd1d163ea52cc6462765f6337e9ddb09e681274
        }
        admin adm = new admin();
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (vhtb.Text == Properties.Settings.Default.pass)
            {
                Close();
                adm.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
