using BarraTool.Unidades;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BarraTool.ComandosExternos
{
    /// <summary>
    /// Interação lógica para PontoAPontoBLinha.xam
    /// </summary>
    public partial class PontoAPontoBLinha : UserControl , INotifyPropertyChanged
    {
        #region Campos
        private Point pontoA = new Point(10,10);
        private Point pontoB = new Point(300, 300);
        private ManipuladorDeUnidades manipuladorDeUnidades;
        private ScaleTransform escalaDoDesenho;
        private bool ponto1Inserido = false;
        #endregion

        public PontoAPontoBLinha(ScaleTransform escala, ManipuladorDeUnidades manipuladorDeUnidades)
        {
            this.escalaDoDesenho = escala;
            this.manipuladorDeUnidades = manipuladorDeUnidades;
            InitializeComponent();
            this.DataContext = this;
            escalaDoDesenho.Changed += EscalaDoDesenho_Changed;
            Ponto1Inserido = false;
        }
        #region Propriedades
        public bool Ponto1Inserido
        {
            get => ponto1Inserido;
            set
            {
                ponto1Inserido = value;
                if(ponto1Inserido==true)
                {
                    stackPanel.Visibility = Visibility.Visible;
                    Linha.Visibility = Visibility.Visible;
                    Cirulo1.Visibility= Visibility.Visible;
                }
                else
                {
                    stackPanel.Visibility = Visibility.Hidden;
                    Linha.Visibility = Visibility.Hidden;
                    Cirulo1.Visibility = Visibility.Hidden;
                }
            }
        }
        public Point PontoA
        {
            get => pontoA;
            set
            {
                pontoA = value;
                DeltaXVisual = DeltaXVisual;
                OnPropertyChanged();
                OnPropertyChanged("PosicaoTextBoxX");
                OnPropertyChanged("PosicaoTextBoxY");
                OnPropertyChanged("DeltaXVisual");
                OnPropertyChanged("DeltaYVisual");
            }
        }
        public Point PontoB
        {
            get => pontoB;
            set
            {
                pontoB = value;
                OnPropertyChanged();
                OnPropertyChanged("PosicaoTextBoxX");
                OnPropertyChanged("PosicaoTextBoxY");
                OnPropertyChanged("DeltaXVisual");
                OnPropertyChanged("DeltaYVisual");
            }
        }
        public double PosicaoTextBoxX
        {
            get
            {
                return (PontoA.X + PontoB.X) / 2 - 25/escalaDoDesenho.ScaleX;
            }
        }
        public double PosicaoTextBoxY
        {
            get
            {
                return (PontoA.Y + PontoB.Y) / 2-18 / escalaDoDesenho.ScaleX;
            }
        }

        public double DeltaXVisual
        {
            get
            {
                return ManipuladorDeUnidades.ArredondarComprimento(PontoB.X - PontoA.X)*manipuladorDeUnidades.ComprimentoFator;
            }
            set
            {
                PontoB = new Point(pontoA.X + ManipuladorDeUnidades.ArredondarComprimento(value) / manipuladorDeUnidades.ComprimentoFator, pontoB.Y);
                OnPropertyChanged();
            }
        }
        public double DeltaYVisual
        {
            get
            {
                return -ManipuladorDeUnidades.ArredondarComprimento(PontoB.Y - PontoA.Y) * manipuladorDeUnidades.ComprimentoFator;
            }
            set
            {
                PontoB = new Point(pontoB.X, pontoA.Y - ManipuladorDeUnidades.ArredondarComprimento(value) / manipuladorDeUnidades.ComprimentoFator);
                OnPropertyChanged();
            }
        }

        public double DeltaX
        {
            get
            {
                return (PontoB.X - PontoA.X);
            }
        }
        public double DeltaY
        {
            get
            {
                return -(PontoB.Y - PontoA.Y);
            }
        }

        public ScaleTransform EscalaDoDesenho
        {
            get
            {
                return escalaDoDesenho;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EscalaDoDesenho_Changed(object sender, EventArgs e)
        {
            Linha.StrokeThickness = 1 / escalaDoDesenho.ScaleX;
            C1.RadiusX = 1.5 / escalaDoDesenho.ScaleX;
            C1.RadiusY = 1.5 / escalaDoDesenho.ScaleX;
            C2.RadiusX = 1.5 / escalaDoDesenho.ScaleX;
            C2.RadiusY = 1.5 / escalaDoDesenho.ScaleX;
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
