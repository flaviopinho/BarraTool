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

namespace BarraTool.PréProcessador.Seções
{
    /// <summary>
    /// Interação lógica para JanelaSeçãoRetangular.xam
    /// </summary>
    public partial class PropriedadesSeçãoRetangular : UserControl, INotifyPropertyChanged
    {
        #region Campos
        private SeçãoRetangular _seçãoRetangular;
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        private FábricaDeComandosInternos _comandos;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public PropriedadesSeçãoRetangular(SeçãoRetangular seção, ModeloVisual modeloVisual)
        {
            _seçãoRetangular = seção;
            _manipuladorDeUnidades = modeloVisual.ManipuladorDeUnidades;
            _comandos = modeloVisual.ComandosInternos;
            InitializeComponent();
            this.DataContext = this;
            WeakEventManager<ManipuladorDeUnidades, PropertyChangedEventArgs>.AddHandler(_manipuladorDeUnidades, "PropertyChanged", manipuladorDeUnidades_PropertyChanged);
            WeakEventManager<SeçãoRetangular, PropertyChangedEventArgs>.AddHandler(_seçãoRetangular, "PropertyChanged", manipuladorDeUnidades_PropertyChanged);
        }

        #region Propriedades
        public double Hx
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_seçãoRetangular.Hx);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetComprimento(value);
                
                if (_seçãoRetangular.Hx == valorNovo)
                    return;
                if (valorNovo <= 0 || valorNovo >= 500)
                    throw new ArgumentException("A dimensão das seções retangulares devem estar entre 0 e 500 cm");

                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_seçãoRetangular);
                comando.Propriedade = typeof(SeçãoRetangular).GetProperty("Hx");
                comando.ValorAntigo = _seçãoRetangular.Hx;
                comando.ValorNovo = valorNovo;
                _comandos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public double Hy
        {
            get
            {
                return _manipuladorDeUnidades.GetComprimento(_seçãoRetangular.Hy);
            }
            set
            {
                double valorNovo = _manipuladorDeUnidades.SetComprimento(value);
                if (_seçãoRetangular.Hy == valorNovo)
                    return;
                if (valorNovo <= 0 || valorNovo >= 500)
                    throw new ArgumentException("A dimensão das seções retangulares devem estar entre 0 e 500 cm");

                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_seçãoRetangular);
                comando.Propriedade = typeof(SeçãoRetangular).GetProperty("Hy");
                comando.ValorAntigo = _seçãoRetangular.Hy;
                comando.ValorNovo = valorNovo;
                _comandos.AdicionarComando(comando);
                OnPropertyChanged();
            }
        }
        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; }
        #endregion

        #region Métodos
        private void manipuladorDeUnidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Hx");
            OnPropertyChanged("Hy");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
