using BarraTool.PréProcessador.Carregamentos;
using BarraTool.PréProcessador.Elementos;
using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Nós
{
    public class NóVisual : ObjetoVisual
    {
        #region Campos
        private double _coordX;
        private double _coordY;
        private EllipseGeometry _círculo = new EllipseGeometry();
        private EllipseGeometry _centro = new EllipseGeometry();
        private Path _shape1 = new Path();
        private Path _shape2 = new Path();
        private ObservableCollection<ElementoVisual> _elementosConectados;
        #endregion

        #region Propriedades
        public double CoordX
        {
            get
            {
                return _coordX;
            }
            set
            {
                if (_coordX == value) return;
                _coordX = ManipuladorDeUnidades.ArredondarComprimento(value);
                OnPropertyChanged();
                AtualizarDesenho();
            }
        }
        public double CoordY
        {
            get
            {
                return _coordY;
            }
            set
            {
                if (_coordY == value) return;
                _coordY = ManipuladorDeUnidades.ArredondarComprimento(value);
                OnPropertyChanged();
                AtualizarDesenho();
            }
        }
        public ObservableCollection<CarregamentoVisualDoNó> Carregamentos { get; }
        public ObservableCollection<ElementoVisual> ElementosConectados { get => _elementosConectados; }
        #endregion

        #region Construtores
        public NóVisual() : this(0, 0) { }
        public NóVisual(double coordX, double coordY)
        {

            OpçõesVisuaisDosNós opçõesVisuaisDosNós = OpçõesVisuaisDosNós.Instância;
            WeakEventManager<OpçõesVisuaisDosNós, PropertyChangedEventArgs>.AddHandler(opçõesVisuaisDosNós, "PropertyChanged", (object sender, PropertyChangedEventArgs e) => { AtualizarDesenho(); });


            _elementosConectados = new ObservableCollection<ElementoVisual>();
            _coordX = ManipuladorDeUnidades.ArredondarComprimento(coordX);
            _coordY = ManipuladorDeUnidades.ArredondarComprimento(coordY);
            base.Selecionado = false;
            base.CorPadrão = opçõesVisuaisDosNós.CorPadrão;
            base.Cor = opçõesVisuaisDosNós.CorPadrão;

            _shape1.Data = _círculo;
            _shape1.Fill = Cor;
            _shape1.Stroke = Brushes.Black;
            _shape1.StrokeThickness = opçõesVisuaisDosNós.EspessuraDasLinhas;

            _shape2.Data = _centro;
            _shape2.Fill = Brushes.Black;
            _shape2.StrokeThickness = opçõesVisuaisDosNós.EspessuraDasLinhas;

            this.Children.Add(_shape1);
            this.Children.Add(_shape2);
            this.Clip = _círculo;
            Nome = "";

            Carregamentos = new ObservableCollection<CarregamentoVisualDoNó>();

            AtualizarDesenho();
        }
        #endregion

        #region Métodos
        public void Mover(double DeltaX, double DeltaY)
        {
            CoordX += DeltaX;
            CoordY += DeltaY;
        }
        public override void AtualizarDesenho()
        {
            OpçõesVisuaisDosNós _opçõesVisuaisDosNós = OpçõesVisuaisDosNós.Instância;

            base.CorPadrão = _opçõesVisuaisDosNós.CorPadrão;
            if(Selecionado==false)
                base.Cor = _opçõesVisuaisDosNós.CorPadrão;

            _círculo.Center = new Point(_coordX, -_coordY);
            _círculo.RadiusX = _opçõesVisuaisDosNós.Raio;
            _círculo.RadiusY = _opçõesVisuaisDosNós.Raio;

            _centro.Center = new Point(_coordX, -_coordY);
            _centro.RadiusX = _opçõesVisuaisDosNós.Raio / 5;
            _centro.RadiusY = _opçõesVisuaisDosNós.Raio / 5;

            _shape1.StrokeThickness = _opçõesVisuaisDosNós.EspessuraDasLinhas;
            _shape2.StrokeThickness = _opçõesVisuaisDosNós.EspessuraDasLinhas;

            foreach (var elm in ElementosConectados)
                elm.AtualizarDesenho();
        }
        public override IntersectionDetail VerificarIntersecção(Geometry geo)
        { 
            return geo.FillContainsWithDetail(_shape1.RenderedGeometry);
        }
        public override Rect RetânguloExterno()
        {
            OpçõesVisuaisDosNós _opçõesVisuaisDosNós = OpçõesVisuaisDosNós.Instância;
            Rect rec = new Rect();
            rec.X = CoordX - _opçõesVisuaisDosNós.Raio;
            rec.Y = -(CoordY - _opçõesVisuaisDosNós.Raio);
            rec.Width = 2 * _opçõesVisuaisDosNós.Raio;
            rec.Height = 2 * _opçõesVisuaisDosNós.Raio;
            return rec;
        }
        #endregion
    }
}
