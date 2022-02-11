using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    class ComandoInternoInserirSeção : ComandoInterno
    {
        private Seção _seçãoInserida;
        private FábricaDeElementosENós _fábricaDeNósEElementos;
        public ComandoInternoInserirSeção(Seção seção, FábricaDeElementosENós fábrica)
        {
            _fábricaDeNósEElementos = fábrica;
            _seçãoInserida = seção;
        }
        public override bool Executar()
        {
            _fábricaDeNósEElementos.InserirSeção(_seçãoInserida);
            return true;
        }
        public override void ComandoInverso()
        {
            _fábricaDeNósEElementos.DeletarSeção(_seçãoInserida);
        }

        public override string LogDoComando()
        {
            return _seçãoInserida.Nome + " inserida.";
        }
    }
}
