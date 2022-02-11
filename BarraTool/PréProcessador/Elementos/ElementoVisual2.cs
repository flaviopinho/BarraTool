using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.PréProcessador.Elementos
{
    public partial class ElementoVisual
    {
        public ObservableCollection<CarregamentoVisualDoElemento> Carregamentos { get; set; }
    }
}
