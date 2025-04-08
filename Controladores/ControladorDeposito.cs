using Dominio;
using LogicaNegocio;

namespace Controladores;
public class ControladorDeposito
{
    private LogicaDeposito _logicaDeposito;

    public ControladorDeposito(LogicaDeposito logicaDeposito)
    {
        _logicaDeposito = logicaDeposito;
    }

    public Deposito AgregarDeposito(Deposito deposito)
    {
        return _logicaDeposito.Add(deposito);
    }

    public List<Deposito> ObtenerTodosLosDepositos()
    {
        return _logicaDeposito.GetAll();
    }

    public Deposito BuscarDeposito(Func<Deposito, bool> filtro)
    {
        return _logicaDeposito.Find(filtro);
    }

    public void EliminarDeposito(List<Reserva> listaReservas, Deposito deposito)
    {
        _logicaDeposito.Delete(listaReservas, deposito);
    }

    public List<Deposito> ObtenerDepositosEnRango(RangoDeFechas disponibilidad)
    {
        return _logicaDeposito.ObtenerDepositosEnRango(disponibilidad);
    }
}