using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eventagency.Model;

namespace eventagency.VM
{
    internal class ClientsMvvm : BaseVM
    {
        private Client newClient = new();

        public Client NewClient
        {
            get => newClient;
            set
            {
                newClient = value;
                Signal();
            }
        }

        private Client selectedClient = new();

        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                Signal();
            }
        }

        private ObservableCollection<Client> clients = new();

        public ObservableCollection<Client> Clients
        {
            get => clients;
            set
            {
                clients = value;
                Signal();
            }
        }


        public CommandMvvm InsertClient { get; set; }

        public CommandMvvm NextPage { get; set; }
        public ClientsMvvm()
        {
            SelectAll();
            InsertClient = new CommandMvvm(() =>
            {
                ClientDB.GetDb().Insert(NewClient);
                NewClient = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newClient.FullName) &&
                !string.IsNullOrEmpty(newClient.Phone) &&
                !string.IsNullOrEmpty(newClient.Email) &&
                !string.IsNullOrEmpty(newClient.Notes));

            NextPage = new CommandMvvm(() =>
            {
                Events events = new Events(new Order { Client = SelectedClient });
                events.Show();
                close?.Invoke();
            },
                () => SelectedClient != null);
        }

        private void SelectAll()
        {
            Clients = new ObservableCollection<Client>(ClientDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
