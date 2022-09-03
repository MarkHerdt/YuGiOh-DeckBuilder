using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constrcutor
        public MainWindow()
        {
            this.InitializeComponent();
            this.Init();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Invokes the <see cref="Search"/>-method, with the <see cref="TextBox.Text"/> of <see cref="TextBox_Search"/>
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public ICommand? SearchCommand { get; private set; }
        #endregion
        
        #region Methods
        /// <summary>
        /// Downloads all <see cref="Packs"/> and <see cref="Cards"/> from https://yugioh.fandom.com/wiki/Yu-Gi-Oh!_Wiki
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private async void Button_Download_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            var yuGiOhFandom = new YuGiOhFandom();

            await yuGiOhFandom.DownloadData();
            
            this.IndexCards();
            
            Console.WriteLine("\nFinished Downloading\n");
            
            if (TestOutputHelper != null)
            {
                Log.Errors.ToList().ForEach(TestOutputHelper.WriteLine);   
            }
            else
            {
                Log.Errors.ToList().ForEach(Console.WriteLine);  
            }
        }

        /// <summary>
        /// Downloads all new <see cref="Packs"/> and <see cref="Cards"/> from https://yugioh.fandom.com/wiki/Yu-Gi-Oh!_Wiki
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private async void Button_Update_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            var yuGiOhFandom = new YuGiOhFandom();

            await yuGiOhFandom.UpdateData();
            
            this.IndexCards();
            
            Console.WriteLine("\nFinished Downloading\n");
            
            if (TestOutputHelper != null)
            {
                Log.Errors.ToList().ForEach(TestOutputHelper.WriteLine);   
            }
            else
            {
                Log.Errors.ToList().ForEach(Console.WriteLine);  
            }
        }
        
        /// <summary>
        /// Loads all <see cref="Packs"/> and <see cref="Cards"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private async void Button_Load_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            // TODO: Call these on program start
            await this.LoadPacks();
            await this.LoadCards();
            
            this.IndexCards();
        }
        
        /// <summary>
        /// Exports all cards in <see cref="DeckListView"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Export_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            const string main = "#main\n";
            const string extra = "#extra\n";
            const string side = "!side";

            var deckCardPasscodes = this.DeckListView.Aggregate(string.Empty, (current, card) => $"{current}{card.GetCardData().Passcode.ToString()}\n");
            
            File.WriteAllText(Structure.BuildPath(Folder.Export, this.TextBox_Export.Text, Extension.ydk), main + deckCardPasscodes + extra + side);

            this.TextBox_Export.Text = string.Empty;
        }
        
        /// <summary>
        /// Searches cards, depending on <see cref="filterSettings"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Search_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Search(this.TextBox_Search.Text);
        }
        
        /// <summary>
        /// Opens/closes the <see cref="FilterSettingsWindow"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Settings_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this.settingsWindow is not { IsVisible: true })
            {
                this.settingsWindow = new FilterSettingsWindow(filterSettings);
                this.settingsWindow.Show();
                this.settingsWindow.Closed += (_, _) => { this.settingsWindow = null; };
            }
            else
            {
                this.settingsWindow.Close();
            }
        }
        
        /// <summary>
        /// Is fired when the selection in <see cref="ComboBox_Sort"/> changes
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="selectionChangedEventArgs"><see cref="SelectionChangedEventArgs"/></param>
        private void ComboBox_Sort_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            this.CardsListView = this.SortCards(this.CardsListView, (Sorting)this.ComboBox_Sort.SelectedValue, this.sortingOrder[this.currentSortingOrder]);
        }

        /// <summary>
        /// Sets the sorting order for <see cref="CardsListView"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_SortOrder_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            this.currentSortingOrder = !this.currentSortingOrder;
            this.Button_SortingOrder.Content = this.sortingOrder[this.currentSortingOrder].ToString();

            if (this.ComboBox_Sort.SelectedItem != null)
            {
                this.CardsListView = this.SortCards(this.CardsListView, (Sorting)this.ComboBox_Sort.SelectedItem, this.sortingOrder[this.currentSortingOrder]);   
            }
        }
        #endregion

        // https://github.com//MarkHerdt/YuGiOh-DeckBuilder/raw/master/YuGiOh-DeckBuilder.rar
        private async void Button_Test_OnClick(object sender, RoutedEventArgs e) // https://github.com//MarkHerdt/YuGiOh-DeckBuilder/blob/master/YuGiOh-DeckBuilder.rar?raw=true
        {
            // const string uri = "https://github.com/MarkHerdt/YuGiOh-DeckBuilder/raw/master/YuGiOh-DeckBuilder.rar";
            // const string filePath = @"D:\Downloads\Test.zip";
            //
            // await using var download = (await WebClient.DownloadStreamAsync(uri))!;
            // await using var file = File.OpenWrite(filePath);
            // await download.CopyToAsync(file);
        }
    }
}
