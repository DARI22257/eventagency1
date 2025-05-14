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
using eventagency.Model;
using eventagency.VM;

namespace eventagency
{
    /// <summary>
    /// Логика взаимодействия для Summary.xaml
    /// </summary>
    public partial class Summary : Window
    {
        public Summary()
        {
            InitializeComponent();
            ((EventContractorMvvm)this.DataContext).SetClose(Close);
        }
        public Summary(Order selectedOrder)
        {
            InitializeComponent();
            ((EventContractorMvvm)this.DataContext).SetClose(Close);
            ((EventContractorMvvm)this.DataContext).SelectedOrder = selectedOrder;
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
