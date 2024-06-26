using System;

namespace Internships.Core.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}