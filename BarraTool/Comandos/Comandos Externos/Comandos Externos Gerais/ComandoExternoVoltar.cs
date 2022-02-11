using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoVoltar : ICommand
    {
        private ModeloVisual modeloVisual;

        public ComandoExternoVoltar(ModeloVisual modeloVisual)
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
            return modeloVisual.ComandosInternos.PermitirVoltar && modeloVisual.HabilitarComandos;
        }
        public void Execute(object parameter)
        {
            modeloVisual.AtualizarSeleção();
            modeloVisual.ComandosInternos.Desfazer();
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
