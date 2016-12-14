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

        public DependencyProperty ItemTextProperty = DependencyProperty.Register(nameof(ItemText), 
            typeof(string), typeof(LeftNavItemControl), new PropertyMetadata(null));
        public string ItemText
        {
            get { return (string)GetValue(ItemTextProperty); }
            set { SetValue(ItemTextProperty, value); }
        }

        public DependencyProperty NavigationPageProperty = DependencyProperty.Register(nameof(NavigationPage), 
            typeof(Page), typeof(LeftNavItemControl), new PropertyMetadata(null));
        public Type NavigationPage
        {
            get { return (Type)GetValue(NavigationPageProperty); }
            set { SetValue(NavigationPageProperty, value); }
        }
        public LeftNavItemControl()
        {
            this.InitializeComponent();
        }
    }
}
