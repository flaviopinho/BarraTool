using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace BarraTool.PréProcessador.Seções
{
    public class SeçãoRetangular : GeometriaDaSeção, INotifyPropertyChanged
    {
        private double _hx;
        private double _hy;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Hx
        {
            get
            {
                return _hx;
            }
            set
            {
                if (_hx == value) return;
                _hx = ManipuladorDeUnidades.ArredondarComprimento(value);
                OnPropertyChanged();
            }
        }
        public double Hy
        {
            get
            {
                return _hy;
            }
            set
            {
                if (_hy == value) return;
                _hy = ManipuladorDeUnidades.ArredondarComprimento(value);
                OnPropertyChanged();
            }
        }
        public override string TipoDeSeção => "Seção Retangular";

        public SeçãoRetangular()
        {
            _hx = 15;
            _hy = 50;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override UserControl JanelaDasPropriedadesGeométricas(ModeloVisual modeloVisual)
        {
            return new PropriedadesSeçãoRetangular(this, modeloVisual);
        }
    }
}
