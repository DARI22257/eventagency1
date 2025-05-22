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
        public Order SelectedOrder { get; internal set; }
        public CommandMvvm RemoveContractor { get; set; }
        public CommandMvvm OpenEditContractor { get; set; }

        public ContractorsMvvm()
        {
            if (NewContractor == null)
                NewContractor = new Contractor();
            SelectAll();
            InsertContractor = new CommandMvvm(() =>
            {
                if (NewContractor.ID == 0)
                    ContractorDB.GetDb().Insert(NewContractor);
                else
                    ContractorDB.GetDb().Update(NewContractor);
                NewContractor = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newContractor.Title) &&
                !string.IsNullOrEmpty(newContractor.Type) &&
                !string.IsNullOrEmpty(newContractor.Email) &&
                !string.IsNullOrEmpty(newContractor.Notes));

            NextPage = new CommandMvvm(() =>
            {
                SelectedOrder.Contractor = SelectedContractor;
                Summary contractor = new Summary(SelectedOrder);
                contractor.Show();
                close?.Invoke();
            },
                () => SelectedOrder != null);

            RemoveContractor = new CommandMvvm(() =>
            {
                ContractorDB.GetDb().Remove(SelectedContractor);
                SelectAll();
            }, () => SelectedContractor != null);

            OpenEditContractor = new CommandMvvm(() =>
            {
                int id = SelectedContractor.ID;
                //ClientDB.GetDb().Update(SelectedClient);
                SelectAll();
                NewContractor = Contractors.FirstOrDefault(c => c.ID == id);

            }, () => SelectedContractor != null);
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
