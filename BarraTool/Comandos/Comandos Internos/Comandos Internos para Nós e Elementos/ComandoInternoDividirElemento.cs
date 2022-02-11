using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoDividirElemento : ComandoInterno
    {
        FábricaDeElementosENós fabricaDeObjetos;
        ElementoVisual elmDividido;
        ElementoVisual elmA;
        ElementoVisual elmB;
        NóVisual no;
        ComandoInternoComposto comandoInternoComposto;
        public ComandoInternoDividirElemento(ElementoVisual elm, NóVisual no, ModeloVisual modelo)
        {
            comandoInternoComposto = new ComandoInternoComposto();
            this.no = no;
            elmDividido = elm;
            elmA = new ElementoVisual(elmDividido.Nó1, this.no, elmDividido.Seção);
            elmB = new ElementoVisual(this.no, elmDividido.Nó2, elmDividido.Seção);
            fabricaDeObjetos = modelo.Modelo;

            comandoInternoComposto.AdicionarComando(new ComandoInternoInserirElemento(elmA, fabricaDeObjetos));
            comandoInternoComposto.AdicionarComando(new ComandoInternoInserirElemento(elmB, fabricaDeObjetos));
            foreach (var car in elmDividido.Carregamentos)
            {
                comandoInternoComposto.AdicionarComando(new ComandoInternoAplicarCarregamentoNoElemento(car, elmA, modelo.Carregamentos));
                comandoInternoComposto.AdicionarComando(new ComandoInternoAplicarCarregamentoNoElemento(car, elmB, modelo.Carregamentos));
            }
            comandoInternoComposto.AdicionarComando(
                new ComandoInternoDeletarSelecionados(elmDividido, fabricaDeObjetos, modelo.Carregamentos)
                );
            
        }
        public override void ComandoInverso()
        {
            /*fabricaDeObjetos.DeletarElemento(elmA);
            fabricaDeObjetos.DeletarElemento(elmB);
            fabricaDeObjetos.InserirElemento(elmDividido);*/
            comandoInternoComposto.ComandoInverso();
        }

        public override bool Executar()
        {
            /*fabricaDeObjetos.DeletarElemento(elmDividido);
            fabricaDeObjetos.InserirElemento(elmA);
            fabricaDeObjetos.InserirElemento(elmB);*/
            comandoInternoComposto.Executar();
            return true;
        }

        public override string LogDoComando()
        {
            string log = elmDividido.Nome + " dividido.";
            return log + comandoInternoComposto.LogDoComando();
        }
    }
}
