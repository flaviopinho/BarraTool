using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BarraTool.ComandosInternos;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoInserirElemento : ICommand
    {
        private ModeloVisual modeloVisual;

        private bool circuloAAdicionado = false;
        private bool circuloBAdicionado = false;
        private bool linhaAdicionado = false;

        private bool capturaDoMouse = false;

        private bool MouseDown1 = false;
        private bool MouseMove1 = false;
        private bool MouseUp1 = false;

        private bool MouseDown2 = false;
        private bool MouseMove2 = false;
        private bool MouseUp2 = false;

        private Path pathA;
        private Path pathB;
        private Path pathC;
        private EllipseGeometry circuloA;
        private EllipseGeometry circuloB;
        LineGeometry linha;
        NóVisual noA;
        NóVisual noB;

        private bool noANovo = true;
        private bool noBNovo = true;

        ElementoVisual divididoA;
        ElementoVisual divididoB;

        public ComandoExternoInserirElemento(ModeloVisual modelo)
        {
            this.modeloVisual = modelo;
            circuloA = new EllipseGeometry();
            circuloA.RadiusX = 1;
            circuloA.RadiusY = 1;
            pathA = new Path();
            pathA.Data = circuloA;
            pathA.Fill = Brushes.Red;

            circuloB = new EllipseGeometry();
            circuloB.RadiusX = 1;
            circuloB.RadiusY = 1;
            pathB = new Path();
            pathB.Data = circuloB;
            pathB.Fill = Brushes.Red;

            linha = new LineGeometry();
            pathC = new Path();
            pathC.Data = linha;
            pathC.Stroke = Brushes.Red;

            modelo.ScaleTransform.Changed += AlterarEscalaDoDesenho;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return modeloVisual.HabilitarComandos;
        }
        public void Execute(object parameter)
        {
            Inicio();
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }


        private void Inicio()
        {
            modeloVisual.HabilitarComandos = false;
            modeloVisual.TipoDeSeleção = TipoDeSeleção.Nenhum;
            //
            capturaDoMouse = true;
            modeloVisual.CaptureMouse();
            //
            MouseMove1 = true;
            modeloVisual.MouseMove += MoverMouse1;
            //
            circuloAAdicionado = true;
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(pathA);
            //
            MouseDown1 = true;
            modeloVisual.MouseLeftButtonDown += CapturarPrimeiroPonto;
            //
            MouseUp1 = true;
            modeloVisual.MouseLeftButtonUp += LiberarPrimeiroPonto;
        }
        private void MoverMouse1(object sender, MouseEventArgs e)
        {
            Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            circuloA.Center = atual;
        }
        private void CapturarPrimeiroPonto(object sender, MouseEventArgs e)
        {
            circuloA.Center = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            circuloB.Center = circuloA.Center;
            divididoA = null;
            foreach(var no in modeloVisual.Modelo.Nós)
            {
                IntersectionDetail inters= no.VerificarIntersecção(circuloA);
                if(inters!=IntersectionDetail.Empty)
                {
                    noA = no;
                    noANovo = false;
                    circuloA.Center = new Point(noA.CoordX, -noA.CoordY);

                    Meio();
                    return;
                }
            }
            foreach (var elm in modeloVisual.Modelo.Elementos)
            {
                IntersectionDetail inters = elm.VerificarIntersecção(circuloA);
                if (inters != IntersectionDetail.Empty)
                {
                    Point Ponto1 = new Point(elm.Nó1.CoordX, -elm.Nó1.CoordY);
                    Point Ponto2 = new Point(elm.Nó2.CoordX, -elm.Nó2.CoordY);

                    Vector V1 = Ponto2 - Ponto1;
                    Vector V2 = circuloA.Center - Ponto1;
                    Vector V3 = (V1 * V2) / (V1 * V1) * V1;

                    Point Final = Ponto1 + V3;
                    circuloA.Center = Final;

                    noA = new NóVisual(circuloA.Center.X, -circuloA.Center.Y);
                    noANovo = true;
                    divididoA = elm;
                    Meio();
                    return;
                }
            }
            noA = new NóVisual(circuloA.Center.X, -circuloA.Center.Y);
            noANovo = true;

            Meio();
        }
        private void MoverMouse2(object sender, MouseEventArgs e)
        {
            Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            bool orto = (modeloVisual.Ortogonal ^ (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)));
            if(orto)
            {
                double wx = Math.Abs(atual.X - circuloA.Center.X);
                double wy = Math.Abs(atual.Y - circuloA.Center.Y);
                if (wx > wy)
                {
                    circuloB.Center = new Point(atual.X, circuloA.Center.Y);
                    linha.EndPoint = circuloB.Center;
                }
                else
                {
                    circuloB.Center = new Point(circuloA.Center.X, atual.Y);
                    linha.EndPoint = circuloB.Center;
                }
            }
            else
            {
                circuloB.Center = atual;
                linha.EndPoint = atual;
            }
        }
        private void Meio()
        {
            linha.StartPoint = circuloA.Center;
            linha.EndPoint = circuloB.Center;

            circuloBAdicionado = true;
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(pathB);

            linhaAdicionado = true;
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(pathC);

            MouseMove1 = false;
            modeloVisual.MouseMove -= MoverMouse1;
            
            MouseMove2 = true;
            modeloVisual.MouseMove += MoverMouse2;
            
        }
        private void LiberarPrimeiroPonto(object sender, MouseButtonEventArgs e)
        {
            MouseDown1 = false;
            modeloVisual.MouseLeftButtonDown -= CapturarPrimeiroPonto;
            
            MouseDown2 = true;
            modeloVisual.MouseLeftButtonDown += CapturarSegundoPonto;

            MouseUp1 = false;
            modeloVisual.MouseLeftButtonUp -= LiberarPrimeiroPonto;

            MouseUp2 = true;
            modeloVisual.MouseLeftButtonUp += LiberarSegundoPonto;
        }
        private void CapturarSegundoPonto(object sender, MouseEventArgs e)
        {
            MouseMove2 = false;
            modeloVisual.MouseMove -= MoverMouse2;

            divididoB = null;

            var atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);

            bool orto = (modeloVisual.Ortogonal ^ (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)));
            if (orto)
            {
                double wx = Math.Abs(atual.X - circuloA.Center.X);
                double wy = Math.Abs(atual.Y - circuloA.Center.Y);
                if (wx > wy)
                {
                    circuloB.Center = new Point(atual.X, circuloA.Center.Y);
                    linha.EndPoint = circuloB.Center;
                }
                else
                {
                    circuloB.Center = new Point(circuloA.Center.X, atual.Y);
                    linha.EndPoint = circuloB.Center;
                }
            }
            else
            {
                circuloB.Center = atual;
                linha.EndPoint = atual;
            }

            foreach (var no in modeloVisual.Modelo.Nós)
            {
                IntersectionDetail inters = no.VerificarIntersecção(circuloB);
                if (inters != IntersectionDetail.Empty)
                {
                    noB = no;
                    noBNovo = false;
                    circuloB.Center = new Point(noB.CoordX, -noB.CoordY);
                    return;
                }
            }
            foreach (var elm in modeloVisual.Modelo.Elementos)
            {
                IntersectionDetail inters = elm.VerificarIntersecção(circuloB);
                if (inters != IntersectionDetail.Empty)
                {
                    Point Ponto1 = new Point(elm.Nó1.CoordX, -elm.Nó1.CoordY);
                    Point Ponto2 = new Point(elm.Nó2.CoordX, -elm.Nó2.CoordY);

                    Vector V1 = Ponto2 - Ponto1;
                    Vector V2 = circuloB.Center - Ponto1;
                    Vector V3 = (V1 * V2) / (V1 * V1) * V1;

                    Point Final = Ponto1 + V3;
                    circuloB.Center = Final;

                    noB = new NóVisual(circuloB.Center.X, -circuloB.Center.Y);
                    noBNovo = true;
                    divididoB = elm;
                    return;
                }
            }
            noB = new NóVisual(circuloB.Center.X, -circuloB.Center.Y);
            noBNovo = true;
        }
        private void LiberarSegundoPonto(object sender, MouseButtonEventArgs e)
        {

            MouseDown2 = false;
            modeloVisual.MouseLeftButtonDown -= CapturarSegundoPonto;


            MouseUp2 = false;
            modeloVisual.MouseLeftButtonUp -= LiberarSegundoPonto;

            Fim();
        }
        private void Fim()
        {
            if (linhaAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathC);
            linhaAdicionado = false;
            if (circuloAAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathA);
            circuloAAdicionado = false;
            if (circuloBAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathB);
            circuloBAdicionado = false;

            ComandoInternoComposto comandoComposto = new ComandoInternoComposto();

            if (noANovo == true)
            {
                comandoComposto.AdicionarComando(new ComandoInternoInserirNó(noA, modeloVisual.Modelo));
            }
            if (noBNovo == true)
                comandoComposto.AdicionarComando(new ComandoInternoInserirNó(noB, modeloVisual.Modelo));
            if (divididoA != null && divididoA == divididoB)
            {
                throw new Exception();
            }
            if (divididoA != null)
            {
                comandoComposto.AdicionarComando(new ComandoInternoDividirElemento(divididoA, noA, modeloVisual));
            }
            if (divididoB != null)
            {
                comandoComposto.AdicionarComando(new ComandoInternoDividirElemento(divididoB, noB, modeloVisual));
            }
            ElementoVisual elm = new ElementoVisual(noA, noB, modeloVisual.Modelo.SeçãoAtual);
            comandoComposto.AdicionarComando(new ComandoInternoInserirElemento(elm, modeloVisual.Modelo));
            modeloVisual.ComandosInternos.AdicionarComando(comandoComposto);
            noA = null;
            noB = null;
            divididoA = null;
            divididoB = null;

            modeloVisual.HabilitarComandos = true;
            modeloVisual.TipoDeSeleção = TipoDeSeleção.ObjetoVisual;
            
            modeloVisual.ReleaseMouseCapture();
            capturaDoMouse = false;

            this.Execute(null);
        }


        private void AlterarEscalaDoDesenho(object sender, EventArgs e)
        {
            circuloA.RadiusX = 2 / modeloVisual.ScaleTransform.ScaleX;
            circuloA.RadiusY = 2 / modeloVisual.ScaleTransform.ScaleY;
            circuloB.RadiusX = 2 / modeloVisual.ScaleTransform.ScaleX;
            circuloB.RadiusY = 2 / modeloVisual.ScaleTransform.ScaleY;
            pathC.StrokeThickness = 1.5 / modeloVisual.ScaleTransform.ScaleY;
        }
        public void Cancelar()
        {
            if (capturaDoMouse == true)
                modeloVisual.ReleaseMouseCapture();
            capturaDoMouse = false;
            if (MouseDown1 == true)
                modeloVisual.MouseLeftButtonDown -= CapturarPrimeiroPonto;
            MouseDown1 = false;
            if (MouseMove1 == true)
                modeloVisual.MouseMove -= MoverMouse1;
            MouseMove1 = false;
            if (MouseUp1 == true)
                modeloVisual.MouseLeftButtonUp -= LiberarPrimeiroPonto;
            MouseUp1 = false;
            if (MouseDown1==true)
                modeloVisual.MouseLeftButtonDown -= CapturarSegundoPonto;
            MouseDown1 = false;
            if (MouseMove2 == true)
                modeloVisual.MouseMove -= MoverMouse2;
            MouseMove2 = false;
            if (MouseUp2 == true)
                modeloVisual.MouseLeftButtonUp -= LiberarSegundoPonto;
            MouseUp2 = false;

            if (linhaAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathC);
            linhaAdicionado = false;
            if (circuloAAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathA);
            circuloAAdicionado = false;
            if (circuloBAdicionado == true)
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(pathB);
            circuloBAdicionado = false;

            noA = null;
            noB = null;
            divididoA = null;
            divididoB = null;
        }
    }
}
