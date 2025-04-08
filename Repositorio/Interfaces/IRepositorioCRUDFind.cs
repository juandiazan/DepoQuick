namespace Repositorio;

public interface IRepositorioCRUDFind<T> : IRepositorioFindDelete<T>
{
    T Update(T elementoAActualizar);
 }