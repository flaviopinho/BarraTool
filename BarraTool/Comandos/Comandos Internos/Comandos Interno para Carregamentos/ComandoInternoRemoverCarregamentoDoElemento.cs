using BarraTool.PréProcessador.Carregamentos;
using BarraTool.PréProcessador.Elementos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoRemoverCarregamentoDoElemento : ComandoInterno
    {
        private List<ElementoVisual> _elementos;
        private CarregamentoVisualDoElemento _carregamento;
        private FábricaDeCarregamentos _fábrica;
        public ComandoInternoRemoverCarregamentoDoElemento(CarregamentoVisualDoElemento carregamento, ElementoVisual elemento, FábricaDeCarregamentos fábrica)
        {
            _elementos = new List<ElementoVisual>();
            _elementos.Add(elemento);
            _carregamento = carregamento;
            _fábrica = fábrica;
        }
        public ComandoInternoRemoverCarregamentoDoElemento(CarregamentoVisualDoElemento carregamento, ObservableCollection<ElementoVisual> elementos, FábricaDeCarregamentos fábrica)
        {
            _elementos = new List<ElementoVisual>();
            _carregamento = carregamento;
            _fábrica = fábrica;
            foreach (var elemento in elementos)
            {
                if(_carregamento.ElementoConectadosEDesenhos.ContainsKey(elemento))
                    _elementos.Add(elemento);
            }
        }
        public override bool Executar()
        {
            if (_elementos.Count > 0)
            {
                _fábrica.RemoverCarregamento(_carregamento, _elementos);
                return true;
            }
            else
                return false;
        }
        public override void ComandoInverso()
        {
            _fábrica.AplicarCarregamento(_carregamento, _elementos);
        }

        public override string LogDoComando()
        {
            if (_elementos.Count > 0)
            {
                string log = _carregamento.Nome + " removido de: ";
                foreach (var elm in _elementos)
                {
                    log += elm.Nome + "; ";
                }
                return log;
            }
            else
            {
                string log = "Nenhum elemento que possui o carregamento selecionado.";
                return log;
            }
        }
    }
}
