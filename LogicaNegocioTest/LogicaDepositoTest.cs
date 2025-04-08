using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaDepositoTest
{
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    private LogicaDeposito _logicaDeposito;
    private LogicaReserva _logicaReserva;
    private LogicaCalculoPrecio _logicaCalculoPrecio;

    private Usuario _adminParaInicioSesion;
    private Usuario _clienteParaInicioSesion;
    private ContextoSql _contexto;

    [TestInitialize]
    public void SetUp() {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(new RepositorioUsuarioBaseDeDatos(_contexto), _logicaSesion);
        _logicaReserva = new LogicaReserva(new RepositorioReservaBaseDeDatos(_contexto), _logicaSesion);
        _logicaDeposito = new LogicaDeposito(new RepositorioDepositoBaseDeDatos(_contexto), _logicaSesion);
        _logicaCalculoPrecio = new LogicaCalculoPrecio();
        
        _adminParaInicioSesion = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };
        _clienteParaInicioSesion = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(_adminParaInicioSesion);
        _logicaUsuario.Add(_clienteParaInicioSesion);
        
    }
    
    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AgregarDepositoSinPromocionOk() {
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnNombreDepo"
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Deposito depositoAgregado = _logicaDeposito.Add(unDeposito);
        
        Assert.AreEqual(unDeposito.Id, depositoAgregado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void AgregarDepositoCuandoNoHayUsuarioEnSesion() {
        Deposito unDeposito = new Deposito();
        
        _logicaDeposito.Add(unDeposito);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void AgregarDepositoCuandoHayClienteEnSesion() {
        Deposito unDeposito = new Deposito();
        
        _logicaSesion.IniciarSesion(_clienteParaInicioSesion, _clienteParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        _logicaDeposito.Add(unDeposito);
    }

    [TestMethod]
    public void AgregarDepositoYQueLaIdAumenteCorrectamente()
    {
        Deposito primerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoUno"
        };
        Deposito segundoDeposito = new Deposito
        {
            Nombre = "UnNombreDepoDos"
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        _logicaDeposito.Add(primerDeposito);
        _logicaDeposito.Add(segundoDeposito);
        
        Assert.AreNotEqual(primerDeposito.Id, segundoDeposito.Id);
    }

    [TestMethod]
    public void BajaDepositoOk()
    {
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnNombreDepo"
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(unDeposito);
        _logicaSesion.CerrarSesion();
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Delete(_logicaReserva.GetAll(), unDeposito);
        
        Assert.AreEqual(0, _logicaDeposito.GetAll().Count);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void EliminarDepositoSinUsuarioEnSesion()
    {
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnNombreDepo"
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(unDeposito);
        _logicaSesion.CerrarSesion();
        
        _logicaDeposito.Delete(_logicaReserva.GetAll(), unDeposito);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void EliminarDepositoConClienteEnSesion()
    {
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnNombreDepo"
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(unDeposito);
        _logicaSesion.CerrarSesion();
        
        _logicaSesion.IniciarSesion(_clienteParaInicioSesion, _clienteParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Delete(_logicaReserva.GetAll(), unDeposito);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void EliminarDepositoQueNoExiste() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        _logicaDeposito.Delete(_logicaReserva.GetAll(), new Deposito());
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void EliminarDepositoConReserva()
    {
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnNombreDepo"
        };
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        unDeposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        Reserva unaReservaDelDeposito = new Reserva()
        {
            Usuario = _clienteParaInicioSesion,
            RangoDeFechas = rango,
            Deposito = unDeposito
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(unDeposito);

        _logicaReserva.ConfirmarReserva(unaReservaDelDeposito, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito,  unaReservaDelDeposito.RangoDeFechas));
        _logicaReserva.Add(unaReservaDelDeposito);
        
        _logicaDeposito.Delete(_logicaReserva.GetAll(), unDeposito);
    }

    [TestMethod]
    public void ObtenerTodosLosDepositosQueEstenDisponiblesEnUnRangoOkTest() {
        RangoDeFechas rangoParaPrimerDeposito = new RangoDeFechas();
        rangoParaPrimerDeposito.FechaInicio = new DateTime(2024, 1, 1);
        rangoParaPrimerDeposito.FechaFin = new DateTime(2024, 1, 30);
        Deposito primerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoUno"
        };
        primerDeposito.AgregarFechaDeDisponibilidad(rangoParaPrimerDeposito, _logicaReserva.GetAll());
        
        RangoDeFechas rangoParaSegundoDeposito = new RangoDeFechas();
        rangoParaSegundoDeposito.FechaInicio = new DateTime(2024, 1, 5);
        rangoParaSegundoDeposito.FechaFin = new DateTime(2024, 1, 25);
        Deposito segundoDeposito = new Deposito
        {
            Nombre = "UnNombreDepoDos"
        };
        segundoDeposito.AgregarFechaDeDisponibilidad(rangoParaSegundoDeposito, _logicaReserva.GetAll());
        
        RangoDeFechas rangoParaTercerDeposito = new RangoDeFechas();
        rangoParaTercerDeposito.FechaInicio = new DateTime(2024, 2, 1);
        rangoParaTercerDeposito.FechaFin = new DateTime(2024, 2, 28);
        Deposito tercerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoTres"
        };
        tercerDeposito.AgregarFechaDeDisponibilidad(rangoParaTercerDeposito, _logicaReserva.GetAll());

        RangoDeFechas filtroRango = new RangoDeFechas();
        filtroRango.FechaInicio = new DateTime(2024, 1, 10);
        filtroRango.FechaFin = new DateTime(2024, 1, 20);
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(primerDeposito);
        _logicaDeposito.Add(segundoDeposito);
        _logicaDeposito.Add(tercerDeposito);
        
        List<Deposito> depositosDisponiblesEnPrimerMes = _logicaDeposito.ObtenerDepositosEnRango(filtroRango);
        
        Assert.AreEqual(2, depositosDisponiblesEnPrimerMes.Count);
        Assert.IsTrue(depositosDisponiblesEnPrimerMes.Contains(primerDeposito) 
                      && depositosDisponiblesEnPrimerMes.Contains(segundoDeposito));
    }

    [TestMethod]
    public void ObtenerDepositosEnRangoEnElQueNingunoEsteDisponibleOkTest() {
        RangoDeFechas rangoParaPrimerDeposito = new RangoDeFechas();
        rangoParaPrimerDeposito.FechaInicio = new DateTime(2024, 1, 1);
        rangoParaPrimerDeposito.FechaFin = new DateTime(2024, 1, 30);
        Deposito primerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoUno"
        };
        primerDeposito.AgregarFechaDeDisponibilidad(rangoParaPrimerDeposito, _logicaReserva.GetAll());
        
        RangoDeFechas rangoParaSegundoDeposito = new RangoDeFechas();
        rangoParaSegundoDeposito.FechaInicio = new DateTime(2024, 1, 5);
        rangoParaSegundoDeposito.FechaFin = new DateTime(2024, 1, 25);
        Deposito segundoDeposito = new Deposito
        {
            Nombre = "UnNombreDepoDos"
        };
        segundoDeposito.AgregarFechaDeDisponibilidad(rangoParaSegundoDeposito, _logicaReserva.GetAll());
        
        RangoDeFechas rangoParaTercerDeposito = new RangoDeFechas();
        rangoParaTercerDeposito.FechaInicio = new DateTime(2024, 2, 1);
        rangoParaTercerDeposito.FechaFin = new DateTime(2024, 2, 28);
        Deposito tercerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoTres"
        };
        tercerDeposito.AgregarFechaDeDisponibilidad(rangoParaTercerDeposito, _logicaReserva.GetAll());

        RangoDeFechas filtroRango = new RangoDeFechas();
        filtroRango.FechaInicio = new DateTime(2024, 3, 10);
        filtroRango.FechaFin = new DateTime(2024, 4, 10);
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(primerDeposito);
        _logicaDeposito.Add(segundoDeposito);
        _logicaDeposito.Add(tercerDeposito);
        
        List<Deposito> depositosDisponiblesEnPrimerMes = _logicaDeposito.ObtenerDepositosEnRango(filtroRango);
        
        Assert.AreEqual(0, depositosDisponiblesEnPrimerMes.Count);
        Assert.IsFalse(depositosDisponiblesEnPrimerMes.Contains(primerDeposito) 
                      && depositosDisponiblesEnPrimerMes.Contains(segundoDeposito));
    }

    [TestMethod]
    public void ObtenerDepositoParcialmenteEnRangoYQueNoLoMuestreOkTest() {
        RangoDeFechas rangoParaPrimerDeposito = new RangoDeFechas();
        rangoParaPrimerDeposito.FechaInicio = new DateTime(2024, 1, 1);
        rangoParaPrimerDeposito.FechaFin = new DateTime(2024, 1, 15);
        Deposito primerDeposito = new Deposito
        {
            Nombre = "UnNombreDepoUno"
        };
        primerDeposito.AgregarFechaDeDisponibilidad(rangoParaPrimerDeposito, _logicaReserva.GetAll());
        
        RangoDeFechas rangoParaSegundoDeposito = new RangoDeFechas();
        rangoParaSegundoDeposito.FechaInicio = new DateTime(2024, 1, 20);
        rangoParaSegundoDeposito.FechaFin = new DateTime(2024, 1, 25);
        Deposito segundoDeposito = new Deposito
        {
            Nombre = "UnNombreDepoDos"
        };
        segundoDeposito.AgregarFechaDeDisponibilidad(rangoParaSegundoDeposito, _logicaReserva.GetAll());

        RangoDeFechas filtroRango = new RangoDeFechas();
        filtroRango.FechaInicio = new DateTime(2024, 1, 13);
        filtroRango.FechaFin = new DateTime(2024, 1, 23);
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(primerDeposito);
        _logicaDeposito.Add(segundoDeposito);
        
        List<Deposito> depositosDisponiblesEnPrimerMes = _logicaDeposito.ObtenerDepositosEnRango(filtroRango);
        
        Assert.AreEqual(0, depositosDisponiblesEnPrimerMes.Count);
        Assert.IsFalse(depositosDisponiblesEnPrimerMes.Contains(primerDeposito) 
                       && depositosDisponiblesEnPrimerMes.Contains(segundoDeposito));

    }

    [TestMethod]
    public void EncontrarDepositoEnListaOkTest()
    {
        Deposito unDepo = new Deposito
        {
            Nombre = "UnNombreDepo"
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaDeposito.Add(unDepo);

        Deposito depoRetorno = _logicaDeposito.Find(x => x.Id == unDepo.Id);
        
        Assert.AreEqual(depoRetorno.Id, unDepo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaDepositoException))]
    public void BuscarDepositoQueNoExisteTest() {
        _logicaDeposito.Find(x => x.Id == 5);
    }
}