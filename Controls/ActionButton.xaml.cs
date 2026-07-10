using System;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Controls
{
    public partial class ActionButton : UserControl
    {
        public ActionButton()
        {
            InitializeComponent();

            Button.Click += (_, __) =>
            {
                Click?.Invoke(this, EventArgs.Empty);
            };
        }

        public event EventHandler? Click;

        public string Icon
        {
            get => IconText.Text;
            set => IconText.Text = value;
        }

        public string Text
        {
            get => LabelText.Text;
            set => LabelText.Text = value;
        }

        public bool IsButtonEnabled
        {
            get => Button.IsEnabled;
            set => Button.IsEnabled = value;
        }
    }
}