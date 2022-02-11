using BarraTool.Opções;
using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoOpçõesVisuais : ICommand
    {
        private ModeloVisual _modeloVisual;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            JanelaOpçõesDeVisualização janela = new JanelaOpçõesDeVisualização(_modeloVisual.ManipuladorDeUnidades, _modeloVisual.ComandosInternos);
            janela.ShowDialog();
        }

        public ComandoExternoOpçõesVisuais(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
        }
    }
}
