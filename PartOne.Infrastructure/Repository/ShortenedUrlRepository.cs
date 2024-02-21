using PartOne.Application.Common.Interfaces;
using PartOne.Domain.Entities;
using PartOne.Infrastructure.Data;

namespace PartOne.Infrastructure.Repository;

public class ShortenedUrlRepository : Repository<ShortenedUrl>, IShortenedUrlRepository
{
    private readonly ApplicationDbContext _db;
    
    public ShortenedUrlRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}