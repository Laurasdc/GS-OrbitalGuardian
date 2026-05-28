using OrbitalGuardian.Enums;

namespace OrbitalGuardian.Models
{
    public class SensorAmbiental : Sensor
    {
        public SensorAmbiental(int id, string nome)
            : base(id, nome, TipoSensor.Ambiental)
        {
        }

        public override string ColetarDados()
        {
            ValidarSensorAtivo();
            return $"Sensor ambiental {Nome} coletou dados de temperatura, umidade e vento.";
        }
    }
}