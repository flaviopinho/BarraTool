using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarraTool.Unidades
{
    public enum UnidadeDeComprimento
    {
        cm = 0,
        m = 1,
        mm = 2
    }
    public enum UnidadeDeForça
    {
        kN = 0,
        N = 1,
        MN = 2,
        tf = 3,
        kgf=4
    }
    public enum UnidadeDeMassa
    {
        kg = 0,
        g = 1,
        t = 2,
    }
    public enum UnidadeDeÂngulo
    {
        rad = 0,
        grau = 1,
    }
    public enum UnidadeDeTemperatura
    {
        C = 0,
        K = 1,
        F = 2,
    }
    public class ManipuladorDeUnidades : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Comprimento
        private UnidadeDeComprimento comprimento = UnidadeDeComprimento.cm;
        private ObservableCollection<string> comprimentos;// = new ObservableCollection<string> { "cm", "m", "mm" };
        public UnidadeDeComprimento Comprimento
        {
            get
            {
                return comprimento;
            }
            set
            {
                if (comprimento == value) return;
                comprimento = value;
                OnPropertyChanged();
            }
        }
        public string ComprimentoKey
        {
            get
            {
                return comprimento.ToString();
            }
        }
        public double ComprimentoFator
        {
            get
            {
                if (comprimento == UnidadeDeComprimento.cm)
                    return 1;
                else if (comprimento == UnidadeDeComprimento.m)
                    return 0.01;
                else //if (comprimento == UnidadeDeComprimento.Milimetros)
                    return 10;
            }
        }
        public int ComprimentoÍndice
        {
            get
            {
                return (int)comprimento;
            }
            set
            {
                comprimento = (UnidadeDeComprimento)value;
                OnPropertyChanged();
                OnPropertyChanged("Comprimento");
                OnPropertyChanged("ComprimentoKey");
                OnPropertyChanged("MomentoKey");
                OnPropertyChanged("CargaDistribuidaKey");
                OnPropertyChanged("TensãoKey");
                OnPropertyChanged("DensidadeKey");
            }
        }
        public ObservableCollection<string> Comprimentos { get => comprimentos; }
        public double GetComprimento(double valor)
        {
            return valor * ComprimentoFator;
        }
        public double SetComprimento(double valor)
        {
            return valor / ComprimentoFator;
        }
        #endregion

        #region Força
        private UnidadeDeForça força = UnidadeDeForça.kN;
        private ObservableCollection<string> forças;// = new ObservableCollection<string> { "kN", "N", "MN", "tf", "kgf" };
        public UnidadeDeForça Força
        {
            get
            {
                return força;
            }
            set
            {
                if (força == value) return;
                força = value;
                OnPropertyChanged();
            }
        }
        public string ForçaKey
        {
            get
            {
                return força.ToString();
            }
        }
        public double ForçaFator
        {
            get
            {
                if (força == UnidadeDeForça.kN)
                    return 1;
                else if (força == UnidadeDeForça.MN)
                    return 0.001;
                else if (força == UnidadeDeForça.N)
                    return 1000;
                else if (força == UnidadeDeForça.tf)
                    return 0.1;
                else //if (força == UnidadeDeForça.kgf)
                    return 100;
            }
        }
        public int ForçaÍndice
        {
            get
            {
                return (int)força;
            }
            set
            {
                força = (UnidadeDeForça)value;

                OnPropertyChanged();
                OnPropertyChanged("Força");
                OnPropertyChanged("ForçaKey");
                OnPropertyChanged("MomentoKey");
                OnPropertyChanged("CargaDistribuidaKey");
                OnPropertyChanged("TensãoKey");
            }
        }
        public ObservableCollection<string> Forças { get => forças; }
        public double GetForça(double valor)
        {
            return valor * ForçaFator;
        }
        public double SetForça(double valor)
        {
            return valor / ForçaFator;
        }
        #endregion

        #region Massa
        private UnidadeDeMassa massa = UnidadeDeMassa.kg;
        private ObservableCollection<string> massas;
        public UnidadeDeMassa Massa
        {
            get
            {
                return massa;
            }
            set
            {
                if (massa == value) return;
                massa = value;
                OnPropertyChanged();
            }
        }
        public string MassaKey
        {
            get
            {
                return massa.ToString();
            }
        }
        public double MassaFator
        {
            get
            {
                if (massa == UnidadeDeMassa.kg)
                    return 1;
                else if (massa == UnidadeDeMassa.g)
                    return 1000;
                else //if (massa == UnidadeDeMassa.t)
                    return 0.001;
            }
        }
        public int MassaÍndice
        {
            get
            {
                return (int)massa;
            }
            set
            {
                massa = (UnidadeDeMassa)value;

                OnPropertyChanged();
                OnPropertyChanged("Massa");
                OnPropertyChanged("MassaKey");
                OnPropertyChanged("DensidadeKey");
            }
        }
        public ObservableCollection<string> Massas { get => massas; }
        public double GetMassa(double valor)
        {
            return valor * MassaFator;
        }
        public double SetMassa(double valor)
        {
            return valor / MassaFator;
        }
        #endregion

        #region Temperatura
        private UnidadeDeTemperatura temperatura = UnidadeDeTemperatura.C;
        private ObservableCollection<string> temperaturas;
        public UnidadeDeTemperatura Temperatura
        {
            get
            {
                return temperatura;
            }
            set
            {
                if (temperatura == value) return;
                temperatura = value;
                OnPropertyChanged();
            }
        }
        public string TemperaturaKey
        {
            get
            {
                return temperatura.ToString();
            }
        }
        public double TemperaturaFator
        {
            get
            {
                if (temperatura == UnidadeDeTemperatura.C)
                    return 1;
                else if (temperatura == UnidadeDeTemperatura.K)
                    return 1;
                else // if (temperatura == UnidadeDeTemperatura.F)
                    return 5.0 / 9.0;
            }
        }
        public int TemperaturaÍndice
        {
            get
            {
                return (int)temperatura;
            }
            set
            {
                temperatura = (UnidadeDeTemperatura)value;

                OnPropertyChanged();
                OnPropertyChanged("Temperatura");
                OnPropertyChanged("TemperaturaKey");
                OnPropertyChanged("CoeficienteDeDilataçãoKey");
            }
        }
        public ObservableCollection<string> Temperaturas { get => temperaturas; }
        public double GetTemperatura(double valor)
        {
            return valor * TemperaturaFator;
        }
        public double SetTemperatura(double valor)
        {
            return valor / TemperaturaFator;
        }
        #endregion

        #region Construtor
        public ManipuladorDeUnidades()
        {
            comprimentos = new ObservableCollection<string>();
            var values = Enum.GetValues(typeof(UnidadeDeComprimento));
            foreach (var v in values)
            {
                comprimentos.Add(v.ToString());
            }

            forças = new ObservableCollection<string>();
            var values2 = Enum.GetValues(typeof(UnidadeDeForça));
            foreach (var v in values2)
            {
                forças.Add(v.ToString());
            }

            massas = new ObservableCollection<string>();
            var values3 = Enum.GetValues(typeof(UnidadeDeMassa));
            foreach (var v in values3)
            {
                massas.Add(v.ToString());
            }

            temperaturas = new ObservableCollection<string>();
            var values4 = Enum.GetValues(typeof(UnidadeDeTemperatura));
            foreach (var v in values4)
            {
                temperaturas.Add(v.ToString());
            }

            ComprimentoÍndice = 0;
            ForçaÍndice = 0;
            MassaÍndice = 0;
            TemperaturaÍndice = 0;
        }
        #endregion

        public static int NúmeroDeCasasDecimaisComprimentos = 3;

        public ManipuladorDeUnidades Clone()
        {
            return this.MemberwiseClone() as ManipuladorDeUnidades;
        }
        public static double ArredondarComprimento(double valor)
        {
            return Math.Round(valor, NúmeroDeCasasDecimaisComprimentos);
        }
        public static double ArredondarGeral(double valor, int númeroDeCasasDecimais=8)
        {
            return Math.Round(valor, númeroDeCasasDecimais);
        }

        #region Outas conversões
        public string MomentoKey
        {
            get
            {
                return ForçaKey + "." + ComprimentoKey;
            }
        }
        public double GetMomento(double valor)
        {
            return valor * ForçaFator * ComprimentoFator;
        }
        public double SetMomento(double valor)
        {
            return valor / (ForçaFator * ComprimentoFator);
        }
        public string CargaDistribuidaKey
        {
            get
            {
                return ForçaKey + "/" + ComprimentoKey;
            }
        }
        public double GetCargaDistribuida(double valor)
        {
            return valor * ForçaFator / ComprimentoFator;
        }
        public double SetCargaDistribuida(double valor)
        {
            return valor / (ForçaFator / ComprimentoFator);
        }
        public string TensãoKey
        {
            get
            {
                return ForçaKey + "/" + ComprimentoKey + "²";
            }
        }
        public double GetTensão(double valor)
        {
            return valor * ForçaFator / Math.Pow(ComprimentoFator, 2);
        }
        public double SetTensão(double valor)
        {
            return valor / (ForçaFator / Math.Pow(ComprimentoFator, 2));
        }
        public string DensidadeKey
        {
            get
            {
                return MassaKey + "/" + ComprimentoKey + "³";
            }
        }
        public double GetDensidade(double valor)
        {
            return valor * MassaFator / Math.Pow(ComprimentoFator, 3);
        }
        public double SetDensidade(double valor)
        {
            return valor / (MassaFator / Math.Pow(ComprimentoFator, 3));
        }
        public string CoeficienteDeDilataçãoKey
        {
            get
            {
                return "/" + CoeficienteDeDilataçãoKey;
            }
        }
        public double GetCoeficienteDeDilatação(double valor)
        {
            return valor / TemperaturaFator;
        }
        public double SetCoeficienteDeDilatação(double valor)
        {
            return valor * TemperaturaFator;
        }
        #endregion
    }
}
