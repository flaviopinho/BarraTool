using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoAplicarSeção : ComandoInterno
    {
        private Seção _seçãoNova;
        private List<Seção> _seçõesAntigas;
        private List<ElementoVisual> _elementos;
        public ComandoInternoAplicarSeção(Seção seção, ElementoVisual elementoVisual)
        {
            _seçãoNova = seção;
            _seçõesAntigas = new List<Seção>();
            _seçõesAntigas.Add(elementoVisual.Seção);
            _elementos = new List<ElementoVisual>();
            _elementos.Add(elementoVisual);
        }
        public ComandoInternoAplicarSeção(Seção seção, ModeloVisual modelo)
        {
            _seçãoNova = seção;
            _elementos = new List<ElementoVisual>();
            _seçõesAntigas = new List<Seção>();
            foreach(var elm in modelo.ElementosSelecionados)
            {
                if (!_seçãoNova.ElementosConectados.Contains(elm))
                {
                    _elementos.Add(elm);
                    _seçõesAntigas.Add(elm.Seção);
                }
            }
        }
        public override void ComandoInverso()
        {
            for(int i=0; i<_elementos.Count; i++)
            {
                _seçãoNova.ElementosConectados.Remove(_elementos[i]);
                _elementos[i].Seção = _seçõesAntigas[i];
                _seçõesAntigas[i].ElementosConectados.Add(_elementos[i]);
            }
        }

        public override bool Executar()
        {
            if (_elementos.Count > 0)
            {
                foreach (var elm in _elementos)
                {
                    elm.Seção.ElementosConectados.Remove(elm);
                    elm.Seção = _seçãoNova;
                    elm.Seção.ElementosConectados.Add(elm);
                }
                return true;
            }
            else
                return false;
        }
        
        public override string LogDoComando()
        {
            if (_elementos.Count > 0)
            {
                string log = "Seção de - ";
                foreach (var elm in _elementos)
                {
                    log += elm.Nome + " ";
                }
                log += "- alterada para " + _seçãoNova.Nome;
                return log;
            }
            else
            {
                string log = "Nenhum elemento que não continha a seção foi selecionado.";
                return log;
            }
        }
    }
}
