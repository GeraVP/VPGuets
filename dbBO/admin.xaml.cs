using System;
using System.Collections.Generic;
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
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;

namespace dbBO
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        
        string dbFilePath;
        string connectionString;
        SQLiteConnection connection;
        public admin()
        {
            InitializeComponent();
            dbFilePath = "C:\\dbBO\\dbBO\\dbG.db";
            connectionString = $"Data Source={dbFilePath};Version=3;";
            connection = new SQLiteConnection(connectionString);

            UpdateS("Sotr");
            UpdateG("Gosty");
            CB2.Items.Add("00:00 - 12:00"); CB2.Items.Add("12:00 - 24:00");
            cbin.Items.Add("00:00 - 12:00"); cbin.Items.Add("12:00 - 24:00");
            text.Content = Properties.Settings.Default.Name;
        }
        public void UpdateG(string tableName)
        {
            DataTable Table = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM {tableName}", connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(Table);
            dg1.ItemsSource = Table.DefaultView;
        }
        public void UpdateS(string tableName)
        {
            DataTable Table = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM {tableName}", connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(Table);
            dg2.ItemsSource = Table.DefaultView;
        }
        public void UpdateSearch(string s)
        {
            DataTable Table = new DataTable();
            string connectionS = $"select * from Gosty Where Phone='{s}'";
            SQLiteDataAdapter data1 = new SQLiteDataAdapter(connectionS, connection);
            data1.Fill(Table);
            dg1.ItemsSource = Table.DefaultView;
        }
        public void UpdateDgs(string s)
        {
            DataTable Table = new DataTable();
            string connectionS = $"select FIO from Sotr Where Interv='{s}'";
            SQLiteDataAdapter data1 = new SQLiteDataAdapter(connectionS, connection);
            data1.Fill(Table);
            dgS.ItemsSource = Table.DefaultView;
        }
        public void UpdateAv()
        {
            DataTable Table = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM AVg", connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(Table);
            dgAv.ItemsSource = Table.DefaultView;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cb = CB2.SelectedItem.ToString();
            SQLiteCommand cmd = new SQLiteCommand(($"INSERT INTO  Sotr(FIO, Prof, Phone, Interv) VALUES (@fio, @prof,@pe,@iv)"), connection);
            cmd.Parameters.AddWithValue("fio", fioS.Text);
            cmd.Parameters.AddWithValue("prof", profS.Text);
            cmd.Parameters.AddWithValue("pe", telS.Text);
            cmd.Parameters.AddWithValue("iv", cb);

            cmd.ExecuteNonQuery();
            UpdateS("Sotr");
            fioS.Clear(); profS.Clear(); telS.Clear(); CB2.Text = null;
        } // Добавить сотрудника

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand(($"delete from Sotr Where fio=@delete;"), connection);
            cmd.Parameters.AddWithValue("delete", dels.Text);
            cmd.ExecuteNonQuery();
            UpdateS("Sotr");
            dels.Clear();
        } // Удалить сотрудника

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string a = dp1.Text;
            string b = dp2.Text;

            cbg.Items.Clear();

            SQLiteCommand cmd = new SQLiteCommand();
            
            cmd.CommandText = $"select ID from Nomera Where ID NOT IN (select Number from Gosty Where de>@zaetb AND cn<@oteztb)";
            cmd.Parameters.AddWithValue("zaetb", a);
            cmd.Parameters.AddWithValue("oteztb", b);
            cmd.Connection = connection;

            SQLiteDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                string name = dataReader["ID"].ToString();
                cbg.Items.Add(name);
            }
        } // Проверка свободных номеров

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            int n = Convert.ToInt32(cbg.SelectedItem.ToString());

            cmd = new SQLiteCommand("select count (*) from Gosty", connection);
            int ks = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

            cmd.CommandText = $"INSERT INTO Gosty(ID,FIO,Phone,Passport,cn,de,Number,Wishes) values (@idi,@fio,@pe,@pt,@c,@dp,@nr,@ws)";
            cmd.Parameters.AddWithValue("idi", ks);
            cmd.Parameters.AddWithValue("fio", fioG.Text);
            cmd.Parameters.AddWithValue("pe", telg.Text);
            cmd.Parameters.AddWithValue("pt", paspg.Text);
            cmd.Parameters.AddWithValue("c", dp1.Text);
            cmd.Parameters.AddWithValue("dp", dp2.Text);
            cmd.Parameters.AddWithValue("nr", n);
            cmd.Parameters.AddWithValue("ws", pozG.Text);


            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            UpdateG("Gosty");
            pen p = new pen();
            p.Show();

            fioG.Clear(); telg.Clear(); paspg.Clear(); pozG.Clear(); dp1.Text = null; dp2.Text = null; cbg.Items.Clear(); // Очистка
        } // Бронирование номера

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            UpdateG("Gosty");
        } // Обновление таблицы гости

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            UpdateSearch($"{tbg.Text}");
            tbg.Clear();
        } // Найти гостя

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Dt.Content = $"Время последнего обновления: {DateTime.Now.ToString("HH:mm")}";

            switch (cbin.SelectedItem)
            {
                case "00:00 - 12:00":
                    UpdateDgs("00:00 - 12:00");
                    break;
                case "12:00 - 24:00":
                    UpdateDgs("12:00 - 24:00");
                    break;
            }

            string dtu = Convert.ToString(DateTime.Now); 
            SQLiteCommand ko = new SQLiteCommand();
            ko = new SQLiteCommand("select count (*) from Gosty Where cn <= @dt and de >= @dt", connection);
            ko.Parameters.AddWithValue("dt", dtu);
            int ks = Convert.ToInt32(ko.ExecuteScalar());
            Zn.Content = ($"Количество занятых номеров: {ks.ToString()}");
        } // Обновить статус

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            
            SQLiteCommand cmd = new SQLiteCommand($"insert into AVg(ID,FIO,Phone,Wishes,LastDay) select ID,FIO,Phone,Wishes,de from Gosty Where Phone=@delete", connection);
            cmd.Parameters.AddWithValue("delete", delg.Text);
            cmd.ExecuteNonQuery();

            SQLiteCommand cmd2 = new SQLiteCommand($"delete from Gosty Where Phone=@delete;", connection);
           cmd2.Parameters.AddWithValue("delete", delg.Text);
            cmd2.ExecuteNonQuery();

            UpdateG("Gosty");
            UpdateAv();
            delg.Clear();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            vhod mv = new vhod();
            mv.Show();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            DataTable Table = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM AVg Where FIO=@Stb", connection);
            cmd.Parameters.AddWithValue("@Stb", fios.Text);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(Table);
            
            dgAv.ItemsSource = Table.DefaultView;

            /*fioS.Clear();*/
        } // Поиск по архиву

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = $"delete from AVg Where FIO=@Dtb";
            cmd.Parameters.AddWithValue("Dtb",fiod.Text);
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            UpdateAv();
            fiod.Clear();
        } // Удаление из архивв
          private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            cbav.Items.Clear();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = $"select FIO from AVg Where Phone=@pn";
            cmd.Parameters.AddWithValue("pn", sga.Text);
            cmd.Connection = connection;

            SQLiteDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                string name = dataReader["FIO"].ToString();
                cbav.Items.Add(name);
            }
            sga.Clear();
        } // Метод combobox. Поиск человека в архиве.
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            SQLiteCommand c = new SQLiteCommand($"select * from AVg Where FIO=@i");
            c.Parameters.AddWithValue("i",cbav.SelectedItem.ToString());
            c.Connection = connection;
            SQLiteDataReader reader  = c.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    string fo = reader.GetString(1);
                    string pe = reader.GetString(2);
                    string ws = reader.GetString(3);
                    fioG.Text = fo;
                    telg.Text = pe;
                    pozG.Text = ws;
                }
            }
        } // Вставить в textboxы данные по клиенту из архива

       
    }
}
