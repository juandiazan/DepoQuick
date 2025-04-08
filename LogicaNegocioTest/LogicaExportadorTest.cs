using System.Text;
using Dominio;
using LogicaNegocio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaExportadorTest
{
    [TestMethod]
    public void ExportadorCsvDeberiaExportarCorrectamenteTest()
    {
        var reservas = new List<Reserva>
        {
            new Reserva { Deposito = new Deposito { Id = 1, Area = AreasDeDeposito.A, Tamanio = TamaniosDeDeposito.Pequeño, Climatizacion = true }, Id = 1, Pago = new Pago { Estado = "Reservado" }, Usuario = new Usuario { Nombre = "Test", Apellido = "User" }, RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1) } },
            new Reserva { Deposito = new Deposito { Id = 2, Area = AreasDeDeposito.A, Tamanio = TamaniosDeDeposito.Pequeño, Climatizacion = true }, Id = 2, Pago = new Pago { Estado = "Capturado" }, Usuario = new Usuario { Nombre = "Test", Apellido = "User" }, RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1) } }
        };
        
        IExportadorReporte<Reserva> exportador = ExportadorReporteFactory.CrearExportador(FormatoExportacion.csv);

        byte[] resultado = exportador.Exportar(reservas);
        string resultadoToString = Encoding.UTF8.GetString(resultado).Replace("\r\n", "\n");
        
        string esperadoString = "DEPOSITO,RESERVA,PAGO\n" + $"{reservas[0].Deposito},{reservas[0]},Reservado\n" + $"{reservas[1].Deposito},{reservas[1]},Capturado\n".Replace("\r\n", "\n");
        Assert.AreEqual(esperadoString, resultadoToString);
    }
    
    [TestMethod]
    public void ExportadorTxtDeberiaExportarCorrectamenteTest()
    {
        var reservas = new List<Reserva>
        {
            new Reserva { Deposito = new Deposito { Id = 1, Area = AreasDeDeposito.A, Tamanio = TamaniosDeDeposito.Pequeño, Climatizacion = true }, Id = 1, Pago = new Pago { Estado = "Reservado" }, Usuario = new Usuario { Nombre = "Test", Apellido = "User" }, RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1) } },
            new Reserva { Deposito = new Deposito { Id = 2, Area = AreasDeDeposito.A, Tamanio = TamaniosDeDeposito.Pequeño, Climatizacion = true }, Id = 2, Pago = new Pago { Estado = "Capturado" }, Usuario = new Usuario { Nombre = "Test", Apellido = "User" }, RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1) } }
        };
        
        IExportadorReporte<Reserva> exportador = ExportadorReporteFactory.CrearExportador(FormatoExportacion.txt);
        
        byte[] resultado = exportador.Exportar(reservas);
        string resultadoToString = Encoding.UTF8.GetString(resultado).Replace("\r\n", "\n");
        
        string esperadoString = "DEPOSITO\tRESERVA\tPAGO\n" + $"{reservas[0].Deposito}\t{reservas[0]}\tReservado\n" + $"{reservas[1].Deposito}\t{reservas[1]}\tCapturado\n".Replace("\r\n", "\n");
        Assert.AreEqual(esperadoString, resultadoToString);
    }
}