using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace BarraTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vCulture = new CultureInfo("pt-PT");

            Thread.CurrentThread.CurrentCulture = vCulture;
            Thread.CurrentThread.CurrentUICulture = vCulture;
            CultureInfo.DefaultThreadCurrentCulture = vCulture;
            CultureInfo.DefaultThreadCurrentUICulture = vCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
         XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        }

        private void TabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var tab=(TabItem)((TextBlock)sender).Parent;
            tab.IsSelected = !tab.IsSelected;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PréProcessador.Carregamentos.JanelaInserirCarregamento aaa = new PréProcessador.Carregamentos.JanelaInserirCarregamento(this.ModeloPrincipal);
            aaa.ShowDialog();
        }
    }
}
