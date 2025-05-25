using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using eventagency.Model;

namespace eventagency.VM
{
    internal class LibrarysMvvm : BaseVM
    {
        private List<Client> orders;
        private string search;
        private Client? selectedClient;
        private EventContractor selectedOrder;
        public CommandMvvm RemoveClientWithData { get; set; }
        private ObservableCollection<EventContractor> eventContractors;
        public CommandMvvm RemoveOrder { get; set; }

        public ObservableCollection<EventContractor> EventContractors
        {
            get => eventContractors;
            set
            {
                eventContractors = value;
                Signal();
            }
        }

        public List<Client> Clients
        {
            get => orders;
            set
            {
                orders = value;
                Signal();
            }
        }

        public string Search
        {
            get => search;
            set
            {
                search = value;
                SearchClient(search);
            }
        }

        public Client? SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                Signal();
                SelectAll();
                ViewOrders = Visibility.Visible;
                ViewOrder = Visibility.Collapsed;
            }
        }

        public EventContractor SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                if (value != null)
                {
                    ViewOrder = Visibility.Visible;
                    ViewOrders = Visibility.Collapsed;
                }
                Signal();
            }
        }
        private void SelectAll()
        {
            if (SelectedClient != null)
            {
                var all = EventContractorDB.GetDb().SelectAll(SelectedClient.ID);
                EventContractors = new ObservableCollection<EventContractor>(all);
                TotalPrice = all.Sum(ec => ec.Price); // ← сумма всех заказов
            }
            else
            {
                EventContractors = new ObservableCollection<EventContractor>();
                TotalPrice = 0;
            }
        }
        private int totalPriceAll;
        public int TotalPriceAll
        {
            get => totalPriceAll;
            set
            {
                totalPriceAll = value;
                Signal();
            }

        }



        private void SearchClient(string search)
        {
            Clients = Library.GetTable().SearchClient(search);
        }

        public Visibility ViewOrders
        {
            get => viewOrders;
            set
            {
                viewOrders = value; 
                Signal();
            }
        }
        public Visibility ViewOrder
        {
            get => viewOrder;
            set
            {
                viewOrder = value;
                Signal();
            }
        }
        private int totalPrice;
        public int TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                Signal();
            }
        }

        public LibrarysMvvm()
        {
            Clients = Library.GetTable().SearchClient(search);
            ViewOrders = Visibility.Visible;
            ViewOrder = Visibility.Collapsed;

            RemoveClientWithData = new CommandMvvm(() =>
            {
                if (SelectedClient != null)
                {
                    if (MessageBox.Show("Удалить клиента и все связанные данные?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        ClientDB.GetDb().RemoveWithRelations(SelectedClient);
                        Clients = Library.GetTable().SearchClient("");
                        EventContractors = new ObservableCollection<EventContractor>();
                        SelectedClient = null;

                        // ⬇️ Обновляем сумму всех заказов
                        TotalPriceAll = EventContractorDB.GetDb().GetTotalPriceAll();
                    }
                }
            }, () => SelectedClient != null);

            TotalPriceAll = EventContractorDB.GetDb().GetTotalPriceAll();

        }



        Action close;
        private Visibility viewOrders;
        private Visibility viewOrder;



        internal void SetClose(Action close)
        {
            this.close = close;
        }
        
    }
}
