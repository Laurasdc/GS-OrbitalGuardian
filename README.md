# Orbital Guardian

## 👥 Membros
- Laura Souza de Carvalho RM: 556320
- Vinicius Henrique RM: 556908
- Enzo Dias RM: 558225
- Gustavo Pierre RM: 558928
- Gabriel Belo RM: 551669

## Plataforma de Triagem e Priorização de Alertas Climáticos

O **Orbital Guardian** é uma aplicação em **C# com .NET Console** desenvolvida para simular uma central de monitoramento ambiental. O sistema usa regiões monitoradas, sensores simulados, cenários climáticos e uma lógica de classificação de risco para gerar alertas, calcular prioridade operacional e apoiar decisões em situações de risco climático.

---

## Objetivo

O projeto tem como objetivo identificar riscos ambientais, principalmente relacionados a queimadas, e indicar quais alertas devem ser atendidos primeiro.

A solução considera fatores como:

- Temperatura;
- Umidade;
- Velocidade do vento;
- Detecção simulada de fumaça;
- Confiança da IA simulada;
- Proximidade de áreas habitadas;
- Histórico de ocorrências da região.

---

## Funcionalidades

- Listagem de regiões monitoradas;
- Listagem de sensores ambientais e orbitais;
- Registro de medições por cenário ambiental;
- Classificação de risco: Baixo, Médio, Alto ou Crítico;
- Cálculo de prioridade operacional;
- Geração de alertas;
- Histórico de alertas em arquivo `.txt`;
- Filtro de alertas críticos;
- Simulação em lote;
- Abertura e finalização de ocorrências operacionais;
- Exportação de relatório operacional;
- Tratamento de exceções.

---

## Cenários Simulados

O sistema possui diferentes cenários ambientais:

- Rotina normal de monitoramento;
- Calor seco em área vegetal;
- Fumaça detectada por visão computacional;
- Vento forte com baixa umidade;
- Queimada crítica próxima de área habitada;
- Pós-alerta em observação;
- Anomalia térmica sem fumaça;
- Foco inicial de incêndio.

---

## Tecnologias Utilizadas

- C#;
- .NET;
- Console Application;
- Programação Orientada a Objetos;
- LINQ;
- DateTime;
- Manipulação de arquivos `.txt`.

---

## Conceitos Aplicados

O projeto aplica os principais conceitos solicitados no enunciado:

- Classes públicas, privadas e estáticas;
- Herança e polimorfismo;
- Classe abstrata `Sensor`;
- Interfaces;
- Injeção de dependência;
- Struct `Coordenada`;
- Classe `partial`;
- Tratamento de exceções específicas;
- Manipulação de `DateTime`;
- Organização em pastas;
- Manipulação de arquivos.

---

## Estrutura do Projeto

Estrutura Pastas: <img width="281" height="857" alt="estrutura pastas" src="https://github.com/user-attachments/assets/e7fb45d0-692d-4d32-9a23-a5f3ee778250" />

---

## Menu Principal

```txt
1  - Listar regiões monitoradas
2  - Listar sensores
3  - Registrar medição por cenário ambiental
4  - Listar alertas por prioridade
5  - Filtrar alertas críticos da execução atual
6  - Exibir relatório operacional
7  - Testar exceção de medição inválida
8  - Gerar cenário crítico simulado
9  - Consultar histórico salvo em arquivo
10 - Filtrar alertas críticos salvos em arquivo
11 - Executar simulação em lote
12 - Listar cenários ambientais disponíveis
13 - Abrir ocorrência para alerta crítico
14 - Listar ocorrências operacionais
15 - Finalizar ocorrência
16 - Exportar relatório operacional
17 - Limpar histórico salvo em arquivo
0  - Sair
```

---

## Manipulação de Arquivos

O projeto não utiliza banco de dados. Em vez disso, utiliza arquivos `.txt` para armazenar informações geradas durante a execução.

Arquivos gerados:

```txt
Data/historico_alertas.txt
Data/relatorio_operacional.txt
```

O histórico salva dados como data, cenário, região, risco, prioridade, status e recomendação. O relatório operacional resume os alertas, ocorrências e prioridades da execução.

---

## Fluxo do Sistema

```txt
Região monitorada
↓
Cenário ambiental
↓
Medição climática simulada
↓
Classificação de risco
↓
Cálculo de prioridade
↓
Geração de alerta
↓
Registro em arquivo
↓
Abertura de ocorrência
↓
Finalização da ocorrência
↓
Relatório operacional
```

---

## Diagrama de Fluxo

<img width="601" height="1401" alt="Diagrama drawio" src="https://github.com/user-attachments/assets/a9bbd947-d502-4f46-b16e-041df308e7d0" />

---

## Evidências de Execução

As evidências de execução estão disponíveis na pasta:

`docs/evidencias-execucao`

## Como Executar

1. Abra o projeto no Visual Studio.
2. Execute a aplicação.
3. Use o menu do console para testar as funcionalidades.

Sugestão de teste:

```txt
8  -> Gerar cenário crítico simulado
13 -> Abrir ocorrência para alerta crítico
14 -> Listar ocorrências operacionais
15 -> Finalizar ocorrência
16 -> Exportar relatório operacional
9  -> Consultar histórico salvo em arquivo
```

---

## Integração com a Global Solution

O projeto está alinhado ao tema da Global Solution ao propor uma solução tecnológica para prevenção, monitoramento e resposta a desastres climáticos.

O MVP atual é focado em queimadas e risco ambiental, mas a arquitetura pode ser expandida futuramente para enchentes, deslizamentos e outros eventos climáticos.
