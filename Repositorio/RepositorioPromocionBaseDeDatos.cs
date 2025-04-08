using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class RepositorioPromocionBaseDeDatos : IRepositorioCRUDFind<Promocion>
{
    private ContextoSql _contexto;

    public RepositorioPromocionBaseDeDatos(ContextoSql contexto) {
        _contexto = contexto;
    }
    public Promocion Add(Promocion unaPromocion) {
        _contexto.Promociones.Add(unaPromocion);
        _contexto.SaveChanges();
        return unaPromocion;
    }
    public Promocion Update(Promocion unaPromocion) {
        _contexto.Promociones.ToList().FirstOrDefault(promo => promo.Id == unaPromocion.Id);
        _contexto.Promociones.Update(unaPromocion);
        _contexto.SaveChanges();
        return unaPromocion;
    }
    public void Delete(Promocion unaPromocion)
    {
        _contexto.Promociones.Remove(Find(promo => promo.Id == unaPromocion.Id));
        _contexto.SaveChanges();
    }
    public List<Promocion> GetAll()
    {
        return _contexto.Promociones.Include(x => x.rangoFechas).ToList();
    }
    public Promocion? Find(Func<Promocion, bool> filtro)
    {  
        return _contexto.Promociones.FirstOrDefault(filtro);
    }
}