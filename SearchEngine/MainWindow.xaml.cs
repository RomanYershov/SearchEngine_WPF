using Search.Bll.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace SearchEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void ResultTbDelegate();   
        private List<CheckBox> _checkBoxsDir;
        public MainWindow()
        {
            InitializeComponent();
            BuildDirectories();

        }

        private void BuildDirectories()
        {
            var drives = DriveInfo.GetDrives();
            int drivesCount = drives.Length;
            _checkBoxsDir = new List<CheckBox>(drivesCount);
            for (int i = 0; i < drivesCount; i++)
            {
                var lbl = new Label();
                var chBox = new CheckBox();
                lbl.Content = drives[i];
                chBox.Name = "chBox" + (i + 1);
                chBox.Tag = drives[i];
                chBox.Click += CheckBox_Click;
                chBox.Content = lbl;
                chBox.IsChecked = true;
                chBox.Margin = new Thickness(50 + i * 50, 0, 0, 0);
                GridChBoxes.Children.Add(chBox);
                _checkBoxsDir.Add(chBox);
            }
        }

        public void CheckBox_Click(object sender, EventArgs e)
        {
            var curentChBox = ((CheckBox)sender);
            if (!curentChBox.IsChecked.Value)
            {
                _checkBoxsDir.Remove(curentChBox);
            }
            else
            {
                _checkBoxsDir.Add(curentChBox);
            }
        }



        private void BtnStartSearch_OnClick(object sender, RoutedEventArgs e)
        {
            //ExecuteToStream(BtnStartSearch, () => BtnStartSearch.Visibility = Visibility.Hidden);
            var searchService = new SimleSearchService();
            searchService.Directories.AddRange(_checkBoxsDir.Select(x => x.Tag.ToString()));
            searchService.SearchData = TbSearchData.Text;
            Task.Run(() =>
            {
                var searchingResult = searchService.GetYield();
             
                foreach (var resultStringPath in searchingResult)
                {
                    ExecuteInParallelThread(RtbResult, () => RtbResult.AppendText($"{resultStringPath}{Environment.NewLine}"));
                }
            });
        }

        private void ExecuteInParallelThread(Control control, ResultTbDelegate callBack)
        {
            control.Dispatcher?.Invoke(DispatcherPriority.Background, new Action(callBack));
        }
    }
}
