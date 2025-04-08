using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class RepositorioDepositoBaseDeDatos : IRepositorioFindDelete<Deposito>
{
    private ContextoSql _contexto;

    public RepositorioDepositoBaseDeDatos(ContextoSql contexto) {
        _contexto = contexto;
    }
    
    public Deposito Add(Deposito nuevoDeposito) {
        _contexto.Depositos.Add(nuevoDeposito);
        _contexto.SaveChanges();
        return nuevoDeposito;
    }
    public void Delete(Deposito unDeposito) {
        _contexto.Depositos.Remove(Find(depo => depo.Id == unDeposito.Id));
        _contexto.SaveChanges();
    }
    public List<Deposito> GetAll() {
        CargarRangoDeFechasDePromocionesDeDepositos();
        
        return _contexto.Depositos.Include(x => x.ListaPromociones).Include(x => x.ListaDisponibilidad).ToList();
    }
    public Deposito? Find(Func<Deposito, bool> filtro) {
        return _contexto.Depositos.FirstOrDefault(filtro);
    }

    private void CargarRangoDeFechasDePromocionesDeDepositos() {
        foreach (var depo in _contexto.Depositos.Include(x => x.ListaPromociones).Include(x => x.ListaDisponibilidad).ToList()) {
            foreach (var promo in depo.ListaPromociones) {
                _contexto.Entry(promo).Reference(p => p.rangoFechas).Load();
            }
        }
    }
}