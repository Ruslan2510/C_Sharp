using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;

namespace HW_DB_Boroday
{
    class DBClass
    {
        public static string GetConnectionStrings()
        {
            string connectionString = String.Format($"Data Source = 192.168.56.2; Initial Catalog = HomeTaskDB;Persist Security Info=False;User={ConnectionWindow.login};" +
                $"PWD = {ConnectionWindow.password}; Pooling = False; MultipleActiveResultSets = False; Encrypt = False; TrustServerCertificate = False");
            return connectionString;
        }

        public static string sqlCommand;
        public static SqlConnection connection = new SqlConnection();
        public static SqlCommand command;
       
        public static SqlDataReader reader;
        public static SqlDataAdapter adapter;
        public static bool isConnected = false;
        public static MainWindow mainWindow;

        public static void Connection()
        {
            openConnection();
            if (isConnected)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        public static void openConnection()
        {
            try
            {
                connection.ConnectionString = GetConnectionStrings();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    isConnected = true;
                }
                else
                {
                    throw new Exception("Неправильный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The system failed to establish a connection.\n" +
                    $"Description: {ex.Message}", "C# WPF Connect to SQL Server", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public static void closeConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ShowDB(string table)
        {
            string sqlCommand = string.Format($"SELECT * FROM {table}");
            try
            {
                command = new SqlCommand(sqlCommand, connection);
                command.ExecuteNonQuery();

                adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable(string.Format($"{table}"));
                adapter.Fill(dataTable);
                mainWindow.BottomDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"Description: {ex.Message}", "Error\n", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FirstTask()
        {
            string biddingId = "";
            string securitiesId = "";
            string securitiesAmount = "";
            string dealDate = "";
            string customer = "";
            string dealType = "";
            try
            {
                biddingId = mainWindow.FirstTaskBiddingId.Text;
                securitiesId = mainWindow.FirstTaskSecuritiesId.Text;
                securitiesAmount = mainWindow.FirstTaskSecuritiesAmount.Text;
                dealDate = mainWindow.LabelCurrentDateTime.Content.ToString();
                customer = mainWindow.ComboBoxCustomer.Text;
                dealType = mainWindow.ComboBoxDealType.Text;
                if (biddingId.Equals("") || securitiesId.Equals("") || securitiesAmount.Equals(""))
                {
                    throw new Exception("Ошибка ввода данных");
                }
                if (customer.Equals("Не выбрано"))
                {
                    throw new Exception("Ошибка ввода клиента!");
                }
                if (dealType.Equals("Не выбрано"))
                {
                    throw new Exception("Ошибка ввода типа сделки!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (customer.Equals("Биржа"))
            {
                customer = "Б";
            }
            else if (customer.Equals("Человек"))
            {
                customer = "К";
            } 
            
            sqlCommand = String.Format($"EXEC SecuritiesOperations {biddingId}, {securitiesId}, {securitiesAmount}, " +
                                $"\'{dealDate}\', \'{customer}\', \'{dealType}\'");

            DataTable dataTable;
            try
            {
                dataTable = new DataTable();
                command = new SqlCommand(sqlCommand, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                MessageBox.Show("Транзакция проведена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SecondTask()
        {
            string brokerId = "";

            try
            {
                brokerId = mainWindow.SecondTaskBrokerId.Text;
                if (brokerId.Equals(""))
                {
                    throw new Exception("Ошибка ввода данных!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            sqlCommand = String.Format($"SELECT * FROM GetExchangeSecurities({brokerId})");

            DataTable dataTable = new DataTable();
            try
            {
                command = new SqlCommand(sqlCommand, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                mainWindow.BottomDataGrid.ItemsSource = dataTable.DefaultView;
                MessageBox.Show("Функция выполнена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ThirdTask()
        {
            string dateFrom = string.Format($"{mainWindow.ThirdTaskDateLabelFrom.Content.ToString()} {mainWindow.ThirdTaskTimeLabelFrom.Content.ToString()}");
            string dateTo = string.Format($"{mainWindow.ThirdTaskDateLabelTo.Content.ToString()} {mainWindow.ThirdTaskTimeLabelTo.Content.ToString()}");


            if (DateTime.Compare(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)) > 0)
            {
                string temp = dateFrom;
                dateFrom = dateTo;
                dateTo = temp;
            }
            else if (DateTime.Compare(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)) == 0)
            {
                MessageBox.Show("Даты не могу совпадать.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            sqlCommand = String.Format($"SELECT dbo.GetBrokerageCompanyIncome (\'{dateFrom}\', \'{dateTo}\')");


            try
            {
                command = new SqlCommand(sqlCommand, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ReadSingleRow((IDataRecord)reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                reader.Close();
            }
        }

        private static void ReadSingleRow(IDataRecord record)
        {
            MessageBox.Show(String.Format("{0}", record[0]), "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void FourthTask()
        {
            sqlCommand = "SELECT * FROM GetMostProfitableSecurities()";

            DataTable dataTable = new DataTable();
            try
            {
                command = new SqlCommand(sqlCommand, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                mainWindow.BottomDataGrid.ItemsSource = dataTable.DefaultView;
                MessageBox.Show("Функция выполнена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FifthTask()
        {
            string dateFrom = string.Format($"{mainWindow.FifthTaskDateLabelFrom.Content.ToString()} {mainWindow.FifthTaskTimeLabelFrom.Content.ToString()}");
            string dateTo = string.Format($"{mainWindow.FifthTaskDateLabelTo.Content.ToString()} {mainWindow.FifthTaskTimeLabelTo.Content.ToString()}");


            if (DateTime.Compare(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)) > 0)
            {
                string temp = dateFrom;
                dateFrom = dateTo;
                dateTo = temp;
            }
            else if (DateTime.Compare(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)) == 0)
            {
                MessageBox.Show("Даты не могу совпадать.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            sqlCommand = String.Format($"SELECT * FROM dbo.GetBurseIncome (\'{dateFrom}\', \'{dateTo}\')");


            DataTable dataTable = new DataTable();
            try
            {
                command = new SqlCommand(sqlCommand, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                mainWindow.BottomDataGrid.ItemsSource = dataTable.DefaultView;
                MessageBox.Show("Функция выполнена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SixthTask()
        {
            string date = string.Format($"{mainWindow.SixthTaskDateLabel.Content.ToString()} {mainWindow.SixthTaskTimeLabel.Content.ToString()}");

            sqlCommand = String.Format($"SELECT * FROM dbo.GetSecuritiesAmountOnBurse (\'{date}\')");


            DataTable dataTable = new DataTable();
            try
            {
                command = new SqlCommand(sqlCommand, connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                mainWindow.BottomDataGrid.ItemsSource = dataTable.DefaultView;
                MessageBox.Show("Функция выполнена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}