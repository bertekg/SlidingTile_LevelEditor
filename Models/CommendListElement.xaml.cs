using System.Windows.Controls;
using System.Windows.Data;

namespace SlidingTile_LevelEditor.Models;

public partial class CommendListElement : UserControl
{
    public CommendListElement()
    {
        InitializeComponent();
        Binding binding = new("Text")
        {
            Source = tbCommandText
        };
        tbCommandText.SetBinding(TextBlock.TextProperty, binding);
    }
}
