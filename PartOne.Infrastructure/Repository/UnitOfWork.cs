using PartOne.Application.Common.Interfaces;
using PartOne.Infrastructure.Data;

namespace PartOne.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public IShortenedUrlRepository ShortenedUrl { get; private set; }
    
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        ShortenedUrl = new ShortenedUrlRepository(_db);
    }
    
    
    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}