using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneBreak.Controls
{
    public sealed partial class LeftNavItemControl : UserControl
    {
        public DependencyProperty IconDataProperty = DependencyProperty.Register(nameof(IconData), 
            typeof(string), typeof(LeftNavItemControl), new PropertyMetadata(null));
        public string IconData
        {
            get { return (string)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        public DependencyProperty ItemTextProperty = DependencyProperty.Register(nameof(Text), 
            typeof(string), typeof(LeftNavItemControl), new PropertyMetadata(null));
        public string Text
        {
            get { return (string)GetValue(ItemTextProperty); }
            set { SetValue(ItemTextProperty, value); }
        }

        public Type NavigationPage { get; set; }

        public LeftNavItemControl()
        {
            this.InitializeComponent();
        }
    }
}
