using equipment.equipment_rentDataSetTableAdapters;
using System;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");

            // Загрузить данные в таблицу users
            usersTableAdapter equipment_rentDataSetusersTableAdapter = new usersTableAdapter();
            equipment_rentDataSetusersTableAdapter.Fill(equipment_rentDataSet.users);
            CollectionViewSource usersViewSource = (CollectionViewSource)FindResource("usersViewSource");
            usersViewSource.View.MoveCurrentToFirst();

            // Загрузить данные в таблицу types
            equip_typesTableAdapter equipment_rentDataSetequip_typesTableAdapter = new equip_typesTableAdapter();
            equipment_rentDataSetequip_typesTableAdapter.Fill(equipment_rentDataSet.equip_types);

            // Загрузить данные в таблицу equipment
            equipmentTableAdapter equipment_rentDataSetequipmentTableAdapter = new equipmentTableAdapter();
            equipment_rentDataSetequipmentTableAdapter.Fill(equipment_rentDataSet.equipment);
            CollectionViewSource equipmentViewSource = (CollectionViewSource)FindResource("equipmentViewSource");
            equipmentViewSource.View.MoveCurrentToFirst();

            // Загрузить данные в таблицу order
            orderTableAdapter equipment_rentDataSetorderTableAdapter = new orderTableAdapter();
            equipment_rentDataSetorderTableAdapter.Fill(equipment_rentDataSet.order);
            CollectionViewSource orderViewSource = (CollectionViewSource)FindResource("orderViewSource");
            orderViewSource.View.MoveCurrentToFirst();

            // Загрузить данные в таблицу equip
            equipTableAdapter equipment_rentDataSetequipTableAdapter = new equipTableAdapter();
            equipment_rentDataSetequipTableAdapter.Fill(equipment_rentDataSet.equip);
            CollectionViewSource equipViewSource = (CollectionViewSource)FindResource("equipViewSource");
            equipViewSource.View.MoveCurrentToFirst();
        }

        private void btn_users_add_Click(object sender, RoutedEventArgs e)
        {
            usersTableAdapter equipment_rentDataSetusersTableAdapter = new usersTableAdapter();
            equipment_rentDataSetusersTableAdapter.Insert(tb_users_name.Text, tb_users_phone.Text, cb_users_admin.IsChecked);

            equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
            equipment_rentDataSetusersTableAdapter.Fill(equipment_rentDataSet.users);
            tb_users_name.Clear();
            tb_users_phone.Clear();
        }

        private void btn_equip_add_Click(object sender, RoutedEventArgs e)
        {
            equipTableAdapter equipment_rentDataSetEquipTableAdapter = new equipTableAdapter();
            equipment_rentDataSetEquipTableAdapter.Insert((int)cb_equip_type.SelectedValue, tb_equip_model.Text, int.Parse(tb_equip_amount.Text), int.Parse(tb_equip_amount.Text));

            equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
            equipmentTableAdapter equipment_rentDataSetequipmentTableAdapter = new equipmentTableAdapter();
            equipment_rentDataSetequipmentTableAdapter.Fill(equipment_rentDataSet.equipment);
            tb_equip_model.Clear();
            tb_equip_amount.Clear();
        }

        private void Btn_orders_add_Click(object sender, RoutedEventArgs e)
        {
            ordersTableAdapter equipment_rentDataSetordersTableAdapter = new ordersTableAdapter();
            equipment_rentDataSetordersTableAdapter.Insert((int)cb_orders_user.SelectedValue, (int)cb_orders_eqiup.SelectedValue, int.Parse(tb_orders_amount.Text),
                DateTime.Parse(dp_orders_issue.Text), DateTime.Parse(dp_orders_return.Text), false);

            equipment_rentDataSet equipment_rentDataSet = (equipment_rentDataSet)FindResource("equipment_rentDataSet");
            orderTableAdapter equipment_rentDataSetorderTableAdapter = new orderTableAdapter();
            equipment_rentDataSetorderTableAdapter.Fill(equipment_rentDataSet.order);
            tb_orders_amount.Clear();
        }



        private void Bold_Checked(object sender, RoutedEventArgs e)
        {
            DG_orders.FontWeight = FontWeights.Bold;
        }

        private void Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            DG_orders.FontWeight = FontWeights.Normal;
        }

        private void Italic_Checked(object sender, RoutedEventArgs e)
        {
            DG_orders.FontStyle = FontStyles.Italic;
        }

        private void Italic_Unchecked(object sender, RoutedEventArgs e)
        {
            DG_orders.FontStyle = FontStyles.Normal;
        }

        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {
            if (DG_orders.FontSize < 18)
            {
                DG_orders.FontSize += 2;
            }
        }

        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            if (DG_orders.FontSize > 10)
            {
                DG_orders.FontSize -= 2;
            }
        }

        private void ReturnOrder_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
