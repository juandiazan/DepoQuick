namespace Repositorio;

public interface IRepositorioFind<T> : IRepositorioBase<T>
{
    T? Find(Func<T, bool> unFiltro);
}