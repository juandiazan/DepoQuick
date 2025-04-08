using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class RepositorioReservaBaseDeDatos : IRepositorioUpdate<Reserva>
{
    private ContextoSql _contexto;

    public RepositorioReservaBaseDeDatos(ContextoSql contexto) {
        _contexto = contexto;
    }
    
    public Reserva Add(Reserva unElemento) {
        _contexto.Reservas.Add(unElemento);
        _contexto.SaveChanges();
        return unElemento;
    }

    public List<Reserva> GetAll() {
        return _contexto.Reservas
            .Include(x => x.Usuario)
            .Include(x => x.Deposito)
            .Include(x => x.RangoDeFechas)
            .Include(x => x.Pago)
            .ToList();
    }

    public Reserva Update(Reserva elementoAActualizar) {
        _contexto.Reservas.ToList().FirstOrDefault(x => x.Id == elementoAActualizar.Id);
        _contexto.Reservas.Update(elementoAActualizar);
        _contexto.SaveChanges();

        return elementoAActualizar;
    }
}