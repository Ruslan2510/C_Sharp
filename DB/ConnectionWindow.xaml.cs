using System.Windows;
using System.Windows.Controls;


namespace HW_DB_Boroday
{
    public partial class ConnectionWindow : Window
    {
        public static string login;
        public static string password;
        public ConnectionWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            login = LoginTBox.Text;
            password = PasswordBox.Password;
            ConnectionButton.Content = "Connection...";
            DBClass.Connection();
            ConnectionButton.Content = "Connect";
            if (DBClass.isConnected)
            {
                this.Close();
            }
        }
    }
}
