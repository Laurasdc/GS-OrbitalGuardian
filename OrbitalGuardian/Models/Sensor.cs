using OrbitalGuardian.Enums;
using OrbitalGuardian.Exceptions;

namespace OrbitalGuardian.Models
{
    public abstract class Sensor
    {
        public int Id { get; protected set; }
        public string Nome { get; protected set; }
        public TipoSensor Tipo { get; protected set; }
        public bool Ativo { get; protected set; }
        public DateTime DataInstalacao { get; protected set; }

        protected Sensor(int id, string nome, TipoSensor tipo)
        {
            Id = id;
            Nome = nome;
            Tipo = tipo;
            Ativo = true;
            DataInstalacao = DateTime.Now;
        }

        public abstract string ColetarDados();

        public void Desativar()
        {
            Ativo = false;
        }

        protected void ValidarSensorAtivo()
        {
            if (!Ativo)
            {
                throw new SensorInativoException($"O sensor {Nome} está inativo e não pode coletar dados.");
            }
        }
    }
}