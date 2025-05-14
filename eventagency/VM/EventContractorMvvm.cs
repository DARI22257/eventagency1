using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eventagency.Model;

namespace eventagency.VM
{
    internal class EventContractorMvvm : BaseVM
    {
        private EventContractor newEventContractor = new();

        public EventContractor NewEventContractor
        {
            get => newEventContractor;
            set
            {
                newEventContractor = value;
                Signal();
            }
        }

        private EventContractor selectedEventContractor = new();

        public EventContractor SelectedEventContractor
        {
            get => selectedEventContractor;
            set
            {
                selectedEventContractor = value;
                Signal();
            }
        }



        public CommandMvvm InsertEventContractor { get; set; }

        public CommandMvvm NextPage { get; set; }
        public Order SelectedOrder { 
            get => selectedOrder; 
            internal set
            {
                selectedOrder = value;
                Signal();
            }
        }

        public EventContractorMvvm()
        {
            InsertEventContractor = new CommandMvvm(() =>
            {
                NewEventContractor = new EventContractor { 
                 DescriptionService = SelectedOrder.EventContractor.DescriptionService,
                  Price = SelectedOrder.EventContractor.Price,
                    idTask = SelectedOrder.Task.ID,
                    idClient = SelectedOrder.Client.ID,
                    idEvents = SelectedOrder.Event.ID,
                    idContractor = SelectedOrder.Contractor.ID,


                };
                if(EventContractorDB.GetDb().Insert(NewEventContractor))
                {
                    close();
                    LibraryClients library = new LibraryClients();
                    library.Show();
                }

                NewEventContractor = new();
                
            },
                () => SelectedOrder != null &&
                 SelectedOrder.EventContractor.Price != 0 &&
                !string.IsNullOrEmpty(SelectedOrder.EventContractor.DescriptionService));


        }

        
        Action close;
        private Order selectedOrder;

        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
