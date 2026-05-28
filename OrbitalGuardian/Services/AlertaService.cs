using OrbitalGuardian.Interfaces;
using OrbitalGuardian.Models;

namespace OrbitalGuardian.Services
{
    public partial class AlertaService
    {
        private readonly IClassificadorRisco _classificadorRisco;
        private readonly ICalculadoraPrioridade _calculadoraPrioridade;

        public AlertaService(
            IClassificadorRisco classificadorRisco,
            ICalculadoraPrioridade calculadoraPrioridade)
        {
            _classificadorRisco = classificadorRisco;
            _calculadoraPrioridade = calculadoraPrioridade;
        }

        public Alerta GerarAlerta(MedicaoClimatica medicao, RegiaoMonitorada regiao)
        {
            var risco = _classificadorRisco.Classificar(medicao);
            int prioridade = _calculadoraPrioridade.Calcular(medicao, regiao, risco);

            return new Alerta(regiao, risco, prioridade, DateTime.Now);
        }

        public List<Alerta> OrdenarPorPrioridade(List<Alerta> alertas)
        {
            return alertas
                .OrderByDescending(alerta => alerta.Prioridade)
                .ThenByDescending(alerta => alerta.DataCriacao)
                .ToList();
        }
    }
}