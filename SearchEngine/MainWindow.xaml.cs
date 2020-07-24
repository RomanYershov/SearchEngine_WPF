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
using Search.Bll.Abstraction;


namespace SearchEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void ResultTbDelegate();   
        private List<CheckBox> _checkBoxsDir;
        private  SearchServiceBase _searchService;
        public MainWindow()
        {
            InitializeComponent();
            BuildDirectoriesChBoxes();
            _searchService = new SimleSearchService();
            RtbResult.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

       
        private void BuildDirectoriesChBoxes()
        {
            var drives = DriveInfo.GetDrives();
            int drivesCount = drives.Length;
            _checkBoxsDir = new List<CheckBox>(drivesCount);
            for (int i = 0; i < drivesCount; i++)
            {
                var chBox = new CheckBox();
                chBox.Name = "chBox" + (i + 1);
                chBox.Tag = drives[i];
                chBox.Click += CheckBox_Click;
                chBox.Content = drives[i];
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



        private async void BtnStartSearch_OnClick(object sender, RoutedEventArgs e)
        {
            ExecuteInParallelThread(BtnStartSearch, () => BtnStartSearch.IsEnabled = false);
            var directories = _checkBoxsDir.Select(x => x.Tag.ToString()).ToList();
            _searchService.Directories = directories;
            _searchService.SearchData = TbSearchData.Text;


            //var searchingResult = await searchService.GetAsync(@"F:\");
            //if (searchingResult != null)
            //{
            //    ExecuteInParallelThread(lblCount, () => lblCount.Content = searchingResult.Count());
            //    foreach (var resultStringPath in searchingResult)
            //    {
            //        ExecuteInParallelThread(RtbResult,
            //            () => RtbResult.AppendText($"{resultStringPath}{Environment.NewLine}"));
            //    }
            //}





            await Task.Run(() =>
            {
                int count = 0;
                var searchingResult = _searchService.GetYield();

                foreach (var resultStringPath in searchingResult)
                {
                    ++count;
                    ExecuteInParallelThread(RtbResult, () => RtbResult.AppendText($"{resultStringPath}{Environment.NewLine}"));
                }
                ExecuteInParallelThread(lblCount, () => lblCount.Content = count);
                ExecuteInParallelThread(BtnStartSearch, () => BtnStartSearch.IsEnabled = true);
                Console.Beep();
            });
        }

        private void ExecuteInParallelThread(Control control, ResultTbDelegate callBack)
        {
            control.Dispatcher?.Invoke(DispatcherPriority.Background, new Action(callBack));
        }

    

        private void SelectedOptionsRadioBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedOption = ((RadioButton)sender);
            if (selectedOption.Tag.ToString().Contains("file"))
                _searchService = new SimleSearchService();
            else if(selectedOption.Tag.ToString().Contains("text"))
                _searchService = new TextSearchEngine();
        }
    }
}
