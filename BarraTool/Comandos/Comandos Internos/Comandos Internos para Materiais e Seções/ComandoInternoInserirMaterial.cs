using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Materiais;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoInserirMaterial : ComandoInterno
    {
        private Material _materialInserido;
        private FábricaDeElementosENós _fábricaDeNósEElementos;
        public ComandoInternoInserirMaterial(Material material, FábricaDeElementosENós fábrica)
        {
            _fábricaDeNósEElementos = fábrica;
            _materialInserido = material;
        }
        public override bool Executar()
        {
            _fábricaDeNósEElementos.InserirMaterial(_materialInserido);
            return true;
        }
        public override void ComandoInverso()
        {
            _fábricaDeNósEElementos.DeletarMaterial(_materialInserido);
        }

        public override string LogDoComando()
        {
            return _materialInserido.Nome + " inserido.";
        }
    }
}
