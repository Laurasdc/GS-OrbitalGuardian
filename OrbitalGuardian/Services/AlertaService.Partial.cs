using OrbitalGuardian.Enums;
using OrbitalGuardian.Models;

namespace OrbitalGuardian.Services
{
    public partial class AlertaService
    {
        public void ExibirResumoAlerta(Alerta alerta)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("          RESUMO DO ALERTA");
            Console.WriteLine("======================================");
            Console.WriteLine(alerta);
            Console.WriteLine("======================================");
        }

        public bool DeveAcionarEmergencia(Alerta alerta)
        {
            return alerta.NivelRisco == NivelRisco.Critico && alerta.Prioridade >= 80;
        }
    }
}