namespace Elements.Services.Public.Interfaces
{
    using System;

    public interface IDateTimeService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
