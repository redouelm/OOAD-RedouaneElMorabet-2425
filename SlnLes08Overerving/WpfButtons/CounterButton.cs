using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfButtons
{
    public enum DirectionType
    {
        Up,
        Down
    }
    internal class CounterButton : Button
    {
        public string Prefix { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Count { get; set; }
        public DirectionType Direction { get; set; }
        public bool Loop { get; set; } = false;

        public CounterButton(string prefix, int min, int max, DirectionType direction)
        {
            Prefix = prefix;
            Min = min;
            Max = max;
            Direction = direction;

            Count = Min;
            UpdateContent();
            this.Margin = new Thickness(5);
        }

        protected override void OnClick()
        {
            // Extra logica voor Down counting met Looping
            if (Direction == DirectionType.Up)
            {
                if (Count < Max)
                    Count++;
                else if (Loop)
                    Count = Min;
            }
            else // Direction == Down
            {
                if (Count > Min)
                    Count--;
                else if (Loop)
                    Count = Max;
            }

            UpdateContent();  // Verwerk de tekstwijziging na de update
            base.OnClick();
        }

        private void UpdateContent()
        {
            this.Content = $"{Prefix}{Count}";
        }

        public void Reset()
        {
            Count = Min;
            UpdateContent();
        }
    }
}
