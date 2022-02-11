using BarraTool.ComandosInternos;
using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoInserirNó : ICommand
    {
        #region Campos
        public event EventHandler CanExecuteChanged;
        private readonly ModeloVisual modeloVisual;
        private bool[] isSubscribed = new bool[] { false, false, false };
        UserContralInserirNo userControlInserirNo;
        #endregion

        #region Construtores
        public ComandoExternoInserirNó(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;
            userControlInserirNo = new UserContralInserirNo(modeloVisual.ScaleTransform, modeloVisual.ManipuladorDeUnidades);
        }
        #endregion

        #region Métodos
        public bool CanExecute(object parameter)
        {
            return modeloVisual.HabilitarComandos;
        }
        public void Execute(object parameter)
        {
            InserirNo();
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
                        modeloVisual.MouseLeftButtonDown += inserirNo_MouseLeftDown;
                    isSubscribed[0] = true;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == false)
                        modeloVisual.MouseLeftButtonUp += inserirNo_MouseLeftUp;
                    isSubscribed[1] = true;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == false)
                        modeloVisual.MouseMove += inserirNo_MouseMove;
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
                        modeloVisual.MouseLeftButtonDown -= inserirNo_MouseLeftDown;
                    isSubscribed[0] = false;
                    break;
                case "mouseUp":
                    if (isSubscribed[1] == true)
                        modeloVisual.MouseLeftButtonUp -= inserirNo_MouseLeftUp;
                    isSubscribed[1] = false;
                    break;
                case "mouseMove":
                    if (isSubscribed[2] == true)
                        modeloVisual.MouseMove -= inserirNo_MouseMove;
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
        private void InserirNo()
        {
            modeloVisual.TipoDeSeleção = TipoDeSeleção.Nenhum;
            modeloVisual.HabilitarComandos = false;
            SubscribeEvent("mouseDown");
            SubscribeEvent("mouseMove");
        }
        private void inserirNo_MouseMove(object sender, MouseEventArgs e)
        {
            modeloVisual.CaptureMouse();
            Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            userControlInserirNo.Ponto = atual;
            if (!modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(userControlInserirNo))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Add(userControlInserirNo);
            modeloVisual.HabilitarComandos = false;
        }
        private void inserirNo_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            SubscribeEvent("mouseUp");
            Point ponto = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            userControlInserirNo.Ponto = ponto;

            ComandoInternoComposto comandoComposto = new ComandoInternoComposto();

            NóVisual noVisual = new NóVisual(userControlInserirNo.Ponto.X, -userControlInserirNo.Ponto.Y);

            foreach (var no in modeloVisual.Modelo.Nós)
            {
                IntersectionDetail inters = no.VerificarIntersecção(userControlInserirNo.Circulo);
                if (inters != IntersectionDetail.Empty)
                {
                    return;
                }
            }
            
            comandoComposto.AdicionarComando(new ComandoInternoInserirNó(noVisual, modeloVisual.Modelo));

            foreach (var elm in modeloVisual.Modelo.Elementos)
            {
                IntersectionDetail inters = elm.VerificarIntersecção(userControlInserirNo.Circulo);
                if (inters != IntersectionDetail.Empty)
                {
                    Point Ponto1 = new Point(elm.Nó1.CoordX, -elm.Nó1.CoordY);
                    Point Ponto2 = new Point(elm.Nó2.CoordX, -elm.Nó2.CoordY);

                    Vector V1 = Ponto2 - Ponto1;
                    Vector V2 = ponto - Ponto1;
                    Vector V3 = (V1 * V2) / (V1 * V1) * V1;

                    Point Final = Ponto1 + V3;
                    userControlInserirNo.Ponto = Final;

                    noVisual.CoordX = userControlInserirNo.Ponto.X;
                    noVisual.CoordY = -userControlInserirNo.Ponto.Y;

                    comandoComposto.AdicionarComando(new ComandoInternoDividirElemento(elm, noVisual, modeloVisual));
                }
            }

            modeloVisual.ComandosInternos.AdicionarComando(comandoComposto);
        }
        private void inserirNo_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            UnsubscribeEvents();
            Point ponto = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            userControlInserirNo.Ponto = ponto;
            Execute(null);
        }
        public void Cancelar()
        {
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(userControlInserirNo))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(userControlInserirNo);
            UnsubscribeEvents();
        }
        #endregion
    }
}
