using Dominio;

namespace DominioTest;

[TestClass]
public class PagoTest
{
    [TestMethod]
    public void CrearPagoOkTest()
    {
        Pago unPago = new Pago()
        {
            Id = 1,
            Monto = 1000,
            Estado = "Reservado"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPagoException))]
    public void CrearPagoConMontoNegativoTest()
    {
        Pago unPago = new Pago()
        {
            Id = 1,
            Monto = -1000,
            Estado = "Reservado"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPagoException))]
    public void CrearPagoConEstadoVacioTest()
    {
        Pago unPago = new Pago()
        {
            Id = 1,
            Monto = 1000,
            Estado = ""
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPagoException))]
    public void CrearPagoConEstadoNullTest()
    {
        Pago unPago = new Pago()
        {
            Id = 1,
            Monto = 1000,
            Estado = null
        };
    }

}