using OrbitalGuardian.Enums;
using OrbitalGuardian.Utils;

namespace OrbitalGuardian.Models
{
    public class Ocorrencia
    {
        public int Id { get; private set; }
        public Alerta Alerta { get; private set; }
        public string Responsavel { get; private set; }
        public string Observacao { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public StatusOcorrencia Status { get; private set; }

        public Ocorrencia(int id, Alerta alerta, string responsavel, string observacao)
        {
            Id = id;
            Alerta = alerta;
            Responsavel = responsavel;
            Observacao = observacao;
            DataAbertura = DateTime.Now;
            DataConclusao = null;
            Status = StatusOcorrencia.Aberta;
        }

        public void IniciarAtendimento()
        {
            Status = StatusOcorrencia.EmAtendimento;
            Alerta.MarcarEmAnalise();
        }

        public void Finalizar(string observacaoConclusao)
        {
            Status = StatusOcorrencia.Finalizada;
            DataConclusao = DateTime.Now;
            Observacao += $" | Conclusão: {observacaoConclusao}";
            Alerta.MarcarComoResolvido();
        }

        public override string ToString()
        {
            string dataConclusao = DataConclusao.HasValue
                ? FormatadorData.Formatar(DataConclusao.Value)
                : "Ainda não finalizada";

            return $"Ocorrência #{Id}\n" +
                   $"Região: {Alerta.Regiao.Nome}\n" +
                   $"Risco: {Alerta.NivelRisco}\n" +
                   $"Prioridade: {Alerta.Prioridade}\n" +
                   $"Status da ocorrência: {Status}\n" +
                   $"Status do alerta: {Alerta.Status}\n" +
                   $"Responsável: {Responsavel}\n" +
                   $"Abertura: {FormatadorData.Formatar(DataAbertura)}\n" +
                   $"Conclusão: {dataConclusao}\n" +
                   $"Observação: {Observacao}";
        }
    }
}