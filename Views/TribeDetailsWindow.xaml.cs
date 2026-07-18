using ArkPilot.Models;
using System.Windows;

namespace ArkPilot.Views
{
    public partial class TribeDetailsWindow : Window
    {
        private readonly TribeInfo tribe;


        public TribeDetailsWindow(
            TribeInfo tribe)
        {
            InitializeComponent();


            this.tribe =
                tribe;


            LoadTribe();
        }


        // =========================
        // LOAD TRIBE
        // =========================

        private void LoadTribe()
        {
            TribeNameText.Text =
                tribe.Name;


            TribeIdText.Text =
                tribe.Id;


            TribeMemberCountText.Text =
                tribe.MemberCount.ToString();


            TribeMembersGrid.ItemsSource =
                tribe.MemberDetails;
        }


        // =========================
        // CLOSE
        // =========================

        private void Close_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}