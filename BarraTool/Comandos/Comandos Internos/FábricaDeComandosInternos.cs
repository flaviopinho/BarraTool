using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarraTool.ComandosInternos
{
    public class FábricaDeComandosInternos: INotifyPropertyChanged
    {
        #region Campos
        private const int _númeroMáximoDeComandosArmazenados = 100;
        private int _estado;
        private List<ComandoInterno> _comandosExecutados;
        private string _logDeComandos;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Proprieades
        public string LogDeComandos
        {
            get => _logDeComandos;
            set
            {
                _logDeComandos = value;
                OnPropertyChanged();
            }
        }
        public int Estado
        {
            get
            {
                return _estado;
            }
            set
            {
                _estado = value;
                OnPropertyChanged();
                OnPropertyChanged("PermitirVoltar");
                OnPropertyChanged("PermitirRefazer");
            }
        }
        public bool PermitirVoltar
        {
            get
            {
                if (_estado < _comandosExecutados.Count)
                    return true;
                else
                    return false;
            }
        }
        public bool PermitirRefazer
        {
            get
            {
                if (_estado > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Construtores
        public FábricaDeComandosInternos()
        {
            _comandosExecutados = new List<ComandoInterno>();
            Estado = 0;
        }
        #endregion

        #region Métodos
        public void Refazer()
        {
            if (_estado >= 0)
            {
                int total = _comandosExecutados.Count;
                _comandosExecutados[total - _estado].Executar();

                LogDeComandos += System.Environment.NewLine + "Comando refeito: " + _comandosExecutados[total - _estado].LogDoComando();
                
                Estado--;
            }
        }
        public void Desfazer()
        {
            int total = _comandosExecutados.Count;
            if (_estado < total)
            {
                _comandosExecutados[total - _estado - 1].ComandoInverso();
                LogDeComandos += System.Environment.NewLine + "Comando desfeito: " + _comandosExecutados[total - _estado - 1].LogDoComando();
                Estado++;
            }
        }
        public void AdicionarComando(ComandoInterno comando)
        {
            bool executado=comando.Executar();
            if (executado)
            {
                LogDeComandos += System.Environment.NewLine + "Comando executado: " + comando.LogDoComando();

                int total = _comandosExecutados.Count;
                if (_estado == 0)
                {
                    _comandosExecutados.Add(comando);
                    while (_comandosExecutados.Count > _númeroMáximoDeComandosArmazenados)
                    {
                        _comandosExecutados.RemoveAt(0);
                    }
                    Estado = 0;
                }
                else
                {
                    while (_estado != 0)
                    {
                        _comandosExecutados.RemoveAt(total - 1);
                        total--;
                        Estado--;
                    }
                    _comandosExecutados.Add(comando);
                }
                OnPropertyChanged("PermitirVoltar");
                OnPropertyChanged("PermitirRefazer");
            }
            else
            {
                LogDeComandos += System.Environment.NewLine + "Comando não executado: " + comando.LogDoComando();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
