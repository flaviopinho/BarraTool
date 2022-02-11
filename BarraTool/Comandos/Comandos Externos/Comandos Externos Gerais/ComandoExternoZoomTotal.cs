using BarraTool.PréProcessador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BarraTool.ComandosExternos
{
    public class ComandoExternoZoomTotal : ICommand
    {
        private ModeloVisual modeloVisual;
        public ComandoExternoZoomTotal(ModeloVisual modelo)
        {
            modeloVisual = modelo;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return modeloVisual.HabilitarComandos;
        }

        public void Execute(object parameter)
        {
            ZoomGlobal();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        private void ZoomGlobal()
        {
            if (modeloVisual.Modelo.Nós.Count == 0) return;
            Rect retanguloTotal = modeloVisual.Modelo.Nós[0].RetânguloExterno();

            foreach (ObjetoVisual elm in modeloVisual.Modelo.Nós)
            {
                Rect retangulo = elm.RetânguloExterno();

                Point A = retangulo.Location;
                Point B = new Point(A.X + retangulo.Width, A.Y + retangulo.Height);

                Point a = retanguloTotal.Location;
                Point b = new Point(a.X + retanguloTotal.Width, a.Y + retanguloTotal.Height);

                retanguloTotal.X = Math.Min(A.X, a.X);
                retanguloTotal.Y = Math.Min(A.Y, a.Y);

                retanguloTotal.Width = Math.Max(B.X, b.X) - retanguloTotal.X;
                retanguloTotal.Height = Math.Max(B.Y, b.Y) - retanguloTotal.Y;
            }

            modeloVisual.ZoomDoisPontos(retanguloTotal);
        }
    }
}
