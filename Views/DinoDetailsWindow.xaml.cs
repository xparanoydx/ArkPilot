using System.Windows;
using ArkPilot.Models;

namespace ArkPilot.Views
{
    public partial class DinoDetailsWindow : Window
    {
        public DinoDetailsWindow(DinoSaveInfo dino)
        {
            InitializeComponent();

            Title = string.IsNullOrWhiteSpace(dino.Name)
                ? $"🦖 {dino.Species}"
                : $"🦖 {dino.Species} - {dino.Name}";

            DataContext = dino;

        }
    }
}