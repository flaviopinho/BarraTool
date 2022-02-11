using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Nós;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using BarraTool.ComandosInternos;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoDeletarSelecionados : ICommand
    {
        private ModeloVisual modeloVisual;
        public event EventHandler CanExecuteChanged;
        public ComandoExternoDeletarSelecionados(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
        }
        public bool CanExecute(object parameter)
        {
            int numeroDeElementosSelecionados = modeloVisual.NúmeroDeElementosENósSelecionados;
            if (modeloVisual.HabilitarComandos == true && numeroDeElementosSelecionados > 0)
            {
                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            deletar();
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        private void deletar()
        {
            if (modeloVisual.NúmeroDeElementosENósSelecionados>0)
            {
                /*
                var noS = new ObservableCollection<NóVisual>();
                var elmS = new ObservableCollection<ElementoVisual>();
                foreach(var no in modeloVisual.NósSelecionados)
                {
                    //if (no.ElementosConectados.Count == 0)
                        noS.Add(no);
                }
                foreach(var elm in modeloVisual.ElementosSelecionados)
                {
                    elmS.Add(elm);
                }
                if (elmS.Count+noS.Count == 0)
                    return;
                
                ComandoInternoDeletarSelecionados deletarSelecionados = new ComandoInternoDeletarSelecionados(noS, elmS, modeloVisual.Modelo,modeloVisual.Carregamentos);
                modeloVisual.ComandosInternos.AdicionarComando(deletarSelecionados);
                */
                ComandoInternoDeletarSelecionados deletarSelecionados = new ComandoInternoDeletarSelecionados(modeloVisual.NósSelecionados, modeloVisual.ElementosSelecionados, modeloVisual.Modelo, modeloVisual.Carregamentos);
                modeloVisual.ComandosInternos.AdicionarComando(deletarSelecionados);
            }
        }
    }
}
