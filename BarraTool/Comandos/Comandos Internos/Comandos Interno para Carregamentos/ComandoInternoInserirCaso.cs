using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoInserirCaso : ComandoInterno
    {
        private ModeloVisual modeloVisual;
        private Caso casoInserido;
        public ComandoInternoInserirCaso(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
            casoInserido = new Caso(modeloVisual.ManipuladorDeUnidades) { Descrição="" };
        }
        public override void ComandoInverso()
        {
            modeloVisual.Carregamentos.DeletarCaso(casoInserido);
            if (modeloVisual.Carregamentos.CasoAtual == casoInserido || modeloVisual.Carregamentos.CasoAtual == null)
                modeloVisual.Carregamentos.CasoAtual = modeloVisual.Carregamentos.Casos[0];
        }

        public override bool Executar()
        {
            modeloVisual.Carregamentos.InserirCaso(casoInserido);
            return true;
        }

        public override string LogDoComando()
        {
            return casoInserido.Nome + " inserido;";
        }
    }
}
