using Newtonsoft.Json;
using System.Windows;

namespace CITest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string json = JsonConvert.SerializeObject(new
            {
                name = "Test2",
            });
            MessageBox.Show(json);
        }
    }
}