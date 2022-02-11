using BarraTool.PréProcessador.Carregamentos;
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
    /// Interação lógica para ÁrvoreDeCarregamentos.xam
    /// </summary>
    public partial class ÁrvoreDeCarregamentos : UserControl
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
        public ÁrvoreDeCarregamentos()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
