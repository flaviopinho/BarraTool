using BarraTool.PréProcessador.Materiais;
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

namespace BarraTool.PréProcessador.Seções
{
    /// <summary>
    /// Lógica interna para JanelaInserirSeção.xaml
    /// </summary>
    /// 

    public partial class JanelaInserirSeção : Window
    {
        #region Campos
        private double _hx;
        private double _hy;
        private string _nome;
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private ModeloVisual _modeloVisual;

        #endregion
        
        public double Hx
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_hx);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetComprimento(value);
                if (_hx == valorNovo)
                    return;
                if (valorNovo <= 0 || valorNovo >= 500)
                    throw new ArgumentException("A dimensão das seções retangulares devem estar entre 0 e 500 cm");
                _hx = valorNovo;
            }
        }
        public double Hy
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_hy);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetComprimento(value);
                if (_hy == valorNovo)
                    return;
                if (valorNovo <= 0 || valorNovo >= 500)
                    throw new ArgumentException("A dimensão das seções retangulares devem estar entre 0 e 500 cm");
                _hy = valorNovo;
            }
        }
        public string Nome { get => _nome; set => _nome = value; }
        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; set => _manipuladorDeUnidades = value; }
        public ModeloVisual ModeloVisual { get => _modeloVisual; set => _modeloVisual = value; }

        public JanelaInserirSeção(ModeloVisual modeloVisual)
        {
            _modeloVisual = modeloVisual;
            _manipuladorDeUnidades = _modeloVisual.ManipuladorDeUnidades;
            _hx = 15;
            _hy = 50;
            _nome = modeloVisual.Modelo.GerarNovoNomeDeSeção();
            InitializeComponent();
            this.DataContext = this;
            /*this.Loaded += (object sender, RoutedEventArgs e) =>
                    {
                        ManipuladorDeUnidades = ModeloVisual.ManipuladorDeUnidades;
                        Nome = ModeloVisual.Modelo.GerarNovoNomeDeSeção();
                        Hx = ManipuladorDeUnidades.GetComprimento(15);
                        Hy = ManipuladorDeUnidades.GetComprimento(50);
                    };*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Seção seção = new Seção();
            seção.Nome = Nome;
            seção.Cor = new SolidColorBrush((Color)ColorPicker.SelectedColor);
            seção.Material = (Material)ComboBoxMaterial.SelectedItem;
            var seçãoRetangular = new SeçãoRetangular();
            seçãoRetangular.Hx = _hx;
            seçãoRetangular.Hy = _hy;
            seção.GeometriaDaSeção = seçãoRetangular;
            ComandosInternos.ComandoInternoInserirSeção cmd = new ComandosInternos.ComandoInternoInserirSeção(seção, ModeloVisual.Modelo);
            ModeloVisual.ComandosInternos.AdicionarComando(cmd);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
