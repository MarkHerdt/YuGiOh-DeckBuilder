using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOh_DeckBuilder.Utility;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.Web;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
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
        protected override void OnClosed(EventArgs eventArgs)
        {
            base.OnClosed(eventArgs);

            ConsoleAllocator.Kill();
        }

        /// <summary>
        /// Shows/hides the console
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Console_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!ConsoleAllocator.IsEnabled)
            {           
                ConsoleAllocator.ShowConsoleWindow();
                Console.OutputEncoding = Encoding.UTF8;
                
            }
            else
            {
                ConsoleAllocator.HideConsoleWindow();
            }
        }
        
        /// <summary>
        /// Downloads all <see cref="Packs"/> and <see cref="Cards"/> from https://yugioh.fandom.com/wiki/Yu-Gi-Oh!_Wiki
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private async void Button_Download_OnClick(object sender, RoutedEventArgs routedEventArgs)
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
        private async void Button_UpdateCards_OnClick(object sender, RoutedEventArgs routedEventArgs)
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
        private async void Button_LoadCards_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            // TODO: Call these on program start
            await this.LoadPacks();
            await this.LoadCards();
            
            this.IndexCards();
        }
        
        /// <summary>
        /// Sets the sorting order for <see cref="CardsListView"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private async void Button_UpdateApp_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Button_UpdateApp.IsEnabled = false;
            
            const string uri = "https://github.com/MarkHerdt/YuGiOh-DeckBuilder/releases/download/YGO-DB/YuGiOh-DeckBuilder.rar";
            var filePath = Path.Combine(AppContext.BaseDirectory, App.RarFile);
            
            await using var download = (await WebClient.DownloadStreamAsync(uri))!;
            await using var fileStream = File.OpenWrite(filePath);
            await download.CopyToAsync(fileStream);

            var deckBuilderExePath = Environment.ProcessPath!;
            var installerFilePath = Structure.BuildPath(Folder.Installer, nameof(Folder.Installer), Extension.exe);
            using var process = Process.GetCurrentProcess();

            Process.Start(installerFilePath, new []
            {
                process.Id.ToString(),    // Id of this process
                AppContext.BaseDirectory, // Path of the folder to update
                nameof(YuGiOh),           // Folder name to exclude
                App.RarFile,              // Name of the file to update the folder with
                deckBuilderExePath        // Path to the .exe of the DeckBuilder
            });
        }
        
        /// <summary>
        /// Exports all cards in <see cref="DeckListView"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Export_OnClick(object sender, RoutedEventArgs routedEventArgs)
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
        private void Button_Search_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Search(this.TextBox_Search.Text);
        }
        
        /// <summary>
        /// Opens/closes the <see cref="FilterSettingsWindow"/>
        /// </summary>
        /// <param name="sender"><see cref="object"/> from which this method is called</param>
        /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
        private void Button_Settings_OnClick(object sender, RoutedEventArgs routedEventArgs)
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
        private void Button_SortOrder_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.currentSortingOrder = !this.currentSortingOrder;
            this.Button_SortingOrder.Content = this.sortingOrder[this.currentSortingOrder].ToString();

            if (this.ComboBox_Sort.SelectedItem != null)
            {
                this.CardsListView = this.SortCards(this.CardsListView, (Sorting)this.ComboBox_Sort.SelectedItem, this.sortingOrder[this.currentSortingOrder]);   
            }
        }
        #endregion
    }
}
