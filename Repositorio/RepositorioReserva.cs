using Dominio;

namespace Repositorio;

public class RepositorioReserva : IRepositorioBase<Reserva>
{
    private List<Reserva> _listaReservas = new List<Reserva>();
    
    public Reserva Add (Reserva unElemento)
    {
        unElemento.Id = _listaReservas.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        
        unElemento.Pago.Id = _listaReservas.OrderByDescending(x => x.Id)
            .Select(x => x.Pago.Id)
            .FirstOrDefault() + 1;
        
        _listaReservas.Add(unElemento);
        
        return unElemento;
    }
    public List<Reserva> GetAll() {
        return _listaReservas;
    }
}