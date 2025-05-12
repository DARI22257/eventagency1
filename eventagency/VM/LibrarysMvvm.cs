using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eventagency.Model;

namespace eventagency.VM
{
    internal class LibrarysMvvm : BaseVM
    {
        private List<Client> orders;
        private string search;
        private Client? selectedClient;
        private Order selectedOrder;

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
                SelectedOrder = Library.GetTable().GetLastOrder(value);
            }
        }

        public Order SelectedOrder { 
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                Signal();
            }
        }


        private void SearchClient(string search)
        {
            Clients = Library.GetTable().SearchClient(search);
        }
    }
}
