﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InterShell.DataSource;

namespace InterShell {

    public class LibraryManager: INotifyPropertyChanged {

        public Library Library = new Library();

        public static string AppName = "InterShell";
        public static int[] WindowSize = { 800, 600 };

        #region Logic

        public void Open(string home) {
            Library.Home = home;
            Library.LoadPrefs();
            Library.LoadData();
            MapModel(Division.Library);
            WindowWidth = Library.GetPrefInt("window-width", WindowSize[0]);
            WindowHeight = Library.GetPrefInt("window-height", WindowSize[1]);
        }

        public void UpdateSetting() {
            if (Setting.IsEmpty) return;
            Library.SetSetting(Setting.Name, Setting.Value);
            Library.SaveData(); 
            MapModel(Division.Setting);
        }

        public void SelectGroup() {
            if (Group.IsEmpty) return;
            Library.SetPref(Library.GroupKey, Group.Name);
            Library.SavePrefs();
            MapModel(Division.Group);
        }

        public void UpdateLibrary() {
            Library.SetCode(LibraryCode);
            Library.SaveData();
            MapModel(Division.Library);
        }

        public void PreselectItem(Division division, int index) {
            if (division == Division.Command) {
                Command = index == Selection.None ? new Command() : Commands[index];
                CommandStatus = "";
            }
            else if (division == Division.Setting) {
                Setting = index == Selection.None ? new Setting() : Settings[index];
            }
            else if (division == Division.Group) {
                Group = index == Selection.None ? new Group() : Groups[index];
            }
        }

        public void DisplayDetails(Division division) {
            if (division == Division.Group) {
                DetailLabel = Group.Name;
                DetailContent = string.Join(Environment.NewLine, Group.Encode());
            }
            else if (division == Division.Command) {
                DetailLabel = Command.Name;
                DetailContent = string.Join(Environment.NewLine, Command.Encode());
            }
        }

        void MapModel(Division division) {
            if (division == Division.Library) {
                Groups = Library.GetGroups(false);
                Group = new Group();
            }
            if (division == Division.Library || division == Division.Group) {
                Commands = Library.GetCommands(false);
                Command = new Command();
                Setting = new Setting();
                CommandStatus = "";
                WindowTitle = AppName + Library.GetPref(Library.GroupKey, format: " - {0}");
            }
            if (division == Division.Library || division == Division.Group || division == Division.Setting) {
                Settings = Library.GetSettings(false);
            }
            if (division == Division.Library || division == Division.Setting) {
                LibraryCode = Library.GetCode();
            }
        }

        #endregion

        #region Model

        public string WindowTitle {
            get { return windowTitle; }
            set { windowTitle = value; OnPropertyChanged(nameof(WindowTitle)); }
        }
        string windowTitle = AppName;

        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public string GuideUrl { get { return Library.GetPrefUrl(Library.GuideKey, Library.DefaultGuideUrl); } }

        public List<Group> Groups {
            get { return Library.GetGroups(); }
            set { OnPropertyChanged(nameof(Groups)); }
        }

        public List<Command> Commands {
            get { return Library.GetCommands(); }
            set { OnPropertyChanged(nameof(Commands)); }
        }

        public List<Setting> Settings {
            get { return Library.GetSettings(); }
            set { OnPropertyChanged(nameof(Settings)); }
        }

        public Group Group {
            get { return group; }
            set { group = value; OnPropertyChanged(nameof(Group)); }
        }
        Group group = new Group();

        public Command Command {
            get { return command; }
            set { command = value; OnPropertyChanged(nameof(Command)); }
        }
        Command command = new Command();

        public Setting Setting {
            get { return setting; }
            set { setting = value; OnPropertyChanged(nameof(Setting)); }
        }
        Setting setting = new Setting();

        public string CommandStatus {
            get { return commandStatus; }
            set { commandStatus = value; OnPropertyChanged(nameof(CommandStatus)); }
        }
        string commandStatus = "";

        public string DetailContent {
            get { return detailContent; }
            set { detailContent = value; OnPropertyChanged(nameof(DetailContent)); }
        }
        string detailContent = "";

        public string DetailLabel {
            get { return detailLabel; }
            set { detailLabel = value; OnPropertyChanged(nameof(DetailLabel)); }
        }
        string detailLabel = "";

        public string LibraryCode {
            get { return libraryCode; }
            set { libraryCode = value; OnPropertyChanged(nameof(LibraryCode)); }
        }
        string libraryCode = "";

        public string LibraryStatus {
            get { return libraryStatus; }
            set { libraryStatus = value; OnPropertyChanged(nameof(LibraryStatus)); }
        }
        string libraryStatus = "";

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

    }
}
