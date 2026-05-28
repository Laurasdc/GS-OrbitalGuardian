using OrbitalGuardian.Enums;
using OrbitalGuardian.Interfaces;
using OrbitalGuardian.Models;

namespace OrbitalGuardian.Services
{
    public class CalculadoraPrioridadeService : ICalculadoraPrioridade
    {
        public int Calcular(MedicaoClimatica medicao, RegiaoMonitorada regiao, NivelRisco risco)
        {
            int prioridade = 0;

            prioridade += risco switch
            {
                NivelRisco.Critico => 50,
                NivelRisco.Alto => 35,
                NivelRisco.Medio => 20,
                NivelRisco.Baixo => 10,
                _ => 0
            };

            if (regiao.DistanciaAreaHabitadaKm <= 5)
                prioridade += 25;
            else if (regiao.DistanciaAreaHabitadaKm <= 15)
                prioridade += 15;

            prioridade += regiao.HistoricoOcorrencias * 2;

            if (medicao.ConfiancaIA >= 0.8)
                prioridade += 15;
            else if (medicao.ConfiancaIA >= 0.6)
                prioridade += 10;

            if (medicao.FumacaDetectada)
                prioridade += 10;

            return prioridade;
        }
    }
}