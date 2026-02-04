namespace CleanArchitecture.PracticalTest.Application.Exceptions;

public class NotFoundException(string entityTypes, Guid id) : Exception($"Entity {entityTypes} with ID {id} was not found.")
{
    public string EntityType { get; } = entityTypes;
    public Guid Id { get; } = id;
}
