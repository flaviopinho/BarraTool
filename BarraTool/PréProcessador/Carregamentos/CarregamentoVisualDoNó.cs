using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.PréProcessador.Carregamentos
{
    public abstract class CarregamentoVisualDoNó : CarregamentoVisual
    {
        public ObservableCollection<NóVisual> NósConectados { get; set; }
    }
}
