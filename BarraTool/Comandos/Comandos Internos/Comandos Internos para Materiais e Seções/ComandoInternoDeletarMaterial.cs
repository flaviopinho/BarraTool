using System;
using System.Collections.Generic;
using System.Text;
using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Materiais;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoDeletarMaterial : ComandoInterno
    {
        private ModeloVisual _modeloVisual;
        private Material _materialInserido;
        private FábricaDeElementosENós _fábricaDeNósEElementos;
        public ComandoInternoDeletarMaterial(Material material, ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _fábricaDeNósEElementos = _modeloVisual.Modelo;
            _materialInserido = material;
        }
        public override bool Executar()
        {
            if (_materialInserido.SeçõesConectadas.Count == 0)
            {
                _fábricaDeNósEElementos.DeletarMaterial(_materialInserido);
                _modeloVisual.MaterialSelecionado = _fábricaDeNósEElementos.Materiais[0];
                return true;
            }
            else
            {
                return false;
            }

        }
        public override void ComandoInverso()
        {
            _fábricaDeNósEElementos.InserirMaterial(_materialInserido);
        }

        public override string LogDoComando()
        {
            if (_materialInserido.SeçõesConectadas.Count == 0)
                return _materialInserido.Nome + " deletado.";
            else
                return "Há seções conectadas ao material " + _materialInserido.Nome;
        }
    }
}
