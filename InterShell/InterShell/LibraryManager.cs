using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InterShell.DataSource;

namespace InterShell {

    public class LibraryManager: INotifyPropertyChanged {

        public Library Library = new Library();

        #region Logic

        public void Open(string home) {
            Library.Home = home;
            Library.LoadPrefs();
            Library.LoadData();
            MapModel(Division.Library);
        }

        public void UpdateSetting() {
            if (Setting.IsEmpty) return;
            Library.SetSetting(Setting.Name, Setting.Value)
            Library.SaveData(); 
            MapModel(Division.Setting);
        }

        public void SelectGroup() {
            if (Group.IsEmpty) return;
            Library.SetGroupName(Group.Name);
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
                Setting = index == Selection.None ? new Setting() : Settings[index].Clone();
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
                Groups = Library.GetGroups();
                Group = new Group();
            }
            if (division == Division.Library || division == Division.Group) {
                Commands = Library.GetCommands();
                FullTitle = "InterShell" + (string.IsNullOrEmpty(Library.GetGroupName()) ? "" : $" - {Library.GetGroupName()}");
                Command = new Command();
                Setting = new Setting();
                Status = "";
            }
            if (division == Division.Library || division == Division.Group || division == Division.Setting) {
                Settings = Library.GetSettings();
            }
            if (division == Division.Library || division == Division.Setting) {
                LibraryCode = Library.GetCode();
            }
        }

        #endregion

        #region Model

        public string FullTitle {
            get { return fullTitle; }
            set { fullTitle = value; OnPropertyChanged(nameof(FullTitle)); }
        }
        string fullTitle = "InterShell";

        public List<Group> Groups {
            get { return groups; }
            set { groups = value; OnPropertyChanged(nameof(Groups)); }
        }
        List<Group> groups = new List<Group>();

        public List<Command> Commands {
            get { return commands; }
            set { commands = value; OnPropertyChanged(nameof(Commands)); }
        }
        List<Command> commands = new List<Command>();

        public List<Setting> Settings {
            get { return settings; }
            set { settings = value; OnPropertyChanged(nameof(Settings)); }
        }
        List<Setting> settings = new List<Setting>();

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
