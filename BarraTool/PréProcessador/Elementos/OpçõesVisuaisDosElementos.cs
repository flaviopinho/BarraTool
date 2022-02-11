using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Elementos
{
    public enum CorDoElemento
    {
        Elemento = 0,
        Secao = 0,
        Material = 0
    }
    public enum DesenhoDoElemento
    {
        DesenharEixo,
        DesenharSecao,
    }
    public static class OpçõesVisuaisDosElementos
    {
        public static CorDoElemento CorDoElemento = CorDoElemento.Elemento;
        public static SolidColorBrush CorPadrão = Brushes.DarkGray;
        public static double EspessuraDoEixo = 1;
        public static DesenhoDoElemento DesenhoDoElemento = DesenhoDoElemento.DesenharEixo;
    }
}
