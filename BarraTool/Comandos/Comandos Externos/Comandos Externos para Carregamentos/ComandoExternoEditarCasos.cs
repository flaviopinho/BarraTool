using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoEditarCasos : ICommand
    {
        private ModeloVisual modeloVisual;
        public ComandoExternoEditarCasos(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            JanelaEditarCasos JanelaCasos = new JanelaEditarCasos(modeloVisual);
            JanelaCasos.ShowDialog();
        }
    }
}
