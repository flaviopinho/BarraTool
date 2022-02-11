using BarraTool.ComandosInternos;
using BarraTool.PréProcessador.Materiais;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Seções
{
    /// <summary>
    /// Interação lógica para PropriedadesDasSeções.xam
    /// </summary>
    public partial class PropriedadesDasSeções : UserControl, INotifyPropertyChanged
    {
        private Seção _seção;
        private ModeloVisual _modeloVisual;
        
        public PropriedadesDasSeções(Seção seção, ModeloVisual modeloVisual)
        {
            _seção = seção;
            _modeloVisual = modeloVisual;

            InitializeComponent();
            this.DataContext = this;

            var janela = _seção.GeometriaDaSeção.JanelaDasPropriedadesGeométricas(_modeloVisual);
            PropriedadesGeométricas.Children.Add(janela);
            //_seção.PropertyChanged += _seção_PropertyChanged;
            WeakEventManager<Seção, PropertyChangedEventArgs>.AddHandler(_seção, "PropertyChanged", _seção_PropertyChanged);
        }

        private void _seção_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Nome");
            OnPropertyChanged("Cor");
            OnPropertyChanged("Material");
        }

        public string Nome
        {
            get
            {
                return _seção.Nome;
            }
            set
            {
                if (_seção.Nome == value)
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_seção);
                comando.Propriedade = typeof(Seção).GetProperty("Nome");
                comando.ValorAntigo = _seção.Nome;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public Color Cor
        {
            get
            {
                return _seção.Cor.Color;
            }
            set
            {
                if (_seção.Cor.Color == value) return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_seção);
                comando.Propriedade = typeof(Seção).GetProperty("Cor");
                comando.ValorAntigo = _seção.Cor.Clone();
                comando.ValorNovo = new SolidColorBrush(value);
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public Seção Seção { get => _seção; }
        public ModeloVisual ModeloVisual { get => _modeloVisual; }
        public Material Material
        {
            get
            {
                return _seção.Material;
            }
            set
            {
                _modeloVisual.ComandosInternos.AdicionarComando(new ComandoInternoAplicarMaterial(value, _seção));
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SelecionarElementos_Click(object sender, RoutedEventArgs e)
        {
            _modeloVisual.CancelarSeleçãoDosElementos();
            foreach (var elm in _seção.ElementosConectados)
            {
                _modeloVisual.SeleçãoDoObjeto(elm, true);
            }
        }
        private void AplicarSeção_Click(object sender, RoutedEventArgs e)
        {
            _modeloVisual.ComandosInternos.AdicionarComando(new ComandoInternoAplicarSeção(_seção, _modeloVisual));
        }

        private void ExcluirSeção_Click(object sender, RoutedEventArgs e)
        {
            _modeloVisual.ComandosInternos.AdicionarComando(new ComandoInternoDeletarSeção(_seção, _modeloVisual));
        }
    }
}
