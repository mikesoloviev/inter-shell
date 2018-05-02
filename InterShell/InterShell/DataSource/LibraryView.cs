using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.DataSource {

    public class LibraryView: INotifyPropertyChanged {

        public Library Library = new Library();

        #region Interface

        public void OpenLibrary(string home) {
            Library.Home = home;
            Library.Load();
        }

        public void BindAll() {
            BindGroup();
            Groups = Library.GetGroups();
            LibraryCode = Library.GetCode();
        }

        public void BindGroup() {
            Reset();
            Commands = Library.GetCommands();
            Settings = Library.GetSettings();
        }

        public void Reset() {
            Group = new Group();
            Command = new Command();
            Setting = new Setting();
            Status = "";
        }

        public void SelectGroup() {
            Library.SetGroupName(Group.Name);
        }

        #endregion

        #region Bindings

        public string FullTitle { get { return "InterShell" + (string.IsNullOrEmpty(Library.GetGroupName()) ? "" : $" - {Library.GetGroupName()}"); } }

        public List<Group> Groups {
            get { return _groups; }
            set { _groups = value; OnPropertyChanged(nameof(Groups)); }
        }
        List<Group> _groups = new List<Group>();

        public List<Command> Commands {
            get { return _commands; }
            set { _commands = value; OnPropertyChanged(nameof(Commands)); }
        }
        List<Command> _commands = new List<Command>();

        public List<Setting> Settings {
            get { return _settings; }
            set { _settings = value; OnPropertyChanged(nameof(Settings)); }
        }
        List<Setting> _settings = new List<Setting>();

        public Group Group {
            get { return _group; }
            set { _group = value; OnPropertyChanged(nameof(Group)); }
        }
        Group _group = new Group();

        public Command Command {
            get { return _command; }
            set { _command = value; OnPropertyChanged(nameof(Command)); }
        }
        Command _command = new Command();

        public Setting Setting {
            get { return _setting; }
            set { _setting = value; OnPropertyChanged(nameof(Setting)); }
        }
        Setting _setting = new Setting();

        public string Status {
            get { return _status; }
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }
        string _status = "";

        public string LibraryCode {
            get { return _libraryCode; }
            set { _libraryCode = value; OnPropertyChanged(nameof(LibraryCode)); }
        }
        string _libraryCode = "";

        public string DetailInfo { get; set; } = "";

        public string DetailLabel { get; set; } = "";

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
