namespace BarraTool.ComandosInternos
{
    public abstract class ComandoInterno
    {
        public abstract string LogDoComando();
        public abstract bool Executar();
        public abstract void ComandoInverso();
    }

}
