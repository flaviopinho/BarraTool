using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoAlterarCasoDoCarregamento : ComandoInterno
    {
        private CarregamentoVisual _carregamento;
        private Caso _casoNovo;
        private Caso _casoAntigo;
        private FábricaDeCarregamentos _fábrica;
        public ComandoInternoAlterarCasoDoCarregamento(CarregamentoVisual carregamento, Caso caso, FábricaDeCarregamentos fábrica)
        {
            _carregamento = carregamento;
            _casoAntigo = carregamento.Caso;
            _casoNovo = caso;
            _fábrica = fábrica;
        }
        public override void ComandoInverso()
        {
            _casoNovo.Carregamentos.Remove(_carregamento);
            _casoAntigo.Carregamentos.Add(_carregamento);
            _carregamento.Caso = _casoAntigo;
        }

        public override bool Executar()
        {
            _casoAntigo.Carregamentos.Remove(_carregamento);
            _casoNovo.Carregamentos.Add(_carregamento);
            _carregamento.Caso = _casoNovo;
            return true;
        }

        public override string LogDoComando()
        {
            return "Caso de " + _carregamento.Nome + " alterado para " + _casoNovo.Nome;
        }
    }
}
