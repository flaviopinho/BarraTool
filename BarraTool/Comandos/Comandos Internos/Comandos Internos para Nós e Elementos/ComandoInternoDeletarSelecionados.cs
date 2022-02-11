using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Carregamentos;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoDeletarSelecionados : ComandoInterno
    {
        private ObservableCollection<NóVisual> _nósSelecionados;
        private ObservableCollection<ElementoVisual> _elementosSelecionados;
        private FábricaDeElementosENós _fábricaDeElementosENós;
        private FábricaDeCarregamentos _fábricaDeCarregamentos;
        private ComandoInternoComposto _comandosAuxiliares;
        private string log = "";

        public ComandoInternoDeletarSelecionados(ObservableCollection<NóVisual> nosSelecionados1, 
            ObservableCollection<ElementoVisual> elementosSelecionados1,
            FábricaDeElementosENós fabricaDeObjetos,
            FábricaDeCarregamentos fábricaDeCarregamentos)
        {
            _nósSelecionados = new ObservableCollection<NóVisual>();
            _elementosSelecionados = new ObservableCollection<ElementoVisual>();
            _fábricaDeElementosENós = fabricaDeObjetos;
            _fábricaDeCarregamentos = fábricaDeCarregamentos;
            _comandosAuxiliares = new ComandoInternoComposto();
            foreach (var nó in nosSelecionados1)
            {
                _nósSelecionados.Add(nó);
            }
            foreach (var elm in elementosSelecionados1)
            {
                _elementosSelecionados.Add(elm);
                foreach(var car in elm.Carregamentos)
                {
                    _comandosAuxiliares.AdicionarComando(new ComandoInternoRemoverCarregamentoDoElemento(car, elm, _fábricaDeCarregamentos));
                }
            }
        }

        public ComandoInternoDeletarSelecionados(ElementoVisual elemento,
            FábricaDeElementosENós fabricaDeObjetos,
            FábricaDeCarregamentos fábricaDeCarregamentos)
        {
            _nósSelecionados = new ObservableCollection<NóVisual>();
            _elementosSelecionados = new ObservableCollection<ElementoVisual>();
            _fábricaDeElementosENós = fabricaDeObjetos;
            _fábricaDeCarregamentos = fábricaDeCarregamentos;
            _comandosAuxiliares = new ComandoInternoComposto();
            _elementosSelecionados.Add(elemento);
            foreach (var car in elemento.Carregamentos)
            {
                _comandosAuxiliares.AdicionarComando(new ComandoInternoRemoverCarregamentoDoElemento(car, elemento, _fábricaDeCarregamentos));
            }
        }

        public ComandoInternoDeletarSelecionados(NóVisual nó,
            FábricaDeElementosENós fabricaDeObjetos,
            FábricaDeCarregamentos fábricaDeCarregamentos)
        {
            _nósSelecionados = new ObservableCollection<NóVisual>();
            _elementosSelecionados = new ObservableCollection<ElementoVisual>();
            _fábricaDeElementosENós = fabricaDeObjetos;
            _fábricaDeCarregamentos = fábricaDeCarregamentos;
            _comandosAuxiliares = new ComandoInternoComposto();
            _nósSelecionados.Add(nó);
        }

        public override void ComandoInverso()
        {
            foreach (var no in _nósSelecionados)
                _fábricaDeElementosENós.InserirNó(no);
            foreach (var elm in _elementosSelecionados)
                _fábricaDeElementosENós.InserirElemento(elm);
            _comandosAuxiliares.ComandoInverso();
        }
        public override bool Executar()
        {
            _comandosAuxiliares.Executar();
            _fábricaDeElementosENós.DeletarSeleção(_elementosSelecionados);
            int countInicial = _nósSelecionados.Count;

            for (int i = _nósSelecionados.Count - 1; i >= 0; i--)
            {
                if (_nósSelecionados[i].ElementosConectados.Count > 0)
                {
                    _nósSelecionados.RemoveAt(i);
                }
            }

            if (countInicial > _nósSelecionados.Count)
                log += "(nós conectados não foram deletados) ";
            _fábricaDeElementosENós.DeletarSeleção(_nósSelecionados);
            
            if (_nósSelecionados.Count + _elementosSelecionados.Count == 0)
                return false;

            return true;
        }

        public override string LogDoComando()
        {
            if (_nósSelecionados.Count + _elementosSelecionados.Count > 0)
            {
                log += "objetos deletados: ";
                foreach (var no in _nósSelecionados)
                {
                    log += no.Nome + "; ";
                }
                foreach (var elm in _elementosSelecionados)
                {
                    log += elm.Nome + "; ";
                }
            }
            else
            {

                log += "Nenhum objeto foi deletado.";
            }
            
            string log2 = log;
            log = "";
            return log2;

        }
    }
}
