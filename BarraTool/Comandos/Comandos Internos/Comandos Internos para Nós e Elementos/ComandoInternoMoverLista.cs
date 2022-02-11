using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoMoverLista<T>  : ComandoInterno where T : NóVisual
    {
        private List<T> listaDeNos;
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public List<T> ListaDeNos { get => listaDeNos; set => listaDeNos = value; }

        public ComandoInternoMoverLista(ObservableCollection<T> selecionados) : base()
        {
            listaDeNos = new List<T>(selecionados.Count);

            foreach (var elm in selecionados)
            {
                listaDeNos.Add(elm);
            }
        }
        public override bool Executar()
        {
            if (listaDeNos.Count > 0)
            {
                foreach (var elm in listaDeNos)
                    elm.Mover(DeltaX, DeltaY);
                return true;
            }
            else
                return false;
        }

        public override void ComandoInverso()
        {
            foreach (var elm in listaDeNos)
                elm.Mover(-DeltaX, -DeltaY);
        }

        public override string LogDoComando()
        {
            if (listaDeNos.Count > 0)
            {
                string log = "Nós movidos: ";
                foreach (var nó in listaDeNos)
                {
                    log += nó.Nome + "; ";
                }
                return log;
            }
            else
            {
                string log = "Nenhum nó ou elemento selecionado.";
                return log;
            }
        }
    }
}
