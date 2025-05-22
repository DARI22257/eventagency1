using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using eventagency.Model;

namespace eventagency.VM
{
    internal class EventsMvvm : BaseVM
    {
        private Event newEvent = new();

        public Event NewEvent
        {
            get => newEvent;
            set
            {
                newEvent = value;
                Signal();
            }
        }

        private Event selectedEvent = new();

        public Event SelectedEvent
        {
            get => selectedEvent;
            set
            {
                selectedEvent = value;
                Signal();
            }
        }

        private ObservableCollection<Event> events = new();

        public ObservableCollection<Event> Events
        {
            get => events;
            set
            {
                events = value;
                Signal();
            }
        }


        public CommandMvvm InsertEvent { get; set; }

        public CommandMvvm NextPage { get; set; }
        public Order SelectedOrder { get; internal set; }
        public CommandMvvm RemoveEvent { get; set; }
        public CommandMvvm OpenEditClient { get; set; }

        public EventsMvvm()
        {
            if (NewEvent == null)
                NewEvent = new Event();
            SelectAll();
            InsertEvent = new CommandMvvm(() =>
            {
                if (NewEvent.ID == 0)
                    EventDB.GetDb().Insert(NewEvent);
                else
                    EventDB.GetDb().Update(NewEvent);
                NewEvent = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(NewEvent.Title) &&
                DateTime.MinValue != NewEvent.Date &&
                !string.IsNullOrEmpty(NewEvent.Place) &&
                NewEvent.Budget != 0 &&
                !string.IsNullOrEmpty(NewEvent.Status));

            NextPage = new CommandMvvm(() =>
            {
                SelectedOrder.Event = SelectedEvent;
                Tasks tasks = new Tasks(SelectedOrder);
                tasks.Show();
                close?.Invoke();
            },
                () => SelectedEvent != null);
            RemoveEvent = new CommandMvvm(() =>
            {
                EventDB.GetDb().Remove(SelectedEvent);
                SelectAll();
            }, () => SelectedEvent != null);

            OpenEditClient = new CommandMvvm(() =>
            {
                int id = SelectedEvent.ID;
                //ClientDB.GetDb().Update(SelectedClient);
                SelectAll();
                NewEvent = Events.FirstOrDefault(c => c.ID == id);

            }, () => SelectedEvent != null);
        }

        private void SelectAll()
        {
            Events = new ObservableCollection<Event>(EventDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}

