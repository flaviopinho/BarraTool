using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Materiais;
using BarraTool.PréProcessador.Nós;
using BarraTool.PréProcessador.Seções;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarraTool.PréProcessador
{
    public class FábricaDeElementosENós : INotifyPropertyChanged
    {
        #region Campos
        private int _númeroDeNosInseridos = 0;
        private int _númeroDeElementosInseridos = 0;
        private int _númeroDeMateriaisInseridos = 0;
        private int _númeroDeSeçõesInseridas = 0;
        private ObservableCollection<NóVisual> _nós;
        private ReadOnlyObservableCollection<NóVisual> _nós2;
        private ObservableCollection<ElementoVisual> _elementos;
        private ReadOnlyObservableCollection<ElementoVisual> _elementos2;
        private ObservableCollection<Materiais.Material> _materiais;
        private ReadOnlyObservableCollection<Materiais.Material> _materiais2;
        private ObservableCollection<Seções.Seção> _seções;
        private ReadOnlyObservableCollection<Seções.Seção> _seções2;
        private Seções.Seção _seçãoAtual;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Propriedades
        public ReadOnlyObservableCollection<NóVisual> Nós { get => _nós2;}
        public ReadOnlyObservableCollection<ElementoVisual> Elementos { get => _elementos2;  }
        public ReadOnlyObservableCollection<Materiais.Material> Materiais { get => _materiais2;  }
        public ReadOnlyObservableCollection<Seções.Seção> Seções { get => _seções2; }
        public Seções.Seção SeçãoAtual
        {
            get => _seçãoAtual;
            set
            {
                if (_seçãoAtual == value) return;
                if (_seçãoAtual != null)
                    _seçãoAtual.SeçãoAtual = false;
                _seçãoAtual = value;
                _seçãoAtual.SeçãoAtual = true;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Construtores
        public FábricaDeElementosENós()
        {
            _nós = new ObservableCollection<NóVisual>();
            _elementos = new ObservableCollection<ElementoVisual>();
            _materiais = new ObservableCollection<Materiais.Material>();
            _seções = new ObservableCollection<Seções.Seção>();

            _nós2 = new ReadOnlyObservableCollection<NóVisual>(_nós);
            _elementos2 = new ReadOnlyObservableCollection<ElementoVisual>(_elementos);
            _materiais2 = new ReadOnlyObservableCollection<Materiais.Material>(_materiais);
            _seções2 = new ReadOnlyObservableCollection<Seções.Seção>(_seções);

            _materiais.Add(new Materiais.Material());
            _materiais.Add(new Materiais.Material());
            _materiais[0].Nome = GerarNovoNomeDeMaterial();
            _materiais[1].Nome = GerarNovoNomeDeMaterial();


            _seções.Add(new Seções.Seção { Material = _materiais[0], GeometriaDaSeção = new SeçãoRetangular { Hx = 15, Hy = 50 } });
            _seções.Add(new Seções.Seção { Material = _materiais[1], GeometriaDaSeção = new SeçãoRetangular { Hx = 15, Hy = 50 } });
            _seções[0].Nome = GerarNovoNomeDeSeção();
            _seções[1].Nome = GerarNovoNomeDeSeção();

            _materiais[0].SeçõesConectadas.Add(_seções[0]);
            _materiais[1].SeçõesConectadas.Add(_seções[1]);

            SeçãoAtual = _seções[0];
        }
        #endregion

        #region Métodos

        public void InserirNó(NóVisual nó)
        {
            if (nó.Nome == "")
                nó.Nome = GerarNovoNomeDeNó();
            _nós.Add(nó);
        }
        public void DeletarNó(NóVisual no)
        {
            _nós.Remove(no);
        }
        public void InserirElemento(ElementoVisual elm)
        {
            if (elm.Nome == "")
                elm.Nome = GerarNovoNomeDeElemento();
            elm.Nó1.ElementosConectados.Add(elm);
            elm.Nó2.ElementosConectados.Add(elm);
            elm.Seção.ElementosConectados.Add(elm);
            _elementos.Add(elm);
        }
        public void DeletarElemento(ElementoVisual elm)
        {
            elm.Nó1.ElementosConectados.Remove(elm);
            elm.Nó2.ElementosConectados.Remove(elm);
            elm.Seção.ElementosConectados.Remove(elm);
            foreach (var car in elm.Carregamentos)
                car.ElementoConectadosEDesenhos.Remove(elm);
            _elementos.Remove(elm);
        }
        public void InserirMaterial(Materiais.Material mat)
        {
            if (mat.Nome == "")
                mat.Nome = GerarNovoNomeDeElemento();
            _materiais.Add(mat);
        }
        public void DeletarMaterial(Materiais.Material mat)
        {
            _materiais.Remove(mat);
            mat.Selecionado = false;
        }
        public void InserirSeção(Seções.Seção sec)
        {
            if (sec.Nome == "")
                sec.Nome = GerarNovoNomeDeElemento();
            _seções.Add(sec);
            sec.Material.SeçõesConectadas.Add(sec);
        }
        public void DeletarSeção(Seções.Seção sec)
        {
            _seções.Remove(sec);
            sec.Material.SeçõesConectadas.Remove(sec);
            if (sec == SeçãoAtual)
            {
                SeçãoAtual = _seções[0];
            }
            sec.Selecionado = false;
        }

        public string GerarNovoNomeDeNó()
        {
            _númeroDeNosInseridos++;
            return "Nó " + _númeroDeNosInseridos;
        }
        public string GerarNovoNomeDeElemento()
        {
            _númeroDeElementosInseridos++;
            return "Elemento " + _númeroDeElementosInseridos;
        }
        public string GerarNovoNomeDeMaterial()
        {
            _númeroDeMateriaisInseridos++;
            return "Material " + _númeroDeMateriaisInseridos;
        }
        public string GerarNovoNomeDeSeção()
        {
            _númeroDeSeçõesInseridas++;
            return "Seção " + _númeroDeSeçõesInseridas;
        }
        public void DeletarSeleção(ObservableCollection<NóVisual> nosSelecionados,
            ObservableCollection<ElementoVisual> elementosSelecionados)
        {
            foreach (var elm in elementosSelecionados)
            {
                elm.Selecionado = false;
                DeletarElemento(elm);
            }
            foreach (var no in nosSelecionados)
            {
                no.Selecionado = false;
                DeletarNó(no);
            }
        }

        public void DeletarSeleção(ObservableCollection<ElementoVisual> elementosSelecionados)
        {
            foreach (var elm in elementosSelecionados)
            {
                elm.Selecionado = false;
                DeletarElemento(elm);
            }
        }
        public void DeletarSeleção(ObservableCollection<NóVisual> nosSelecionados)
        {
            foreach (var no in nosSelecionados)
            {
                no.Selecionado = false;
                DeletarNó(no);
            }
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
