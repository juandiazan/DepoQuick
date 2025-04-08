using Controladores;
using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace ControladoresTest;

[TestClass]
public class ControladorDepositoTest
{
    private LogicaSesion _logicaSesion;
    private LogicaDeposito _logicaDeposito;
    private ControladorDeposito _controladorDeposito;
    private ContextoSql _contexto;
    private Usuario _admin;
    private List<Reserva> _reservas;

    [TestInitialize]
    public void Setup()
    {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        _logicaSesion = new LogicaSesion();
        _logicaDeposito = new LogicaDeposito(new RepositorioDepositoBaseDeDatos(_contexto), _logicaSesion);
        _controladorDeposito = new ControladorDeposito(_logicaDeposito);

        _admin = new Usuario
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@correo.com",
            Contrasena = "@Admin123",
            VerificacionContrasena = "@Admin123",
            RolDeUsuario = Rol.Administrador
        };

        _logicaSesion.IniciarSesion(_admin, _admin.Contrasena, new List<Usuario> { _admin });

        _reservas = new List<Reserva>();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _contexto.Database.EnsureDeleted();
    }

    [TestMethod]
    public void AgregarDepositoNuevoDepositoDepositoAgregadoTest()
    {
        Deposito nuevoDeposito = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };

        Deposito resultado = _controladorDeposito.AgregarDeposito(nuevoDeposito);

        Assert.AreEqual(nuevoDeposito.Nombre, resultado.Nombre);
        Assert.AreEqual(nuevoDeposito.Tamanio, resultado.Tamanio);
    }

    [TestMethod]
    public void ObtenerTodosLosDepositosDevuelveDepositosTest()
    {
        Deposito nuevoDeposito1 = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };

        Deposito nuevoDeposito2 = new Deposito
        {
            Nombre = "DepositoDos",
            Tamanio = TamaniosDeDeposito.Mediano,
            Climatizacion = false
        };

        _controladorDeposito.AgregarDeposito(nuevoDeposito1);
        _controladorDeposito.AgregarDeposito(nuevoDeposito2);

        List<Deposito> depositos = _controladorDeposito.ObtenerTodosLosDepositos();

        Assert.AreEqual(2, depositos.Count);
    }

    [TestMethod]
    public void BuscarDepositoDepositoExistenteDevuelveDepositoTest()
    {
        Deposito nuevoDeposito = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };

        _controladorDeposito.AgregarDeposito(nuevoDeposito);

        Deposito resultado = _controladorDeposito.BuscarDeposito(d => d.Nombre == "DepositoUno");

        Assert.IsNotNull(resultado);
        Assert.AreEqual("DepositoUno", resultado.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void EliminarDepositoDepositoReservadoExcepcionTest()
    {
        Deposito nuevoDeposito = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };

        _controladorDeposito.AgregarDeposito(nuevoDeposito);
        _reservas.Add(new Reserva { Deposito = nuevoDeposito });

        _controladorDeposito.EliminarDeposito(_reservas, nuevoDeposito);
    }

    [TestMethod]
    public void ObtenerDepositosEnRangoDisponibilidadCorrectaDevuelveDepositosTest()
    {
        Deposito nuevoDeposito1 = new Deposito
        {
            Nombre = "DepositoUno",
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };

        Deposito nuevoDeposito2 = new Deposito
        {
            Nombre = "DepositoDos",
            Tamanio = TamaniosDeDeposito.Mediano,
            Climatizacion = false
        };

        _controladorDeposito.AgregarDeposito(nuevoDeposito1);
        _controladorDeposito.AgregarDeposito(nuevoDeposito2);
        
        

        RangoDeFechas rango = new RangoDeFechas
        {
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddMonths(1)
        };

        List<Deposito> depositos = _controladorDeposito.ObtenerDepositosEnRango(rango);

        Assert.AreEqual(0, depositos.Count);
    }

}

