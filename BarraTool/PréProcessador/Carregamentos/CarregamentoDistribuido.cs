using BarraTool.Opções;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Nós;
using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace BarraTool.PréProcessador.Carregamentos
{
    public class CarregamentoDistribuido : CarregamentoVisualDoElemento
    {
        private double _cargaX;
        private double _cargaY;
        private bool _sistemaGlobal = false;
        private bool _sistemaLocal = true;
        public double CargaX
        {
            get
            {
                return _cargaX;
            }
            set
            {
                if (_cargaX == value) return;
                _cargaX = value;
                AtualizarDesenho();
                OnPropertyChanged();
            }
        }
        public double CargaY
        {
            get
            {
                return _cargaY;
            }
            set
            {
                if (_cargaY == value) return;
                _cargaY = value;
                AtualizarDesenho();
                OnPropertyChanged();
            }
        }
        public bool SistemaGlobal
        {
            get
            {
                return _sistemaGlobal;
            }
            set
            {
                if (_sistemaGlobal == value) return;
                _sistemaGlobal = value;
                _sistemaLocal = !value;
                OnPropertyChanged();
                OnPropertyChanged("SistemaLocal");
                AtualizarDesenho();
            }
        }
        public bool SistemaLocal
        {
            get
            {
                return _sistemaLocal;
            }
            set
            {
                if (_sistemaLocal == value) return;
                _sistemaLocal = value;
                _sistemaGlobal = !value;
                OnPropertyChanged();
                OnPropertyChanged("SistemaGlobal");
                AtualizarDesenho();
            }
        }
        public CarregamentoDistribuido(double cargaX, double cargaY, Caso caso)
        {
            Nome = "";
            _caso = caso;
            _cargaX = cargaX;
            _cargaY = cargaY;
            ElementoConectadosEDesenhos = new Dictionary<ElementoVisual, Canvas>();
        }
        public override void AtualizarDesenho(ElementoVisual Elemento)
        {
            if (_sistemaLocal == true)
                AtualizarDesenhoSistemaLocal(Elemento);
            else
            {
                AtualizarDesenhoSistemaGlobal(Elemento);
            }
        }
        private void AtualizarDesenhoSistemaGlobal(ElementoVisual Elemento)
        {
            OpçõesVisuaisDosCarregamentos opçõesVisuais = OpçõesVisuaisDosCarregamentos.Instância;
            Canvas canvas = ElementoConectadosEDesenhos[Elemento];
            canvas.Children.Clear();
            NóVisual Nó1 = Elemento.Nó1;
            NóVisual Nó2 = Elemento.Nó2;
            Point P1 = new Point(Nó1.CoordX, -Nó1.CoordY);
            Point P2 = new Point(Nó2.CoordX, -Nó2.CoordY);

            Vector eixo = P2 - P1;
            Vector e1 = eixo / eixo.Length;
            Vector e2 = new Vector(-e1.Y, e1.X);
            Vector ex = new Vector(1, 0);
            Vector ey = new Vector(0, -1);

            Vector eCarga = ex * _cargaX + ey * _cargaY;
            eCarga.Normalize();

            double carga = Math.Sqrt(_cargaX * _cargaX + _cargaY * _cargaY);

            int númeroDeSetas = Math.Max((int)Math.Round(eixo.Length / opçõesVisuais.TamanhoDaCabeçaDaSeta / 4.0) + 1, 3);
            double espaçamento = eixo.Length / (númeroDeSetas - 1);



            int sinal = Math.Sign(e2 * eCarga);
            if (sinal == 0)
                sinal = 1;

            Vector desloc = -sinal * e2 * 4;
            double tamanhoDaSeta = carga / opçõesVisuais.RazãoCargaDistribuidaComprimento;
            Line linha = new Line();

            linha.X1 = (P1 - eCarga * tamanhoDaSeta + desloc).X;
            linha.Y1 = (P1 - eCarga * tamanhoDaSeta + desloc).Y;
            linha.X2 = (P2 - eCarga * tamanhoDaSeta + desloc).X;
            linha.Y2 = (P2 - eCarga * tamanhoDaSeta + desloc).Y;
            linha.StrokeThickness = 0.5;
            linha.Stroke = Brushes.Black;
            canvas.Children.Add(linha);

            for (int i = 0; i < númeroDeSetas; i++)
            {
                double x1 = i * espaçamento;
                double y1 = 0;
                Point começo = P1 + e1 * x1 + desloc;

                Point final = começo - eCarga * tamanhoDaSeta;
                Desenhos.ArrowLine seta = new Desenhos.ArrowLine();
                seta.X2 = começo.X;
                seta.Y2 = começo.Y;

                seta.X1 = final.X;
                seta.Y1 = final.Y;

                seta.ArrowLength = opçõesVisuais.TamanhoDaCabeçaDaSeta;
                seta.ArrowAngle = 36;

                seta.Fill = Brushes.Black;
                seta.Stroke = Brushes.Black;
                seta.StrokeThickness = 0.5;
                canvas.Children.Add(seta);
            }
            TextBlock texto = new TextBlock();
            texto.Text = ManipuladorDeUnidades.ArredondarGeral(ManipuladorDeUnidades.ArredondarGeral(Math.Abs(carga),4) / Caso.ManipuladorDeUnidades.ComprimentoFator * Caso.ManipuladorDeUnidades.ForçaFator)
                + Caso.ManipuladorDeUnidades.ForçaKey + "/" + Caso.ManipuladorDeUnidades.ComprimentoKey;
            texto.Foreground = Brushes.Black;
            texto.FontSize = opçõesVisuais.TamanhoDoTexto;
            texto.TextAlignment = TextAlignment.Center;
            texto.HorizontalAlignment = HorizontalAlignment.Center;

            texto.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            texto.Arrange(new Rect(texto.DesiredSize)); ;

            double wx = texto.ActualWidth;
            double wy = texto.ActualHeight;

            double sin = ex.X * e1.Y - e1.X * ex.Y;
            double cos = ex.X * e1.X + ex.Y * e1.Y;

            double ângulo = Math.Atan2(sin, cos) * (180 / Math.PI);

            if (ângulo < -90 || ângulo > 90)
                ângulo += 180;

            TransformGroup group = new TransformGroup();
            group.Children.Add(new TranslateTransform(-wx / 2, -wy / 2));
            group.Children.Add(new RotateTransform(ângulo));
            texto.RenderTransform = group;

            Point posiçãoTexto = P1 + e1 * (eixo.Length / 2) - eCarga * (tamanhoDaSeta) - sinal * e2 * wy / 2 + desloc;

            Canvas.SetLeft(texto, posiçãoTexto.X);
            Canvas.SetTop(texto, posiçãoTexto.Y);
            canvas.Children.Add(texto);

        }
        private void AtualizarDesenhoSistemaLocal(ElementoVisual Elemento)
        {
            OpçõesVisuaisDosCarregamentos opçõesVisuais = OpçõesVisuaisDosCarregamentos.Instância;

            Canvas canvas = ElementoConectadosEDesenhos[Elemento];
            canvas.Children.Clear();
            NóVisual Nó1 = Elemento.Nó1;
            NóVisual Nó2 = Elemento.Nó2;
            Point P1 = new Point(Nó1.CoordX, -Nó1.CoordY);
            Point P2 = new Point(Nó2.CoordX, -Nó2.CoordY);

            Vector eixo = P2 - P1;
            Vector e1 = eixo / eixo.Length;
            Vector e2 = new Vector(-e1.Y, e1.X);
            Vector ex = new Vector(1, 0);
            Vector ey = new Vector(0, 1);

            int númeroDeSetas = Math.Max((int)Math.Round(eixo.Length / opçõesVisuais.TamanhoDaCabeçaDaSeta / 4.0) + 1,3);
            double espaçamento = eixo.Length / (númeroDeSetas - 1);

            if (_cargaY != 0)
            {
                Vector desloc = Math.Sign(_cargaY) * e2 * 4;
                double tamanhoDaSetaY = _cargaY / opçõesVisuais.RazãoCargaDistribuidaComprimento;
                Line linha = new Line();

                linha.X1 = (P1 + e2 * tamanhoDaSetaY + desloc).X;
                linha.Y1 = (P1 + e2 * tamanhoDaSetaY + desloc).Y;
                linha.X2 = (P2 + e2 * tamanhoDaSetaY + desloc).X;
                linha.Y2 = (P2 + e2 * tamanhoDaSetaY + desloc).Y;
                linha.StrokeThickness = 0.5;
                linha.Stroke = Brushes.Black;
                canvas.Children.Add(linha);

                for (int i = 0; i < númeroDeSetas; i++)
                {
                    double x1 = i * espaçamento;
                    double y1 = 0;
                    Point começo = P1 + e1 * x1 + desloc;

                    Point final = começo + e2 * tamanhoDaSetaY;
                    Desenhos.ArrowLine seta = new Desenhos.ArrowLine();
                    seta.X2 = começo.X;
                    seta.Y2 = começo.Y;

                    seta.X1 = final.X;
                    seta.Y1 = final.Y;

                    seta.ArrowLength = opçõesVisuais.TamanhoDaCabeçaDaSeta;
                    seta.ArrowAngle = 36;

                    seta.Fill = Brushes.Black;
                    seta.Stroke = Brushes.Black;
                    seta.StrokeThickness = 0.5;
                    canvas.Children.Add(seta);
                }
                TextBlock texto = new TextBlock();
                texto.Text = Math.Abs(_cargaY) / Caso.ManipuladorDeUnidades.ComprimentoFator * Caso.ManipuladorDeUnidades.ForçaFator
                    + Caso.ManipuladorDeUnidades.ForçaKey + "/" + Caso.ManipuladorDeUnidades.ComprimentoKey;
                texto.Foreground = Brushes.Black;
                texto.FontSize = opçõesVisuais.TamanhoDoTexto;
                texto.TextAlignment = TextAlignment.Center;
                texto.HorizontalAlignment = HorizontalAlignment.Center;

                texto.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                texto.Arrange(new Rect(texto.DesiredSize)); ;

                double wx = texto.ActualWidth;
                double wy = texto.ActualHeight;

                double sin = ex.X * e1.Y - e1.X * ex.Y;
                double cos = ex.X * e1.X + ex.Y * e1.Y;

                double ângulo = Math.Atan2(sin, cos) * (180 / Math.PI);

                if (ângulo < -90 || ângulo > 90)
                    ângulo += 180;

                TransformGroup group = new TransformGroup();
                group.Children.Add(new TranslateTransform(-wx / 2, -wy / 2));
                group.Children.Add(new RotateTransform(ângulo));
                texto.RenderTransform = group;

                Point posiçãoTexto = P1 + e1 * (eixo.Length / 2) + e2 * (tamanhoDaSetaY + Math.Sign(_cargaY) * wy / 2) + desloc;

                Canvas.SetLeft(texto, posiçãoTexto.X);
                Canvas.SetTop(texto, posiçãoTexto.Y);
                canvas.Children.Add(texto);
            }
            if (CargaX != 0)
            {
                double _cargaY2 = _cargaY;
                if (_cargaY2 == 0)
                    _cargaY2 = 1;

                Vector desloc = -Math.Sign(_cargaY2) * e2 * 4;

                double tamanhoDaSetaX = _cargaX / opçõesVisuais.RazãoCargaDistribuidaComprimento;
                Line linha = new Line();

                linha.X1 = (P1 - e1 * tamanhoDaSetaX + desloc).X;
                linha.Y1 = (P1 - e1 * tamanhoDaSetaX + desloc).Y;
                linha.X2 = (P2 - e1 * tamanhoDaSetaX + desloc).X;
                linha.Y2 = (P2 - e1 * tamanhoDaSetaX + desloc).Y;
                linha.StrokeThickness = 0.5;
                linha.Stroke = Brushes.Black;
                canvas.Children.Add(linha);

                for (int i = 0; i < númeroDeSetas; i++)
                {
                    double x1 = i * espaçamento;
                    double y1 = 0;
                    Point começo = P1 + e1 * x1 + desloc;

                    Point final = começo - e1 * tamanhoDaSetaX;
                    Desenhos.ArrowLine seta = new Desenhos.ArrowLine();
                    seta.X2 = começo.X;
                    seta.Y2 = começo.Y;

                    seta.X1 = final.X;
                    seta.Y1 = final.Y;

                    seta.ArrowLength = opçõesVisuais.TamanhoDaCabeçaDaSeta;
                    seta.ArrowAngle = 36;

                    seta.Fill = Brushes.Black;
                    seta.Stroke = Brushes.Black;
                    seta.StrokeThickness = 0.5;
                    canvas.Children.Add(seta);
                }
                TextBlock texto = new TextBlock();
                texto.Text = Math.Abs(_cargaX) / Caso.ManipuladorDeUnidades.ComprimentoFator * Caso.ManipuladorDeUnidades.ForçaFator
                    + Caso.ManipuladorDeUnidades.ForçaKey + "/" + Caso.ManipuladorDeUnidades.ComprimentoKey;
                texto.Foreground = Brushes.Black;
                texto.FontSize = opçõesVisuais.TamanhoDoTexto;
                texto.TextAlignment = TextAlignment.Center;
                texto.HorizontalAlignment = HorizontalAlignment.Center;

                texto.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                texto.Arrange(new Rect(texto.DesiredSize)); ;

                double wx = texto.ActualWidth;
                double wy = texto.ActualHeight;

                double sin = ex.X * e1.Y - e1.X * ex.Y;
                double cos = ex.X * e1.X + ex.Y * e1.Y;

                double ângulo = Math.Atan2(sin, cos) * (180 / Math.PI);

                if (ângulo < -90 || ângulo > 90)
                    ângulo += 180;

                TransformGroup group = new TransformGroup();
                group.Children.Add(new TranslateTransform(-wx / 2, -wy / 2));
                group.Children.Add(new RotateTransform(ângulo));
                texto.RenderTransform = group;

                Point posiçãoTexto = P1 + e1 * (eixo.Length / 2) - e2 * (Math.Sign(_cargaY2) * wy / 2) + desloc;

                Canvas.SetLeft(texto, posiçãoTexto.X);
                Canvas.SetTop(texto, posiçãoTexto.Y);
                canvas.Children.Add(texto);
            }

        }
        public override void AtualizarDesenho()
        {
            foreach(var elm in ElementoConectadosEDesenhos)
            {
                AtualizarDesenho(elm.Key);
            }
        }
    }
}
