using Controladores;
using Dominio;
using LogicaNegocio;

namespace ControladoresTest;

[TestClass]
public class ControladorCalculoPrecioTest
{
    private LogicaCalculoPrecio _logicaCalculoPrecio;
    private ControladorCalculoPrecio _controladorCalculoPrecio;

    [TestInitialize]
    public void Setup()
    {
        _logicaCalculoPrecio = new LogicaCalculoPrecio();
        _controladorCalculoPrecio = new ControladorCalculoPrecio(_logicaCalculoPrecio);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoPequenioSinClimatizacionPrecioCorrectoTest()
    {
        Deposito deposito = new Deposito
        {
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = false
        };

        RangoDeFechas rango = new RangoDeFechas
        {
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(10)
        };

        double precio = _controladorCalculoPrecio.CalcularPrecioBase(deposito, rango);

        Assert.AreEqual(475, precio); 
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoGrandeSinClimatizacionConDescuentoPrecioCorrectoTest()
    {
        Deposito deposito = new Deposito
        {
            Tamanio = TamaniosDeDeposito.Grande,
            Climatizacion = false
        };

        RangoDeFechas rango = new RangoDeFechas
        {
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(20)
        };

        double precio = _controladorCalculoPrecio.CalcularPrecioBase(deposito, rango);

        Assert.AreEqual(1800, precio);
    }
}