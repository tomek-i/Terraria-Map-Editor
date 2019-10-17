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
using System.Windows.Shapes;
using TEditXNA.Terraria;

namespace TEditXna.Editor.Plugins
{
    /// <summary>
    /// Interaction logic for ExpandMapPluginView.xaml
    /// </summary>
    public partial class ExpandMapPluginView : Window
    {
       
        public ExpandMapPluginView(World world)
        {
            InitializeComponent();
            DataContext = world;
        }

               
        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
