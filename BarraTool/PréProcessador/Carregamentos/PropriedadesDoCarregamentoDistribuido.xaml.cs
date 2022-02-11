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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Carregamentos
{
    /// <summary>
    /// Interação lógica para PropriedadesDoCarregamentoDistribuido.xam
    /// </summary>
    public partial class PropriedadesDoCarregamentoDistribuido : UserControl, INotifyPropertyChanged
    {
        #region Campos
        private CarregamentoDistribuido _carregamento;
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private ModeloVisual _modeloVisual;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public PropriedadesDoCarregamentoDistribuido(CarregamentoDistribuido carregamento, ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _carregamento = carregamento;
            _manipuladorDeUnidades = _modeloVisual.ManipuladorDeUnidades;
            InitializeComponent();

            this.DataContext = this;

            WeakEventManager<CarregamentoDistribuido, PropertyChangedEventArgs>.AddHandler(_carregamento, "PropertyChanged", carregamento_PropertyChanged);
            WeakEventManager<ManipuladorDeUnidades, PropertyChangedEventArgs>.AddHandler(_manipuladorDeUnidades, "PropertyChanged", unidades_PropertyChanged);
        }

        #region Propriedades
        public string Nome
        {
            get
            {
                return _carregamento.Nome;
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_carregamento);
                comando.Propriedade = typeof(CarregamentoDistribuido).GetProperty("Nome");
                comando.ValorAntigo = _carregamento.Nome;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double CargaX
        {
            get
            {
                return ManipuladorDeUnidades.ArredondarGeral(_carregamento.CargaX / _manipuladorDeUnidades.ComprimentoFator * _manipuladorDeUnidades.ForçaFator);
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_carregamento);
                comando.Propriedade = typeof(CarregamentoDistribuido).GetProperty("CargaX");
                comando.ValorAntigo = _carregamento.CargaX;
                comando.ValorNovo = value * _manipuladorDeUnidades.ComprimentoFator / _manipuladorDeUnidades.ForçaFator;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }

        public double CargaY
        {
            get
            {
                return ManipuladorDeUnidades.ArredondarGeral(_carregamento.CargaY / _manipuladorDeUnidades.ComprimentoFator * _manipuladorDeUnidades.ForçaFator);
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_carregamento);
                comando.Propriedade = typeof(CarregamentoDistribuido).GetProperty("CargaY");
                comando.ValorAntigo = _carregamento.CargaY;
                comando.ValorNovo = value * _manipuladorDeUnidades.ComprimentoFator / _manipuladorDeUnidades.ForçaFator;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public bool SistemaGlobal
        {
            get
            {
                return _carregamento.SistemaGlobal;
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_carregamento);
                comando.Propriedade = typeof(CarregamentoDistribuido).GetProperty("SistemaGlobal");
                comando.ValorAntigo = _carregamento.SistemaGlobal;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
                OnPropertyChanged("SistemaLocal");
            }
        }
        public bool SistemaLocal
        {
            get
            {
                return _carregamento.SistemaLocal;
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_carregamento);
                comando.Propriedade = typeof(CarregamentoDistribuido).GetProperty("SistemaLocal");
                comando.ValorAntigo = _carregamento.SistemaLocal;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
                OnPropertyChanged("SistemaGlobal");
            }
        }
        public ManipuladorDeUnidades ManipuladorDeUnidades
        {
            get => _manipuladorDeUnidades;
        }
        public ModeloVisual ModeloVisual { get => _modeloVisual; }
        public Caso Caso
        {
            get
            {
                return _carregamento.Caso;
            }
            set
            {
                ComandoInternoAlterarCasoDoCarregamento comando = new ComandoInternoAlterarCasoDoCarregamento(_carregamento, value, _modeloVisual.Carregamentos);
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        #endregion

        private void carregamento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        private void unidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CargaX");
            OnPropertyChanged("CargaY");
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SelecionarElementos_Click(object sender, RoutedEventArgs e)
        {
            _modeloVisual.CancelarSeleção();
            foreach (var elm in _carregamento.ElementoConectadosEDesenhos)
                _modeloVisual.SeleçãoDoObjeto(elm.Key, true);
        }

        private void AplicarCarregamento_Click(object sender, RoutedEventArgs e)
        {
            ComandoInternoAplicarCarregamentoNoElemento comando =
                new ComandoInternoAplicarCarregamentoNoElemento(_carregamento,
                _modeloVisual.ElementosSelecionados,
                _modeloVisual.Carregamentos);
            _modeloVisual.ComandosInternos.AdicionarComando(comando);
        }
        private void RemoverCarregamento_Click(object sender, RoutedEventArgs e)
        {
            ComandoInternoRemoverCarregamentoDoElemento comando =
                new ComandoInternoRemoverCarregamentoDoElemento(_carregamento,
                _modeloVisual.ElementosSelecionados,
                _modeloVisual.Carregamentos);
            _modeloVisual.ComandosInternos.AdicionarComando(comando);
        }
    }
}
