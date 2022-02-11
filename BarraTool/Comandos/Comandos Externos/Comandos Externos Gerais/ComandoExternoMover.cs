using BarraTool.PréProcessador;
using BarraTool.PréProcessador.Nós;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BarraTool.ComandosInternos;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoMover : ICommand
    {
        #region Campos
        public event EventHandler CanExecuteChanged;
        private readonly ModeloVisual modeloVisual;
        private bool[] isSubscribed = new bool[] { false, false, false, false, false };

        //Comando mover objetos
        private Point moverA;
        private Point moverB;
        PontoAPontoBLinha linha_mover;
        #endregion

        #region Construtores
        public ComandoExternoMover(ModeloVisual modeloVisual)
        {
            this.modeloVisual = modeloVisual;

            linha_mover = new PontoAPontoBLinha(modeloVisual.ScaleTransform, modeloVisual.ManipuladorDeUnidades);
        }
        #endregion

        #region Métodos
        public bool CanExecute(object parameter)
        {
            int numeroDeElementosSelecionados = modeloVisual.NúmeroDeElementosENósSelecionados;
            if (modeloVisual.HabilitarComandos == true && numeroDeElementosSelecionados > 0)
            {
                return true;
            }
            return false;
        }
        public void Execute(object parameter)
        {
            Mover();
        }

        public void Mover()
        {
            modeloVisual.HabilitarComandos = false;
            modeloVisual.TipoDeSeleção = TipoDeSeleção.Nenhum;
            SubscribeEvent("mouseDown1");
            SubscribeEvent("mouseMove");
            linha_mover.Ponto1Inserido = false;
            modeloVisual.ObjetosParaExecuçãoDeComandos.Add(linha_mover);
        }
        private void moverObjetos_MouseLeftDownPonto1(object sender, MouseButtonEventArgs e)
        {
            modeloVisual.CaptureMouse();
            moverA = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            linha_mover.PontoB = moverA;
            linha_mover.PontoA = moverA;
            linha_mover.Ponto1Inserido = true;
            SubscribeEvent("mouseUp1");
        }
        private void moverObjetos_MouseMove(object sender, MouseEventArgs e)
        {
            Point atual = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);
            bool orto = (modeloVisual.Ortogonal ^ (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)));
            if (orto && linha_mover.Ponto1Inserido)
            {
                double wx = Math.Abs(atual.X - moverA.X);
                double wy = Math.Abs(atual.Y - moverA.Y);
                if (wx > wy)
                {
                    linha_mover.PontoB = new Point(atual.X, moverA.Y);
                }
                else
                {
                    linha_mover.PontoB = new Point(moverA.X, atual.Y);
                }
            }
            else
            {
                linha_mover.PontoB = atual;
            }
        }
        private void moverObjetos_MouseLeftUpPonto1(object sender, MouseButtonEventArgs e)
        {
            UnsubscribeEvent("mouseDown1");
            SubscribeEvent("mouseDown2");
        }
        private void moverObjetos_MouseLeftDownPonto2(object sender, MouseButtonEventArgs e)
        {
            moverB = e.GetPosition(modeloVisual.CanvasManipulaçãoDeObjetos);

            bool orto = (modeloVisual.Ortogonal ^ (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)));
            if (orto)
            {
                double wx = Math.Abs(moverB.X - moverA.X);
                double wy = Math.Abs(moverB.Y - moverA.Y);
                if (wx > wy)
                {
                    linha_mover.PontoB = new Point(moverB.X, moverA.Y);
                }
                else
                {
                    linha_mover.PontoB = new Point(moverA.X, moverB.Y);
                }
            }
            else
            {
                linha_mover.PontoB = moverB;
            }
            UnsubscribeEvent("mouseMove");
            UnsubscribeEvent("mouseUp1");
            SubscribeEvent("mouseUp2");
        }
        private void moverObjetos_MouseLeftUpPonto2(object sender, MouseButtonEventArgs e)
        {
            modeloVisual.ReleaseMouseCapture();
            modeloVisual.ComandoExternoEnter.ExecuteMethod = moverConfirmar;
            modeloVisual.RequerConfirmação = true;
            UnsubscribeEvent("mouseDown2");
            UnsubscribeEvent("mouseUp2");
        }
        private void moverConfirmar(object sender)
        {
            ComandoInternoMoverLista<NóVisual> comando = new ComandoInternoMoverLista<NóVisual>(modeloVisual.NósSelecionados);
            comando.DeltaX = linha_mover.DeltaX;
            comando.DeltaY = linha_mover.DeltaY;
            foreach(var elm in modeloVisual.ElementosSelecionados)
            {
                if (elm.Nó1.Selecionado == false)
                    comando.ListaDeNos.Add(elm.Nó1);
                if (elm.Nó2.Selecionado == false)
                    comando.ListaDeNos.Add(elm.Nó2);
            }
            modeloVisual.ComandosInternos.AdicionarComando(comando);

            //modeloVisual.CancelarSelecao();
            modeloVisual.TipoDeSeleção = TipoDeSeleção.ObjetoVisual;
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(linha_mover))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(linha_mover);

            modeloVisual.HabilitarComandos = true;
            modeloVisual.RequerConfirmação = false;
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
            switch(key)
            {
                case "mouseDown1":
                    if (isSubscribed[0] == false)
                        modeloVisual.MouseLeftButtonDown += moverObjetos_MouseLeftDownPonto1;
                    isSubscribed[0] = true;
                    break;
                case "mouseDown2":
                    if (isSubscribed[1] == false)
                        modeloVisual.MouseLeftButtonDown += moverObjetos_MouseLeftDownPonto2;
                    isSubscribed[1] = true;
                    break;
                case "mouseUp1":
                    if (isSubscribed[2] == false)
                        modeloVisual.MouseLeftButtonUp += moverObjetos_MouseLeftUpPonto1;
                    isSubscribed[2] = true;
                    break;
                case "mouseUp2":
                    if (isSubscribed[3] == false)
                        modeloVisual.MouseLeftButtonUp += moverObjetos_MouseLeftUpPonto2;
                    isSubscribed[3] = true;
                    break;
                case "mouseMove":
                    if(isSubscribed[4]==false)
                        modeloVisual.MouseMove += moverObjetos_MouseMove;
                    isSubscribed[4] = true;
                    break;
                default:
                    throw new Exception("Key Inválida");
            }
        }
        private void UnsubscribeEvent(string key)
        {
            switch (key)
            {
                case "mouseDown1":
                    if(isSubscribed[0] == true)
                        modeloVisual.MouseLeftButtonDown -= moverObjetos_MouseLeftDownPonto1;
                    isSubscribed[0] = false;
                    break;
                case "mouseDown2":
                    if (isSubscribed[1] == true)
                        modeloVisual.MouseLeftButtonDown -= moverObjetos_MouseLeftDownPonto2;
                    isSubscribed[1] = false;
                    break;
                case "mouseUp1":
                    if (isSubscribed[2] == true)
                        modeloVisual.MouseLeftButtonUp -= moverObjetos_MouseLeftUpPonto1;
                    isSubscribed[2] = false;
                    break;
                case "mouseUp2":
                    if (isSubscribed[3] == true)
                        modeloVisual.MouseLeftButtonUp -= moverObjetos_MouseLeftUpPonto2;
                    isSubscribed[3] = false;
                    break;
                case "mouseMove":
                    if (isSubscribed[4] == true)
                        modeloVisual.MouseMove -= moverObjetos_MouseMove;
                    isSubscribed[4] = false;
                    break;
                default:
                    throw new Exception("Key Inválida");
            }
        }
        private void UnsubscribeEvents()
        {
            UnsubscribeEvent("mouseDown1");
            UnsubscribeEvent("mouseDown2");
            UnsubscribeEvent("mouseUp1");
            UnsubscribeEvent("mouseUp2");
            UnsubscribeEvent("mouseMove");
        }

        public void Cancelar()
        {
            UnsubscribeEvents();
            if (modeloVisual.ObjetosParaExecuçãoDeComandos.Contains(linha_mover))
                modeloVisual.ObjetosParaExecuçãoDeComandos.Remove(linha_mover);
        }
        #endregion
    }

}
