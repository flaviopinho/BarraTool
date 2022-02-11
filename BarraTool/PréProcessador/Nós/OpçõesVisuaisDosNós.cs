using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Nós
{
    public class OpçõesVisuaisDosNós: INotifyPropertyChanged
    {
        #region Campos
        private double _raio = 3;
        private SolidColorBrush _corPadrão = new SolidColorBrush();
        private double _espessuraDasLinhas = 1;

        private static OpçõesVisuaisDosNós _instância = null;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private OpçõesVisuaisDosNós()
        {
            CorPadrão = Brushes.Gray;
        }

        #region Propriedades
        public static OpçõesVisuaisDosNós Instância
        {
            get
            {
                if (_instância == null)
                    _instância = new OpçõesVisuaisDosNós();
                return _instância;
            }
        }

        public double Raio
        {
            get => _raio;
            set
            {
                if (_raio == value)
                    return;
                _raio = value;
                OnPropertyChanged();
            }
        }
        public SolidColorBrush CorPadrão
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
        public double EspessuraDasLinhas
        {
            get => _espessuraDasLinhas;
            set
            {
                if (_espessuraDasLinhas == value)
                    return;
                _espessuraDasLinhas = value;
                OnPropertyChanged();
            }
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
