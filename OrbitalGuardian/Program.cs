using OrbitalGuardian.Enums;
using OrbitalGuardian.Exceptions;
using OrbitalGuardian.Interfaces;
using OrbitalGuardian.Models;
using OrbitalGuardian.Repositories;
using OrbitalGuardian.Services;

namespace OrbitalGuardian
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Orbital Guardian - Plataforma de Triagem Climática";

            IClassificadorRisco classificador = new ClassificadorRiscoService();
            ICalculadoraPrioridade calculadora = new CalculadoraPrioridadeService();
            AlertaService alertaService = new AlertaService(classificador, calculadora);

            IArquivoRepository arquivoRepository = new ArquivoRepository();

            string caminhoHistoricoAlertas = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "historico_alertas.txt"
            );

            List<Sensor> sensores = CriarSensores();
            List<RegiaoMonitorada> regioes = CriarRegioes();
            List<CenarioAmbiental> cenarios = CriarCenariosAmbientais();

            List<MedicaoClimatica> historicoMedicoes = new List<MedicaoClimatica>();
            List<Alerta> historicoAlertas = new List<Alerta>();

            int opcao;

            do
            {
                ExibirMenu();
                Console.Write("Escolha uma opção: ");

                bool entradaValida = int.TryParse(Console.ReadLine(), out opcao);

                if (!entradaValida)
                {
                    ExibirMensagemErro("Opção inválida. Digite um número.");
                    PausarTela(opcao);
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            ListarRegioes(regioes);
                            break;

                        case 2:
                            ListarSensores(sensores);
                            break;

                        case 3:
                            RegistrarMedicaoPorCenario(
                                regioes,
                                cenarios,
                                historicoMedicoes,
                                historicoAlertas,
                                alertaService,
                                arquivoRepository,
                                caminhoHistoricoAlertas
                            );
                            break;

                        case 4:
                            ListarAlertasPorPrioridade(historicoAlertas, alertaService);
                            break;

                        case 5:
                            ListarAlertasCriticosDaExecucaoAtual(historicoAlertas);
                            break;

                        case 6:
                            ExibirRelatorio(regioes, sensores, historicoMedicoes, historicoAlertas);
                            break;

                        case 7:
                            TestarExcecao(alertaService, regioes[0]);
                            break;

                        case 8:
                            GerarCenarioCritico(
                                regioes,
                                cenarios,
                                historicoMedicoes,
                                historicoAlertas,
                                alertaService,
                                arquivoRepository,
                                caminhoHistoricoAlertas
                            );
                            break;

                        case 9:
                            ConsultarHistoricoArquivo(arquivoRepository, caminhoHistoricoAlertas);
                            break;

                        case 10:
                            ConsultarCriticosArquivo(arquivoRepository, caminhoHistoricoAlertas);
                            break;

                        case 11:
                            ExecutarSimulacaoEmLote(
                                regioes,
                                cenarios,
                                historicoMedicoes,
                                historicoAlertas,
                                alertaService,
                                arquivoRepository,
                                caminhoHistoricoAlertas
                            );
                            break;

                        case 12:
                            ListarCenariosAmbientais(cenarios);
                            break;

                        case 0:
                            ExibirMensagemSucesso("Encerrando o Orbital Guardian...");
                            break;

                        default:
                            ExibirMensagemErro("Opção não encontrada.");
                            break;
                    }
                }
                catch (MedicaoInvalidaException ex)
                {
                    ExibirMensagemErro($"Erro na medição climática: {ex.Message}");
                }
                catch (SensorInativoException ex)
                {
                    ExibirMensagemErro($"Erro no sensor: {ex.Message}");
                }
                catch (IOException ex)
                {
                    ExibirMensagemErro($"Erro de arquivo: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ExibirMensagemErro($"Erro inesperado no sistema: {ex.Message}");
                }

                PausarTela(opcao);

            } while (opcao != 0);
        }

        static List<Sensor> CriarSensores()
        {
            return new List<Sensor>
            {
                new SensorAmbiental(1, "Sensor Ambiental A1"),
                new SensorOrbital(2, "Satélite Simulado O1"),
                new SensorAmbiental(3, "Sensor Ambiental B2"),
                new SensorOrbital(4, "Satélite Simulado O2")
            };
        }

        static List<RegiaoMonitorada> CriarRegioes()
        {
            return new List<RegiaoMonitorada>
            {
                new RegiaoMonitorada(1, "Serra Verde", new Coordenada(-23.5505, -46.6333), 4.5, 6),
                new RegiaoMonitorada(2, "Vale Norte", new Coordenada(-22.9122, -43.2302), 14, 2),
                new RegiaoMonitorada(3, "Mata Sul", new Coordenada(-25.4284, -49.2733), 25, 1),
                new RegiaoMonitorada(4, "Parque das Águas", new Coordenada(-20.3155, -40.3128), 3.2, 8),
                new RegiaoMonitorada(5, "Reserva Norte", new Coordenada(-3.1190, -60.0217), 8.8, 5),
                new RegiaoMonitorada(6, "Encosta Azul", new Coordenada(-22.9068, -43.1729), 2.5, 9)
            };
        }

        static List<CenarioAmbiental> CriarCenariosAmbientais()
        {
            return new List<CenarioAmbiental>
            {
                new CenarioAmbiental(
                    1,
                    "Rotina normal de monitoramento",
                    "Condições ambientais estáveis, sem indicação visual de fumaça.",
                    22,
                    30,
                    45,
                    75,
                    4,
                    16,
                    false,
                    0.50,
                    0.68,
                    NivelRisco.Baixo
                ),

                new CenarioAmbiental(
                    2,
                    "Calor seco em área vegetal",
                    "Temperatura elevada e baixa umidade aumentam a possibilidade de ignição.",
                    34,
                    39,
                    20,
                    34,
                    8,
                    22,
                    false,
                    0.60,
                    0.78,
                    NivelRisco.Medio
                ),

                new CenarioAmbiental(
                    3,
                    "Fumaça detectada por visão computacional",
                    "O sensor orbital identifica fumaça, mas as condições climáticas ainda são moderadas.",
                    30,
                    36,
                    28,
                    45,
                    10,
                    24,
                    true,
                    0.70,
                    0.88,
                    NivelRisco.Alto
                ),

                new CenarioAmbiental(
                    4,
                    "Vento forte com baixa umidade",
                    "Vento intenso pode acelerar a propagação de focos de incêndio.",
                    33,
                    40,
                    15,
                    28,
                    26,
                    40,
                    false,
                    0.65,
                    0.82,
                    NivelRisco.Alto
                ),

                new CenarioAmbiental(
                    5,
                    "Queimada crítica próxima de área habitada",
                    "Fumaça confirmada, temperatura extrema, baixa umidade e vento forte.",
                    39,
                    46,
                    10,
                    22,
                    28,
                    45,
                    true,
                    0.86,
                    0.98,
                    NivelRisco.Critico
                ),

                new CenarioAmbiental(
                    6,
                    "Pós-alerta em observação",
                    "Condições começaram a melhorar, mas ainda exigem acompanhamento.",
                    28,
                    34,
                    35,
                    55,
                    8,
                    20,
                    false,
                    0.55,
                    0.72,
                    NivelRisco.Baixo
                ),

                new CenarioAmbiental(
                    7,
                    "Anomalia térmica sem fumaça",
                    "Temperatura alta identificada, mas sem confirmação visual de fumaça ou fogo.",
                    37,
                    43,
                    24,
                    38,
                    8,
                    20,
                    false,
                    0.62,
                    0.80,
                    NivelRisco.Medio
                ),

                new CenarioAmbiental(
                    8,
                    "Foco inicial de incêndio",
                    "Fumaça detectada com aumento de temperatura e confiança relevante da IA.",
                    35,
                    41,
                    18,
                    32,
                    18,
                    32,
                    true,
                    0.78,
                    0.92,
                    NivelRisco.Critico
                )
            };
        }

        static void ExibirMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("                ORBITAL GUARDIAN");
            Console.WriteLine("      Plataforma de Triagem Climática");
            Console.WriteLine("==================================================");
            Console.ResetColor();

            Console.WriteLine("1  - Listar regiões monitoradas");
            Console.WriteLine("2  - Listar sensores");
            Console.WriteLine("3  - Registrar medição por cenário ambiental");
            Console.WriteLine("4  - Listar alertas por prioridade");
            Console.WriteLine("5  - Filtrar alertas críticos da execução atual");
            Console.WriteLine("6  - Exibir relatório operacional");
            Console.WriteLine("7  - Testar exceção de medição inválida");
            Console.WriteLine("8  - Gerar cenário crítico simulado");
            Console.WriteLine("9  - Consultar histórico salvo em arquivo");
            Console.WriteLine("10 - Filtrar alertas críticos salvos em arquivo");
            Console.WriteLine("11 - Executar simulação em lote");
            Console.WriteLine("12 - Listar cenários ambientais disponíveis");
            Console.WriteLine("0  - Sair");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }

        static void ListarRegioes(List<RegiaoMonitorada> regioes)
        {
            ExibirTitulo("REGIÕES MONITORADAS");

            foreach (RegiaoMonitorada regiao in regioes)
            {
                Console.WriteLine($"ID: {regiao.Id}");
                Console.WriteLine($"Nome: {regiao.Nome}");
                Console.WriteLine($"Localização: {regiao.Localizacao}");
                Console.WriteLine($"Distância de área habitada: {regiao.DistanciaAreaHabitadaKm} km");
                Console.WriteLine($"Histórico de ocorrências: {regiao.HistoricoOcorrencias}");
                ExibirSeparador();
            }
        }

        static void ListarSensores(List<Sensor> sensores)
        {
            ExibirTitulo("SENSORES DO SISTEMA");

            foreach (Sensor sensor in sensores)
            {
                Console.WriteLine($"ID: {sensor.Id}");
                Console.WriteLine($"Nome: {sensor.Nome}");
                Console.WriteLine($"Tipo: {sensor.Tipo}");
                Console.WriteLine($"Ativo: {sensor.Ativo}");
                Console.WriteLine($"Instalação: {sensor.DataInstalacao:dd/MM/yyyy HH:mm:ss}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(sensor.ColetarDados());
                Console.ResetColor();

                ExibirSeparador();
            }
        }

        static void ListarCenariosAmbientais(List<CenarioAmbiental> cenarios)
        {
            ExibirTitulo("CENÁRIOS AMBIENTAIS DISPONÍVEIS");

            foreach (CenarioAmbiental cenario in cenarios)
            {
                ExibirNivelRisco(cenario.RiscoEsperado);
                Console.WriteLine(cenario);
                ExibirSeparador();
            }
        }

        static void RegistrarMedicaoPorCenario(
            List<RegiaoMonitorada> regioes,
            List<CenarioAmbiental> cenarios,
            List<MedicaoClimatica> historicoMedicoes,
            List<Alerta> historicoAlertas,
            AlertaService alertaService,
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            ExibirTitulo("REGISTRAR MEDIÇÃO POR CENÁRIO");

            RegiaoMonitorada? regiaoSelecionada = SelecionarRegiao(regioes);

            if (regiaoSelecionada == null)
            {
                return;
            }

            CenarioAmbiental? cenarioSelecionado = SelecionarCenario(cenarios);

            if (cenarioSelecionado == null)
            {
                return;
            }

            Random random = new Random();

            MedicaoClimatica medicao = cenarioSelecionado.GerarMedicao(random);

            historicoMedicoes.Add(medicao);

            Alerta alerta = alertaService.GerarAlerta(medicao, regiaoSelecionada);
            historicoAlertas.Add(alerta);

            SalvarAlertaEmArquivo(alerta, cenarioSelecionado, arquivoRepository, caminhoHistoricoAlertas);

            ExibirMensagemSucesso("Medição registrada com sucesso.");

            Console.WriteLine($"\nCenário aplicado: {cenarioSelecionado.Nome}");
            Console.WriteLine($"Descrição: {cenarioSelecionado.Descricao}");

            Console.WriteLine("\nDados climáticos coletados:");
            Console.WriteLine(medicao);

            Console.WriteLine("\nAlerta gerado:");
            ExibirAlertaColorido(alerta, alertaService);
        }

        static RegiaoMonitorada? SelecionarRegiao(List<RegiaoMonitorada> regioes)
        {
            Console.WriteLine("Escolha a região monitorada:\n");

            foreach (RegiaoMonitorada regiao in regioes)
            {
                Console.WriteLine($"{regiao.Id} - {regiao.Nome}");
            }

            Console.Write("\nDigite o ID da região: ");

            bool idValido = int.TryParse(Console.ReadLine(), out int idRegiao);

            if (!idValido)
            {
                ExibirMensagemErro("ID inválido. Digite um número.");
                return null;
            }

            RegiaoMonitorada? regiaoSelecionada = regioes.FirstOrDefault(r => r.Id == idRegiao);

            if (regiaoSelecionada == null)
            {
                ExibirMensagemErro("Região não encontrada.");
                return null;
            }

            return regiaoSelecionada;
        }

        static CenarioAmbiental? SelecionarCenario(List<CenarioAmbiental> cenarios)
        {
            Console.WriteLine("\nEscolha o cenário ambiental:\n");

            foreach (CenarioAmbiental cenario in cenarios)
            {
                Console.WriteLine($"{cenario.Id} - {cenario.Nome} | Risco esperado: {cenario.RiscoEsperado}");
            }

            Console.WriteLine("0 - Escolher cenário aleatório");

            Console.Write("\nDigite o ID do cenário: ");

            bool idValido = int.TryParse(Console.ReadLine(), out int idCenario);

            if (!idValido)
            {
                ExibirMensagemErro("ID inválido. Digite um número.");
                return null;
            }

            Random random = new Random();

            if (idCenario == 0)
            {
                return cenarios[random.Next(cenarios.Count)];
            }

            CenarioAmbiental? cenarioSelecionado = cenarios.FirstOrDefault(c => c.Id == idCenario);

            if (cenarioSelecionado == null)
            {
                ExibirMensagemErro("Cenário não encontrado.");
                return null;
            }

            return cenarioSelecionado;
        }

        static void ListarAlertasPorPrioridade(List<Alerta> alertas, AlertaService alertaService)
        {
            if (!alertas.Any())
            {
                ExibirMensagemErro("Nenhum alerta foi gerado ainda nesta execução.");
                return;
            }

            List<Alerta> alertasPriorizados = alertaService.OrdenarPorPrioridade(alertas);

            ExibirTitulo("ALERTAS ORDENADOS POR PRIORIDADE");

            foreach (Alerta alerta in alertasPriorizados)
            {
                ExibirAlertaColorido(alerta, alertaService);
                ExibirSeparador();
            }
        }

        static void ListarAlertasCriticosDaExecucaoAtual(List<Alerta> alertas)
        {
            List<Alerta> criticos = alertas
                .Where(a => a.NivelRisco == NivelRisco.Critico)
                .OrderByDescending(a => a.Prioridade)
                .ToList();

            if (!criticos.Any())
            {
                ExibirMensagemErro("Nenhum alerta crítico encontrado nesta execução.");
                Console.WriteLine("Dica: use a opção 8 para gerar um cenário crítico ou a opção 10 para consultar críticos salvos em arquivo.");
                return;
            }

            ExibirTitulo("ALERTAS CRÍTICOS DA EXECUÇÃO ATUAL");

            foreach (Alerta alerta in criticos)
            {
                ExibirNivelRisco(alerta.NivelRisco);
                Console.WriteLine(alerta);
                ExibirSeparador();
            }
        }

        static void ExibirRelatorio(
            List<RegiaoMonitorada> regioes,
            List<Sensor> sensores,
            List<MedicaoClimatica> medicoes,
            List<Alerta> alertas)
        {
            ExibirTitulo("RELATÓRIO OPERACIONAL DA EXECUÇÃO ATUAL");

            Console.WriteLine($"Total de regiões monitoradas: {regioes.Count}");
            Console.WriteLine($"Total de sensores cadastrados: {sensores.Count}");
            Console.WriteLine($"Sensores ativos: {sensores.Count(s => s.Ativo)}");
            Console.WriteLine($"Total de medições registradas: {medicoes.Count}");
            Console.WriteLine($"Total de alertas gerados: {alertas.Count}");

            Console.WriteLine("\nAlertas por nível de risco:");
            Console.WriteLine($"Críticos: {alertas.Count(a => a.NivelRisco == NivelRisco.Critico)}");
            Console.WriteLine($"Altos: {alertas.Count(a => a.NivelRisco == NivelRisco.Alto)}");
            Console.WriteLine($"Médios: {alertas.Count(a => a.NivelRisco == NivelRisco.Medio)}");
            Console.WriteLine($"Baixos: {alertas.Count(a => a.NivelRisco == NivelRisco.Baixo)}");

            if (alertas.Any())
            {
                Alerta alertaMaisCritico = alertas.OrderByDescending(a => a.Prioridade).First();

                Console.WriteLine($"\nMaior prioridade registrada: {alertaMaisCritico.Prioridade}");
                Console.WriteLine($"Região mais crítica: {alertaMaisCritico.Regiao.Nome}");

                Console.Write("\nNível do alerta mais crítico: ");
                ExibirNivelRisco(alertaMaisCritico.NivelRisco);
            }
        }

        static void TestarExcecao(AlertaService alertaService, RegiaoMonitorada regiao)
        {
            ExibirTitulo("TESTE DE EXCEÇÃO");

            Console.WriteLine("Testando medição inválida...");

            MedicaoClimatica medicaoInvalida = new MedicaoClimatica(
                temperatura: 120,
                umidade: 20,
                velocidadeVento: 10,
                fumacaDetectada: true,
                confiancaIA: 0.90
            );

            alertaService.GerarAlerta(medicaoInvalida, regiao);
        }

        static void GerarCenarioCritico(
            List<RegiaoMonitorada> regioes,
            List<CenarioAmbiental> cenarios,
            List<MedicaoClimatica> historicoMedicoes,
            List<Alerta> historicoAlertas,
            AlertaService alertaService,
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            ExibirTitulo("CENÁRIO CRÍTICO SIMULADO");

            Random random = new Random();

            List<CenarioAmbiental> cenariosCriticos = cenarios
                .Where(c => c.RiscoEsperado == NivelRisco.Critico)
                .ToList();

            CenarioAmbiental cenarioCritico = cenariosCriticos[random.Next(cenariosCriticos.Count)];
            RegiaoMonitorada regiaoCritica = regioes[random.Next(regioes.Count)];

            MedicaoClimatica medicaoCritica = cenarioCritico.GerarMedicao(random);

            historicoMedicoes.Add(medicaoCritica);

            Alerta alerta = alertaService.GerarAlerta(medicaoCritica, regiaoCritica);
            historicoAlertas.Add(alerta);

            SalvarAlertaEmArquivo(alerta, cenarioCritico, arquivoRepository, caminhoHistoricoAlertas);

            Console.WriteLine($"Região selecionada: {regiaoCritica.Nome}");
            Console.WriteLine($"Cenário aplicado: {cenarioCritico.Nome}");
            Console.WriteLine($"Descrição: {cenarioCritico.Descricao}");

            Console.WriteLine("\nMedição crítica:");
            Console.WriteLine(medicaoCritica);

            Console.WriteLine("\nAlerta gerado:");
            ExibirAlertaColorido(alerta, alertaService);
        }

        static void ExecutarSimulacaoEmLote(
            List<RegiaoMonitorada> regioes,
            List<CenarioAmbiental> cenarios,
            List<MedicaoClimatica> historicoMedicoes,
            List<Alerta> historicoAlertas,
            AlertaService alertaService,
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            ExibirTitulo("SIMULAÇÃO EM LOTE");

            Console.Write("Quantas simulações deseja executar? ");

            bool quantidadeValida = int.TryParse(Console.ReadLine(), out int quantidade);

            if (!quantidadeValida || quantidade <= 0)
            {
                ExibirMensagemErro("Quantidade inválida.");
                return;
            }

            Random random = new Random();

            for (int i = 1; i <= quantidade; i++)
            {
                RegiaoMonitorada regiao = regioes[random.Next(regioes.Count)];
                CenarioAmbiental cenario = cenarios[random.Next(cenarios.Count)];
                MedicaoClimatica medicao = cenario.GerarMedicao(random);

                historicoMedicoes.Add(medicao);

                Alerta alerta = alertaService.GerarAlerta(medicao, regiao);
                historicoAlertas.Add(alerta);

                SalvarAlertaEmArquivo(alerta, cenario, arquivoRepository, caminhoHistoricoAlertas);

                Console.WriteLine($"\nSimulação {i}");
                Console.WriteLine($"Região: {regiao.Nome}");
                Console.WriteLine($"Cenário: {cenario.Nome}");
                Console.WriteLine($"Risco classificado: {alerta.NivelRisco}");
                Console.WriteLine($"Prioridade: {alerta.Prioridade}");
                ExibirSeparador();
            }

            ExibirMensagemSucesso($"{quantidade} simulações executadas com sucesso.");
        }

        static void SalvarAlertaEmArquivo(
            Alerta alerta,
            CenarioAmbiental cenario,
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            string linha = $"{alerta.DataCriacao:dd/MM/yyyy HH:mm:ss} | " +
                           $"Cenário: {cenario.Nome} | " +
                           $"Região: {alerta.Regiao.Nome} | " +
                           $"Risco: {alerta.NivelRisco} | " +
                           $"Prioridade: {alerta.Prioridade} | " +
                           $"Status: {alerta.Status} | " +
                           $"Recomendação: {alerta.Recomendacao}";

            arquivoRepository.SalvarLinha(caminhoHistoricoAlertas, linha);
        }

        static void ConsultarHistoricoArquivo(
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            List<string> linhas = arquivoRepository.LerLinhas(caminhoHistoricoAlertas);

            if (!linhas.Any())
            {
                ExibirMensagemErro("Nenhum histórico salvo em arquivo até o momento.");
                return;
            }

            ExibirTitulo("HISTÓRICO DE ALERTAS SALVO EM ARQUIVO");

            foreach (string linha in linhas)
            {
                Console.WriteLine(linha);
                ExibirSeparador();
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Arquivo salvo em: {caminhoHistoricoAlertas}");
            Console.ResetColor();
        }

        static void ConsultarCriticosArquivo(
            IArquivoRepository arquivoRepository,
            string caminhoHistoricoAlertas)
        {
            List<string> linhas = arquivoRepository.LerLinhas(caminhoHistoricoAlertas);

            List<string> criticos = linhas
                .Where(linha => linha.Contains("Risco: Critico"))
                .ToList();

            if (!criticos.Any())
            {
                ExibirMensagemErro("Nenhum alerta crítico salvo em arquivo encontrado.");
                return;
            }

            ExibirTitulo("ALERTAS CRÍTICOS SALVOS EM ARQUIVO");

            Console.ForegroundColor = ConsoleColor.Red;

            foreach (string linha in criticos)
            {
                Console.WriteLine(linha);
                ExibirSeparador();
            }

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Arquivo consultado em: {caminhoHistoricoAlertas}");
            Console.ResetColor();
        }

        static void ExibirAlertaColorido(Alerta alerta, AlertaService alertaService)
        {
            ExibirNivelRisco(alerta.NivelRisco);

            Console.WriteLine(alerta);

            if (alertaService.DeveAcionarEmergencia(alerta))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nStatus operacional: EMERGÊNCIA DEVE SER ACIONADA.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nStatus operacional: monitoramento e resposta conforme prioridade.");
                Console.ResetColor();
            }
        }

        static void ExibirNivelRisco(NivelRisco nivelRisco)
        {
            switch (nivelRisco)
            {
                case NivelRisco.Critico:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("NÍVEL DE RISCO: CRÍTICO");
                    break;

                case NivelRisco.Alto:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("NÍVEL DE RISCO: ALTO");
                    break;

                case NivelRisco.Medio:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("NÍVEL DE RISCO: MÉDIO");
                    break;

                case NivelRisco.Baixo:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("NÍVEL DE RISCO: BAIXO");
                    break;
            }

            Console.ResetColor();
        }

        static void ExibirTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==================================================");
            Console.WriteLine($" {titulo}");
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }

        static void ExibirSeparador()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();
        }

        static void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{mensagem}");
            Console.ResetColor();
        }

        static void ExibirMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{mensagem}\n");
            Console.ResetColor();
        }

        static void PausarTela(int opcao)
        {
            if (opcao != 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ResetColor();
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}