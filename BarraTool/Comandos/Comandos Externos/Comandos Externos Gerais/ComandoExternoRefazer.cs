using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoRefazer : ICommand
    {
        private ModeloVisual modeloVisual;

        public ComandoExternoRefazer(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
            this.modeloVisual.ComandosInternos.PropertyChanged += Comandos_PropertyChanged;
        }

        private void Comandos_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return modeloVisual.ComandosInternos.PermitirRefazer && modeloVisual.HabilitarComandos;
        }

        public void Execute(object parameter)
        {
            modeloVisual.AtualizarSeleção();
            modeloVisual.ComandosInternos.Refazer();
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
