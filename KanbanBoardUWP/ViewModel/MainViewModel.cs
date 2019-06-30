﻿using KanbanBoardUWP.Base;
using Syncfusion.UI.Xaml.Kanban;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoardUWP.ViewModel
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<KanbanModel> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> ColorKeys { get; set; }
        public KanbanModel Task = new KanbanModel();
        private KanbanModel _originalCardModel;
        private KanbanModel _cardModel;
        private ObservableCollection<string> _tagsCollection;
        private ObservableCollection<KanbanModel> _tasks;

        public MainViewModel()
        {
            Tasks = DataAccess.GetData();
        }

        public void EditTaskHelper(KanbanModel selectedModel, ObservableCollection<string> categories, ObservableCollection<string> colorKeys, ObservableCollection<string> tags)
        {
            // Get content ready to show in splitview pane
            OriginalCardModel = selectedModel;
            CardModel = selectedModel;
            Categories = categories;
            ColorKeys = colorKeys;
            TagsCollection = tags;

            // Store tags as a single string using csv format
            // When calling GetData(), the string will be parsed into separate tags and stored into the list view
            //List<string> tagsList = new List<string>();
            //foreach (var tag in lstViewTags.Items)
            //    tagsList.Add(tag.ToString());
            //var tags = string.Join(',', tagsList); // Convert to a csv string to store in database cell
        }

        public void AddTagToCollection(string tag)
        {
            TagsCollection.Add(tag);
        }

        public void NewTaskHelper()
        {
            CardModel = null;
            // Create null items 
            //ID = null;
            //Title = null;
            //Description = null;
            //Category = null;
            //ColorKey = null;
            //Tags = null;

            // Try? 
            //_selectedCard = null;
        }


        public void SaveTask(string tags)
        {
            // Tags are stroed as string[] in KanbanModel
            // STrip string into a string[]
            var tagsArray = tags.Split(',');

            // Create model and add to Tasks Colelction
            var newModel = new KanbanModel
            {
                ID = ID,
                Title = Title,
                Description = Description,
                Category = Category,
                ColorKey = ColorKey,
                Tags = tagsArray
            };

            // Update item in collection
            // DEBUG ISSUE -- Deletes item
            var found = Tasks.FirstOrDefault(x => x.ID == ID);
            int i = Tasks.IndexOf(found);
            Tasks[i] = newModel;

            // Update item in database
            //DataAccess.UpdateTask(ID, Title,
            //    Description, "Open",
            //    "Low", tags);
        }

        public void AddTask(string tags)
        {
            // Tags are stored as as string[] in KanbanModel
            // Strip string into a sting[]
            string[] tagsArray = new string[] { };
            if (tags != null) 
                tagsArray = tags.Split(',');
            else
                tags = ""; // No tags

            // Create model and add to Tasks collection
            var model = new KanbanModel
            {
                ID = ID,
                Title = Title,
                Description = Description,
                Category = "Open",
                ColorKey = "Low",
                Tags = tagsArray
            };
            Tasks.Add(model);

            // Add task to database
            DataAccess.AddTask(Title,
                Description, "Open",
                "Low", tags);
        }

        public KanbanModel OriginalCardModel
        {
            get;
            set;
        }

        public ObservableCollection<string> TagsCollection
        {
            get { return _tagsCollection; }
            set
            {
                _tagsCollection = value;
                OnPropertyChanged();
            }
        }

        public KanbanModel CardModel
        {
            get { return _cardModel; }
            set
            {
                _cardModel = value;

                // Update Task Properties to Selected Cards
                if (_cardModel == null)
                {
                    ID = null;
                    Title = null;
                    Description = null;
                    Category = null;
                    ColorKey = null;
                    Tags = new string[] { };
                    TagsCollection = new ObservableCollection<string>();
                    OnPropertyChanged();
                }
                else
                {
                    ID = _cardModel.ID;
                    Title = _cardModel.Title;
                    Description = _cardModel.Description;
                    Category = _cardModel.Category.ToString();
                    ColorKey = _cardModel.ColorKey.ToString();
                    Tags = _cardModel.Tags;
                    OnPropertyChanged();
                }
            }
        }

        public string ID
        {
            get
            {
                if (Task.ID == null)
                    return "";
                else
                    return Task.ID;
            }
            set
            {
                Task.ID = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                if (Task.Title == null)
                    return "";
                else
                    return Task.Title;
            }
            set
            {
                Task.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                if (Task.Description == null)
                    return "";
                else
                    return Task.Description;
            }
            set
            {
                Task.Description = value;
                OnPropertyChanged();
            }
        }

        public object Category
        {
            get {
                return Task.Category;
            }
            set
            {
                Task.Category = value;
                OnPropertyChanged();
            }
        }

        public object ColorKey
        {
            get
            {
                return Task.ColorKey;
            }
            set
            {
                Task.ColorKey = value;
                OnPropertyChanged();
            }
        }

        public string[] Tags
        {
            get
            {
                //if (Model.Tags == null)
                //    return;
                //else
                //    return Model.Tags;
                return Task.Tags;
            }
            set
            {
                Task.Tags = value;
                OnPropertyChanged();
            }
        }

    }
}
