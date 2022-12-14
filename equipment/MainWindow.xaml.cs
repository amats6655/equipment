using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace equipment
{
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable usersTable;
        DataTable equipmentTable;
        DataTable orderTable;
        DataTable typesTable;
        DataTable lastUserTable;

        //TODO: ГОТОВО Добавить возможность редактирования количества в наличии при выдаче оборудования 
        //TODO: Создать маску на TextBox
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["equipmentDbConnect"].ConnectionString;
        }

        //Заполнение ComboBox при загрузке формы
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql_types = "SELECT * FROM [equip_types]";
            string sql_users = "SELECT [id], [name] FROM [users]";
            string sql_models = "SELECT [id], [model] FROM [equip]";
            typesTable = new DataTable();
            usersTable = new DataTable();
            equipmentTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command_types = new SqlCommand(sql_types, connection);
                adapter = new SqlDataAdapter(command_types);
                adapter.Fill(typesTable);
                SqlCommand command_users = new SqlCommand(sql_users, connection);
                adapter = new SqlDataAdapter(command_users);
                adapter.Fill(usersTable);
                SqlCommand command_models = new SqlCommand(sql_models, connection);
                adapter = new SqlDataAdapter(command_models);
                adapter.Fill(equipmentTable);

                cb_orders_user.ItemsSource = usersTable.DefaultView;
                cb_orders_user.DisplayMemberPath = usersTable.Columns["name"].ToString();
                cb_orders_user.SelectedValuePath = usersTable.Columns["id"].ToString();


                cb_orders_eqiup.ItemsSource = equipmentTable.DefaultView;
                cb_orders_eqiup.DisplayMemberPath = equipmentTable.Columns["model"].ToString();
                cb_orders_eqiup.SelectedValuePath = equipmentTable.Columns["id"].ToString();

                cb_equip_type.ItemsSource = typesTable.DefaultView;
                cb_equip_type.DisplayMemberPath = typesTable.Columns["type"].ToString();
                cb_equip_type.SelectedValuePath = typesTable.Columns["id"].ToString();
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

        //Загрузка данных в users
        private void CMO_users(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM [users]";
            usersTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(usersTable);
                UsersGrid.ItemsSource = usersTable.DefaultView;
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
        //Загрузка данных в equipment
        private void CMO_equipments(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM [equipment]";
            equipmentTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(equipmentTable);
                EquipmentsGrid.ItemsSource = equipmentTable.DefaultView;
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
        //загрузка данных в orders
        private void CMO_orders(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM [order]";
            orderTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(orderTable);
                OrdersGrid.ItemsSource = orderTable.DefaultView;
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


        // Обработка кнопок

        // Добавление нового пользователя
        private void Btn_users_add_Click(object sender, RoutedEventArgs e)
        {
            string users_name = tb_users_name.Text;
            string users_phone = tb_users_phone.Text;
            string sql = $"INSERT INTO users (name, phone, debt) VALUES  ('{users_name}', '{users_phone}', 0)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            tb_users_name.Clear();
            tb_users_phone.Clear();
        }
        // Добавление нового оборудования
        private void Btn_equip_add_Click(object sender, RoutedEventArgs e)
        {
            int type = int.Parse(cb_equip_type.SelectedValue.ToString());
            string model = tb_equip_model.Text;
            int amount = int.Parse(tb_equip_amount.Text);
            string sql = $"INSERT INTO equip (id_types, model, amt, rem) VALUES ('{type}', '{model}', '{amount}', '{amount}')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            tb_equip_model.Clear();
            tb_equip_amount.Clear();
        }
        // Выдача оборудования
        private void Btn_orders_add_Click(object sender, RoutedEventArgs e)
        {
            int model = int.Parse(cb_orders_eqiup.SelectedValue.ToString());
            int amount = int.Parse(tb_orders_amount.Text);
            DateTime date_issue = DateTime.Parse(dp_orders_issue.Text);
            DateTime date_return = DateTime.Parse(dp_orders_return.Text);

            string name = tb_order_name.Text;
            string phone = tb_order_phone.Text;

            string sql_updateAmount =
                $"UPDATE equip " +
                $"SET rem = rem - {amount} " +
                $"WHERE id = {model}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (cb_new_user.IsChecked == false)
                {
                    connection.Open();

                    // Добавляем информацию в order
                    int user = int.Parse(cb_orders_user.SelectedValue.ToString());
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
                else if (cb_new_user.IsChecked == true)
                {
                    connection.Open();

                    // Добавляем нового пользователя
                    string sql_addUser = $"INSERT users VALUES ('{name}', '{phone}', 1)";
                    lastUserTable = new DataTable();
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
                cb_orders_eqiup.SelectedIndex = -1;
                tb_orders_amount.Clear();
                dp_orders_issue.ClearValue(UidProperty);
                cb_orders_user.SelectedIndex = -1;
                dp_orders_return.ClearValue(UidProperty);
                tb_order_name.Clear();
                tb_order_phone.Clear();

            }
        }

        // Выдача оборудования по двойному клику по модели таблице оборудования
        private void MDC_equipment(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EquipmentsGrid.SelectedItems.Count == 0) return;
            int index = EquipmentsGrid.SelectedIndex;
            OrderWindow orderWindow = new OrderWindow(index);
            orderWindow.ShowDialog();
        }

        private void new_user(object sender, RoutedEventArgs e)
        {
            if (cb_new_user.IsChecked == true)
            {
                lbl_order_phone.Visibility = Visibility.Visible;
                lbl_order_name.Visibility = Visibility.Visible;
                tb_order_name.Visibility = Visibility.Visible;
                tb_order_phone.Visibility = Visibility.Visible;
                lbl_order_user.Visibility = Visibility.Collapsed;
                cb_orders_user.Visibility = Visibility.Collapsed;
            }
            else if (cb_new_user.IsChecked == false)
            {
                lbl_order_phone.Visibility = Visibility.Collapsed;
                lbl_order_name.Visibility = Visibility.Collapsed;
                tb_order_name.Visibility = Visibility.Collapsed;
                tb_order_phone.Visibility = Visibility.Collapsed;
                lbl_order_user.Visibility = Visibility.Visible;
                cb_orders_user.Visibility = Visibility.Visible;
            }
        }
    }
}
