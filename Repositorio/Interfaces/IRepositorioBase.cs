namespace Repositorio;

public interface IRepositorioBase<T>
{
    T Add(T unElemento);
    
    List<T> GetAll();
}