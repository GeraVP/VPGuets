using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using System.Windows.Shapes;

namespace dbBO
{
    /// <summary>
    /// Логика взаимодействия для vhod.xaml
    /// </summary>
    public partial class vhod : Window
    {
        
        string dbFilePath;
        string connectionString;
        SQLiteConnection connection;
        public vhod()
        {
            InitializeComponent();
            dbFilePath = "C:\\dbBO\\dbBO\\dbG.db";
            connectionString = $"Data Source={dbFilePath};Version=3;";
            connection = new SQLiteConnection(connectionString);

            TBnk.Text = Properties.Settings.Default.Name;
            TBpass.Text = Properties.Settings.Default.pass;
            Update("Nomera");
        }
        public void Update(string tableName)
        {
            DataTable Table = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM {tableName}", connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(Table);
            dg1.ItemsSource = Table.DefaultView;
        }
        public void addg()
        {
            SQLiteCommand cmd = new SQLiteCommand(($"INSERT INTO  Nomera(ID, Name) VALUES (@value1, @value2)"), connection);
            /*SQLiteCommand cmd = new SQLiteCommand(($"INSERT INTO {tableName} (Column1, Column2) VALUES (@value1, @value2)"),connection);*/

            cmd.Parameters.AddWithValue("@value1", Convert.ToInt32(tb1.Text));
            cmd.Parameters.AddWithValue("@value2", tb2.Text);

            cmd.ExecuteNonQuery();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addg();
            Update("Nomera");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pass = this.TBpass.Text;
            Properties.Settings.Default.Name = this.TBnk.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand(($"delete from Nomera Where ID=@i"), connection);
            cmd.Parameters.AddWithValue("@i", Convert.ToInt32(deln.Text));
            cmd.ExecuteNonQuery();
            Update("Nomera"); 
        }
    }
}
