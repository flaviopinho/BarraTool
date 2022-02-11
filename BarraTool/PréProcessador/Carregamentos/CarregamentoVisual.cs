using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BarraTool.PréProcessador.Carregamentos
{
    public abstract class CarregamentoVisual : Canvas, INotifyPropertyChanged, INome
    {
        protected string _nome;
        protected Caso _caso;
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (value == _nome) return;
                _nome = value;
                OnPropertyChanged();
            }
        }
        public Caso Caso
        {
            get
            {
                return _caso;
            }
            set
            {
                if (value == _caso) return;
                _caso = value;
                OnPropertyChanged();
            }
        }

        public abstract void AtualizarDesenho();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
