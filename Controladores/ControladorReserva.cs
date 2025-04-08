using Dominio;
using LogicaNegocio;

namespace Controladores;

public class ControladorReserva
{
    private LogicaReserva _logicaReserva;

    public ControladorReserva(LogicaReserva logicaReserva)
    {
        _logicaReserva = logicaReserva;
    }

    public Reserva ConfirmarYAgregarReserva(Reserva reserva, double costoReserva)
    {
        return _logicaReserva.ConfirmarYAgregarReserva(reserva, costoReserva);
    }

    public void AceptarReserva(Reserva reserva)
    {
        _logicaReserva.AceptarReserva(reserva);
    }

    public void RechazarReserva(Reserva reserva, string motivo)
    {
        _logicaReserva.RechazarReserva(reserva, motivo);
    }
    
    public List<Reserva> ObtenerTodasLasReservas()
    {
        return _logicaReserva.GetAll();
    }
    
    public List<Reserva> ObtenerReservasDeUsuario(Usuario cliente)
    {
        return _logicaReserva.ObtenerReservasDeUsuario(cliente);
    }

    public List<Reserva> ObtenerReservasDeUnDeposito(int depositoId)
    {
        return _logicaReserva.ObtenerReservasDeUnDeposito(depositoId);
    }
}