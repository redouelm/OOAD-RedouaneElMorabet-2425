using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfButtons
{
    public enum ButtonType
    {
        Ok,
        Cancel,
        No
    }
    public class ColorButton : Button
    {
        private ButtonType _type;
        public ButtonType Type
        {
            get => _type;
            set
            {
                _type = value;
                ApplyStyle();
            }
        }

        public ColorButton(ButtonType ok)
        {
            this.Margin = new Thickness(5);
            this.Width = 100;
            this.Height = 40;
        }

        private void ApplyStyle()
        {
            switch (Type)
            {
                case ButtonType.Ok:
                    this.Content = "OK";
                    this.Background = Brushes.LightGreen;
                    this.Foreground = Brushes.DarkGreen;
                    break;
                case ButtonType.Cancel:
                    this.Content = "Annuleren";
                    this.Background = Brushes.Blue;
                    this.Foreground = Brushes.DarkBlue;
                    break;
                case ButtonType.No:
                    this.Content = "Nee";
                    this.Background = Brushes.IndianRed;
                    this.Foreground = Brushes.DarkRed;
                    break;
            }
        }
    }
}
