using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using eventagency.VM;

namespace eventagency
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        public Clients()
        {
            InitializeComponent();
            ((ClientsMvvm)this.DataContext).SetClose(Close);
        }
        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Events events = new Events();
            events.Show();
        }
    }
}
