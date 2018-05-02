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

        LibraryView LibraryView { get; set; }

        int None = -1;

        public MainWindow() {
            InitializeComponent();
            LibraryView = new LibraryView();
            DataContext = LibraryView;
        }

        void Window_Loaded(object sender, RoutedEventArgs e) {
            LibraryView.OpenLibrary(AppContext.BaseDirectory);
            LibraryView.BindAll();
        }

        void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = CommandList.SelectedIndex;
            LibraryView.Command = index == None ? new Command() : LibraryView.Commands[index];
            LibraryView.Status = "";
        }

        void SettingList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = SettingList.SelectedIndex;
            LibraryView.Setting = index == None ? new Setting() : LibraryView.Settings[index].Clone();
        }

        void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = GroupList.SelectedIndex;
            LibraryView.Group = index == None ? new Group() : LibraryView.Groups[index];
        }
        
        void CommandExecute_Click(object sender, RoutedEventArgs e) {

        }

        void RunCommand() {

        }

        void CommandDetails_Click(object sender, RoutedEventArgs e) {

        }

        void SettingUpdate_Click(object sender, RoutedEventArgs e) {
            DetailText.Text = LibraryView.Setting.Value;

            // setting update
            // Library.SetSetting(Setting.Name, Setting.Value)
            // Library.SaveData(); 
            // LibraryText.Text = Library.Code;   
            // SettingList.ItemsSource = Library.TheSettings;
        }

        void GroupSelect_Click(object sender, RoutedEventArgs e) {
            if (LibraryView.Group.Name.Length == 0) return;
            LibraryView.SelectGroup();
            LibraryView.Library.SavePrefs();
            LibraryView.BindGroup();
            this.Title = LibraryView.FullTitle;
        }

        void GroupDetails_Click(object sender, RoutedEventArgs e) {

        }

        void LibraryUpdate_Click(object sender, RoutedEventArgs e) {
            LibraryView.Library.SetCode(LibraryView.LibraryCode);
            LibraryView.Library.SaveData();
            LibraryView.BindAll();
        }

        void LibraryImport_Click(object sender, RoutedEventArgs e) {

        }

        void LibraryExport_Click(object sender, RoutedEventArgs e) {

        }

    }
}
