using BarraTool.ComandosExternos;
using BarraTool.ComandosInternos;
using BarraTool.PréProcessador.Carregamentos;
using BarraTool.PréProcessador.Elementos;
using BarraTool.PréProcessador.Materiais;
using BarraTool.PréProcessador.Nós;
using BarraTool.PréProcessador.Seções;
using BarraTool.Unidades;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BarraTool.PréProcessador
{
    public class ModeloVisual : Grid, INotifyPropertyChanged
    {
        #region Campos

        #region Campos Principais (Modelo, Unidades, Comandos, Canvas)
        //Modelo do MVVM. Contem todos os objetos importantes que serão apresentados no modelo visual
        private FábricaDeElementosENós _modelo;
        //Classe para alternar entre as unidades (cm,KN...)
        private ManipuladorDeUnidades _manipuladorDeUnidades;
        //Comandos que vão alterar o objeto modelo (Diferente dos comandos do modelo visual)
        private FábricaDeComandosInternos _comandos;
        //Carregamentos, casos e combinações
        private FábricaDeCarregamentos _carregamentos;


        //Canvas onde serão desenhados todos os objetos do modelo
        private ItemsControl _canvasNós = new ItemsControl();
        private ItemsControl _canvasElementos = new ItemsControl();
        private ItemsControl _canvasCarregamentosDoCaso = new ItemsControl();
        private ItemsControl _canvasManipulaçãoDeObjetos = new ItemsControl();
        #endregion

        #region Zoom, Mover e Seleção
        //Velocidade do zoom
        private double _zoomSpeed = 1;

        //Escala e movimento do canvas
        private ScaleTransform _scaleTransform;
        private TranslateTransform _translateTransform;
        private bool _janelaSendoMovida = false;

        //Pontos para pan
        private Point _origem;
        private Point _start;

        //Lista de objetos que foram selecionados
        private ObservableCollection<NóVisual> _nósSelecionados;
        private ObservableCollection<ElementoVisual> _elementosSelecionados;
        private Materiais.Material _materialSelecionado;
        private Seções.Seção _seçãoSelecionada;
        private CarregamentoVisual _carregamentoSelecionado;

        #endregion

        #region Comandos

        //Campos que influenciam mais de um comando
        private bool _habilitarComandos = true;
        private bool _requerConfirmação = false;
        private bool _ortogonal = false;
        //Comando de confirmação
        private ComandoExternoEnter _comandoExternoEnter;
        //Comando de Cancelamento
        private ComandoExternoEsc _comandoExternoEsc;

        //Comando seleção
        private TipoDeSeleção _tipoDeSeleção;
        private ComandoExternoSeleção _comandoExternoSeleção;

        private ComandoExternoMover _comandoExternoMoverObjetoVisual;

        //Comando Inserir Nó
        private ComandoExternoInserirNó _comandoExternoInserirNó;

        //Comando Voltar
        private ComandoExternoVoltar _comandoExternoVoltar;
        private ComandoExternoRefazer _comandoExternoRefazer;

        //Comando Zoom Dois Pontos
        private ComandoExternoZoomDoisPontos _comandoExternoZoomDoisPontos;

        //Comando InserirElemento
        private ComandoExternoInserirElemento _comandoExternoInserirElemento;

        //Comando deletar
        private ComandoExternoDeletarSelecionados _comandoExternoDeletar;

        //Comando Zoom Total
        private ComandoExternoZoomTotal _comandoExternoZoomTotal;

        //Editar casos
        private ComandoExternoEditarCasos _comandoExternoEditarCasos;
        #endregion

        //Objetos que serão inseridos ao Canvas para manipular os demais objetos
        private ObservableCollection<UIElement> _objetosParaExecuçãDeComandos;
        private ObservableCollection<UIElement> _janelaDePropriedadesDosNósEElementos;
        private ObservableCollection<UIElement> _janelaDePropriedadesDosMateriaisESeções;
        private ObservableCollection<UIElement> _janelaDePropriedadesDosCarregamentos;

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Propriedades
        public ObservableCollection<UIElement> JanelaDePropriedadesDosNósEElementos
        {
            get
            {
                return _janelaDePropriedadesDosNósEElementos;
            }
            set
            {
                _janelaDePropriedadesDosNósEElementos = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UIElement> JanelaDePropriedadesDosMateriaisESeções
        {
            get
            {
                return _janelaDePropriedadesDosMateriaisESeções;
            }
            set
            {
                _janelaDePropriedadesDosMateriaisESeções = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UIElement> JanelaDePropriedadesDosCarregamentos
        {
            get
            {
                return _janelaDePropriedadesDosCarregamentos;
            }
            set
            {
                _janelaDePropriedadesDosCarregamentos = value;
                OnPropertyChanged();
            }
        }
        public ManipuladorDeUnidades ManipuladorDeUnidades
        {
            get => _manipuladorDeUnidades;
        }
        public FábricaDeComandosInternos ComandosInternos
        {
            get => _comandos;
        }
        public int NúmeroDeElementosENósSelecionados
        {
            get => _nósSelecionados.Count + _elementosSelecionados.Count;
        }
        public FábricaDeElementosENós Modelo { get => _modelo; }
        public FábricaDeCarregamentos Carregamentos { get => _carregamentos; }
        public ObservableCollection<NóVisual> NósSelecionados { get => _nósSelecionados; set => _nósSelecionados = value; }
        public ObservableCollection<ElementoVisual> ElementosSelecionados { get => _elementosSelecionados; set => _elementosSelecionados = value; }
        public Materiais.Material MaterialSelecionado
        {
            get => _materialSelecionado;
            set
            {
                _materialSelecionado = value;
                if (_materialSelecionado != null)
                {
                    JanelaDePropriedadesDosMateriaisESeções.Clear();
                    GC.Collect();
                    JanelaDePropriedadesDosMateriaisESeções.Add(new PropriedadeDosMateriais(_materialSelecionado, this));
                }
            }
        }
        public Seções.Seção SeçãoSelecionada
        {
            get => _seçãoSelecionada;
            set
            {
                _seçãoSelecionada = value;
                if (_seçãoSelecionada != null)
                {
                    JanelaDePropriedadesDosMateriaisESeções.Clear();
                    GC.Collect();
                    JanelaDePropriedadesDosMateriaisESeções.Add(new PropriedadesDasSeções(_seçãoSelecionada, this));
                }
            }
        }
        public CarregamentoVisual CarregamentoSelecionado
        {
            get => _carregamentoSelecionado;
            set
            {
                _carregamentoSelecionado = value;
                if (_carregamentoSelecionado != null)
                {
                    JanelaDePropriedadesDosCarregamentos.Clear();
                    GC.Collect();
                    JanelaDePropriedadesDosCarregamentos.Add(new PropriedadesDoCarregamentoDistribuido((CarregamentoDistribuido)_carregamentoSelecionado, this));
                }
            }
        }
        public TipoDeSeleção TipoDeSeleção
        {
            get
            {
                return _tipoDeSeleção;
            }
            set
            {
                _tipoDeSeleção = value;
                _comandoExternoSeleção.RaiseCanExecuteChanged();
                if (_tipoDeSeleção == TipoDeSeleção.ObjetoVisual)
                    _comandoExternoSeleção.Execute(null);
                else
                    _comandoExternoSeleção.Cancelar();
                OnPropertyChanged();
            }
        }
        public bool Ortogonal
        {
            get
            {
                return _ortogonal;
            }
            set
            {
                if (value == _ortogonal) return;
                _ortogonal = value;
                OnPropertyChanged();
            }
        }
        public bool HabilitarComandos
        {
            get => _habilitarComandos;
            set
            {
                if (value == _habilitarComandos) return;
                _habilitarComandos = value;
                OnPropertyChanged();
                ComandoExternoMoverObjetoVisual.RaiseCanExecuteChanged();
                ComandoExternoZoomDoisPontos.RaiseCanExecuteChanged();
                ComandoExternoZoomTotal.RaiseCanExecuteChanged();
                ComandoExternoInserirNó.RaiseCanExecuteChanged();
                ComandoExternoVoltar.RaiseCanExecuteChanged();
                ComandoExternoRefazer.RaiseCanExecuteChanged();
                ComandoExternoDeletar.RaiseCanExecuteChanged();
                ComandoExternoInserirElemento.RaiseCanExecuteChanged();
            }
        }
        public bool RequerConfirmação
        {
            get => _requerConfirmação;

            set
            {
                if (_requerConfirmação == value) return;
                _requerConfirmação = value;
                ComandoExternoEnter.RaiseCanExecuteChanged();
            }
        }
        public bool JanelaSendoMovida { get => _janelaSendoMovida; set => _janelaSendoMovida = value; }
        public ComandoExternoEnter ComandoExternoEnter { get => _comandoExternoEnter; set => _comandoExternoEnter = value; }
        public ComandoExternoMover ComandoExternoMoverObjetoVisual { get => _comandoExternoMoverObjetoVisual; set => _comandoExternoMoverObjetoVisual = value; }
        public ComandoExternoEsc ComandoExternoEsc { get => _comandoExternoEsc; set => _comandoExternoEsc = value; }
        public ComandoExternoSeleção ComandoExternoSeleção { get => _comandoExternoSeleção; set => _comandoExternoSeleção = value; }
        public ComandoExternoVoltar ComandoExternoVoltar { get => _comandoExternoVoltar; set => _comandoExternoVoltar = value; }
        public ComandoExternoZoomDoisPontos ComandoExternoZoomDoisPontos { get => _comandoExternoZoomDoisPontos; set => _comandoExternoZoomDoisPontos = value; }
        public ComandoExternoInserirNó ComandoExternoInserirNó { get => _comandoExternoInserirNó; set => _comandoExternoInserirNó = value; }
        public ComandoExternoRefazer ComandoExternoRefazer { get => _comandoExternoRefazer; set => _comandoExternoRefazer = value; }
        public ComandoExternoInserirElemento ComandoExternoInserirElemento { get => _comandoExternoInserirElemento; set => _comandoExternoInserirElemento = value; }
        public ComandoExternoDeletarSelecionados ComandoExternoDeletar { get => _comandoExternoDeletar; set => _comandoExternoDeletar = value; }
        public ComandoExternoZoomTotal ComandoExternoZoomTotal { get => _comandoExternoZoomTotal; set => _comandoExternoZoomTotal = value; }
        public ComandoExternoEditarCasos ComandoExternoEditarCasos { get => _comandoExternoEditarCasos; set => _comandoExternoEditarCasos = value; }
        public ComandoExternoInserirSeção ComandoExternoInserirSeção { get; set; }
        public ComandoExternoInserirMaterial ComandoExternoInserirMaterial { get; set; }
        public ComandoExternoOpçõesVisuais ComandoExternoOpçõesVisuais { get;set;}

        public ScaleTransform ScaleTransform { get => _scaleTransform; set => _scaleTransform = value; }
        public ObservableCollection<UIElement> ObjetosParaExecuçãoDeComandos { get => _objetosParaExecuçãDeComandos; set => _objetosParaExecuçãDeComandos = value; }
        public ItemsControl CanvasManipulaçãoDeObjetos { get => _canvasManipulaçãoDeObjetos; set => _canvasManipulaçãoDeObjetos = value; }
        #endregion

        #region Construtores
        public ModeloVisual()
        {
            _manipuladorDeUnidades = new ManipuladorDeUnidades();
            _comandos = new FábricaDeComandosInternos();
            _modelo = new FábricaDeElementosENós();
            _carregamentos = new FábricaDeCarregamentos(_manipuladorDeUnidades);
            _carregamentos.PropertyChanged += _carregamentos_PropertyChanged;
            
            _objetosParaExecuçãDeComandos = new ObservableCollection<UIElement>();

            _canvasElementos.ItemsPanel = new ItemsPanelTemplate() { VisualTree = new FrameworkElementFactory(typeof(Canvas)) };
            _canvasElementos.ItemsSource = _modelo.Elementos;

            _canvasNós.ItemsPanel = new ItemsPanelTemplate() { VisualTree = new FrameworkElementFactory(typeof(Canvas)) };
            _canvasNós.ItemsSource = _modelo.Nós;

            _canvasCarregamentosDoCaso.ItemsPanel = new ItemsPanelTemplate() { VisualTree = new FrameworkElementFactory(typeof(Canvas)) };
            _canvasCarregamentosDoCaso.ItemsSource = _carregamentos.CasoAtual.Carregamentos;

            _canvasManipulaçãoDeObjetos.ItemsPanel = new ItemsPanelTemplate() { VisualTree = new FrameworkElementFactory(typeof(Canvas)) };
            _canvasManipulaçãoDeObjetos.ItemsSource = _objetosParaExecuçãDeComandos;

            this.Children.Add(_canvasElementos);
            this.Children.Add(_canvasCarregamentosDoCaso);
            this.Children.Add(_canvasNós);
            this.Children.Add(_canvasManipulaçãoDeObjetos);

            _nósSelecionados = new ObservableCollection<NóVisual>();
            _elementosSelecionados = new ObservableCollection<ElementoVisual>();
            _materialSelecionado = null;
            _seçãoSelecionada = null;

            _modelo.InserirNó(new NóVisual(100, 100));
            _modelo.InserirNó(new NóVisual(200, 200));
            _modelo.InserirNó(new NóVisual(100, 200));
            _modelo.InserirNó(new NóVisual(200, 100));

            _modelo.InserirElemento(new ElementoVisual(_modelo.Nós[0], 
                _modelo.Nós[2], _modelo.Seções[0]));

            TransformGroup group = new TransformGroup();
            _scaleTransform = new ScaleTransform();
            group.Children.Add(_scaleTransform);
            _translateTransform = new TranslateTransform();
            group.Children.Add(_translateTransform);

            _canvasNós.RenderTransform = group;
            _canvasNós.RenderTransformOrigin = new Point(0.0, 0.0);

            _canvasElementos.RenderTransform = group;
            _canvasElementos.RenderTransformOrigin = new Point(0.0, 0.0);

            _canvasCarregamentosDoCaso.RenderTransform = group;
            _canvasCarregamentosDoCaso.RenderTransformOrigin = new Point(0.0, 0.0);

            _canvasManipulaçãoDeObjetos.RenderTransform = group;
            _canvasManipulaçãoDeObjetos.RenderTransformOrigin = new Point(0.0, 0.0);

            this.MouseWheel += Zoom;
            this.MouseDown += moverJanela_MouseMiddleButtonDown;
            this.MouseMove += moverJanela_MouseMove;
            this.MouseUp += moverJanela_MouseMiddleButtonUp;
            this.Background = Brushes.Transparent;

            _nósSelecionados.CollectionChanged += Selecionados_CollectionChanged;
            _elementosSelecionados.CollectionChanged += Selecionados_CollectionChanged;

            this.ClipToBounds = true;

            _janelaDePropriedadesDosNósEElementos = new ObservableCollection<UIElement>();
            _janelaDePropriedadesDosMateriaisESeções = new ObservableCollection<UIElement>();
            _janelaDePropriedadesDosCarregamentos = new ObservableCollection<UIElement>();

            _comandoExternoEnter = new ComandoExternoEnter(this);
            _comandoExternoEsc = new ComandoExternoEsc(this);
            _comandoExternoMoverObjetoVisual = new ComandoExternoMover(this);
            _comandoExternoSeleção = new ComandoExternoSeleção(this);

            ComandoExternoVoltar = new ComandoExternoVoltar(this);
            ComandoExternoRefazer = new ComandoExternoRefazer(this);

            ComandoExternoZoomTotal = new ComandoExternoZoomTotal(this);
            ComandoExternoZoomDoisPontos = new ComandoExternoZoomDoisPontos(this);
            ComandoExternoInserirNó = new ComandoExternoInserirNó(this);
            ComandoExternoInserirElemento = new ComandoExternoInserirElemento(this);

            ComandoExternoDeletar = new ComandoExternoDeletarSelecionados(this);

            ComandoExternoEditarCasos = new ComandoExternoEditarCasos(this);

            ComandoExternoInserirSeção = new ComandoExternoInserirSeção(this);
            ComandoExternoInserirMaterial = new ComandoExternoInserirMaterial(this);
            ComandoExternoOpçõesVisuais = new ComandoExternoOpçõesVisuais(this);

            RequerConfirmação = false;
            JanelaSendoMovida = false;
            HabilitarComandos = true;
            TipoDeSeleção = TipoDeSeleção.ObjetoVisual;

            CarregamentoDistribuido car = new CarregamentoDistribuido(-10,-2, _carregamentos.CasoAtual);
            _carregamentos.InserirCarregamento(car);
            _carregamentos.AplicarCarregamento(car, _modelo.Elementos[0]);

            this.Focusable = true;
            this.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                this.Focus();
            };
        }

        #endregion

        #region Métodos para pan e zoom
        private void Zoom(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? .2 : -.2;
            if (!(e.Delta > 0) && (_scaleTransform.ScaleX < .4 * _zoomSpeed || _scaleTransform.ScaleY < .4 * _zoomSpeed))
                return;

            Point relative = e.GetPosition(_canvasNós);
            double absoluteX;
            double absoluteY;

            absoluteX = relative.X * _scaleTransform.ScaleX + _translateTransform.X;
            absoluteY = relative.Y * _scaleTransform.ScaleY + _translateTransform.Y;

            _scaleTransform.ScaleX += zoom;
            _scaleTransform.ScaleY += zoom;

            _translateTransform.X = absoluteX - relative.X * _scaleTransform.ScaleX;
            _translateTransform.Y = absoluteY - relative.Y * _scaleTransform.ScaleY;
        }
        private void moverJanela_MouseMiddleButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                _start = e.GetPosition(this);
                _origem = new Point(_translateTransform.X, _translateTransform.Y);
                JanelaSendoMovida = true;
                this.Cursor = Cursors.Hand;
                this.CaptureMouse();
            }
        }
        private void moverJanela_MouseMiddleButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                this.ReleaseMouseCapture();
                JanelaSendoMovida = false;
                this.Cursor = Cursors.Arrow;
            }
        }
        private void moverJanela_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured && JanelaSendoMovida == true)
            {
                Vector v = _start - e.GetPosition(this);
                _translateTransform.X = _origem.X - v.X;
                _translateTransform.Y = _origem.Y - v.Y;
            }
        }
        public void ZoomDoisPontos(Rect retangulo)
        {
            _translateTransform.X = 0;
            _translateTransform.Y = 0;
            _scaleTransform.ScaleX = 1;
            _scaleTransform.ScaleY = 1;

            double Wx = this.ActualWidth;
            double Wy = this.ActualHeight;

            double wx = retangulo.Width;
            double wy = retangulo.Height;

            double scale = Math.Min(Wx / wx, Wy / wy) * 0.8;
            //double scale = 1;

            _scaleTransform.CenterX = 0;
            _scaleTransform.CenterY = 0;
            _scaleTransform.ScaleX = scale;
            _scaleTransform.ScaleY = scale;

            double centroX = Wx / 2 - (retangulo.X + wx / 2) * scale;
            double centroY = Wy / 2 - (retangulo.Y + wy / 2) * scale;

            _translateTransform.X += centroX;
            _translateTransform.Y += centroY;
        }
        
        #endregion

        #region Métodos para seleção de objetos
        public void SeleçãoDoObjeto(ObjetoVisual objSelecionavel, bool selecionado)
        {
            if (objSelecionavel is NóVisual)
            {
                NóVisual noVisual = objSelecionavel as NóVisual;
                if (selecionado == true)
                {
                    objSelecionavel.Selecionado = true;
                    if (!_nósSelecionados.Contains(noVisual))
                        _nósSelecionados.Add(noVisual);
                }
                else
                {
                    objSelecionavel.Selecionado = false;
                    if (_nósSelecionados.Contains(noVisual))
                        _nósSelecionados.Remove(noVisual);
                }
            }
            else if (objSelecionavel is ElementoVisual)
            {
                ElementoVisual elmVisual = objSelecionavel as ElementoVisual;
                if (selecionado == true)
                {
                    objSelecionavel.Selecionado = true;
                    if (!_elementosSelecionados.Contains(elmVisual))
                        _elementosSelecionados.Add(elmVisual);
                }
                else
                {
                    objSelecionavel.Selecionado = false;
                    if (_elementosSelecionados.Contains(elmVisual))
                        _elementosSelecionados.Remove(elmVisual);
                }
            }
        }
        public void CancelarSeleçãoDosElementos()
        {
            foreach (var elm in _elementosSelecionados)
                elm.Selecionado = false;
            _elementosSelecionados.Clear();
        }
        public void CancelarSeleçãoDosNós()
        {
            foreach (var no in _nósSelecionados)
                no.Selecionado = false;
            _nósSelecionados.Clear();
        }
        public void CancelarSeleção()
        {
            CancelarSeleçãoDosNós();
            CancelarSeleçãoDosElementos();
        }
        public void AtualizarSeleção()
        {
            _nósSelecionados.Clear();
            foreach(var nó in _modelo.Nós)
            {
                if (nó.Selecionado == true)
                    _nósSelecionados.Add(nó);
            }
            _elementosSelecionados.Clear();
            foreach (var elm in _modelo.Elementos)
            {
                if (elm.Selecionado == true)
                    _elementosSelecionados.Add(elm);
            }
        }
        #endregion

        #region Cancelar
        public void AnularComandos()
        {
            ComandoExternoMoverObjetoVisual.Cancelar();
            ComandoExternoSeleção.Cancelar();
            ComandoExternoZoomDoisPontos.Cancelar();
            ComandoExternoInserirNó.Cancelar();
            ComandoExternoInserirElemento.Cancelar();

            RequerConfirmação = false;
            JanelaSendoMovida = false;
            HabilitarComandos = true;
            TipoDeSeleção = TipoDeSeleção.ObjetoVisual;
            CancelarSeleção();
        }
        #endregion

        #region Janela de Propriedades

        private void Selecionados_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (NúmeroDeElementosENósSelecionados == 1)
            {
                if (_nósSelecionados.Count == 1)
                {
                    PropriedadesDosNós propriedadesNo = new PropriedadesDosNós(_nósSelecionados[0], this);
                    JanelaDePropriedadesDosNósEElementos.Add(propriedadesNo);
                }
                else if (_elementosSelecionados.Count == 1)
                {
                    PropriedadesElementos propriedadesElm = new PropriedadesElementos(_elementosSelecionados[0], this);
                    JanelaDePropriedadesDosNósEElementos.Add(propriedadesElm);
                }
            }
            else
            {
                JanelaDePropriedadesDosNósEElementos.Clear();
                GC.Collect();
            }
            ComandoExternoMoverObjetoVisual.RaiseCanExecuteChanged();
            ComandoExternoDeletar.RaiseCanExecuteChanged();
        }

        #endregion

        private void _carregamentos_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CasoAtual")
            {
                _canvasCarregamentosDoCaso.ItemsSource = _carregamentos.CasoAtual.Carregamentos;
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
