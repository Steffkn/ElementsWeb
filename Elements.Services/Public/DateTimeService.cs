namespace Elements.Services.Public
{
    using Elements.Services.Public.Interfaces;
    using System;

    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
