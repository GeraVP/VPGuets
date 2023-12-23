using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
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
    /// Логика взаимодействия для pen.xaml
    /// </summary>
    public partial class pen : Window
    {
        string dbFilePath;
        string connectionString;
        SQLiteConnection connection;
        public pen()
        {
            InitializeComponent();
            dbFilePath = "C:\\dbBO\\dbBO\\dbG.db";
            connectionString = $"Data Source={dbFilePath};Version=3;";
            connection = new SQLiteConnection(connectionString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IK.Strokes.Clear();
        } // Очистка

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            var inkCanvasStrokes = IK.Strokes;
            var inkMemoryStream = new MemoryStream();
            inkCanvasStrokes.Save(inkMemoryStream);
            var inkBytes = inkMemoryStream.ToArray();
            var base64String = Convert.ToBase64String(inkBytes);

            SQLiteCommand cmd = new SQLiteCommand();
            cmd = new SQLiteCommand("select count (*) from Gosty", connection);
            int ks = Convert.ToInt32(cmd.ExecuteScalar());
            b1.Content = ks.ToString();

            cmd.CommandText = $"Update Gosty SET Sign=@base6 where ID=@psd";
            cmd.Parameters.AddWithValue("base6", base64String);
            cmd.Parameters.AddWithValue("psd", ks);
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            /*UpdateG("Gosty");*/
            connection.Close();
            Close();
        } // Добавить

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
        }
    }
}
