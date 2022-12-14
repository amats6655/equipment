using Dynamitey.DynamicObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace equipment
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        readonly string connectionString;
        SqlDataAdapter adapter;
        DataTable usersTable;
        DataTable equipmentTable;
        DataTable lastUserTable;
        int localIndex;
        //TODO: ГОТОВО Добавить возможность указывать нового пользователя, если нет в списке

        public OrderWindow(int index)
        {
            InitializeComponent();
            localIndex = index;
            connectionString = ConfigurationManager.ConnectionStrings["equipmentDbConnect"].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql_users = "SELECT [id], [name] FROM [users]";
            string sql_models = "SELECT [id], [model] FROM [equip]";
            usersTable = new DataTable();
            equipmentTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command_users = new SqlCommand(sql_users, connection);
                adapter = new SqlDataAdapter(command_users);
                adapter.Fill(usersTable);
                SqlCommand command_models = new SqlCommand(sql_models, connection);
                adapter = new SqlDataAdapter(command_models);
                adapter.Fill(equipmentTable);

                cb_user.ItemsSource = usersTable.DefaultView;
                cb_user.DisplayMemberPath = usersTable.Columns["name"].ToString();
                cb_user.SelectedValuePath = usersTable.Columns["id"].ToString();


                cb_model.ItemsSource = equipmentTable.DefaultView;
                cb_model.DisplayMemberPath = equipmentTable.Columns["model"].ToString();
                cb_model.SelectedValuePath = equipmentTable.Columns["id"].ToString();
                cb_model.SelectedIndex = localIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

            private void Button_Click(object sender, RoutedEventArgs e)
        {
            int model = int.Parse(cb_model.SelectedValue.ToString());
            int amount = int.Parse(tb_amount.Text);
            DateTime date_issue = DateTime.Parse(dp_date_issue.Text);
            DateTime date_return = DateTime.Parse(dp_date_return.Text);

            string name = tb_name.Text;
            string phone = tb_phone.Text;

            string sql_updateAmount =
                $"UPDATE equip " +
                $"SET rem = rem - {amount} " +
                $"WHERE id = {model}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (cb_newUser.IsChecked == false)
                {
                    int user = int.Parse(cb_user.SelectedValue.ToString());
                    string sql_addOrder =
                        $"INSERT INTO orders (id_user, id_equip, amount, date_issue, date_return) " +
                        $"VALUES ('{user}', '{model}', '{amount}', '{date_issue}', '{date_return}')";
                    connection.Open();
                    SqlCommand command_insertOrder = new SqlCommand(sql_addOrder, connection);
                    command_insertOrder.ExecuteNonQuery();
                    SqlCommand command_update = new SqlCommand(sql_updateAmount, connection);
                    command_update.ExecuteNonQuery();

                    connection.Close();
                }
                else if (cb_newUser.IsChecked == true)
                {
                    // Добавляем нового пользователя
                    string sql_addUser = $"INSERT users VALUES ('{name}', '{phone}', 1)";
                    lastUserTable = new DataTable();
                    connection.Open();
                    SqlCommand command_insertUser = new SqlCommand(sql_addUser, connection);
                    command_insertUser.ExecuteNonQuery();

                    // Выбираем последнего добавленного пользователя
                    SqlCommand get_userID = new SqlCommand("SELECT TOP 1 * FROM [users] ORDER BY id DESC", connection);
                    adapter = new SqlDataAdapter(get_userID);
                    adapter.Fill(lastUserTable);
                    int user = int.Parse(lastUserTable.Rows[0][0].ToString());

                    // Привязываем к нему выдачу оборудования
                    string sql_addOrder =
                        $"INSERT INTO orders (id_user, id_equip, amount, date_issue, date_return) " +
                        $"VALUES ('{user}', '{model}', '{amount}', '{date_issue}', '{date_return}')";
                    SqlCommand command_insertOrder = new SqlCommand(sql_addOrder, connection);
                    command_insertOrder.ExecuteNonQuery();

                    // Изменяем остаток оборудования
                    SqlCommand command_update = new SqlCommand(sql_updateAmount, connection);
                    command_update.ExecuteNonQuery();
                    connection.Close();
                }
            }
            this.DialogResult = true;
        }

        private void new_user(object sender, RoutedEventArgs e)
        {
            if (cb_newUser.IsChecked == true)
            {
                lbl_phone.Visibility = Visibility.Visible;
                lbl_name.Visibility = Visibility.Visible;
                tb_name.Visibility = Visibility.Visible;
                tb_phone.Visibility = Visibility.Visible;
                lbl_user.Visibility = Visibility.Collapsed;
                cb_user.Visibility = Visibility.Collapsed;
            }
            else if(cb_newUser.IsChecked == false)
            {
                lbl_phone.Visibility = Visibility.Collapsed;
                lbl_name.Visibility = Visibility.Collapsed;
                tb_name.Visibility = Visibility.Collapsed;
                tb_phone.Visibility = Visibility.Collapsed;
                lbl_user.Visibility = Visibility.Visible;
                cb_user.Visibility = Visibility.Visible;
            }
        }
    }
}
