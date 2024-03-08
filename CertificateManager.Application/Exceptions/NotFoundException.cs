using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Certificate.Application.Exceptions;

[DefaultStatusCode(404)]
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    { }
}

public class NotFoundException<TEntity> : NotFoundException
{
    public NotFoundException(Guid id) : base($"{typeof(TEntity)} not found. Id: {id}")
    { }
}
