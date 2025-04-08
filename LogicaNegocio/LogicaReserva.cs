using Dominio;
using Repositorio;

namespace LogicaNegocio;

public class LogicaReserva
{
    private const int LimiteCaracteres = 300;
    private const int CostoMinimo = 0;

    private IRepositorioUpdate<Reserva> _repositorioReserva;
    private LogicaSesion _logicaSesion;
    
    public LogicaReserva(IRepositorioUpdate<Reserva> repositorioReserva, LogicaSesion logicaSesion) {
        _repositorioReserva = repositorioReserva;
        _logicaSesion = logicaSesion;
    }
    
    
    public Reserva ConfirmarYAgregarReserva(Reserva reserva, double costoReserva)
    {
        ConfirmarReserva(reserva, costoReserva);
        return Add(reserva);
    }
    public Reserva Add(Reserva unaReserva) {
        if (ReservaSinPago(unaReserva)) {
            throw new LogicaReservaException("No se puede crear una reserva sin un pago");
        }
        return _repositorioReserva.Add(unaReserva);
    }
    public void ConfirmarReserva(Reserva reserva, double costoReserva)
    {
        if (CostoEsNegativo(costoReserva))
        {
            throw new LogicaReservaException("El costo de la reserva no puede ser negativo");
        }

        if (DepositoOcupadoEnFecha(reserva)) {
            throw new LogicaReservaException("El depósito no está disponible para la fecha ingresada");
        }
        
        reserva.Costo = costoReserva;
        reserva.EstadoAprobacionCliente = true;
        reserva.Pago = new Pago { Monto = costoReserva, Estado = "Reservado" };
    }
    public List<Reserva> GetAll() {
        return _repositorioReserva.GetAll();
    }
    public void AceptarReserva(Reserva reserva)
    {
        ValidarSesionParaAceptar();
        ValidarReservaParaAceptar(reserva);
        
        if (DepositoOcupadoEnFecha(reserva)) {
            throw new LogicaReservaException("Ya existe una reserva para la fecha de reserva del depósito a aceptar");
        }
        
        reserva.Deposito.ReservarFecha(reserva.RangoDeFechas);
        
        reserva.EstadoAprobacionAdmin = true;
        reserva.Pago.Estado = "Capturado";

        _repositorioReserva.Update(reserva);
    }
    public void RechazarReserva(Reserva reserva, string motivo)
    {
        ValidarSesion();
        ValidarReserva(reserva);
        ValidarMotivoRechazo(motivo);
        
        reserva.EstadoAprobacionAdmin = false;
        reserva.MotivoRechazo = motivo;

        _repositorioReserva.Update(reserva);
    }
    public List<Reserva> ObtenerReservasDeUsuario(Usuario cliente)
    {
        return _repositorioReserva.GetAll().FindAll(x => x.Usuario.Email == cliente.Email);
    }
    public List<Reserva> ObtenerReservasDeUnDeposito(int unaId) {
        List<Reserva> reservasDeDeposito = new List<Reserva>();
        foreach (var reserva in _repositorioReserva.GetAll()) {
            if (reserva.Deposito.Id == unaId) {
                reservasDeDeposito.Add(reserva);
            }
        }
        return reservasDeDeposito;
    }
    
    
    private bool ReservaSinPago(Reserva unaReserva) {
        return unaReserva.Pago is null;
    }
    private bool DepositoOcupadoEnFecha(Reserva unaReserva) {
        return !unaReserva.Deposito.EstaDisponibleEnRango(unaReserva.RangoDeFechas);
    }
    private bool CostoEsNegativo(double costoReserva)
    {
        return costoReserva < CostoMinimo;
    }
    private void ValidarSesionParaAceptar()
    {
        if (_logicaSesion.HayUnClienteEnSesion())
        {
            throw new LogicaReservaException("Solo un administrador puede aceptar una reserva");
        } else if (!_logicaSesion.HayUnaSesionIniciada()) {
            throw new LogicaReservaException("Debe iniciar sesión para aceptar una reserva");
        }
    }
    private void ValidarReservaParaAceptar(Reserva reserva)
    {
        if (NoEstaAprobadaPorCliente(reserva))
        {
            throw new LogicaReservaException("La reserva no fue aprobada por el cliente");
        }
    }
    private void ValidarSesion()
    {
        if (_logicaSesion.HayUnClienteEnSesion())
        {
            throw new LogicaReservaException("Solo un administrador puede rechazar una reserva");
        }

        if (!_logicaSesion.HayUnaSesionIniciada())
        {
            throw new LogicaReservaException("Debe iniciar sesión para rechazar una reserva");
        }
    }
    private void ValidarReserva(Reserva reserva)
    {
        if (NoEstaAprobadaPorCliente(reserva))
        {
            throw new LogicaReservaException("La reserva no fue aprobada por el cliente");
        }
    }
    private void ValidarMotivoRechazo(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new LogicaReservaException("El motivo de rechazo no puede ser vacío");
        }
        
        if (MotivoMayorA300Caracteres(motivo))
        {
            throw new LogicaReservaException("El motivo de rechazo no puede superar los 300 caracteres");
        }

    }
    private bool MotivoMayorA300Caracteres(string motivo)
    {
        return motivo.Length > LimiteCaracteres;
    }
    private bool NoEstaAprobadaPorCliente(Reserva reserva)
    {
        return !reserva.EstadoAprobacionCliente;
    }
}