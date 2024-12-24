namespace Weatheryzer.Shared.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task SaveChangesAsync();
}