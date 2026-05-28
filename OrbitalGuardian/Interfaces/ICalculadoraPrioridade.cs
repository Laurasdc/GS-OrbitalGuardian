using OrbitalGuardian.Enums;
using OrbitalGuardian.Models;

namespace OrbitalGuardian.Interfaces
{
    public interface ICalculadoraPrioridade
    {
        int Calcular(MedicaoClimatica medicao, RegiaoMonitorada regiao, NivelRisco risco);
    }
}