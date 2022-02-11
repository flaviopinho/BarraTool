using BarraTool.ComandosInternos;
using BarraTool.Unidades;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Materiais
{
    /// <summary>
    /// Interação lógica para PropriedadeDosMateriais.xam
    /// </summary>
    public partial class PropriedadeDosMateriais : UserControl, INotifyPropertyChanged
    {
        private Material _material;
        private ModeloVisual _modeloVisual;
        private ManipuladorDeUnidades _manipuladorDeUnidades;

        public PropriedadeDosMateriais(Material material, ModeloVisual modeloVisual)
        {
            this._material = material;
            this._modeloVisual = modeloVisual;
            _manipuladorDeUnidades = modeloVisual.ManipuladorDeUnidades;
            InitializeComponent();
            this.DataContext = this;

            WeakEventManager<Material, PropertyChangedEventArgs>.AddHandler(_material, "PropertyChanged", material_PropertyChanged);
            WeakEventManager<ManipuladorDeUnidades, PropertyChangedEventArgs>.AddHandler(_manipuladorDeUnidades, "PropertyChanged", unidades_PropertyChanged);
        }

        public string Nome
        {
            get
            {
                return _material.Nome;
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("Nome");
                comando.ValorAntigo = _material.Nome;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double MóduloDeElasticidade
        {
            get
            {
                return _manipuladorDeUnidades.GetTensão(_material.MóduloDeElasticidade);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetTensão(value);
                if (valorNovo <= 0)
                    throw new ArgumentException("O módulo de elasticidade deve ser um número maior que 0.");
                if (_material.MóduloDeElasticidade == valorNovo)
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("MóduloDeElasticidade");
                comando.ValorAntigo = _material.MóduloDeElasticidade;
                comando.ValorNovo = valorNovo;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public Color Cor
        {
            get
            {
                return _material.Cor.Color;
            }
            set
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("Cor");
                comando.ValorAntigo = _material.Cor.Clone();
                comando.ValorNovo = new SolidColorBrush(value);
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double CoeficienteDePoisson
        {
            get
            {
                return _material.CoeficienteDePoisson;
            }
            set
            {
                if (value <= 0 || value > 0.5)
                    throw new ArgumentException("O coeficiente de Poisson deve estar entre 0 e 0,5.");
                if (_material.CoeficienteDePoisson == value)
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("CoeficienteDePoisson");
                comando.ValorAntigo = _material.CoeficienteDePoisson;
                comando.ValorNovo = value;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double Densidade
        {
            get
            {
                return _manipuladorDeUnidades.GetDensidade(_material.Densidade);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetDensidade(_material.Densidade);
                if(valorNovo<=0)
                    throw new ArgumentException("O valor da densidade deve ser maior que 0.");
                if (_material.Densidade == valorNovo)
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("Densidade");
                comando.ValorAntigo = _material.Densidade;
                comando.ValorNovo = valorNovo;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double CoeficienteDeDilataçãoTérmica
        {
            get
            {
                return _manipuladorDeUnidades.GetCoeficienteDeDilatação(_material.CoeficienteDeDilataçãoTérmica);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetCoeficienteDeDilatação(value);
                if (valorNovo < 0)
                    throw new ArgumentException("O coeficiente de dilatação térmica deve ser positivo.");
                if (_material.CoeficienteDeDilataçãoTérmica == valorNovo)
                    return;
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_material);
                comando.Propriedade = typeof(Material).GetProperty("CoeficienteDeDilataçãoTérmica");
                comando.ValorAntigo = _material.CoeficienteDeDilataçãoTérmica;
                comando.ValorNovo = valorNovo;
                _modeloVisual.ComandosInternos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public Material Material { get => _material; }
        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void material_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        private void unidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("MóduloDeElasticidade");
            OnPropertyChanged("CoeficienteDeDilataçãoTérmica");
            OnPropertyChanged("Densidade");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExcluirMaterial_Click(object sender, RoutedEventArgs e)
        {
            ComandoInternoDeletarMaterial comando = new ComandoInternoDeletarMaterial(_material, _modeloVisual);
            _modeloVisual.ComandosInternos.AdicionarComando(comando);
        }
    }
}
