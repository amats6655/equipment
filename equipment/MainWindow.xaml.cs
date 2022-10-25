using Dynamitey.DynamicObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace equipment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable usersTable;
        DataTable equipmentTable;
        DataTable orderTable;
        DataTable typesTable;


        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["equipmentDbConnect"].ConnectionString;
            OrdersGrid.RowEditEnding += OrdersGrid_RowEditEnding;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM [equip_types]";
            typesTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(typesTable);
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

        /// Загрузка данных в TabItem
        /// 

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
            catch(Exception ex)
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
            string sql = "SELECT * FROM [orders]";
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

        private void Btn_orders_add_Click(object sender, RoutedEventArgs e)
        {
        //    ordersTableAdapter equipment_rentDataSetordersTableAdapter = new ordersTableAdapter();
        //    equipment_rentDataSetordersTableAdapter.Insert((int)cb_orders_user.SelectedValue, (int)cb_orders_eqiup.SelectedValue, int.Parse(tb_orders_amount.Text),
        //        DateTime.Parse(dp_orders_issue.Text), DateTime.Parse(dp_orders_return.Text), false);

        //    equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
        //    orderTableAdapter equipment_rentDataSetorderTableAdapter = new orderTableAdapter();
        //    equipment_rentDataSetorderTableAdapter.Fill(equipment_rentDataSet.order);
        //    tb_orders_amount.Clear();
        }

        private void OrdersGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _ = new SqlCommandBuilder(adapter);
            adapter.Update(orderTable);
        }



        private void MDC_equipment(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EquipmentsGrid.SelectedItems.Count == 0) return;
            String model = ((DataRowView)EquipmentsGrid.SelectedItems[0]).Row["model"].ToString();
            int orderID = (int)((DataRowView)EquipmentsGrid.SelectedItems[0]).Row["id"];
            MessageBox.Show("Сюда можно добавить новый заказ" + model + " " + orderID );
        }
    }
}
 