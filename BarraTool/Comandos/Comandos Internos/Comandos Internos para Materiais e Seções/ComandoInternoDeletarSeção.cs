using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    class ComandoInternoDeletarSeção : ComandoInterno
    {
        private ModeloVisual _modeloVisual;
        private Seção _seçãoInserida;
        private FábricaDeElementosENós _fábricaDeNósEElementos;
        public ComandoInternoDeletarSeção(Seção seção, ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _fábricaDeNósEElementos = modeloVisual.Modelo;
            _seçãoInserida = seção;
        }
        public override bool Executar()
        {
            if (_seçãoInserida.ElementosConectados.Count == 0)
            {
                _fábricaDeNósEElementos.DeletarSeção(_seçãoInserida);
                _modeloVisual.SeçãoSelecionada = _fábricaDeNósEElementos.Seções[0];
                return true;
            }
            else
                return false;
        }
        public override void ComandoInverso()
        {
            _fábricaDeNósEElementos.InserirSeção(_seçãoInserida);
        }

        public override string LogDoComando()
        {
            if (_seçãoInserida.ElementosConectados.Count == 0)
            {
                return _seçãoInserida.Nome + " deletada.";
            }
            else
                return "A seção está conectada a um ou mais elementos.";
            
        }
    }
}
