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
                EventContractors = new ObservableCollection<EventContractor>( EventContractorDB.GetDb().SelectAll(SelectedClient.ID));
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

        public LibrarysMvvm()
        {
            Clients = Library.GetTable().SearchClient(search);
            ViewOrders = Visibility.Visible;
            ViewOrder = Visibility.Collapsed;

            RemoveOrder = new CommandMvvm(() =>
            {
                EventContractorDB.GetDb().Remove(SelectedOrder);
                SelectAll();
            }, () => SelectedOrder != null);
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
