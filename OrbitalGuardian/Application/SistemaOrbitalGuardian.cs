using System.Text;
using OrbitalGuardian.Enums;
using OrbitalGuardian.Exceptions;
using OrbitalGuardian.Interfaces;
using OrbitalGuardian.Models;
using OrbitalGuardian.Repositories;
using OrbitalGuardian.Services;

namespace OrbitalGuardian.Application
{
    public class SistemaOrbitalGuardian
    {
        private readonly IClassificadorRisco _classificador;
        private readonly ICalculadoraPrioridade _calculadora;
        private readonly AlertaService _alertaService;
        private readonly IArquivoRepository _arquivoRepository;
        private readonly string _caminhoHistoricoAlertas;
        private readonly string _caminhoRelatorioOperacional;
        private readonly Random _random;

        private readonly List<Sensor> _sensores;
        private readonly List<RegiaoMonitorada> _regioes;
        private readonly List<CenarioAmbiental> _cenarios;
        private readonly List<MedicaoClimatica> _historicoMedicoes;
        private readonly List<Alerta> _historicoAlertas;
        private readonly List<Ocorrencia> _historicoOcorrencias;

        public SistemaOrbitalGuardian()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Orbital Guardian - Plataforma de Triagem Climática";

            _classificador = new ClassificadorRiscoService();
            _calculadora = new CalculadoraPrioridadeService();
            _alertaService = new AlertaService(_classificador, _calculadora);

            _arquivoRepository = new ArquivoRepository();

            _caminhoHistoricoAlertas = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "historico_alertas.txt"
            );

            _caminhoRelatorioOperacional = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "relatorio_operacional.txt"
            );

            _random = new Random();

            _sensores = CriarSensores();
            _regioes = CriarRegioes();
            _cenarios = CriarCenariosAmbientais();

            _historicoMedicoes = new List<MedicaoClimatica>();
            _historicoAlertas = new List<Alerta>();
            _historicoOcorrencias = new List<Ocorrencia>();
        }

        public void Executar()
        {
            int opcao;

            do
            {
                ExibirMenu();
                Console.Write("Escolha uma opção: ");

                bool entradaValida = int.TryParse(Console.ReadLine(), out opcao);

                if (!entradaValida)
                {
                    ExibirMensagemErro("Opção inválida. Digite um número.");
                    PausarTela(-1);
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            ListarRegioes();
                            break;

                        case 2:
                            ListarSensores();
                            break;

                        case 3:
                            RegistrarMedicaoPorCenario();
                            break;

                        case 4:
                            ListarAlertasPorPrioridade();
                            break;

                        case 5:
                            ListarAlertasCriticosDaExecucaoAtual();
                            break;

                        case 6:
                            ExibirRelatorio();
                            break;

                        case 7:
                            TestarExcecao();
                            break;

                        case 8:
                            GerarCenarioCritico();
                            break;

                        case 9:
                            ConsultarHistoricoArquivo();
                            break;

                        case 10:
                            ConsultarCriticosArquivo();
                            break;

                        case 11:
                            ExecutarSimulacaoEmLote();
                            break;

                        case 12:
                            ListarCenariosAmbientais();
                            break;

                        case 13:
                            AbrirOcorrenciaParaAlertaCritico();
                            break;

                        case 14:
                            ListarOcorrencias();
                            break;

                        case 15:
                            FinalizarOcorrencia();
                            break;

                        case 16:
                            ExportarRelatorioOperacional();
                            break;

                        case 17:
                            LimparHistoricoSalvoEmArquivo();
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

        private List<Sensor> CriarSensores()
        {
            return new List<Sensor>
            {
                new SensorAmbiental(1, "Sensor Ambiental A1"),
                new SensorOrbital(2, "Satélite Simulado O1"),
                new SensorAmbiental(3, "Sensor Ambiental B2"),
                new SensorOrbital(4, "Satélite Simulado O2")
            };
        }

        private List<RegiaoMonitorada> CriarRegioes()
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

        private List<CenarioAmbiental> CriarCenariosAmbientais()
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

        private void ExibirMenu()
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
            Console.WriteLine("13 - Abrir ocorrência para alerta crítico");
            Console.WriteLine("14 - Listar ocorrências operacionais");
            Console.WriteLine("15 - Finalizar ocorrência");
            Console.WriteLine("16 - Exportar relatório operacional");
            Console.WriteLine("17 - Limpar histórico salvo em arquivo");
            Console.WriteLine("0  - Sair");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }

        private void ListarRegioes()
        {
            ExibirTitulo("REGIÕES MONITORADAS");

            foreach (RegiaoMonitorada regiao in _regioes)
            {
                Console.WriteLine($"ID: {regiao.Id}");
                Console.WriteLine($"Nome: {regiao.Nome}");
                Console.WriteLine($"Localização: {regiao.Localizacao}");
                Console.WriteLine($"Distância de área habitada: {regiao.DistanciaAreaHabitadaKm} km");
                Console.WriteLine($"Histórico de ocorrências: {regiao.HistoricoOcorrencias}");
                ExibirSeparador();
            }
        }

        private void ListarSensores()
        {
            ExibirTitulo("SENSORES DO SISTEMA");

            foreach (Sensor sensor in _sensores)
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

        private void ListarCenariosAmbientais()
        {
            ExibirTitulo("CENÁRIOS AMBIENTAIS DISPONÍVEIS");

            foreach (CenarioAmbiental cenario in _cenarios)
            {
                ExibirNivelRisco(cenario.RiscoEsperado);
                Console.WriteLine(cenario);
                ExibirSeparador();
            }
        }

        private void RegistrarMedicaoPorCenario()
        {
            ExibirTitulo("REGISTRAR MEDIÇÃO POR CENÁRIO");

            RegiaoMonitorada? regiaoSelecionada = SelecionarRegiao();

            if (regiaoSelecionada == null)
            {
                return;
            }

            CenarioAmbiental? cenarioSelecionado = SelecionarCenario();

            if (cenarioSelecionado == null)
            {
                return;
            }

            MedicaoClimatica medicao = cenarioSelecionado.GerarMedicao(_random);

            _historicoMedicoes.Add(medicao);

            Alerta alerta = _alertaService.GerarAlerta(medicao, regiaoSelecionada);
            _historicoAlertas.Add(alerta);

            SalvarAlertaEmArquivo(alerta, cenarioSelecionado);

            ExibirMensagemSucesso("Medição registrada com sucesso.");

            Console.WriteLine($"\nCenário aplicado: {cenarioSelecionado.Nome}");
            Console.WriteLine($"Descrição: {cenarioSelecionado.Descricao}");

            Console.WriteLine("\nDados climáticos coletados:");
            Console.WriteLine(medicao);

            Console.WriteLine("\nAlerta gerado:");
            ExibirAlertaColorido(alerta);
        }

        private RegiaoMonitorada? SelecionarRegiao()
        {
            Console.WriteLine("Escolha a região monitorada:\n");

            foreach (RegiaoMonitorada regiao in _regioes)
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

            RegiaoMonitorada? regiaoSelecionada = _regioes.FirstOrDefault(r => r.Id == idRegiao);

            if (regiaoSelecionada == null)
            {
                ExibirMensagemErro("Região não encontrada.");
                return null;
            }

            return regiaoSelecionada;
        }

        private CenarioAmbiental? SelecionarCenario()
        {
            Console.WriteLine("\nEscolha o cenário ambiental:\n");

            foreach (CenarioAmbiental cenario in _cenarios)
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

            if (idCenario == 0)
            {
                return _cenarios[_random.Next(_cenarios.Count)];
            }

            CenarioAmbiental? cenarioSelecionado = _cenarios.FirstOrDefault(c => c.Id == idCenario);

            if (cenarioSelecionado == null)
            {
                ExibirMensagemErro("Cenário não encontrado.");
                return null;
            }

            return cenarioSelecionado;
        }

        private void ListarAlertasPorPrioridade()
        {
            if (!_historicoAlertas.Any())
            {
                ExibirMensagemErro("Nenhum alerta foi gerado ainda nesta execução.");
                return;
            }

            List<Alerta> alertasPriorizados = _alertaService.OrdenarPorPrioridade(_historicoAlertas);

            ExibirTitulo("ALERTAS ORDENADOS POR PRIORIDADE");

            for (int i = 0; i < alertasPriorizados.Count; i++)
            {
                Console.WriteLine($"Índice: {i + 1}");
                ExibirAlertaColorido(alertasPriorizados[i]);
                ExibirSeparador();
            }
        }

        private void ListarAlertasCriticosDaExecucaoAtual()
        {
            List<Alerta> criticos = ObterAlertasCriticosDaExecucao();

            if (!criticos.Any())
            {
                ExibirMensagemErro("Nenhum alerta crítico encontrado nesta execução.");
                Console.WriteLine("Dica: use a opção 8 para gerar um cenário crítico ou a opção 10 para consultar críticos salvos em arquivo.");
                return;
            }

            ExibirTitulo("ALERTAS CRÍTICOS DA EXECUÇÃO ATUAL");

            for (int i = 0; i < criticos.Count; i++)
            {
                Console.WriteLine($"Índice: {i + 1}");
                ExibirNivelRisco(criticos[i].NivelRisco);
                Console.WriteLine(criticos[i]);
                ExibirSeparador();
            }
        }

        private List<Alerta> ObterAlertasCriticosDaExecucao()
        {
            return _historicoAlertas
                .Where(a => a.NivelRisco == NivelRisco.Critico)
                .OrderByDescending(a => a.Prioridade)
                .ToList();
        }

        private void ExibirRelatorio()
        {
            ExibirTitulo("RELATÓRIO OPERACIONAL DA EXECUÇÃO ATUAL");
            Console.WriteLine(GerarTextoRelatorioOperacional());
        }

        private string GerarTextoRelatorioOperacional()
        {
            StringBuilder relatorio = new StringBuilder();

            relatorio.AppendLine("===== RELATÓRIO OPERACIONAL ORBITAL GUARDIAN =====");
            relatorio.AppendLine($"Data de geração: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            relatorio.AppendLine();

            relatorio.AppendLine($"Total de regiões monitoradas: {_regioes.Count}");
            relatorio.AppendLine($"Total de sensores cadastrados: {_sensores.Count}");
            relatorio.AppendLine($"Sensores ativos: {_sensores.Count(s => s.Ativo)}");
            relatorio.AppendLine($"Total de medições registradas: {_historicoMedicoes.Count}");
            relatorio.AppendLine($"Total de alertas gerados: {_historicoAlertas.Count}");
            relatorio.AppendLine($"Total de ocorrências abertas: {_historicoOcorrencias.Count}");
            relatorio.AppendLine($"Ocorrências finalizadas: {_historicoOcorrencias.Count(o => o.Status == StatusOcorrencia.Finalizada)}");

            relatorio.AppendLine();
            relatorio.AppendLine("Alertas por nível de risco:");
            relatorio.AppendLine($"Críticos: {_historicoAlertas.Count(a => a.NivelRisco == NivelRisco.Critico)}");
            relatorio.AppendLine($"Altos: {_historicoAlertas.Count(a => a.NivelRisco == NivelRisco.Alto)}");
            relatorio.AppendLine($"Médios: {_historicoAlertas.Count(a => a.NivelRisco == NivelRisco.Medio)}");
            relatorio.AppendLine($"Baixos: {_historicoAlertas.Count(a => a.NivelRisco == NivelRisco.Baixo)}");

            if (_historicoAlertas.Any())
            {
                Alerta alertaMaisCritico = _historicoAlertas.OrderByDescending(a => a.Prioridade).First();

                double prioridadeMedia = _historicoAlertas.Average(a => a.Prioridade);
                double percentualCritico = (_historicoAlertas.Count(a => a.NivelRisco == NivelRisco.Critico) * 100.0) / _historicoAlertas.Count;

                string regiaoComMaisAlertas = _historicoAlertas
                    .GroupBy(a => a.Regiao.Nome)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key;

                relatorio.AppendLine();
                relatorio.AppendLine($"Maior prioridade registrada: {alertaMaisCritico.Prioridade}");
                relatorio.AppendLine($"Prioridade média: {prioridadeMedia:F2}");
                relatorio.AppendLine($"Percentual de alertas críticos: {percentualCritico:F1}%");
                relatorio.AppendLine($"Região com mais alertas: {regiaoComMaisAlertas}");
                relatorio.AppendLine($"Último alerta gerado em: {_historicoAlertas.Last().DataCriacao:dd/MM/yyyy HH:mm:ss}");
            }

            return relatorio.ToString();
        }

        private void TestarExcecao()
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

            _alertaService.GerarAlerta(medicaoInvalida, _regioes[0]);
        }

        private void GerarCenarioCritico()
        {
            ExibirTitulo("CENÁRIO CRÍTICO SIMULADO");

            List<CenarioAmbiental> cenariosCriticos = _cenarios
                .Where(c => c.RiscoEsperado == NivelRisco.Critico)
                .ToList();

            CenarioAmbiental cenarioCritico = cenariosCriticos[_random.Next(cenariosCriticos.Count)];
            RegiaoMonitorada regiaoCritica = _regioes[_random.Next(_regioes.Count)];

            MedicaoClimatica medicaoCritica = cenarioCritico.GerarMedicao(_random);

            _historicoMedicoes.Add(medicaoCritica);

            Alerta alerta = _alertaService.GerarAlerta(medicaoCritica, regiaoCritica);
            _historicoAlertas.Add(alerta);

            SalvarAlertaEmArquivo(alerta, cenarioCritico);

            Console.WriteLine($"Região selecionada: {regiaoCritica.Nome}");
            Console.WriteLine($"Cenário aplicado: {cenarioCritico.Nome}");
            Console.WriteLine($"Descrição: {cenarioCritico.Descricao}");

            Console.WriteLine("\nMedição crítica:");
            Console.WriteLine(medicaoCritica);

            Console.WriteLine("\nAlerta gerado:");
            ExibirAlertaColorido(alerta);
        }

        private void ExecutarSimulacaoEmLote()
        {
            ExibirTitulo("SIMULAÇÃO EM LOTE");

            Console.Write("Quantas simulações deseja executar? ");

            bool quantidadeValida = int.TryParse(Console.ReadLine(), out int quantidade);

            if (!quantidadeValida || quantidade <= 0)
            {
                ExibirMensagemErro("Quantidade inválida.");
                return;
            }

            for (int i = 1; i <= quantidade; i++)
            {
                RegiaoMonitorada regiao = _regioes[_random.Next(_regioes.Count)];
                CenarioAmbiental cenario = _cenarios[_random.Next(_cenarios.Count)];
                MedicaoClimatica medicao = cenario.GerarMedicao(_random);

                _historicoMedicoes.Add(medicao);

                Alerta alerta = _alertaService.GerarAlerta(medicao, regiao);
                _historicoAlertas.Add(alerta);

                SalvarAlertaEmArquivo(alerta, cenario);

                Console.WriteLine($"\nSimulação {i}");
                Console.WriteLine($"Região: {regiao.Nome}");
                Console.WriteLine($"Cenário: {cenario.Nome}");
                Console.WriteLine($"Risco classificado: {alerta.NivelRisco}");
                Console.WriteLine($"Prioridade: {alerta.Prioridade}");
                ExibirSeparador();
            }

            ExibirMensagemSucesso($"{quantidade} simulações executadas com sucesso.");
        }

        private void AbrirOcorrenciaParaAlertaCritico()
        {
            ExibirTitulo("ABRIR OCORRÊNCIA PARA ALERTA CRÍTICO");

            List<Alerta> criticos = ObterAlertasCriticosDaExecucao()
                .Where(alerta => !_historicoOcorrencias.Any(o => o.Alerta == alerta))
                .ToList();

            if (!criticos.Any())
            {
                ExibirMensagemErro("Nenhum alerta crítico disponível para abertura de ocorrência.");
                Console.WriteLine("Dica: gere um cenário crítico pela opção 8 antes de abrir uma ocorrência.");
                return;
            }

            Console.WriteLine("Alertas críticos disponíveis:\n");

            for (int i = 0; i < criticos.Count; i++)
            {
                Console.WriteLine($"{i + 1} - Região: {criticos[i].Regiao.Nome} | Prioridade: {criticos[i].Prioridade} | Data: {criticos[i].DataCriacao:dd/MM/yyyy HH:mm:ss}");
            }

            Console.Write("\nEscolha o índice do alerta: ");

            bool indiceValido = int.TryParse(Console.ReadLine(), out int indice);

            if (!indiceValido || indice < 1 || indice > criticos.Count)
            {
                ExibirMensagemErro("Índice inválido.");
                return;
            }

            Alerta alertaSelecionado = criticos[indice - 1];

            string responsavel = SelecionarResponsavel();
            string observacao = SelecionarObservacaoInicial();

            Ocorrencia ocorrencia = new Ocorrencia(
                _historicoOcorrencias.Count + 1,
                alertaSelecionado,
                responsavel,
                observacao
            );

            ocorrencia.IniciarAtendimento();

            _historicoOcorrencias.Add(ocorrencia);

            ExibirMensagemSucesso("Ocorrência aberta com sucesso.");
            Console.WriteLine();
            Console.WriteLine(ocorrencia);
        }

        private void ListarOcorrencias()
        {
            ExibirTitulo("OCORRÊNCIAS OPERACIONAIS");

            if (!_historicoOcorrencias.Any())
            {
                ExibirMensagemErro("Nenhuma ocorrência foi aberta nesta execução.");
                return;
            }

            foreach (Ocorrencia ocorrencia in _historicoOcorrencias)
            {
                ExibirStatusOcorrencia(ocorrencia.Status);
                Console.WriteLine(ocorrencia);
                ExibirSeparador();
            }
        }

        private void FinalizarOcorrencia()
        {
            ExibirTitulo("FINALIZAR OCORRÊNCIA");

            List<Ocorrencia> ocorrenciasAbertas = _historicoOcorrencias
                .Where(o => o.Status != StatusOcorrencia.Finalizada)
                .ToList();

            if (!ocorrenciasAbertas.Any())
            {
                ExibirMensagemErro("Nenhuma ocorrência em aberto para finalizar.");
                return;
            }

            Console.WriteLine("Ocorrências em aberto:\n");

            foreach (Ocorrencia ocorrencia in ocorrenciasAbertas)
            {
                Console.WriteLine($"{ocorrencia.Id} - Região: {ocorrencia.Alerta.Regiao.Nome} | Status: {ocorrencia.Status} | Prioridade: {ocorrencia.Alerta.Prioridade}");
            }

            Console.Write("\nDigite o ID da ocorrência que deseja finalizar: ");

            bool idValido = int.TryParse(Console.ReadLine(), out int idOcorrencia);

            if (!idValido)
            {
                ExibirMensagemErro("ID inválido.");
                return;
            }

            Ocorrencia? ocorrenciaSelecionada = ocorrenciasAbertas.FirstOrDefault(o => o.Id == idOcorrencia);

            if (ocorrenciaSelecionada == null)
            {
                ExibirMensagemErro("Ocorrência não encontrada ou já finalizada.");
                return;
            }

            string conclusao = SelecionarObservacaoConclusao();

            ocorrenciaSelecionada.Finalizar(conclusao);

            ExibirMensagemSucesso("Ocorrência finalizada com sucesso.");
            Console.WriteLine();
            Console.WriteLine(ocorrenciaSelecionada);
        }

        private string SelecionarResponsavel()
        {
            ExibirTitulo("SELECIONAR RESPONSÁVEL PELO ATENDIMENTO");

            Console.WriteLine("1 - Equipe de Campo Norte");
            Console.WriteLine("2 - Brigada Ambiental");
            Console.WriteLine("3 - Defesa Civil");
            Console.WriteLine("4 - Centro de Operações Climáticas");
            Console.WriteLine("5 - Operador Manual");
            Console.WriteLine("6 - Informar responsável personalizado");

            Console.Write("\nEscolha uma opção: ");

            bool opcaoValida = int.TryParse(Console.ReadLine(), out int opcao);

            if (!opcaoValida)
            {
                return "Operador não informado";
            }

            switch (opcao)
            {
                case 1:
                    return "Equipe de Campo Norte";

                case 2:
                    return "Brigada Ambiental";

                case 3:
                    return "Defesa Civil";

                case 4:
                    return "Centro de Operações Climáticas";

                case 5:
                    return "Operador Manual";

                case 6:
                    Console.Write("Digite o nome do responsável: ");
                    string? responsavelPersonalizado = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(responsavelPersonalizado))
                    {
                        return "Operador não informado";
                    }

                    return responsavelPersonalizado;

                default:
                    return "Operador não informado";
            }
        }

        private string SelecionarObservacaoInicial()
        {
            ExibirTitulo("SELECIONAR OBSERVAÇÃO INICIAL");

            Console.WriteLine("1 - Ocorrência aberta após alerta crítico identificado pelo sistema.");
            Console.WriteLine("2 - Equipe enviada para verificação em campo.");
            Console.WriteLine("3 - Monitoramento intensificado por proximidade de área habitada.");
            Console.WriteLine("4 - Necessário acompanhamento contínuo por risco de propagação.");
            Console.WriteLine("5 - Acionamento preventivo devido à alta prioridade operacional.");
            Console.WriteLine("6 - Digitar observação personalizada");

            Console.Write("\nEscolha uma opção: ");

            bool opcaoValida = int.TryParse(Console.ReadLine(), out int opcao);

            if (!opcaoValida)
            {
                return "Ocorrência aberta para atendimento prioritário.";
            }

            switch (opcao)
            {
                case 1:
                    return "Ocorrência aberta após alerta crítico identificado pelo sistema.";

                case 2:
                    return "Equipe enviada para verificação em campo.";

                case 3:
                    return "Monitoramento intensificado por proximidade de área habitada.";

                case 4:
                    return "Necessário acompanhamento contínuo por risco de propagação.";

                case 5:
                    return "Acionamento preventivo devido à alta prioridade operacional.";

                case 6:
                    Console.Write("Digite a observação inicial: ");
                    string? observacaoPersonalizada = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(observacaoPersonalizada))
                    {
                        return "Ocorrência aberta para atendimento prioritário.";
                    }

                    return observacaoPersonalizada;

                default:
                    return "Ocorrência aberta para atendimento prioritário.";
            }
        }

        private string SelecionarObservacaoConclusao()
        {
            ExibirTitulo("SELECIONAR CONCLUSÃO DA OCORRÊNCIA");

            Console.WriteLine("1 - Área verificada e situação controlada.");
            Console.WriteLine("2 - Alerta confirmado e encaminhado para equipe responsável.");
            Console.WriteLine("3 - Risco reduzido após acompanhamento operacional.");
            Console.WriteLine("4 - Ocorrência encerrada sem necessidade de evacuação.");
            Console.WriteLine("5 - Foco monitorado e sem evolução crítica no momento.");
            Console.WriteLine("6 - Digitar conclusão personalizada");

            Console.Write("\nEscolha uma opção: ");

            bool opcaoValida = int.TryParse(Console.ReadLine(), out int opcao);

            if (!opcaoValida)
            {
                return "Ocorrência finalizada pelo operador.";
            }

            switch (opcao)
            {
                case 1:
                    return "Área verificada e situação controlada.";

                case 2:
                    return "Alerta confirmado e encaminhado para equipe responsável.";

                case 3:
                    return "Risco reduzido após acompanhamento operacional.";

                case 4:
                    return "Ocorrência encerrada sem necessidade de evacuação.";

                case 5:
                    return "Foco monitorado e sem evolução crítica no momento.";

                case 6:
                    Console.Write("Digite a conclusão da ocorrência: ");
                    string? conclusaoPersonalizada = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(conclusaoPersonalizada))
                    {
                        return "Ocorrência finalizada pelo operador.";
                    }

                    return conclusaoPersonalizada;

                default:
                    return "Ocorrência finalizada pelo operador.";
            }
        }

        private void ExportarRelatorioOperacional()
        {
            ExibirTitulo("EXPORTAR RELATÓRIO OPERACIONAL");

            string relatorio = GerarTextoRelatorioOperacional();

            _arquivoRepository.SalvarTexto(_caminhoRelatorioOperacional, relatorio);

            ExibirMensagemSucesso("Relatório operacional exportado com sucesso.");
            Console.WriteLine($"Arquivo gerado em: {_caminhoRelatorioOperacional}");
        }

        private void LimparHistoricoSalvoEmArquivo()
        {
            ExibirTitulo("LIMPAR HISTÓRICO SALVO EM ARQUIVO");

            Console.WriteLine("Essa ação limpará o arquivo de histórico de alertas.");
            Console.Write("Deseja continuar? Digite S para confirmar: ");

            string? confirmacao = Console.ReadLine();

            if (confirmacao?.Trim().ToUpper() != "S")
            {
                ExibirMensagemErro("Operação cancelada.");
                return;
            }

            _arquivoRepository.LimparArquivo(_caminhoHistoricoAlertas);

            ExibirMensagemSucesso("Histórico salvo em arquivo foi limpo com sucesso.");
        }

        private void SalvarAlertaEmArquivo(Alerta alerta, CenarioAmbiental cenario)
        {
            string linha = $"{alerta.DataCriacao:dd/MM/yyyy HH:mm:ss} | " +
                           $"Cenário: {cenario.Nome} | " +
                           $"Região: {alerta.Regiao.Nome} | " +
                           $"Risco: {alerta.NivelRisco} | " +
                           $"Prioridade: {alerta.Prioridade} | " +
                           $"Status: {alerta.Status} | " +
                           $"Recomendação: {alerta.Recomendacao}";

            _arquivoRepository.SalvarLinha(_caminhoHistoricoAlertas, linha);
        }

        private void ConsultarHistoricoArquivo()
        {
            List<string> linhas = _arquivoRepository.LerLinhas(_caminhoHistoricoAlertas);

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
            Console.WriteLine($"Arquivo salvo em: {_caminhoHistoricoAlertas}");
            Console.ResetColor();
        }

        private void ConsultarCriticosArquivo()
        {
            List<string> linhas = _arquivoRepository.LerLinhas(_caminhoHistoricoAlertas);

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
            Console.WriteLine($"Arquivo consultado em: {_caminhoHistoricoAlertas}");
            Console.ResetColor();
        }

        private void ExibirAlertaColorido(Alerta alerta)
        {
            ExibirNivelRisco(alerta.NivelRisco);

            Console.WriteLine(alerta);

            if (_alertaService.DeveAcionarEmergencia(alerta))
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

        private void ExibirNivelRisco(NivelRisco nivelRisco)
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

        private void ExibirStatusOcorrencia(StatusOcorrencia status)
        {
            switch (status)
            {
                case StatusOcorrencia.Aberta:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("STATUS DA OCORRÊNCIA: ABERTA");
                    break;

                case StatusOcorrencia.EmAtendimento:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("STATUS DA OCORRÊNCIA: EM ATENDIMENTO");
                    break;

                case StatusOcorrencia.Finalizada:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("STATUS DA OCORRÊNCIA: FINALIZADA");
                    break;
            }

            Console.ResetColor();
        }

        private void ExibirTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==================================================");
            Console.WriteLine($" {titulo}");
            Console.WriteLine("==================================================");
            Console.ResetColor();
        }

        private void ExibirSeparador()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();
        }

        private void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{mensagem}");
            Console.ResetColor();
        }

        private void ExibirMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{mensagem}\n");
            Console.ResetColor();
        }

        private void PausarTela(int opcao)
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