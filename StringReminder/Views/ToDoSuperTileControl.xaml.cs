using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;

namespace StringReminder.Views
{
    public partial class ToDoSuperTileControl : UserControl
    {
        public ToDoSuperTileControl()
        {
            InitializeComponent();
        }
        // more about tiles in Windows Phone here:
        // http://msdn.microsoft.com/en-us/library/hh202948(v=vs.92).aspx

        private static readonly string SecondaryTileUriSource = "Source=SeconaryTile";

        //protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    ShellTile secondaryTile = this.FindTile(SecondaryTileUriSource);
        //    this.cbShowTile.IsChecked = secondaryTile != null;
        //}

        private void btnUpdateSecondaryTile_Click(object sender, RoutedEventArgs e)
        {
            ShellTile secondaryTile = this.FindTile(SecondaryTileUriSource);
            if (secondaryTile == null)
            {
                MessageBox.Show("There is no secondary tile");
                return;
            }

            this.UpdateTile(secondaryTile);
        }

         private void UpdateTile(ShellTile tile)
        {
            StandardTileData newTileData = new StandardTileData();

            // set tile data
            this.SetTileData(this.tbTitle, (text) => newTileData.Title = text);
            this.SetTileData(this.tbBackTitle, (text) => newTileData.BackTitle = text);
            this.SetTileData(this.tbBackContent, (text) => newTileData.BackContent = text);

            // update tile
            tile.Update(newTileData);

            MessageBox.Show("Tile updated. Go to home screen to see the result.");
        }

        private void SetTileData(TextBox textBox, Action<string> tileDataAction)
        {
            string text = textBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                // only set tile data if text has been entered
                tileDataAction(text);
            }
        }

        private ShellTile FindTile(string partOfUri)
        {
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
                tile => tile.NavigationUri.ToString().Contains(partOfUri));

            return shellTile;
        }

        private void cbShowTile_Checked(object sender, RoutedEventArgs e)
        {
            // secondary tiles can be created only as the result
            // of user input in an application
            ShellTile tile = this.FindTile(SecondaryTileUriSource);

            if (tile == null)
            {
                // because the UI will navigate to Start
                // when a new secondary tile is created
                // only one secondary tile can be created at a time
                StandardTileData tileData = this.GetSecondaryTileData();

                // having a unique NavigationUri is necessary for distinguishing this tile
                string tileUri = string.Concat("/Views/MainPage.xaml?", SecondaryTileUriSource);
                ShellTile.Create(new Uri(tileUri, UriKind.Relative), tileData);
            }
        }

        private void cbShowTile_Unchecked(object sender, RoutedEventArgs e)
        {
            ShellTile tile = this.FindTile(SecondaryTileUriSource);
            if (tile != null)
            {
                tile.Delete();
                MessageBox.Show("Secondary tile deleted.");
            }
        }

        private StandardTileData GetSecondaryTileData()
        {
            StandardTileData tileData = new StandardTileData
            {
                Title = "Secondary Tile",
                BackgroundImage = new Uri("/Images/Cloudy-Nighttime.png", UriKind.Relative),
                Count = 5,
                BackTitle = "Secondary Tile",
                BackBackgroundImage = new Uri("", UriKind.Relative),
                BackContent = "WPG Add Remove Tile Sample"
            };

            return tileData;
        }
    }
}
