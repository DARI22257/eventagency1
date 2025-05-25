using eventagency.Model;
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

namespace eventagency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            LoadUpcomingEvents();
        }
        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            clients.Show();
        }
        private void NavigateButton_Click1(object sender, RoutedEventArgs e)
        {
            LibraryClients clients = new LibraryClients();
            clients.Show();
        }
        public void LoadUpcomingEvents()
        {
            List<Event> events = EventDB.GetDb().SelectAll();

            var upcoming = events
                .Where(ev => ev.Date >= DateTime.Today && ev.Date <= DateTime.Today.AddDays(30))
                .OrderBy(ev => ev.Date)
                .Select(ev => $"{ev.Date:dd MMMM}: {ev.Title}")
                .ToList();

            EventsBlock.Text = upcoming.Any()
                ? string.Join("\n", upcoming)
                : "Нет запланированных мероприятий в ближайшие 30 дней.";
        }
    }
}