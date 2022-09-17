using System;
using System.Runtime.Serialization;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Core.Exceptions;

[Serializable]
public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title) : base($"Invalid title: {title}.")
    {
    }

    public string Code { get; } = "invalid_title";
}