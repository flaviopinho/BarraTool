using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.PréProcessador.Carregamentos
{
    public class Combinação: INome
    {
        public string Nome { get; set; }
        public ObservableCollection<Tuple<Caso,double>> Casos;
        public Combinação()
        {

        }
    }
}
