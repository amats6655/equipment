using System;
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
        string connectionString;
        SqlDataAdapter adapter;
        DataTable usersTable;
        DataTable equipmentTable;
        int localIndex;
        //TODO: Добавить возможность указывать нового пользователя, если нет в списке

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
            int user = int.Parse(cb_user.SelectedValue.ToString());
            DateTime date_return = DateTime.Parse(dp_date_return.Text);
            string sql = $"INSERT INTO orders (id_user, id_equip, amount, date_issue, date_return) " +
                $"VALUES ('{user}', '{model}', '{amount}', '{date_issue}', '{date_return}')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            this.DialogResult = true;
        }
    }
}
