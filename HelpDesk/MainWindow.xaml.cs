using Dapper;
using HelpDesk.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelpDesk.Data.Infrastructure;
using HelpDesk.Data.Repositories;


namespace HelpDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITicketRepository _tickets;
        public MainWindow()
        {
            InitializeComponent();

            //читаю строку подключения из appsettings.Development.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var cs = config.GetConnectionString("Postgres")!;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            //создаю репозиторий напрямую
            _tickets = new TicketRepository(new PostgresConnectionFactory(cs));
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //запрашиваю все заявки из бд и привязываю их к таблице
                var tickets = await _tickets.GetAllAsync();
                TicketsGrid.ItemsSource = tickets;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}