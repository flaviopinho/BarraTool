using BarraTool.PréProcessador;
using System.Reflection;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoAlterarPropriedade : ComandoInterno
    {
        #region Campos
        private object _objeto;
        private PropertyInfo _propriedade;
        private object _valorAntigo;
        private object _valorNovo;
        #endregion

        public ComandoInternoAlterarPropriedade(object objeto)
        {
            _objeto = objeto;
        }

        #region Propriedades
        public PropertyInfo Propriedade { get => _propriedade; set => _propriedade = value; }
        public object ValorAntigo { get => _valorAntigo; set => _valorAntigo = value; }
        public object ValorNovo { get => _valorNovo; set => _valorNovo = value; }
        protected object Objeto { get => _objeto; set => _objeto = value; }
        #endregion

        public override void ComandoInverso()
        {
            _propriedade.SetValue(_objeto, _valorAntigo);
        }

        public override bool Executar()
        {
            _propriedade.SetValue(_objeto, _valorNovo);
            return true;
        }

        public override string LogDoComando()
        {
            string nome;
            if(_objeto is INome)
            {
                nome = ((INome)_objeto).Nome;
            }
            else
            {
                nome = _objeto.GetType().Name;
            }
            return "Propriedade " + _propriedade.Name + " do objeto " + nome + " alterada.";
        }
    }
}
