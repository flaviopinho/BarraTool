using BarraTool.PréProcessador.Elementos;
using BarraTool.Unidades;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarraTool.PréProcessador.Carregamentos
{
    public class FábricaDeCarregamentos : INotifyPropertyChanged
    {
        #region Campos
        private ObservableCollection<Caso> _casos;
        private ObservableCollection<Combinação> _combinações;
        private ObservableCollection<CarregamentoVisualDoElemento> _carregamentosDosElementos;
        private ObservableCollection<CarregamentoVisualDoNó> _carregamentosDosNós;

        private ReadOnlyObservableCollection<Caso> _casos2;
        private ReadOnlyObservableCollection<Combinação> _combinações2;
        private ReadOnlyObservableCollection<CarregamentoVisualDoElemento> _carregamentosDosElementos2;
        private ReadOnlyObservableCollection<CarregamentoVisualDoNó> _carregamentosDosNós2;

        private ManipuladorDeUnidades _manipuladorDeUnidades;

        int contador_de_casos = 0;
        int contador_de_combinações = 0;
        int contador_de_carregamentos = 0;

        private Caso casoAtual;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Propriedades
        public Caso CasoAtual
        {
            get => casoAtual;
            set
            {
                if (casoAtual == value) return;
                casoAtual = value;
                casoAtual.AtualizarDesenho();
                OnPropertyChanged();
            }
        }
        public ReadOnlyObservableCollection<Caso> Casos { get => _casos2; }
        public ReadOnlyObservableCollection<Combinação> Combinações { get => _combinações2; }
        public ReadOnlyObservableCollection<CarregamentoVisualDoElemento> CarregamentosDosElementos { get => _carregamentosDosElementos2; }
        public ReadOnlyObservableCollection<CarregamentoVisualDoNó> CarregamentosDosNós { get => _carregamentosDosNós2; }
        #endregion

        #region Construtores
        public FábricaDeCarregamentos(ManipuladorDeUnidades manipuladorDeUnidades)
        {
            _casos = new ObservableCollection<Caso>();
            _combinações = new ObservableCollection<Combinação>();
            _carregamentosDosElementos = new ObservableCollection<CarregamentoVisualDoElemento>();
            _carregamentosDosNós = new ObservableCollection<CarregamentoVisualDoNó>();

            _casos2 = new ReadOnlyObservableCollection<Caso>(_casos);
            _combinações2 = new ReadOnlyObservableCollection<Combinação>(_combinações);
            _carregamentosDosElementos2 = new ReadOnlyObservableCollection<CarregamentoVisualDoElemento>(_carregamentosDosElementos);
            _carregamentosDosNós2 = new ReadOnlyObservableCollection<CarregamentoVisualDoNó>(_carregamentosDosNós);

            _manipuladorDeUnidades = manipuladorDeUnidades;

            InserirCaso(new Caso(_manipuladorDeUnidades));
            CasoAtual = Casos[0];
        }
        #endregion

        #region Métodos
        public void InserirCaso(Caso caso)
        {
            if (caso.Nome == "")
                caso.Nome = GeradorDeNomesDeCasos();
            _casos.Add(caso);
        }
        public void DeletarCaso(Caso caso)
        {
            _casos.Remove(caso);
            for (int i = caso.Carregamentos.Count - 1; i >= 0; i--)
            {
                DeletarCarregamento((CarregamentoVisualDoElemento)caso.Carregamentos[i]);
            }
        }
        public void InserirCarregamento(CarregamentoVisualDoElemento carregamento)
        {
            if (carregamento.Nome == "")
                carregamento.Nome = GeradorDeNomesDeCarregamentos();
            _carregamentosDosElementos.Add(carregamento);
            carregamento.Caso.Carregamentos.Add(carregamento);
        }
        public void DeletarCarregamento(CarregamentoVisualDoElemento carregamento)
        {
            _carregamentosDosElementos.Remove(carregamento);
            carregamento.Caso.Carregamentos.Remove(carregamento);
            foreach (var elm in carregamento.ElementoConectadosEDesenhos)
                elm.Key.Carregamentos.Remove(carregamento);
        }
        public void AplicarCarregamento(CarregamentoVisualDoElemento carregamento, ElementoVisual elemento)
        {
            elemento.Carregamentos.Add(carregamento);
            carregamento.ElementoConectadosEDesenhos.Add(elemento, new System.Windows.Controls.Canvas());
            carregamento.Children.Add(carregamento.ElementoConectadosEDesenhos[elemento]);
            elemento.AtualizarDesenho();
        }
        public void RemoverCarregamento(CarregamentoVisualDoElemento carregamento, ElementoVisual elemento)
        {
            carregamento.ElementoConectadosEDesenhos[elemento].Children.Clear();
            carregamento.Children.Remove(carregamento.ElementoConectadosEDesenhos[elemento]);
            carregamento.ElementoConectadosEDesenhos.Remove(elemento);
            elemento.Carregamentos.Remove(carregamento);
        }
        public void AplicarCarregamento(CarregamentoVisualDoElemento carregamento, List<ElementoVisual> elementos)
        {
            foreach (var elemento in elementos)
            {
                if (!carregamento.ElementoConectadosEDesenhos.ContainsKey(elemento))
                {
                    elemento.Carregamentos.Add(carregamento);
                    carregamento.ElementoConectadosEDesenhos.Add(elemento, new System.Windows.Controls.Canvas());
                    carregamento.Children.Add(carregamento.ElementoConectadosEDesenhos[elemento]);
                    elemento.AtualizarDesenho();
                }
            }
        }
        public void RemoverCarregamento(CarregamentoVisualDoElemento carregamento, List<ElementoVisual> elementos)
        {
            foreach (var elemento in elementos)
            {
                if (carregamento.ElementoConectadosEDesenhos.ContainsKey(elemento))
                {
                    carregamento.ElementoConectadosEDesenhos[elemento].Children.Clear();
                    carregamento.Children.Remove(carregamento.ElementoConectadosEDesenhos[elemento]);
                    carregamento.ElementoConectadosEDesenhos.Remove(elemento);
                    elemento.Carregamentos.Remove(carregamento);
                    elemento.AtualizarDesenho();
                }
            }
        }
        private string GeradorDeNomesDeCasos()
        {
            contador_de_casos++;
            return "Caso " + contador_de_casos;
        }
        private string GeradorDeNomesDeCombinações()
        {
            contador_de_combinações++;
            return "Combinações " + contador_de_combinações;
        }
        private string GeradorDeNomesDeCarregamentos()
        {
            contador_de_carregamentos++;
            return "Carregamento " + contador_de_carregamentos;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
