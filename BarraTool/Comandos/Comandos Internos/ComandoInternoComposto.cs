using System.Collections.Generic;

namespace BarraTool.ComandosInternos
{
    public class ComandoInternoComposto : ComandoInterno
    {
        public List<ComandoInterno> _comandos;
        public ComandoInternoComposto()
        {
            _comandos = new List<ComandoInterno>();
        }
        public void AdicionarComando(ComandoInterno comando)
        {
            _comandos.Add(comando);
        }
        public override void ComandoInverso()
        {
            _comandos.Reverse();
            _comandos.ForEach(cmd => cmd.ComandoInverso());
            _comandos.Reverse();
        }
        public override bool Executar()
        {
            _comandos.ForEach(cmd => cmd.Executar());
            return true;
        }
        public override string LogDoComando()
        {
            string log=""; //= System.Environment.NewLine + "   Comando composto:";
            foreach(var comando in _comandos)
            {
                log += System.Environment.NewLine + "   - " + comando.LogDoComando();
            }
            return log;
        }
    }
}
