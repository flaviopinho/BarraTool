using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador
{
    public abstract class ObjetoVisual : Canvas, INotifyPropertyChanged, ISelecionado, INome
    {
        #region Campos
        private string _nome;
        private SolidColorBrush _cor = new SolidColorBrush();
        private SolidColorBrush _corPadrão = new SolidColorBrush();
        private bool _selecionado = false;
        public event PropertyChangedEventHandler PropertyChanged;
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
                if (_nome == value) return;
                _nome = value;
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
                if (_selecionado == true)
                {
                    _cor.Color = Colors.Red;
                }
                else
                {
                    _cor.Color = _corPadrão.Color;
                }
                OnPropertyChanged();
                OnPropertyChanged("Cor");
            }
        }
        public virtual SolidColorBrush Cor
        {
            get => _cor;
            set
            {
                _cor.Color = value.Color;
                OnPropertyChanged();
            }
        }
        public virtual SolidColorBrush CorPadrão
        {
            get
            {
                return _corPadrão;
            }
            set
            {
                _corPadrão.Color = value.Color;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Construtores
        public ObjetoVisual()
        {
            _cor.Color = Brushes.DarkGray.Color;
            _corPadrão.Color = Brushes.DarkGray.Color;
        }
        #endregion

        #region Métodos
        public abstract IntersectionDetail VerificarIntersecção(Geometry geo);
        public abstract Rect RetânguloExterno();
        public abstract void AtualizarDesenho();
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
