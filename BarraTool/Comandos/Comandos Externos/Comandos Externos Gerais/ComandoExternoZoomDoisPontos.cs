using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoZoomDoisPontos : ICommand
    {
        #region Campos
        public event EventHandler CanExecuteChanged;
        private readonly ModeloVisual modeloVisual;
        private bool[] isSubscribed = new bool[] { false, false, false };
        /*private Action<object, MouseButtonEventArgs> mouseDown;
        private Action<object, MouseButtonEventArgs> mouseUp;
        private Action<object, MouseEventArgs> mouseMove;*/
        
        //Pontos para zoom2Pontos
        private Point zoomA;
        private Point zoomB;

        private Path retangulo_zoom;
        #endregion

        #region Construtores
        public ComandoExternoZoomDoisPontos(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;

            retangulo_zoom = new Path();
            retangulo_zoom.Data = new RectangleGeometry();
            retangulo_zoom.Fill = new SolidColorBrush();
            ((SolidColorBrush)retangulo_zoom.Fill).Color = Colors.Blue;
            retangulo_zoom.Fill.Opacity = 0.2;
            retangulo_zoom.Stroke = Brushes.Blue;
            modeloVisual.ScaleTransform.Changed += ScaleTransform_Changed;
        }
        #endregion

        #region Propriedades
        /*public Action<object, MouseButtonEventArgs> MouseDown { get => mouseDown; set => mouseDown = value; }
        public Action<object, MouseButtonEventArgs> MouseUp { get => mouseUp; set => mouseUp = value; }
        public Action<object, MouseEventArgs> MouseMove { get => mouseMove; set => mouseMove = value; }*/
        #endregion

        #region Métodos
        public bool CanExecute(object parameter)
        {
            return modeloVisual.HabilitarComandos;
        }
        public void Execute(object parameter)
        {
            ZoomDoisPontos();
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        private void ZoomDoisPontos()
        {
            modeloVisual.TipoDeSeleção = TipoDeSeleção.Nenhum;
            modeloVisual.HabilitarComandos = false;
            SubscribeEvent("mouseDown");
        }
        private void zoomDoisPontos_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            zoomA = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(retangulo_zoom);
            (retangulo_zoom.Data as RectangleGeometry).Rect = new Rect(zoomA, zoomA);
            modeloVisual.CaptureMouse();
            SubscribeEvent("mouseMove");
            SubscribeEvent("mouseUp");
        }
        private void zoomDoisPontos_MouseMove(object sender, MouseEventArgs e)
        {
            if (modeloVisual.IsMouseCaptured)
            {
                Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
                (retangulo_zoom.Data as RectangleGeometry).Rect = new Rect(zoomA, atual);
                retangulo_zoom.Fill = new SolidColorBrush();
                ((SolidColorBrush)retangulo_zoom.Fill).Color = Colors.Blue;
                retangulo_zoom.Fill.Opacity = 0.2;
                retangulo_zoom.Stroke = Brushes.Blue;
            }
        }
        private void zoomDoisPontos_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            modeloVisual.ReleaseMouseCapture();

            zoomB = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(retangulo_zoom))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(retangulo_zoom);
            if (zoomA != zoomB)
            {
                modeloVisual.ZoomDoisPontos(new Rect(zoomA, zoomB));
            }
            UnsubscribeEvents();
            modeloVisual.TipoDeSeleção = TipoDeSeleção.ObjetoVisual;
            modeloVisual.HabilitarComandos = true;
        }
        private void SubscribeEvent(string key)
        {
            switch (key)
            {
                case "mouseDown":
                    if (isSubscribed[0] == false)
                        modeloVisual.MouseLeftButtonDown += zoomDoisPontos_MouseLeftDown;
                    isSubscribed[0] = true;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == false)
                        modeloVisual.MouseLeftButtonUp += zoomDoisPontos_MouseLeftUp;
                    isSubscribed[1] = true;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == false)
                        modeloVisual.MouseMove += zoomDoisPontos_MouseMove;
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
                        modeloVisual.MouseLeftButtonDown -= zoomDoisPontos_MouseLeftDown;
                    isSubscribed[0] = false;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == true)
                        modeloVisual.MouseLeftButtonUp -= zoomDoisPontos_MouseLeftUp;
                    isSubscribed[1] = false;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == true)
                        modeloVisual.MouseMove -= zoomDoisPontos_MouseMove;
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
        public void Cancelar()
        {
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(retangulo_zoom))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(retangulo_zoom);
            UnsubscribeEvents();
        }
        private void ScaleTransform_Changed(object sender, EventArgs e)
        {
            retangulo_zoom.StrokeThickness = 1 / modeloVisual.ScaleTransform.ScaleY;
        }
        #endregion
    }
}
