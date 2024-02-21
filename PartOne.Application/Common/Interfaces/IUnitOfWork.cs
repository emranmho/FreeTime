namespace PartOne.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IShortenedUrlRepository ShortenedUrl { get; }
    Task Save();
}