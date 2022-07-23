namespace Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException() : base() { }

    public AlreadyExistsException(string? message) : base(message) { }

    public AlreadyExistsException(string? message, Exception? innerException) 
        : base(message, innerException) { }

    public AlreadyExistsException(string? name, string property, object value)
        : base($"Entity {name} with property \"{property}\" and value \"{value}\" already exists") { }}