﻿using equipment.equipment_rentDataSetTableAdapters;
using System;
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


        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["equipmentDbConnect"].ConnectionString;
            OrdersGrid.RowEditEnding += OrdersGrid_RowEditEnding;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
/// Добавить возможность добавления order из equipment grid
///
///
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

        private void btn_users_add_Click(object sender, RoutedEventArgs e)
        {
            //usersTableAdapter equipment_rentDataSetusersTableAdapter = new usersTableAdapter();
            //equipment_rentDataSetusersTableAdapter.Insert(tb_users_name.Text, tb_users_phone.Text, cb_users_admin.IsChecked);

            //equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
            //equipment_rentDataSetusersTableAdapter.Fill(equipment_rentDataSet.users);
            //tb_users_name.Clear();
            //tb_users_phone.Clear();
        }

        private void btn_equip_add_Click(object sender, RoutedEventArgs e)
        {
            //equipTableAdapter equipment_rentDataSetEquipTableAdapter = new equipTableAdapter();
            //equipment_rentDataSetEquipTableAdapter.Insert((int)cb_equip_type.SelectedValue, tb_equip_model.Text, int.Parse(tb_equip_amount.Text), int.Parse(tb_equip_amount.Text));

            //equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
            //equipmentTableAdapter equipment_rentDataSetequipmentTableAdapter = new equipmentTableAdapter();
            //equipment_rentDataSetequipmentTableAdapter.Fill(equipment_rentDataSet.equipment);
            //tb_equip_model.Clear();
            //tb_equip_amount.Clear();
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
    }
}
