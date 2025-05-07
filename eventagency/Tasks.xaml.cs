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
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Window
    {
        public Tasks()
        {
            InitializeComponent();
            ((TasksMvvm)this.DataContext).SetClose(Close);
        }

        public Tasks(Order selectedOrder)
        {
            InitializeComponent();
            ((TasksMvvm)this.DataContext).SetClose(Close);
            ((TasksMvvm)this.DataContext).SelectedOrder = selectedOrder;
        }

        private void GoToNextPage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
