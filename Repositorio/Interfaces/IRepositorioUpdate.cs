namespace Repositorio;

public interface IRepositorioUpdate<T> : IRepositorioBase<T>
{
    T Update(T elementoAActualizar);
}