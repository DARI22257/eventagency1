using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
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
        public CommandMvvm RemoveClient { get; set; }
        public CommandMvvm OpenEditClient { get; set; }
        public ClientsMvvm()
        {
            if (NewClient == null)
                NewClient = new Client();
            SelectAll();
            InsertClient = new CommandMvvm(() =>
            {
                if (NewClient.ID == 0)
                    ClientDB.GetDb().Insert(NewClient);
                else
                    ClientDB.GetDb().Update(NewClient);
                NewClient = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(NewClient.FullName) &&
                !string.IsNullOrEmpty(NewClient.Phone) &&
                !string.IsNullOrEmpty(NewClient.Email) &&
                !string.IsNullOrEmpty(NewClient.Notes));

            NextPage = new CommandMvvm(() =>
            {
                Events events = new Events(new Order { Client = SelectedClient });
                events.Show();
                close?.Invoke();
            },
            () => SelectedClient != null);

            RemoveClient = new CommandMvvm(() =>
            {
                ClientDB.GetDb().Remove(SelectedClient);
                SelectAll();
            }, () => SelectedClient != null);

            OpenEditClient = new CommandMvvm(() =>
            {
                int id = SelectedClient.ID;
                //ClientDB.GetDb().Update(SelectedClient);
                SelectAll();
                NewClient = Clients.FirstOrDefault(c => c.ID == id);

            }, () => SelectedClient != null);
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
