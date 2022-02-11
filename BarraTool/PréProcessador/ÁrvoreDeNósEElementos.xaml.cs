using BarraTool.PréProcessador.Elementos;
using System;
using System.Collections.Generic;
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
    /// Interação lógica para ArvoreDeObjetos.xam
    /// </summary>
    public partial class ÁrvoreDeNósEElementos : UserControl
    {
        private ModeloVisual modeloVisual;
        private bool passing1 = true;
        private bool passing2 = true;
        public ModeloVisual ModeloVisual
        {
            get => modeloVisual;
            set
            {
                modeloVisual = value;
            }
        }
        public ÁrvoreDeNósEElementos()
        {
            InitializeComponent();
            this.DataContext = this;
            Loaded += (sender, args) =>
            {
                modeloVisual.NósSelecionados.CollectionChanged += NosSelecionados_CollectionChanged;
                modeloVisual.ElementosSelecionados.CollectionChanged += ElementosSelecionados_CollectionChanged;
            };
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (passing1 == true)
            {
                passing2 = false;
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                    || Keyboard.IsKeyDown(Key.LeftCtrl)|| Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    ModeloVisual.CancelarSeleção();
                    if (!sender.Equals(ListBoxElementos))
                        ListBoxElementos.SelectedItems.Clear();
                    if (!sender.Equals(ListBoxNos))
                        ListBoxNos.SelectedItems.Clear();
                }
                foreach (var selct in ((ListBox)sender).SelectedItems)
                {
                    ModeloVisual.SeleçãoDoObjeto((ObjetoVisual)selct, true);
                }
                passing2 = true;
            }
        }

        private void NosSelecionados_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (passing2 == true)
            {
                passing1 = false;
                ListBoxNos.SelectedItems.Clear();
                foreach (var no in modeloVisual.NósSelecionados)
                {
                    ListBoxNos.SelectedItems.Add(no);
                }
                passing1 = true;
            }
        }

        private void ElementosSelecionados_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (passing2 == true)
            {
                passing1 = false;
                ListBoxElementos.SelectedItems.Clear();
                foreach (var elm in modeloVisual.ElementosSelecionados)
                {
                    ListBoxElementos.SelectedItems.Add(elm);
                }
                passing1 = true;
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
