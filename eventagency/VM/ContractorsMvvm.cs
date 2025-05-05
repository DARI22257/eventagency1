using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eventagency.Model;

namespace eventagency.VM
{
    internal class ContractorsMvvm : BaseVM
    {
        private Contractor newContractor = new();

        public Contractor NewContractor
        {
            get => newContractor;
            set
            {
                newContractor = value;
                Signal();
            }
        }

        private Contractor selectedContractor = new();

        public Contractor SelectedContractor
        {
            get => selectedContractor;
            set
            {
                selectedContractor = value;
                Signal();
            }
        }

        private ObservableCollection<Contractor> contractors = new();

        public ObservableCollection<Contractor> Contractors
        {
            get => contractors;
            set
            {
                contractors = value;
                Signal();
            }
        }


        public CommandMvvm InsertContractor { get; set; }

        public CommandMvvm NextPage { get; set; }
        public ContractorsMvvm()
        {
            SelectAll();
            InsertContractor = new CommandMvvm(() =>
            {
                ContractorDB.GetDb().Insert(NewContractor);
                NewContractor = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newContractor.Title) &&
                !string.IsNullOrEmpty(newContractor.Type) &&
                !string.IsNullOrEmpty(newContractor.Email) &&
                !string.IsNullOrEmpty(newContractor.Notes));

            //NextPage = new CommandMvvm(() =>
            //{
            //    Events events = new Events(SelectedClient);
            //    events.Show();
            //    close?.Invoke();
            //},
            //    () => SelectedClient != null);
        }

        private void SelectAll()
        {
            Contractors = new ObservableCollection<Contractor>(ContractorDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
