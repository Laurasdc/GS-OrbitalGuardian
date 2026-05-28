using OrbitalGuardian.Enums;

namespace OrbitalGuardian.Models
{
    public class CenarioAmbiental
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public int TemperaturaMinima { get; private set; }
        public int TemperaturaMaxima { get; private set; }
        public int UmidadeMinima { get; private set; }
        public int UmidadeMaxima { get; private set; }
        public int VentoMinimo { get; private set; }
        public int VentoMaximo { get; private set; }
        public bool FumacaDetectada { get; private set; }
        public double ConfiancaMinimaIA { get; private set; }
        public double ConfiancaMaximaIA { get; private set; }
        public NivelRisco RiscoEsperado { get; private set; }

        public CenarioAmbiental(
            int id,
            string nome,
            string descricao,
            int temperaturaMinima,
            int temperaturaMaxima,
            int umidadeMinima,
            int umidadeMaxima,
            int ventoMinimo,
            int ventoMaximo,
            bool fumacaDetectada,
            double confiancaMinimaIA,
            double confiancaMaximaIA,
            NivelRisco riscoEsperado)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            TemperaturaMinima = temperaturaMinima;
            TemperaturaMaxima = temperaturaMaxima;
            UmidadeMinima = umidadeMinima;
            UmidadeMaxima = umidadeMaxima;
            VentoMinimo = ventoMinimo;
            VentoMaximo = ventoMaximo;
            FumacaDetectada = fumacaDetectada;
            ConfiancaMinimaIA = confiancaMinimaIA;
            ConfiancaMaximaIA = confiancaMaximaIA;
            RiscoEsperado = riscoEsperado;
        }

        public MedicaoClimatica GerarMedicao(Random random)
        {
            double temperatura = random.Next(TemperaturaMinima, TemperaturaMaxima + 1);
            double umidade = random.Next(UmidadeMinima, UmidadeMaxima + 1);
            double vento = random.Next(VentoMinimo, VentoMaximo + 1);
            double confiancaIA = Math.Round(
                random.NextDouble() * (ConfiancaMaximaIA - ConfiancaMinimaIA) + ConfiancaMinimaIA,
                2
            );

            return new MedicaoClimatica(
                temperatura,
                umidade,
                vento,
                FumacaDetectada,
                confiancaIA
            );
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} | Risco esperado: {RiscoEsperado}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Temperatura: {TemperaturaMinima}°C a {TemperaturaMaxima}°C | " +
                   $"Umidade: {UmidadeMinima}% a {UmidadeMaxima}% | " +
                   $"Vento: {VentoMinimo} km/h a {VentoMaximo} km/h | " +
                   $"Fumaça: {FumacaDetectada}";
        }
    }
}