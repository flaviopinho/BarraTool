using BarraTool.ComandosInternos;
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
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Carregamentos
{
    /// <summary>
    /// Lógica interna para JanelaInserirCarregamento.xaml
    /// </summary>
    public partial class JanelaInserirCarregamento : Window, INotifyPropertyChanged
    {
        private ModeloVisual _modeloVisual;
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private CarregamentoDistribuido _carregamentoDistribuido;
        private Caso _caso;
        public event PropertyChangedEventHandler PropertyChanged;

        public double DistribuidoCargaX
        {
            get
            {
                return _manipuladorDeUnidades.GetCargaDistribuida(_carregamentoDistribuido.CargaX);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetCargaDistribuida(value);
                if (valorNovo == _carregamentoDistribuido.CargaX)
                    return;
                _carregamentoDistribuido.CargaX = valorNovo;
                OnPropertyChanged();
            }
        }
        public double DistribuidoCargaY
        {
            get
            {
                return _manipuladorDeUnidades.GetCargaDistribuida(_carregamentoDistribuido.CargaY);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetCargaDistribuida(value);
                if (valorNovo == _carregamentoDistribuido.CargaY)
                    return;
                _carregamentoDistribuido.CargaY = valorNovo;
                OnPropertyChanged();
            }
        }
        public bool DistribuidoSistemaLocal
        {
            get
            {
                return _carregamentoDistribuido.SistemaLocal;
            }
            set
            {
                _carregamentoDistribuido.SistemaLocal = value;
                OnPropertyChanged();
                OnPropertyChanged("DistribuidoSistemaGlobal");
            }
        }
        public bool DistribuidoSistemaGlobal
        {
            get
            {
                return _carregamentoDistribuido.SistemaGlobal;
            }
            set
            {
                _carregamentoDistribuido.SistemaGlobal = value;
                OnPropertyChanged();
                OnPropertyChanged("DistribuidoSistemaLocal");
            }
        }
        public Caso Caso
        {
            get => _caso;
            set
            {
                _caso = value;
                _carregamentoDistribuido.Caso = _caso;
            }
        }
        public ManipuladorDeUnidades ManipuladorDeUnidades
        {
            get => _manipuladorDeUnidades;
        }
        public ModeloVisual ModeloVisual { get => _modeloVisual; }

        public JanelaInserirCarregamento(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _manipuladorDeUnidades = modeloVisual.ManipuladorDeUnidades.Clone();
            _caso=_modeloVisual.Carregamentos.CasoAtual;
            _carregamentoDistribuido = new CarregamentoDistribuido(0, 10, _caso);
            InitializeComponent();
            this.DataContext = this;
            WeakEventManager<ManipuladorDeUnidades, PropertyChangedEventArgs>.AddHandler(_manipuladorDeUnidades, "PropertyChanged", manipuladorDeUnidades_PropertyChanged);
            WeakEventManager<CarregamentoDistribuido, PropertyChangedEventArgs>.AddHandler(_carregamentoDistribuido, "PropertyChanged", manipuladorDeUnidades_PropertyChanged);
        }
        private void manipuladorDeUnidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("DistribuidoCargaX");
            OnPropertyChanged("DistribuidoCargaY");
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            ComandoInternoInserirCarregamento comando = new ComandoInternoInserirCarregamento(_carregamentoDistribuido, _modeloVisual.Carregamentos);
            _modeloVisual.ComandosInternos.AdicionarComando(comando);
            this.Close();
        }
    }
}
