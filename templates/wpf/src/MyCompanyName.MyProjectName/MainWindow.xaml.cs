using System;
using System.Windows;

namespace MyCompanyName.MyProjectName
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HelloWorldService _helloWorldService;

        public MainWindow(HelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            HelloLabel.Content =_helloWorldService.SayHello();
        }
    }
}
