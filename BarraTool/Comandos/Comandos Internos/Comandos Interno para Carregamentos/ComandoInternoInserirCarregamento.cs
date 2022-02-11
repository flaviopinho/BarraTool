using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoInserirCarregamento : ComandoInterno
    {
        private CarregamentoVisualDoElemento _carregamentoInserido;
        private FábricaDeCarregamentos _fábricaDeCarregamentos;
        public ComandoInternoInserirCarregamento (CarregamentoVisualDoElemento carregamento, FábricaDeCarregamentos fábrica)
        {
            _fábricaDeCarregamentos = fábrica;
            _carregamentoInserido = carregamento;
        }
        public override bool Executar()
        {
            _fábricaDeCarregamentos.InserirCarregamento(_carregamentoInserido);
            return true;
        }
        public override void ComandoInverso()
        {
            _fábricaDeCarregamentos.DeletarCarregamento(_carregamentoInserido);
        }

        public override string LogDoComando()
        {
            return _carregamentoInserido.Nome + " inserido.";
        }
    }
}
