using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace BarraTool.PréProcessador.Seções
{
    public abstract class GeometriaDaSeção
    {
        public abstract string TipoDeSeção { get; }
        public abstract UserControl JanelaDasPropriedadesGeométricas(ModeloVisual modeloVisual);
    }
}
