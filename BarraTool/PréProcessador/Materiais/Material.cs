using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Seções;
using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Materiais
{
    public class Material: INotifyPropertyChanged, ISelecionado, INome
    {
        #region Campos
        private SolidColorBrush _cor = new SolidColorBrush();
        private string _nome;
        private double _móduloDeElasticidade;
        private double _coeficienteDePoisson;
        private double _densidade;
        private double _coeficienteDeDilataçãoTérmica;
        private bool _selecionado = false;
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
        public ObservableCollection<Seção> SeçõesConectadas { get; }
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
        public double MóduloDeElasticidade
        {
            get
            {
                return _móduloDeElasticidade;
            }
            set
            {
                if (_móduloDeElasticidade == value) return;
                _móduloDeElasticidade = value;
                OnPropertyChanged();
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
                if (_coeficienteDePoisson == value) return;
                _coeficienteDePoisson = value;
                OnPropertyChanged();
            }
        }
        public double Densidade
        {
            get
            {
                return _densidade;
            }
            set
            {
                if (_densidade == value) return;
                _densidade = value;
                OnPropertyChanged();
            }
        }
        public double CoeficienteDeDilataçãoTérmica
        {
            get
            {
                return _coeficienteDeDilataçãoTérmica;
            }
            set
            {
                if (_coeficienteDeDilataçãoTérmica == value) return;
                _coeficienteDeDilataçãoTérmica = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Construtures
        public Material()
        {
            _nome = "";
            SeçõesConectadas = new ObservableCollection<Seção>();
            _cor = new SolidColorBrush(Colors.Blue);
            _móduloDeElasticidade = 3000;
            _coeficienteDePoisson = 0.3;
            _densidade = 2500e-6;
            _coeficienteDeDilataçãoTérmica = Math.Pow(10, -5);
        }
        #endregion

        #region Métodos
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
