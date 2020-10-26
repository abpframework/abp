using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
