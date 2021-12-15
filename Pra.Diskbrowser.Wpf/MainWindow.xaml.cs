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
using System.IO;
using System.Windows.Threading;
using System.Threading;

namespace Pra.Diskbrowser.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Disk> disks = new List<Disk>();
        public MainWindow()
        {
            InitializeComponent();
            DetectDriveLetters();
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
        private void DetectDriveLetters()
        {
            foreach(DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.DriveType == DriveType.Fixed)
                    disks.Add(new Disk(driveInfo));
            }
            cmbDrive.ItemsSource = disks;

        }

        private void CmbDrive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            DoEvents();
            Disk disk = (Disk)cmbDrive.SelectedItem;
            string path = disk.DriveInfo.RootDirectory.FullName;
            tvFolders.Items.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Tag = directoryInfo.FullName;
            treeViewItem.Header = directoryInfo.Name;
            tvFolders.Items.Add(treeViewItem);
            GetFolders(directoryInfo, treeViewItem);
            this.Cursor = Cursors.Arrow;
        }
        private void GetFolders(DirectoryInfo directoryInfo, TreeViewItem treeViewItem)
        {
            treeViewItem.Items.Clear();
            TreeViewItem subtreeViewItem;
            foreach (DirectoryInfo subdirectory in directoryInfo.GetDirectories())
            {
                try
                {
                    subdirectory.GetAccessControl();
                    subtreeViewItem = new TreeViewItem();
                    subtreeViewItem.Tag = subdirectory.FullName;
                    subtreeViewItem.Header = subdirectory.Name;
                    treeViewItem.Items.Add(subtreeViewItem);
                    GetSubFolders(subdirectory, subtreeViewItem);
                }
                catch { }
            }
        }
        private void GetSubFolders(DirectoryInfo directoryInfo, TreeViewItem treeViewItem)
        {
            treeViewItem.Items.Clear();
            TreeViewItem subtreeViewItem;
            foreach (DirectoryInfo subdirectory in directoryInfo.GetDirectories())
            {
                try
                {
                    subdirectory.GetAccessControl();
                    subtreeViewItem = new TreeViewItem();
                    subtreeViewItem.Tag = subdirectory.FullName;
                    subtreeViewItem.Header = subdirectory.Name;
                    treeViewItem.Items.Add(subtreeViewItem);
                }
                catch { }
            }
        }

        private void TvFolders_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if(tvi != null)
            {
                this.Cursor = Cursors.Wait;
                DoEvents();
                DirectoryInfo directoryInfo = new DirectoryInfo(tvi.Tag.ToString());
                GetFolders(directoryInfo, tvi);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void TvFolders_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if(tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;
                string path = tvi.Tag.ToString();
                this.Cursor = Cursors.Wait;
                DoEvents();
                DisplayFiles(path);
                this.Cursor = Cursors.Arrow;
            }
        }
        private void DisplayFiles(string path)
        {
            lvFiles.ItemsSource = null;
            lvFiles.Items.Refresh();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            List<ListFile> listFiles = new List<ListFile>();
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                listFiles.Add(new ListFile(fileInfo.Name,
                    fileInfo.Length, fileInfo.CreationTime));
            }
            lvFiles.ItemsSource = listFiles;


        }
    }
}
