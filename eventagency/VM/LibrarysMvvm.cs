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
        private List<Client> clients;
        private string search;

        public List<Client> Clients
        {
            get => clients;
            set
            {
                clients = value;
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

        private void SearchClient(string search)
        {
            Clients = Library.GetTable().SearchClient(search);
        }
    }
}
