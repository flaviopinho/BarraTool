using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Materiais;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Seções
{
    public class Seção : INotifyPropertyChanged, ISelecionado, INome
    {
        #region Campos
        private string _nome;
        private SolidColorBrush _cor = new SolidColorBrush();
        private bool _seçãoAtual = false;
        private bool _selecionado = false;
        private Material _material;
        private ObservableCollection<ElementoVisual> _elementosConectados;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Propriedades
        public string Nome
        {
            get => _nome;
            set
            {
                if (_nome == value) return;
                _nome = value;
                OnPropertyChanged();
            }
        }
        public SolidColorBrush Cor
        {
            get => _cor;
            set
            {
                _cor.Color = value.Color;
                OnPropertyChanged();
            }
        }
        public bool Selecionado
        {
            get
            {
                return _selecionado;
            }
            set
            {
                if (value == _selecionado) return;
                _selecionado = value;
                OnPropertyChanged();
            }
        }
        public bool SeçãoAtual
        {
            get
            {
                return _seçãoAtual;
            }
            set
            {
                if (value == _seçãoAtual) return;
                _seçãoAtual = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ElementoVisual> ElementosConectados
        {
            get
            {
                return _elementosConectados;
            }
        }
        public Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                if (value == _material) return;
                _material = value;
                OnPropertyChanged();
            }
        }
        public GeometriaDaSeção GeometriaDaSeção {get; set;}
        #endregion

        #region Construtor
        public Seção()
        {
            _cor = new SolidColorBrush(Colors.Gray);
            _nome = "";
            _elementosConectados = new ObservableCollection<ElementoVisual>();
        }
        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
