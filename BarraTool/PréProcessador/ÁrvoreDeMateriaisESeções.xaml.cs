using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Materiais;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador
{
    /// <summary>
    /// Interação lógica para ÁrvoreDeMateriaisESeções.xam
    /// </summary>
    public partial class ÁrvoreDeMateriaisESeções : UserControl
    {
        private ModeloVisual modeloVisual;
        public ModeloVisual ModeloVisual
        {
            get => modeloVisual;
            set
            {
                modeloVisual = value;
            }
        }
        public ÁrvoreDeMateriaisESeções()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var stack = sender as StackPanel;
                var textBlock = stack.Children[0] as TextBlock;
                string secName = textBlock.Text;
                var path = stack.Children[1] as Path;

                int indexSec = 0;
                for (int i = 0; i < ListBoxSecoes.Items.Count; i++)
                {
                    if (modeloVisual.Modelo.Seções[i].Nome == secName)
                    {
                        indexSec = i;
                        break;
                    }
                }
                modeloVisual.Modelo.SeçãoAtual = modeloVisual.Modelo.Seções[indexSec];
            }
        }
    }
    public class ConverterBoolVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((Visibility)value is Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}