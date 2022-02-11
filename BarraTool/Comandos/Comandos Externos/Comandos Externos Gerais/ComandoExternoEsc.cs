using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoEsc : ICommand
    {
        ModeloVisual modeloVisual;
        public ComandoExternoEsc(ModeloVisual modeloVisual)
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
            modeloVisual.AnularComandos();
        }
    }
}
