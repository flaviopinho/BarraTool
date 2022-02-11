using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoDeletarCarregamentoDoElemento : ComandoInterno
    {
        private CarregamentoVisualDoElemento _carregamentoInserido;
        private FábricaDeCarregamentos _fábricaDeCarregamentos;
        public ComandoInternoDeletarCarregamentoDoElemento(CarregamentoVisualDoElemento carregamento, FábricaDeCarregamentos fábrica)
        {
            _fábricaDeCarregamentos = fábrica;
            _carregamentoInserido = carregamento;
        }
        public override bool Executar()
        {
            _fábricaDeCarregamentos.DeletarCarregamento(_carregamentoInserido);
            return true;
        }
        public override void ComandoInverso()
        {
            _fábricaDeCarregamentos.InserirCarregamento(_carregamentoInserido); 
        }

        public override string LogDoComando()
        {
            throw new NotImplementedException();
        }
    }
}
