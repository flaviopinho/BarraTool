using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Materiais;
using BarraTool.PréProcessador.Seções;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoAplicarMaterial : ComandoInterno
    {
        private Material _materialNovo;
        private Material _materialAntigo;
        private Seção _seção;
        public ComandoInternoAplicarMaterial(Material material, Seção seção)
        {
            _materialNovo = material;
            _seção = seção;
            _materialAntigo = seção.Material;
        }
        public override void ComandoInverso()
        {
            _seção.Material = _materialAntigo;
            _materialNovo.SeçõesConectadas.Remove(_seção);
            _materialAntigo.SeçõesConectadas.Add(_seção);
        }

        public override bool Executar()
        {
            _seção.Material = _materialNovo;
            _materialAntigo.SeçõesConectadas.Remove(_seção);
            _materialNovo.SeçõesConectadas.Add(_seção);
            return true;
        }

        public override string LogDoComando()
        {
            return "Material de " + _seção.Nome + " alterado para " + _materialNovo.Nome;
        }
    }
}
