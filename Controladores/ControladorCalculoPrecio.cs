using Dominio;
using LogicaNegocio;

namespace Controladores;

public class ControladorCalculoPrecio
{
    private LogicaCalculoPrecio _logicaCalculoPrecio;

    public ControladorCalculoPrecio(LogicaCalculoPrecio logicaCalculoPrecio)
    {
        _logicaCalculoPrecio = logicaCalculoPrecio;
    }

    public double CalcularPrecioBase(Deposito deposito, RangoDeFechas rangoFechas)
    {
        return _logicaCalculoPrecio.CalcularPrecioBase(deposito, rangoFechas);
    }
}