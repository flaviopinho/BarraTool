using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Materiais;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoInserirMaterial : ICommand
    {
        private ModeloVisual _modeloVisual;
        public event EventHandler CanExecuteChanged;

        public ComandoExternoInserirMaterial(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            JanelaInserirMaterial janela = new JanelaInserirMaterial(_modeloVisual);
            janela.ShowDialog();
        }
    }
}
