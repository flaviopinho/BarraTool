using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BarraTool.ComandosInternos;

namespace BarraTool.PréProcessador.Nós
{
    /// <summary>
    /// Interação lógica para PropriedadesNo.xam
    /// </summary>
    public partial class PropriedadesDosNós : UserControl, INotifyPropertyChanged
    {
        #region Campos
        private NóVisual _nó;
        private ModeloVisual _modeloVisual;
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private FábricaDeComandosInternos _comandos;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Construtores
        public PropriedadesDosNós(NóVisual nó,
            ModeloVisual modeloVisual)
        {
            _nó = nó;
            _modeloVisual = modeloVisual;
            _manipuladorDeUnidades = _modeloVisual.ManipuladorDeUnidades;
            _comandos = _modeloVisual.ComandosInternos;
            InitializeComponent();
            this.DataContext = this;
            
            WeakEventManager<ManipuladorDeUnidades, PropertyChangedEventArgs>.AddHandler(_manipuladorDeUnidades, "PropertyChanged", manipuladorDeUnidades_PropertyChanged);
            WeakEventManager<NóVisual, PropertyChangedEventArgs>.AddHandler(_nó, "PropertyChanged", nó_PropertyChanged);
        }
        #endregion

        #region Propriedades
        public string Nome
        {
            get
            {
                return _nó.Nome;
            }
            set
            {
                if (_nó.Nome == value) return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_nó);
                comando.Propriedade = typeof(NóVisual).GetProperty("Nome");
                comando.ValorAntigo = _nó.Nome;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double CoordX
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_nó.CoordX);
            }
            set
            {
                if (_nó.CoordX == _manipuladorDeUnidades.SetComprimento(value))
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_nó);
                comando.Propriedade = typeof(NóVisual).GetProperty("CoordX");
                comando.ValorAntigo = _nó.CoordX;
                comando.ValorNovo = _manipuladorDeUnidades.SetComprimento(value);
                _comandos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double CoordY
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_nó.CoordY);
            }
            set
            {
                if (_nó.CoordY == _manipuladorDeUnidades.SetComprimento(value))
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_nó);
                comando.Propriedade = typeof(NóVisual).GetProperty("CoordY");
                comando.ValorAntigo = _nó.CoordY;
                comando.ValorNovo = _manipuladorDeUnidades.SetComprimento(value);
                _comandos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Métodos
        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; }
        private void nó_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        private void manipuladorDeUnidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CoordX");
            OnPropertyChanged("CoordY");
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
