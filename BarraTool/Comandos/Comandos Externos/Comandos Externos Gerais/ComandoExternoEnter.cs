using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoEnter : ICommand
    {
        private ModeloVisual modeloVisual;
        private Action<object> _execute;

        public Action<object> ExecuteMethod { get => _execute; set => _execute = value; }

        public event EventHandler CanExecuteChanged;

        public ComandoExternoEnter(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
        }

        public bool CanExecute(object parameter)
        {
            return modeloVisual.RequerConfirmação;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
