using OrbitalGuardian.Enums;
using OrbitalGuardian.Models;

namespace OrbitalGuardian.Interfaces
{
    public interface IClassificadorRisco
    {
        NivelRisco Classificar(MedicaoClimatica medicao);
    }
}