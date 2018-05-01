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

        public Library Library { get; set; }

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
            GroupsList.ItemsSource = Library.Groups;
            LibraryText.Text = Library.Code;
        }

        void BindGroup() {
            if (Library.TheGroupName.Length > 0) {
                this.Title = $"InterShell - {Library.TheGroupName}";
            }
            else {
                this.Title = "InterShell";
            }
            CommandsList.ItemsSource = Library.TheCommands;
            SettingsList.ItemsSource = Library.TheSettings;
        }

        void LibraryUpdate_Click(object sender, RoutedEventArgs e) {
            Library.Code = LibraryText.Text;
            Library.SaveData();
            BindAll();
        }

        void CommandsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = CommandsList.SelectedIndex;
            if (index < 0) return;
            RunCommand(Library.TheCommands[index]);
        }

        void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        void GroupsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var index = GroupsList.SelectedIndex;
            if (index < 0) return;
            Library.SelectGroup(index);
            Library.SavePrefs();
            BindGroup();
        }

        void RunCommand(Command command) {

        }
    }
}
