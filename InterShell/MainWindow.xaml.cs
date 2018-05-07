using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using InterShell.Components;

//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using InterShell.DataSource;

namespace InterShell {

    public partial class MainWindow : Window {

        LibraryManager Manager { get; set; }
        ShellEngine Engine = new ShellEngine();
        Markease.Transform MarkTransform = new Markease.Transform();

        public MainWindow() {
            InitializeComponent();
            Manager = new LibraryManager();
            DataContext = Manager;
        }

        void Window_Loaded(object sender, RoutedEventArgs e) {
            Config.Home = AppContext.BaseDirectory;
            Manager.Open();
            Width = Manager.WindowWidth;
            Height = Manager.WindowHeight;
            GuideBrowser.NavigateToString(MarkTransform.LoadApply(Path.Combine(Config.Home, Config.HelpFile)));
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
            if (Manager.Command.IsEmpty) return;
            Manager.CommandStatus = "Running...";
            var result = Engine.Run(Config.Home, Config.ShellFile, Manager.Command, Manager.Group);
            Manager.CommandStatus = "Done";
            Manager.DetailLabel = result.Name;
            Manager.DetailContent = result.ToText();
            DetailsTab.IsSelected = true;
        }

        void CommandDetails_Click(object sender, RoutedEventArgs e) {
            Manager.DisplayDetails(Division.Command);
            DetailsTab.IsSelected = true;
        }

        void SettingUpdate_Click(object sender, RoutedEventArgs e) {
            Manager.UpdateSetting();
        }

        void GroupSelect_Click(object sender, RoutedEventArgs e) {
            Manager.SelectGroup();
        }

        void GroupDetails_Click(object sender, RoutedEventArgs e) {
            Manager.DisplayDetails(Division.Group);
            DetailsTab.IsSelected = true;
        }

        void LibraryUpdate_Click(object sender, RoutedEventArgs e) {
            Manager.UpdateLibrary();
        }

        void LibraryImport_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            Manager.SetupFileDialog(dialog);
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true) {
                Manager.ImportLibrary(dialog.FileName);
            }
        }

        void LibraryExport_Click(object sender, RoutedEventArgs e) {
            var dialog = new SaveFileDialog();
            Manager.SetupFileDialog(dialog);
            dialog.FileName = Config.AppName;
            if (dialog.ShowDialog() == true) {
                Manager.ExportLibrary(dialog.FileName);
            }
        }

    }
}
