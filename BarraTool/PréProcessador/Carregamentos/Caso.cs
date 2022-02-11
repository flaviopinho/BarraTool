using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace BarraTool.PréProcessador.Carregamentos
{
    public class Caso: INome
    {
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        public string Nome { get; set; }
        public string Descrição { get; set; }
        public ObservableCollection<CarregamentoVisual> Carregamentos { get; set; }
        public ManipuladorDeUnidades ManipuladorDeUnidades { get=>_manipuladorDeUnidades; }
        public Caso(ManipuladorDeUnidades manipuladorDeUnidades)
        {
            _manipuladorDeUnidades = manipuladorDeUnidades;
            Nome = "";
            Carregamentos = new ObservableCollection<CarregamentoVisual>();
            _manipuladorDeUnidades.PropertyChanged += manipuladorDeUnidades_PropertyChanged;
        }
        private void manipuladorDeUnidades_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AtualizarDesenho();
        }
        public void AtualizarDesenho()
        {
            foreach (var car in Carregamentos)
                car.AtualizarDesenho();
        }
    }
}
