namespace OrbitalGuardian.Models
{
    public class RegiaoMonitorada
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public Coordenada Localizacao { get; private set; }
        public double DistanciaAreaHabitadaKm { get; private set; }
        public int HistoricoOcorrencias { get; private set; }

        public RegiaoMonitorada(
            int id,
            string nome,
            Coordenada localizacao,
            double distanciaAreaHabitadaKm,
            int historicoOcorrencias)
        {
            Id = id;
            Nome = nome;
            Localizacao = localizacao;
            DistanciaAreaHabitadaKm = distanciaAreaHabitadaKm;
            HistoricoOcorrencias = historicoOcorrencias;
        }

        public override string ToString()
        {
            return $"{Nome} | Localização: {Localizacao} | Distância de área habitada: {DistanciaAreaHabitadaKm} km";
        }
    }
}