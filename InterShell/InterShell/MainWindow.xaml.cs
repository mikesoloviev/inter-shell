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

        LibraryManager Manager { get; set; }

        public MainWindow() {
            InitializeComponent();
            Manager = new LibraryManager();
            DataContext = Manager;
        }

        void Window_Loaded(object sender, RoutedEventArgs e) {
            Manager.Open(AppContext.BaseDirectory);
        }

        void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.PreselectItem(Division.Command, CommandList.SelectedIndex);
        }

        void SettingList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.PreselectItem(Division.Setting, SettingList.SelectedIndex);
        }

        void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Manager.PreselectItem(Division.Group, GroupList.SelectedIndex);
        }
        
        void CommandExecute_Click(object sender, RoutedEventArgs e) {

        }

        void CommandDetails_Click(object sender, RoutedEventArgs e) {
            Manager.DisplayDetails(Division.Command);
            // TODO: open Details tab
        }

        void SettingUpdate_Click(object sender, RoutedEventArgs e) {
            Manager.UpdateSetting();
        }

        void GroupSelect_Click(object sender, RoutedEventArgs e) {
            Manager.SelectGroup();
        }

        void GroupDetails_Click(object sender, RoutedEventArgs e) {
            Manager.DisplayDetails(Division.Group);
            // TODO: open Details tab
        }

        void LibraryUpdate_Click(object sender, RoutedEventArgs e) {
            Manager.UpdateLibrary();
        }

        void LibraryImport_Click(object sender, RoutedEventArgs e) {

        }

        void LibraryExport_Click(object sender, RoutedEventArgs e) {

        }

    }
}
