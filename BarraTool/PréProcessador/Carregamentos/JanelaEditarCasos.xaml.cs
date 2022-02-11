using BarraTool.ComandosInternos;
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
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Carregamentos
{
    /// <summary>
    /// Lógica interna para Casos.xaml
    /// </summary>
    public partial class JanelaEditarCasos : Window
    {
        private ModeloVisual modeloVisual;
        public JanelaEditarCasos(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
            InitializeComponent();
            this.DataContext = modeloVisual;
        }

        private void InserirCaso_Click(object sender, RoutedEventArgs e)
        {
            modeloVisual.ComandosInternos.AdicionarComando(new ComandoInternoInserirCaso(modeloVisual));
        }

        private void DeletarCaso_Click(object sender, RoutedEventArgs e)
        {
            if(modeloVisual.Carregamentos.Casos.Count>1)
            {
                Caso caso = DataGridCasos.SelectedItem as Caso;

                ComandoInternoDeletarCaso cmd = new ComandoInternoDeletarCaso(caso, modeloVisual.Carregamentos);
                modeloVisual.ComandosInternos.AdicionarComando(cmd);
            }
        }
    }
}
