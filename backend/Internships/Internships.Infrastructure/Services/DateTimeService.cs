using Internships.Core.Interfaces;
using System;

namespace Internships.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
