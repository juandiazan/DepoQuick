using Dominio;

namespace Repositorio;

public class RepositorioPromocion : IRepositorioCRUDFind<Promocion>
{
    private List<Promocion> _listaPromociones = new List<Promocion>();
    
    public Promocion Add(Promocion unElemento)
    {
        unElemento.Id = _listaPromociones.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _listaPromociones.Add(unElemento);
        return unElemento;
    }
    public Promocion Update(Promocion promocionRetorno)
    {
        Promocion promocionEncontrada = Find(x => x.Id == promocionRetorno.Id);
        promocionEncontrada = promocionRetorno;
        return promocionEncontrada;
    }
    public void Delete(Promocion unaPromocion)
    {
        _listaPromociones.RemoveAll(x => x.Id == unaPromocion.Id);
    }
    public List<Promocion> GetAll()
    {
        return _listaPromociones;
    }
    public Promocion? Find(Func<Promocion, bool> filtro)
    {  
        return _listaPromociones.Where(filtro).FirstOrDefault();
    }
}