using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoInserirElemento : ComandoInterno
    {
        FábricaDeElementosENós fabricaDeObjetos;
        ElementoVisual elmInserido;
        public ComandoInternoInserirElemento(ElementoVisual elm, FábricaDeElementosENós modelo)
        {
            fabricaDeObjetos = modelo;
            elmInserido = elm;
        }
        public override void ComandoInverso()
        {
            fabricaDeObjetos.DeletarElemento(elmInserido);
        }

        public override bool Executar()
        {
            fabricaDeObjetos.InserirElemento(elmInserido);
            return true;
        }

        public override string LogDoComando()
        {
            return elmInserido.Nome + " inserido.";
        }
    }
}
