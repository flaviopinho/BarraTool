using BarraTool.PréProcessador.Carregamentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoDeletarCaso : ComandoInterno
    {
        private FábricaDeCarregamentos fábricaDeCarregamentos;
        private Caso casoDeletado;
        private ComandoInternoComposto comandos;
        public ComandoInternoDeletarCaso(Caso caso, FábricaDeCarregamentos fab)
        {
            casoDeletado = caso;
            fábricaDeCarregamentos = fab;
            comandos = new ComandoInternoComposto();
            foreach (var car in casoDeletado.Carregamentos)
            {
                if (car is CarregamentoVisualDoElemento)
                {
                    var car2 = car as CarregamentoVisualDoElemento;
                    foreach (var elm in car2.ElementoConectadosEDesenhos)
                    {
                        comandos._comandos.Add(new ComandoInternoRemoverCarregamentoDoElemento(car2, elm.Key, fábricaDeCarregamentos));
                    }
                    comandos._comandos.Add(new ComandoInternoDeletarCarregamentoDoElemento(car2, fábricaDeCarregamentos));
                }
            }
        }
        public override bool Executar()
        {
            comandos.Executar();
            fábricaDeCarregamentos.DeletarCaso(casoDeletado);
            return true;
        }
        public override void ComandoInverso()
        {
            fábricaDeCarregamentos.InserirCaso(casoDeletado);
            comandos.ComandoInverso();
        }

        public override string LogDoComando()
        {
            return casoDeletado.Nome + " deletado;";
        }
    }
}
