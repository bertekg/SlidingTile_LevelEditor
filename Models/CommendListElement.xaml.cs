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

namespace SlidingTile_LevelEditor.Models
{
    /// <summary>
    /// Interaction logic for CommendListElement.xaml
    /// </summary>
    public partial class CommendListElement : UserControl
    {
        public CommendListElement()
        {
            InitializeComponent();
            Binding binding = new Binding("Text");
            binding.Source = tbCommandText;
            tbCommandText.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}
