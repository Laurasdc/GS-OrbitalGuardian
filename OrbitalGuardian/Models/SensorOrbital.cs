using OrbitalGuardian.Enums;

namespace OrbitalGuardian.Models
{
    public class SensorOrbital : Sensor
    {
        public SensorOrbital(int id, string nome)
            : base(id, nome, TipoSensor.Orbital)
        {
        }

        public override string ColetarDados()
        {
            ValidarSensorAtivo();
            return $"Sensor orbital {Nome} analisou imagem simulada para detectar fumaça ou fogo.";
        }
    }
}