namespace Repositorio;

public interface IRepositorioFindDelete<T> : IRepositorioFind<T>
{
    void Delete(T unElemento);
}