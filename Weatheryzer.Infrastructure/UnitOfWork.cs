using Microsoft.EntityFrameworkCore;
using Weatheryzer.Shared.Interfaces;

namespace Weatheryzer.Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly WeatherDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(WeatherDbContext context) 
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        if (!_repositories.ContainsKey(typeof(T)))
        {
            var repositoryInstance = new Repository<T>(_context);
            _repositories[typeof(T)] = repositoryInstance;
        }
        return (IRepository<T>)_repositories[typeof(T)];
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}