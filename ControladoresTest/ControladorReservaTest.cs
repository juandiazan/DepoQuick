using Controladores;
using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace ControladoresTest;

[TestClass]
public class ControladorReservaTest
{
    private LogicaSesion _logicaSesion;
    private LogicaReserva _logicaReserva;
    private ControladorReserva _controladorReserva;
    private ContextoSql _contexto;
    private Usuario _admin;
    private Deposito _deposito;

    [TestInitialize]
    public void Setup()
    {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        _logicaSesion = new LogicaSesion();
        _logicaReserva = new LogicaReserva(new RepositorioReservaBaseDeDatos(_contexto), _logicaSesion);
        _controladorReserva = new ControladorReserva(_logicaReserva);

        _admin = new Usuario
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@correo.com",
            Contrasena = "@Admin123",
            VerificacionContrasena = "@Admin123",
            RolDeUsuario = Rol.Administrador
        };

        _deposito = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };
        
        _deposito.AgregarFechaDeDisponibilidad(new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }, new List<Reserva>());

        _logicaSesion.IniciarSesion(_admin, _admin.Contrasena, new List<Usuario> { _admin });
    }

    [TestCleanup]
    public void Cleanup()
    {
        _contexto.Database.EnsureDeleted();
    }

    [TestMethod]
    public void ConfirmarYAgregarReservaReservaValidaReservaAgregadaTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        Reserva resultado = _controladorReserva.ConfirmarYAgregarReserva(nuevaReserva, 100);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(100, resultado.Costo);
    }
    
    [TestMethod]
    public void AceptarReservaReservaValidaReservaAceptadaTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) },
            EstadoAprobacionCliente = true,
            Pago = new Pago { Monto = 100, Estado = "Reservado" }
        };

        _controladorReserva.ConfirmarYAgregarReserva(nuevaReserva, 100);

        _controladorReserva.AceptarReserva(nuevaReserva);

        Assert.IsTrue(nuevaReserva.EstadoAprobacionAdmin);
        Assert.AreEqual("Capturado", nuevaReserva.Pago.Estado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaReservaInvalidaExcepcionTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        _controladorReserva.RechazarReserva(nuevaReserva, "");
    }
    
    [TestMethod]
    public void ObtenerReservasDeUsuarioReservasExistentesDevuelveReservasTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) },
            EstadoAprobacionCliente = true,
            Pago = new Pago { Monto = 100, Estado = "Reservado" }
        };

        _controladorReserva.ConfirmarYAgregarReserva(nuevaReserva, 100);

        List<Reserva> reservas = _controladorReserva.ObtenerReservasDeUsuario(_admin);

        Assert.AreEqual(1, reservas.Count);
    }
    
    [TestMethod]
    public void ObtenerReservasDeUnDepositoReservasExistentesDevuelveReservasTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) },
            EstadoAprobacionCliente = true,
            Pago = new Pago { Monto = 100, Estado = "Reservado" }
        };

        _controladorReserva.ConfirmarYAgregarReserva(nuevaReserva, 100);

        List<Reserva> reservas = _controladorReserva.ObtenerReservasDeUnDeposito(_deposito.Id);

        Assert.AreEqual(1, reservas.Count);
    }

    [TestMethod]
    public void ObtenerTodasLasReservasTest()
    {
        Reserva nuevaReserva = new Reserva
        {
            Usuario = _admin,
            Deposito = _deposito,
            RangoDeFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) },
            EstadoAprobacionCliente = true,
            Pago = new Pago { Monto = 100, Estado = "Reservado" }
        };

        _controladorReserva.ConfirmarYAgregarReserva(nuevaReserva, 100);

        List<Reserva> reservas = _controladorReserva.ObtenerTodasLasReservas();

        Assert.AreEqual(1, reservas.Count);
    }
    
}