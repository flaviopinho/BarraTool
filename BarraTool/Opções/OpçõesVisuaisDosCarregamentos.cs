using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.Opções
{
    public class OpçõesVisuaisDosCarregamentos
    {
        private double _tamanhoDoTexto = 10;
        private double _tamanhoDaCabeçaDaSeta = 5;
        private double _razãoCargaDistribuidaComprimento = 1.0 / 5; //kN/cm
        private double _razãoCargaConcentradaComprimento = 1.0 / 5; //kN/cm
        private double _diâmetroDoMomento = 10;
        private double _espessuraDaLinha = 1; //kN/cm

        private static OpçõesVisuaisDosCarregamentos _instância = null;
        private OpçõesVisuaisDosCarregamentos() { }
        public static OpçõesVisuaisDosCarregamentos Instância
        {
            get
            {
                if (_instância == null)
                    _instância = new OpçõesVisuaisDosCarregamentos();
                return _instância;
            }
        }

        public double TamanhoDoTexto { get => _tamanhoDoTexto; set => _tamanhoDoTexto = value; }
        public double TamanhoDaCabeçaDaSeta { get => _tamanhoDaCabeçaDaSeta; set => _tamanhoDaCabeçaDaSeta = value; }
        public double RazãoCargaDistribuidaComprimento { get => _razãoCargaDistribuidaComprimento; set => _razãoCargaDistribuidaComprimento = value; }
    }
}
