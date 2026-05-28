namespace OrbitalGuardian.Models
{
    public class MedicaoClimatica
    {
        public double Temperatura { get; private set; }
        public double Umidade { get; private set; }
        public double VelocidadeVento { get; private set; }
        public bool FumacaDetectada { get; private set; }
        public double ConfiancaIA { get; private set; }
        public DateTime DataHora { get; private set; }

        public MedicaoClimatica(
            double temperatura,
            double umidade,
            double velocidadeVento,
            bool fumacaDetectada,
            double confiancaIA)
        {
            Temperatura = temperatura;
            Umidade = umidade;
            VelocidadeVento = velocidadeVento;
            FumacaDetectada = fumacaDetectada;
            ConfiancaIA = confiancaIA;
            DataHora = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Temperatura: {Temperatura}°C | Umidade: {Umidade}% | Vento: {VelocidadeVento} km/h | Fumaça: {FumacaDetectada} | Confiança IA: {ConfiancaIA:P0}";
        }
    }
}