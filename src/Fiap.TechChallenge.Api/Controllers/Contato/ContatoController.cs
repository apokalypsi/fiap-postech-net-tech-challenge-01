using System.Diagnostics;
using System.Net;
using Fiap.TechChallenge.Command.v1.Contato;
using Fiap.TechChallenge.Contract.v1.Contato.AtualizarContato;
using Fiap.TechChallenge.Contract.v1.Contato.CriarContato;
using Fiap.TechChallenge.Contract.v1.Contato.ObterContatoPorId;
using Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;
using Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;
using Fiap.TechChallenge.Foundation.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace Fiap.TechChallenge.Api.Controllers.Contato;

/// <summary>
///     API para gerenciamento de contatos regionais.
///     Funcionalidades:
///     - Cadastro de novos contatos, incluindo nome, telefone e e-mail.
///     - Consulta de contatos cadastrados com a opção de filtrar por DDD.
///     - Atualização de dados de contatos existentes.
///     - Exclusão de contatos.
///     Cada contato está associado a um DDD, representando sua região.
///     A API suporta operações de criação, leitura, atualização e exclusão (CRUD) dos contatos.
/// </summary>
[Produces("application/json")]
[Route("fiap/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ContatoController : ControllerBase
{
    private readonly AtualizarContatoCommandHandler _atualizarContatoCommandHandler;
    private readonly CriarContatoCommandHandler _criarContatoCommandHandler;
    private readonly ILogger<ContatoController> _logger;
    private readonly ObterContatoPorIdQueryHandler _obterContatoPorIdQueryHandler;
    private readonly ObterContatosPorDddQueryHandler _obterContatosPorDddQueryHandler;
    private readonly RemoverContatoCommandHandler _removerContatoCommandHandler;

    private readonly IValidator<AtualizarContatoCommand> _validatorAtualizarContatoCommand;
    private readonly IValidator<CriarContatoCommand> _validatorCriarContatoCommand;
    private readonly IValidator<ObterContatoPorIdQueryRequest> _validatorObterContatoPorIdQueryRequest;
    private readonly IValidator<ObterContatosPorDddQueryRequest> _validatorObterContatosPorDddQueryRequest;
    private readonly IValidator<RemoverContatoCommand> _validatorRemoverContatoCommand;

    private static readonly Gauge MemoryUsageByEndpointGauge = Metrics.CreateGauge(
        "api_memory_usage_by_endpoint_bytes",
        "Uso de memória da API por endpoint em bytes",
        new GaugeConfiguration
        {
            LabelNames = new[] { "endpoint" } // Usar 'endpoint' como label para diferenciar os diferentes endpoints
        });


    public ContatoController(ILogger<ContatoController> logger,
        AtualizarContatoCommandHandler atualizarContatoCommandHandler,
        CriarContatoCommandHandler criarContatoCommandHandler,
        RemoverContatoCommandHandler removerContatoCommandHandler,
        ObterContatoPorIdQueryHandler obterContatoPorIdQueryHandler,
        ObterContatosPorDddQueryHandler obterContatosPorDddQueryHandler,
        IValidator<CriarContatoCommand> validatorCriarContatoCommand,
        IValidator<AtualizarContatoCommand> validatorAtualizarContatoCommand,
        IValidator<RemoverContatoCommand> validatorRemoverContatoCommand,
        IValidator<ObterContatoPorIdQueryRequest> validatorObterContatoPorIdQueryRequest,
        IValidator<ObterContatosPorDddQueryRequest> validatorObterContatosPorDddQueryRequest)
    {
        _logger = logger;
        _atualizarContatoCommandHandler = atualizarContatoCommandHandler;
        _criarContatoCommandHandler = criarContatoCommandHandler;
        _removerContatoCommandHandler = removerContatoCommandHandler;
        _obterContatoPorIdQueryHandler = obterContatoPorIdQueryHandler;
        _obterContatosPorDddQueryHandler = obterContatosPorDddQueryHandler;
        _validatorCriarContatoCommand = validatorCriarContatoCommand;
        _validatorAtualizarContatoCommand = validatorAtualizarContatoCommand;
        _validatorRemoverContatoCommand = validatorRemoverContatoCommand;
        _validatorObterContatoPorIdQueryRequest = validatorObterContatoPorIdQueryRequest;
        _validatorObterContatosPorDddQueryRequest = validatorObterContatosPorDddQueryRequest;
    }

    /// <summary>
    ///     Cria um novo contato no sistema.
    /// </summary>
    /// <remarks>
    ///     Esta operação permite a criação de um novo contato com as informações fornecidas,
    ///     como nome, telefone, e-mail e DDD (Discagem Direta à Distância).
    ///     O contato é validado antes de ser processado, garantindo que os dados estejam consistentes.
    /// </remarks>
    /// <param name="command">
    ///     Objeto que contém as informações do contato a ser criado. Deve incluir os seguintes campos:
    ///     - Nome: Nome completo do contato.
    ///     - Telefone: Número de telefone do contato.
    ///     - Email: Endereço de e-mail do contato.
    ///     - DDD: Código da região a que o contato pertence.
    /// </param>
    /// <returns>
    ///     Retorna um status de sucesso (200 OK) se o contato for criado com êxito.
    ///     Caso contrário, retorna um erro de validação (400 BadRequest) caso algum dos dados fornecidos seja inválido.
    /// </returns>
    /// <response code="200">Contato criado com sucesso.</response>
    /// <response code="400">Erro nos dados fornecidos, como falha na validação dos campos.</response>
    [HttpPost("")]
    [ProducesResponseType(typeof(CriarContatoCommandResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CriarContato([FromBody] CriarContatoCommand command)
    {
        string histogram = "criar_contato_latency_seconds";
        string endPoint = "CriarContato";
        string counterName = "criar_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });

        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            await _validatorCriarContatoCommand.ValidateAndThrowAsync(command);
            var result = await _criarContatoCommandHandler.Handle(command);

            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro

            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }

    /// <summary>
    ///     Atualiza as informações de um contato existente com base no identificador fornecido.
    /// </summary>
    /// <remarks>
    ///     Esta operação atualiza os dados de um contato, como nome, telefone, e-mail e DDD.
    ///     O identificador <paramref name="id" /> é usado para encontrar o contato a ser atualizado,
    ///     e as novas informações são fornecidas no corpo da requisição.
    /// </remarks>
    /// <param name="id">
    ///     O identificador único (GUID) do contato a ser atualizado.
    /// </param>
    /// <param name="command">
    ///     Objeto contendo os novos dados do contato. Inclui os seguintes campos:
    ///     - Nome: Nome completo do contato.
    ///     - Telefone: Número de telefone atualizado do contato.
    ///     - Email: Endereço de e-mail atualizado.
    ///     - DDD: Código da região (DDD) atualizado.
    /// </param>
    /// <returns>
    ///     Retorna um status de sucesso (200 OK) se o contato for atualizado com êxito.
    ///     Caso contrário, retorna um erro de validação (400 BadRequest).
    /// </returns>
    /// <response code="200">Contato atualizado com sucesso.</response>
    /// <response code="400">
    ///     Erro nos dados fornecidos ou o contato não foi encontrado. Retorna um <see cref="ErrorResponse" />
    ///     .
    /// </response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AtualizarContatoCommandResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AtualizarContato([FromRoute] string id, [FromBody] AtualizarContatoCommand command)
    {
        string histogram = "atualizar_contato_latency_seconds";
        string endPoint = "AtualizarContato";
        string counterName = "atualizar_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });

        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            command.Id = id; // Vinculando o ID ao comando
            await _validatorAtualizarContatoCommand.ValidateAndThrowAsync(command);
            var result = await _atualizarContatoCommandHandler.Handle(command);

            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso
                        
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro

            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }

    /// <summary>
    ///     Remove um contato com base no identificador fornecido.
    /// </summary>
    /// <remarks>
    ///     Esta operação remove o contato associado ao <paramref name="id" /> fornecido.
    ///     Se o contato for encontrado, ele será removido do sistema.
    ///     Caso contrário, uma resposta de erro apropriada será retornada.
    /// </remarks>
    /// <param name="id">
    ///     O identificador único (GUID) do contato a ser removido.
    /// </param>
    /// <returns>
    ///     Retorna um status de sucesso (200 OK) se o contato for removido com êxito.
    ///     Caso contrário, retorna um erro (400 BadRequest) caso o contato não seja encontrado.
    /// </returns>
    /// <response code="200">Contato removido  com sucesso.</response>
    /// <response code="400">
    ///     Erro nos dados fornecidos ou o contato não foi encontrado. Retorna um <see cref="ErrorResponse" />
    ///     .
    /// </response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RemoverContatoCommandResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoverContato([FromRoute] string id)
    {
        string histogram = "remover_contato_latency_seconds";
        string endPoint = "RemoverContato";
        string counterName = "remover_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });


        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            var command = new RemoverContatoCommand { Id = id };

            await _validatorRemoverContatoCommand.ValidateAndThrowAsync(command);
            var result = await _removerContatoCommandHandler.Handle(command);

            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro

            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }


    /// <summary>
    ///     Obtém os detalhes de um contato específico com base no identificador fornecido.
    /// </summary>
    /// <remarks>
    ///     Esta operação consulta o contato associado ao <paramref name="id" /> fornecido. O identificador é validado,
    ///     e, se for encontrado, as informações do contato serão retornadas.
    ///     Caso o contato não seja encontrado ou o identificador seja inválido, será retornado um erro apropriado.
    /// </remarks>
    /// <param name="id">
    ///     O identificador único (GUID) do contato a ser consultado. Este parâmetro é passado na URL da requisição.
    /// </param>
    /// <returns>
    ///     Retorna um objeto do tipo <see cref="ObterContatoPorIdQueryResult" /> contendo as informações do contato,
    ///     caso o contato seja encontrado.
    /// </returns>
    /// <response code="200">Contato encontrado com sucesso. O objeto <see cref="ObterContatoPorIdQueryResult" /> é retornado.</response>
    /// <response code="400">
    ///     Erro nos dados fornecidos ou o identificador não é válido. Retorna um <see cref="ErrorResponse" />
    ///     .
    /// </response>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ObterContatoPorIdQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ObterContatoPorId([FromRoute] string id)
    {
        string histogram = "obterPorId_contato_latency_seconds";
        string endPoint = "ObterPorIdContato";
        string counterName = "obterPorId_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });

        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            var request = new ObterContatoPorIdQueryRequest { Id = id };
            await _validatorObterContatoPorIdQueryRequest.ValidateAndThrowAsync(request);
            var result = await _obterContatoPorIdQueryHandler.Handle(request);

            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro

            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }

    /// <summary>
    ///     Obtém a lista de contatos filtrados por um DDD específico.
    /// </summary>
    /// <remarks>
    ///     Esta operação permite consultar os contatos cadastrados associados ao DDD fornecido.
    ///     O DDD deve ser um número de dois dígitos. O identificador de DDD é validado, e, se for encontrado,
    ///     retorna uma lista de contatos associados ao código DDD.
    ///     Se o DDD não for válido ou os contatos não forem encontrados, uma resposta de erro apropriada será retornada.
    /// </remarks>
    /// <param name="ddd">
    ///     O DDD (Discagem Direta à Distância) de dois dígitos a ser consultado.
    ///     O DDD identifica a região associada ao contato e é passado na URL da requisição.
    /// </param>
    /// <returns>
    ///     Retorna um objeto do tipo <see cref="ObterContatosPorDddQueryResult" /> contendo a lista de contatos,
    ///     caso sejam encontrados.
    /// </returns>
    /// <response code="200">
    ///     Contatos encontrados com sucesso. O objeto <see cref="ObterContatosPorDddQueryResult" /> é
    ///     retornado.
    /// </response>
    /// <response code="400">Erro nos dados fornecidos ou o DDD não é válido. Retorna um <see cref="ErrorResponse" />.</response>
    [HttpGet]
    [Route("ddd/{ddd}")]
    [ProducesResponseType(typeof(ObterContatosPorDddQueryResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ObterContatosPorDdd([FromRoute] int ddd)
    {
        string histogram = "obterPorDDD_contato_latency_seconds";
        string endPoint = "ObterPorDDDContato";
        string counterName = "obterPorDDD_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });

        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            var request = new ObterContatosPorDddQueryRequest { Ddd = ddd };
            await _validatorObterContatosPorDddQueryRequest.ValidateAndThrowAsync(request);
            var result = await _obterContatosPorDddQueryHandler.Handle(request);

            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro

            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }
}
