using OrbitalGuardian.Enums;
using OrbitalGuardian.Utils;

namespace OrbitalGuardian.Models
{
    public class Alerta
    {
        public RegiaoMonitorada Regiao { get; private set; }
        public NivelRisco NivelRisco { get; private set; }
        public int Prioridade { get; private set; }
        public StatusAlerta Status { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public string Recomendacao { get; private set; }

        public Alerta(RegiaoMonitorada regiao, NivelRisco nivelRisco, int prioridade, DateTime dataCriacao)
        {
            Regiao = regiao;
            NivelRisco = nivelRisco;
            Prioridade = prioridade;
            Status = StatusAlerta.Aberto;
            DataCriacao = dataCriacao;
            Recomendacao = GerarRecomendacao(nivelRisco);
        }

        private string GerarRecomendacao(NivelRisco risco)
        {
            return risco switch
            {
                NivelRisco.Critico => "Acionar equipe de emergência imediatamente e priorizar evacuação preventiva.",
                NivelRisco.Alto => "Enviar equipe para verificação da ocorrência e intensificar monitoramento.",
                NivelRisco.Medio => "Manter observação contínua e atualizar medições periodicamente.",
                NivelRisco.Baixo => "Manter monitoramento padrão da região.",
                _ => "Sem recomendação disponível."
            };
        }

        public void MarcarEmAnalise()
        {
            Status = StatusAlerta.EmAnalise;
        }

        public void MarcarComoResolvido()
        {
            Status = StatusAlerta.Resolvido;
        }

        public override string ToString()
        {
            return $"Região: {Regiao.Nome}\n" +
                   $"Risco: {NivelRisco}\n" +
                   $"Prioridade: {Prioridade}\n" +
                   $"Status: {Status}\n" +
                   $"Data: {FormatadorData.Formatar(DataCriacao)}\n" +
                   $"Recomendação: {Recomendacao}";
        }
    }
}