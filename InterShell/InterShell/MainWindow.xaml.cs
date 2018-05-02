using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InterShell.DataSource;

namespace InterShell {

    public partial class MainWindow : Window {

        Library Library { get; set; }

        int None = -1;

        Group Group { get; set; } = new Group();
        Command Command { get; set; } = new Command();
        Setting Setting { get; set; } = new Setting();
        string Status { get; set; } = "";
        string DetailLabel { get; set; } = "";

        public MainWindow() {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e) {
            Library = new Library(AppContext.BaseDirectory);
            Library.Load();
            BindAll();
        }

        void BindAll() {
            BindGroup();
            GroupList.ItemsSource = Library.Groups;
            LibraryText.Text = Library.Code;
        }

        void BindGroup() {
            Reset();
            if (Library.TheGroupName.Length > 0) {
                this.Title = $"InterShell - {Library.TheGroupName}";
            }
            else {
                this.Title = "InterShell";
            }
            CommandList.ItemsSource = Library.TheCommands;
            SettingList.ItemsSource = Library.TheSettings;
        }

        void Reset() {
            Group = new Group();
            Command = new Command();
            Setting = new Setting();
            Status = "";
        }

        void LibraryUpdate_Click(object sender, RoutedEventArgs e) {
            Library.Code = LibraryText.Text;
            Library.SaveData();
            BindAll();
        }

        void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = CommandList.SelectedIndex;
            Command = index == None ? new Command() : Library.TheCommands[index];
            Status = "";
        }

        void SettingList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = SettingList.SelectedIndex;
            Group = index == None ? new Setting() : Library.TheSettings[index].Clone();
        }

        void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = GroupList.SelectedIndex;
            Group = index == None ? new Group() : Library.TheGroups[index];
        }
        
        // group select
        // Library.SelectGroup(index);
        // Library.SavePrefs();
        // BindGroup();

        // setting update
        // Library.SetSetting(Setting.Name, Setting.Value)
        // Library.SaveData(); 
        // LibraryText.Text = Library.Code;   
        // SettingList.ItemsSource = Library.TheSettings;

        void RunCommand() {

        }
    }
}
