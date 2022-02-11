using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoInserirNó : ComandoInterno
    {
        FábricaDeElementosENós fabricaDeObjetos;
        NóVisual nóInserido;
        public ComandoInternoInserirNó(NóVisual no, FábricaDeElementosENós fabrica)
        {
            nóInserido = no;
            fabricaDeObjetos = fabrica;
        }
        public override void ComandoInverso()
        {
            fabricaDeObjetos.DeletarNó(nóInserido);
        }

        public override bool Executar()
        {
            fabricaDeObjetos.InserirNó(nóInserido);
            return true;
        }

        public override string LogDoComando()
        {
            return nóInserido.Nome + " inserido.";
        }
    }
}
