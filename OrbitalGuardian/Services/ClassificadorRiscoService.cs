using OrbitalGuardian.Enums;
using OrbitalGuardian.Exceptions;
using OrbitalGuardian.Interfaces;
using OrbitalGuardian.Models;
using OrbitalGuardian.Utils;

namespace OrbitalGuardian.Services
{
    public class ClassificadorRiscoService : IClassificadorRisco
    {
        public NivelRisco Classificar(MedicaoClimatica medicao)
        {
            ValidarMedicao(medicao);

            int pontos = 0;

            if (medicao.Temperatura >= ConfiguracoesSistema.TemperaturaCritica)
                pontos += 2;

            if (medicao.Umidade <= ConfiguracoesSistema.UmidadeCritica)
                pontos += 2;

            if (medicao.VelocidadeVento >= ConfiguracoesSistema.VentoForte)
                pontos += 1;

            if (medicao.FumacaDetectada)
                pontos += 3;

            if (medicao.ConfiancaIA >= 0.8)
                pontos += 1;

            if (pontos >= 7)
                return NivelRisco.Critico;

            if (pontos >= 5)
                return NivelRisco.Alto;

            if (pontos >= 3)
                return NivelRisco.Medio;

            return NivelRisco.Baixo;
        }

        private void ValidarMedicao(MedicaoClimatica medicao)
        {
            if (medicao.Temperatura < -50 || medicao.Temperatura > 80)
            {
                throw new MedicaoInvalidaException("Temperatura fora do intervalo esperado.");
            }

            if (medicao.Umidade < 0 || medicao.Umidade > 100)
            {
                throw new MedicaoInvalidaException("Umidade deve estar entre 0% e 100%.");
            }

            if (medicao.VelocidadeVento < 0)
            {
                throw new MedicaoInvalidaException("Velocidade do vento não pode ser negativa.");
            }

            if (medicao.ConfiancaIA < 0 || medicao.ConfiancaIA > 1)
            {
                throw new MedicaoInvalidaException("Confiança da IA deve estar entre 0 e 1.");
            }
        }
    }
}