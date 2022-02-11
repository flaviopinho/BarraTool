using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoInserirSeção : ICommand
    {
        private ModeloVisual _modeloVisual;
        public event EventHandler CanExecuteChanged;

        public ComandoExternoInserirSeção(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            JanelaInserirSeção janela = new JanelaInserirSeção(_modeloVisual);
            janela.ShowDialog();
        }
    }
}
