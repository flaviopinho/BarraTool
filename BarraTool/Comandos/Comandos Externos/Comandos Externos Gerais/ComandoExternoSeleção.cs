using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoSeleção : ICommand
    {
        #region Campos
        public event EventHandler CanExecuteChanged;
        private readonly ModeloVisual modeloVisual;
        private bool[] isSubscribed = new bool[] { false, false, false};
        private Point selecaoA;
        private Point selecaoB;
        Path retangulo_selecao;
        #endregion

        #region Construtores
        public ComandoExternoSeleção(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;

            retangulo_selecao = new Path();
            retangulo_selecao.Data = new RectangleGeometry();
            retangulo_selecao.Fill = new SolidColorBrush();
            ((SolidColorBrush)retangulo_selecao.Fill).Color = Colors.Blue;
            retangulo_selecao.Fill.Opacity = 0.2;
            retangulo_selecao.Stroke = Brushes.Blue;
            modeloVisual.ScaleTransform.Changed += ScaleTransform_Changed;
        }
        #endregion

        #region Métodos
        public bool CanExecute(object parameter)
        {

            if (modeloVisual.TipoDeSeleção == TipoDeSeleção.ObjetoVisual)
            {
                return true;
            }
            return false;
        }
        public void Execute(object parameter)
        {
            SelecionarObjetosVisuais();
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        private void SubscribeEvent(string key)
        {
            switch (key)
            {
                case "mouseDown":
                    if (isSubscribed[0] == false)
                        modeloVisual.MouseLeftButtonDown += selecao_MouseLeftDown;
                    isSubscribed[0] = true;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == false)
                        modeloVisual.MouseLeftButtonUp += selecao_MouseLeftUp;
                    isSubscribed[1] = true;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == false)
                        modeloVisual.MouseMove += selecao_MouseMove;
                    isSubscribed[2] = true;
                    break;
            }
        }
        private void UnsubscribeEvent(string key)
        {
            switch (key)
            {
                case "mouseDown":
                    if (isSubscribed[0] == true)
                        modeloVisual.MouseLeftButtonDown -= selecao_MouseLeftDown;
                    isSubscribed[0] = false;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == true)
                        modeloVisual.MouseLeftButtonUp -= selecao_MouseLeftUp;
                    isSubscribed[1] = false;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == true)
                        modeloVisual.MouseMove -= selecao_MouseMove;
                    isSubscribed[2] = false;
                    break;
            }
        }
        private void UnsubscribeEvents()
        {
            UnsubscribeEvent("mouseDown");
            UnsubscribeEvent("mouseUp");
            UnsubscribeEvent("mouseMove");
        }
        private void SelecionarObjetosVisuais()
        {
            SubscribeEvent("mouseDown");
        }
        private void selecao_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            selecaoA = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(retangulo_selecao);
            (retangulo_selecao.Data as RectangleGeometry).Rect = new Rect(selecaoA, selecaoA);
            modeloVisual.CaptureMouse();
            SubscribeEvent("mouseMove");
            SubscribeEvent("mouseUp");
        }
        private void selecao_MouseMove(object sender, MouseEventArgs e)
        {
            if (modeloVisual.IsMouseCaptured && modeloVisual.TipoDeSeleção == TipoDeSeleção.ObjetoVisual)
            {
                Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
                (retangulo_selecao.Data as RectangleGeometry).Rect = new Rect(selecaoA, atual);
                if (atual.X - selecaoA.X >= 0)
                {
                    retangulo_selecao.Fill = new SolidColorBrush();
                    ((SolidColorBrush)retangulo_selecao.Fill).Color = Colors.Blue;
                    retangulo_selecao.Fill.Opacity = 0.2;
                    retangulo_selecao.Stroke = Brushes.Blue;
                }
                else
                {
                    retangulo_selecao.Fill = new SolidColorBrush();
                    ((SolidColorBrush)retangulo_selecao.Fill).Color = Colors.Green;
                    retangulo_selecao.Fill.Opacity = 0.2;
                    retangulo_selecao.Stroke = Brushes.Green;
                }
            }
        }
        private void selecao_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            modeloVisual.ReleaseMouseCapture();

            selecaoB = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            if (selecaoA == selecaoB)
            {
                selecaoB.X = selecaoA.X - 0.1;
            }
            bool selecionarInterseccao;
            if (selecaoB.X - selecaoA.X >= 0)
            {
                selecionarInterseccao = false;
            }
            else
            {
                selecionarInterseccao = true;
            }

            (retangulo_selecao.Data as RectangleGeometry).Rect = new Rect(selecaoA, selecaoB);
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(retangulo_selecao))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(retangulo_selecao);
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                foreach (var no in modeloVisual.Modelo.Nós)
                {
                    IntersectionDetail intersection = no.VerificarIntersecção(retangulo_selecao.Data as RectangleGeometry);
                    if (selecionarInterseccao == false)
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(no, !no.Selecionado);
                        }
                    }
                    else
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.Intersects || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(no, !no.Selecionado);
                        }
                    }
                }
                foreach (var elm in modeloVisual.Modelo.Elementos)
                {
                    IntersectionDetail intersection = elm.VerificarIntersecção(retangulo_selecao.Data as RectangleGeometry);
                    if (selecionarInterseccao == false)
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(elm, !elm.Selecionado);
                        }
                    }
                    else
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.Intersects || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(elm, !elm.Selecionado);
                        }
                    }

                }
            }
            else
            {
                modeloVisual.CancelarSeleção();
                foreach (var no in modeloVisual.Modelo.Nós)
                {
                    IntersectionDetail intersection = no.VerificarIntersecção(retangulo_selecao.Data as RectangleGeometry);
                    if (selecionarInterseccao == false)
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(no, true);
                        }
                    }
                    else
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.Intersects || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(no, true);
                        }
                    }
                }
                foreach (var elm in modeloVisual.Modelo.Elementos)
                {
                    IntersectionDetail intersection = elm.VerificarIntersecção(retangulo_selecao.Data as RectangleGeometry);
                    if (selecionarInterseccao == false)
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(elm, true);
                        }
                    }
                    else
                    {
                        if (intersection == IntersectionDetail.FullyInside || intersection == IntersectionDetail.Intersects || intersection == IntersectionDetail.FullyContains)
                        {
                            modeloVisual.SeleçãoDoObjeto(elm, true);
                        }
                    }
                }
            }
            UnsubscribeEvents();
            Execute(null);
        }
        public void Cancelar()
        {
            UnsubscribeEvents();
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(retangulo_selecao))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(retangulo_selecao);

            //modeloVisual.CancelarSelecao();
        }
        private void ScaleTransform_Changed(object sender, EventArgs e)
        {
            retangulo_selecao.StrokeThickness = 1 / modeloVisual.ScaleTransform.ScaleY;
        }
        #endregion
    }
}
