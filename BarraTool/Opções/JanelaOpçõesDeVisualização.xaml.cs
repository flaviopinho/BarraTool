using BarraTool.ComandosInternos;
using BarraTool.PréProcessador.Nós;
using BarraTool.Unidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarraTool.Opções
{
    /// <summary>
    /// Lógica interna para OpçõesDeVisualização.xaml
    /// </summary>
    public partial class JanelaOpçõesDeVisualização : Window
    {
        OpçõesVisuaisDosNós _opçõesVisuaisDosNós;
        private FábricaDeComandosInternos _comandos;
        private ManipuladorDeUnidades _manipuladorDeUnidades;

        private double _raioDosNós;

        public ManipuladorDeUnidades ManipuladorDeUnidades { get => _manipuladorDeUnidades; set => _manipuladorDeUnidades = value; }
        public double RaioDosNós
        {
            get
            {
                return _raioDosNós;
            }
            set
            {
                _raioDosNós = value;
            }
        }
        public Color CorDosNós
        {
            get;
            set;
        }
        public double EspessuraDasLinhasDosNós
        {
            get;
            set;
        }

        public JanelaOpçõesDeVisualização(ManipuladorDeUnidades manipuladorDeUnidades, FábricaDeComandosInternos comandos)
        {
            _comandos = comandos;
            _manipuladorDeUnidades = manipuladorDeUnidades;
            _opçõesVisuaisDosNós = OpçõesVisuaisDosNós.Instância;
            InitializeComponent();
            this.DataContext = this;
            RaioDosNós = _manipuladorDeUnidades.GetComprimento(_opçõesVisuaisDosNós.Raio);
            EspessuraDasLinhasDosNós = _manipuladorDeUnidades.GetComprimento(_opçõesVisuaisDosNós.EspessuraDasLinhas);
            CorDosNós = _opçõesVisuaisDosNós.CorPadrão.Color;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            ComandoInternoComposto comandoComposto = new ComandoInternoComposto();
            if(_opçõesVisuaisDosNós.Raio != _manipuladorDeUnidades.SetComprimento(_raioDosNós))
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_opçõesVisuaisDosNós);
                comando.Propriedade = typeof(OpçõesVisuaisDosNós).GetProperty("Raio");
                comando.ValorAntigo = _opçõesVisuaisDosNós.Raio;
                comando.ValorNovo = _manipuladorDeUnidades.SetComprimento(_raioDosNós);
                comandoComposto.AdicionarComando(comando);
            }
            if (_opçõesVisuaisDosNós.EspessuraDasLinhas != _manipuladorDeUnidades.SetComprimento(EspessuraDasLinhasDosNós))
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_opçõesVisuaisDosNós);
                comando.Propriedade = typeof(OpçõesVisuaisDosNós).GetProperty("EspessuraDasLinhas");
                comando.ValorAntigo = _opçõesVisuaisDosNós.EspessuraDasLinhas;
                comando.ValorNovo = _manipuladorDeUnidades.SetComprimento(EspessuraDasLinhasDosNós);
                comandoComposto.AdicionarComando(comando);
            }
            if (CorDosNós != _opçõesVisuaisDosNós.CorPadrão.Color)
            {
                ComandoInternoAlterarPropriedade comando = new ComandoInternoAlterarPropriedade(_opçõesVisuaisDosNós);
                comando.Propriedade = typeof(OpçõesVisuaisDosNós).GetProperty("CorPadrão");
                comando.ValorAntigo = _opçõesVisuaisDosNós.CorPadrão.Clone();
                comando.ValorNovo = new SolidColorBrush(CorDosNós);
                comandoComposto.AdicionarComando(comando);
            }
            _comandos.AdicionarComando(comandoComposto);

        }
    }
}
