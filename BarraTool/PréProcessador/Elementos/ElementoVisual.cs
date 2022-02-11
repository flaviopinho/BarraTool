using BarraTool.PréProcessador.Carregamentos;
using BarraTool.PréProcessador.Materiais;
using BarraTool.PréProcessador.Seções;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BarraTool.PréProcessador.Elementos
{
    public partial class ElementoVisual : ObjetoVisual
    {
        #region Campos
        private NóVisual _nó1;
        private NóVisual _nó2;
        private Seções.Seção _seção;
        private Path _path;
        private PathGeometry _pathGeometry;
        private PathFigure _pathFigure;
        private PolyLineSegment _contorno;
        #endregion

        #region Propriedades
        public NóVisual Nó1 { get => _nó1; /*set => _nó1 = value;*/ }
        public NóVisual Nó2 { get => _nó2; /*set => _nó2 = value;*/ }
        public Seções.Seção Seção
        {
            get
            {
                return _seção;
            }
            set
            {
                if (_seção == value) return;
                _seção = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Construtores
        public ElementoVisual(NóVisual no1, NóVisual no2, Seções.Seção secao)
        {
            this.CorPadrão.Color = Brushes.DeepSkyBlue.Color;
            this.Cor.Color = this.CorPadrão.Color;
            this._nó1 = no1;
            this._nó2 = no2;

            this._seção = secao;
            _path = new Path();
            _path.StrokeThickness = 0.5;
            _path.Stroke = Brushes.Black;
            _path.Fill = Cor;
            _pathFigure = new PathFigure();
            _pathGeometry = new PathGeometry();
            _path.Data = _pathGeometry;
            _pathGeometry.Figures.Add(_pathFigure);
            _contorno = new PolyLineSegment();
            _contorno.Points.Add(new Point());
            _contorno.Points.Add(new Point());
            _contorno.Points.Add(new Point());
            _contorno.Points.Add(new Point());
            _pathFigure.Segments.Add(_contorno);
            Carregamentos = new ObservableCollection<CarregamentoVisualDoElemento>();
            AtualizarDesenho();
            this.Children.Add(_path);
            Nome = "";

            this.Carregamentos = new ObservableCollection<CarregamentoVisualDoElemento>();
        }
        #endregion

        #region Métodos
        public override Rect RetânguloExterno()
        {
            return new Rect(new Point(_nó1.CoordX, _nó1.CoordY), new Point(_nó2.CoordX, _nó2.CoordY));
        }

        public override IntersectionDetail VerificarIntersecção(Geometry geo)
        {
            if (_nó1.VerificarIntersecção(geo) == IntersectionDetail.FullyInside)
                return IntersectionDetail.Empty;
            if (_nó2.VerificarIntersecção(geo) == IntersectionDetail.FullyInside)
                return IntersectionDetail.Empty;
            return geo.FillContainsWithDetail(_path.RenderedGeometry);
        }

        public override void AtualizarDesenho()
        {
            Vector v = new Vector(Nó2.CoordX - Nó1.CoordX, -(Nó2.CoordY - Nó1.CoordY));
            v.Normalize();
            Vector v2 = new Vector(v.Y, -v.X);
            
            Point A = new Point(Nó1.CoordX, -Nó1.CoordY);
            Point B = new Point(Nó2.CoordX, -Nó2.CoordY);
            _pathFigure.StartPoint = A - v2 * 1;
            
            _contorno.Points[0] = B - v2 * 1;
            _contorno.Points[1] = B + v2 * 1;
            _contorno.Points[2] = A + v2 * 1;
            _contorno.Points[3] = A - v2 * 1;
            foreach (var car in this.Carregamentos)
                car.AtualizarDesenho(this);
        }
        #endregion
    }
}
