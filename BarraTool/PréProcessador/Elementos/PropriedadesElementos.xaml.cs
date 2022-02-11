using BarraTool.ComandosInternos;
using BarraTool.PréProcessador.Seções;
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

namespace BarraTool.PréProcessador.Elementos
{
    /// <summary>
    /// Interação lógica para PropriedadesElementos.xam
    /// </summary>
    public partial class PropriedadesElementos : UserControl, INotifyPropertyChanged
    {
        private ModeloVisual _modeloVisual;
        private ElementoVisual _elemento;
        private Seção _seção;
        public event PropertyChangedEventHandler PropertyChanged;

        public PropriedadesElementos(ElementoVisual elemento, ModeloVisual modelo)
        {
            this._elemento = elemento;
            this._modeloVisual = modelo;
            InitializeComponent();
            this.DataContext = this;
            _seção = elemento.Seção;

            WeakEventManager<ElementoVisual, PropertyChangedEventArgs>.AddHandler(_elemento, "PropertyChanged", elemento_PropertyChanged);
        }

        public ElementoVisual Elemento { get => _elemento; }
        public ModeloVisual ModeloVisual { get => _modeloVisual; }
        public Seção Seção
        {
            get => _seção;
            set
            {
                if (_seção == value) return;
                ComandoInternoAplicarSeção alterar = new ComandoInternoAplicarSeção(value,_elemento);
                _modeloVisual.ComandosInternos.AdicionarComando(alterar);
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void elemento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
