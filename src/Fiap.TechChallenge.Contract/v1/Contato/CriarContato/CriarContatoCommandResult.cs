﻿using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Contract.v1.Contato.CriarContato;

public class CriarContatoCommandResult : CommandResult
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}