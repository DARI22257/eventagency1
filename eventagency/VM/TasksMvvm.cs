using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using eventagency.Model;


namespace eventagency.VM
{
    internal class TasksMvvm : BaseVM
    {
        private TaskWork newTask = new();

        public TaskWork NewTask
        {
            get => newTask;
            set
            {
                newTask = value;
                Signal();
            }
        }

        private TaskWork selectedTask = new();

        public TaskWork SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                Signal();
            }
        }

        private ObservableCollection<TaskWork> tasks = new();

        public ObservableCollection<TaskWork> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                Signal();
            }
        }


        public CommandMvvm InsertTask { get; set; }

        public CommandMvvm NextPage { get; set; }
        public Order SelectedOrder { get; internal set; }

        public TasksMvvm()
        {
            SelectAll();
            InsertTask = new CommandMvvm(() =>
            {
                TaskDB.GetDb().Insert(NewTask);
                NewTask = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newTask.Title) &&
                !string.IsNullOrEmpty(newTask.Description) &&
                DateTime.MinValue != newTask.Term &&
                !string.IsNullOrEmpty(newTask.Assigned) &&
                !string.IsNullOrEmpty(newTask.Status));

            NextPage = new CommandMvvm(() => {
                SelectedOrder.Task = SelectedTask;
                Contractors tasks = new Contractors(SelectedOrder);
                tasks.Show();
                close?.Invoke();
            },
                () => SelectedTask != null);
        }

        private void SelectAll()
        {
            Tasks = new ObservableCollection<TaskWork>(TaskDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
