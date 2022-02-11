using BarraTool.PréProcessador.Elementos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace BarraTool.PréProcessador.Carregamentos
{
    public abstract class CarregamentoVisualDoElemento :  CarregamentoVisual
    {
        public Dictionary<ElementoVisual, Canvas> ElementoConectadosEDesenhos { get; set; }
        public abstract void AtualizarDesenho(ElementoVisual Elemento);
    }
}
