using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Materiais
{
    /// <summary>
    /// Lógica interna para JanelaInserirMaterial.xaml
    /// </summary>
    public partial class JanelaInserirMaterial : Window
    {
        #region Campos
        private string _nome;
        private double _móduloDeElasticidade;
        private double _coeficienteDePoisson;
        private double _densidade;
        private double _coeficienteDeDilataçãoTérmica;
        private SolidColorBrush _cor = new SolidColorBrush();
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private ModeloVisual _modeloVisual;
        #endregion

        #region Propriedades
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
            }
        }
        public double MóduloDeElasticidade
        {
            get
            {
                return _manipuladorDeUnidades.GetTensão(_móduloDeElasticidade);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetTensão(value);
                if (valorNovo <= 0)
                    throw new ArgumentException("O módulo de elasticidade deve ser um número maior que 0.");
                if (_móduloDeElasticidade == valorNovo)
                    return;

                _móduloDeElasticidade = valorNovo;
            }
        }
        public Color Cor
        {
            get
            {
                return _cor.Color;
            }
            set
            {
                if (_cor.Color == value)
                    return;
                _cor.Color = value;
            }
        }
        public double CoeficienteDePoisson
        {
            get
            {
                return _coeficienteDePoisson;
            }
            set
            {
                if (value <= 0 || value > 0.5)
                    throw new ArgumentException("O coeficiente de Poisson deve estar entre 0 e 0,5.");
                if (_coeficienteDePoisson == value)
                    return;
                _coeficienteDePoisson = value;
            }
        }
        public double Densidade
        {
            get
            {
                return _manipuladorDeUnidades.GetDensidade(_densidade);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetDensidade(_densidade);
                if (valorNovo <= 0)
                    throw new ArgumentException("O valor da densidade deve ser maior que 0.");
                if (_densidade == valorNovo)
                    return;
                _densidade = valorNovo;
            }
        }
        public double CoeficienteDeDilataçãoTérmica
        {
            get
            {
                return _manipuladorDeUnidades.GetCoeficienteDeDilatação(_coeficienteDeDilataçãoTérmica);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetCoeficienteDeDilatação(value);
                if (valorNovo < 0)
                    throw new ArgumentException("O coeficiente de dilatação térmica deve ser positivo.");
                if (_coeficienteDeDilataçãoTérmica == valorNovo)
                    return;
                _coeficienteDeDilataçãoTérmica = valorNovo;
            }
        }
        public ModeloVisual ModeloVisual { get => _modeloVisual; }
        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; }
        #endregion

        public JanelaInserirMaterial(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _manipuladorDeUnidades = modeloVisual.ManipuladorDeUnidades;
            _nome = ModeloVisual.Modelo.GerarNovoNomeDeMaterial();
            _móduloDeElasticidade = 2000;
            _coeficienteDePoisson = 0.3;
            _densidade = 0.0025;
            _coeficienteDeDilataçãoTérmica = 1e-5;
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Material mat = new Material();
            mat.Nome = Nome;
            mat.Cor = new SolidColorBrush((Color)ColorPicker.SelectedColor);
            mat.MóduloDeElasticidade = ManipuladorDeUnidades.SetTensão(MóduloDeElasticidade);
            mat.CoeficienteDePoisson = CoeficienteDePoisson;
            mat.Densidade = ManipuladorDeUnidades.SetDensidade(Densidade);
            mat.CoeficienteDeDilataçãoTérmica = ManipuladorDeUnidades.SetCoeficienteDeDilatação(CoeficienteDeDilataçãoTérmica);
            ModeloVisual.ComandosInternos.AdicionarComando(new ComandosInternos.ComandoInternoInserirMaterial(mat,ModeloVisual.Modelo));
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
