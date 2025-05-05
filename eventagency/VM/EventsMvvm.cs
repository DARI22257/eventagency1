using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public EventsMvvm()
        {
            SelectAll();
            InsertEvent = new CommandMvvm(() =>
            {
                EventDB.GetDb().Insert(NewEvent);
                NewEvent = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newEvent.Title) &&
                DateOnly.MinValue != newEvent.Date &&
                !string.IsNullOrEmpty(newEvent.Place) &&
                newEvent.Budget != 0 &&
                !string.IsNullOrEmpty(newEvent.Status));

            NextPage = new CommandMvvm(() =>
            {
                Events tasks = new Events(SelectedEvent);
                tasks.Show();
                close?.Invoke();
            },
                () => SelectedEvent != null);
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

