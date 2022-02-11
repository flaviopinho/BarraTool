using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace BarraTool.PréProcessador.Nós
{
    /// <summary>
    /// Interação lógica para InserirNo.xam
    /// </summary>
    public partial class UserContralInserirNo : UserControl, INotifyPropertyChanged
    {
        #region Campos
        private Point ponto = new Point(10, 10);
        private ManipuladorDeUnidades manipuladorDeUnidades;
        private ScaleTransform escalaDoDesenho;
        #endregion
        public UserContralInserirNo(ScaleTransform escala, ManipuladorDeUnidades manipuladorDeUnidades)
        {
            this.escalaDoDesenho = escala;
            this.manipuladorDeUnidades = manipuladorDeUnidades;
            InitializeComponent();
            this.DataContext = this;
            escala.Changed += Escala_Changed;
        }

        private void Escala_Changed(object sender, EventArgs e)
        {
            Circulo.RadiusX = 1.5 / escalaDoDesenho.ScaleX;
            Circulo.RadiusY = 1.5 / escalaDoDesenho.ScaleX;
        }

        public Point Ponto
        {
            get => ponto;
            set
            {
                ponto = value;
                OnPropertyChanged();
                OnPropertyChanged("CoordXVisual");
                OnPropertyChanged("CoordYVisual");
            }
        }
        public double CoordXVisual
        {
            get
            {
                return Ponto.X * manipuladorDeUnidades.ComprimentoFator;
            }
            set
            {
                Ponto = new Point(value / manipuladorDeUnidades.ComprimentoFator, ponto.Y);
                OnPropertyChanged();
            }
        }
        public double CoordYVisual
        {
            get
            {
                return -Ponto.Y * manipuladorDeUnidades.ComprimentoFator;
            }
            set
            {
                Ponto = new Point(ponto.X, - value / manipuladorDeUnidades.ComprimentoFator);
                OnPropertyChanged();
            }
        }

        public double CoordX
        {
            get
            {
                return Ponto.X;
            }
        }
        public double CoordY
        {
            get
            {
                return -Ponto.Y;
            }
        }

        public ScaleTransform EscalaDoDesenho
        {
            get
            {
                return escalaDoDesenho;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TransformInverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Transform transform = value as Transform;
            if (transform == null)
                return Transform.Identity;
            return transform.Inverse;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
