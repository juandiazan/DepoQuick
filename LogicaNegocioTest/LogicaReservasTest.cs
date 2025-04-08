using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaReservasTest
{
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    private LogicaDeposito _logicaDeposito;
    private LogicaReserva _logicaReserva;
    private LogicaCalculoPrecio _logicaCalculoPrecio;

    private Usuario _adminParaInicioSesion;
    private Usuario _clienteParaIniciarSesion;
    private ContextoSql _contexto;
    
    [TestInitialize]
    public void SetUp() {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(new RepositorioUsuarioBaseDeDatos(_contexto), _logicaSesion);
        _logicaDeposito = new LogicaDeposito(new RepositorioDepositoBaseDeDatos(_contexto), _logicaSesion);
        _logicaReserva = new LogicaReserva(new RepositorioReservaBaseDeDatos(_contexto), _logicaSesion);
        _logicaCalculoPrecio = new LogicaCalculoPrecio();
        
        _adminParaInicioSesion = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "UnNombre",
            Apellido = "UnApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };
        _clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "UnNombre",
            Apellido = "UnApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(_adminParaInicioSesion);
        _logicaUsuario.Add(_clienteParaIniciarSesion);
        
    }
    
    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void RealizarReservaDepositoPequeñoSinClimatizacionNiPromoOk()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };
        
        reserva.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);
        
        Reserva reservaConfirmada = _logicaReserva.ConfirmarYAgregarReserva(reserva, costoReserva);
        
        Assert.AreEqual(costoReserva, reservaConfirmada.Costo);
        
        Assert.IsTrue(reservaConfirmada.EstadoAprobacionCliente);
        
        Assert.AreEqual(1, reserva.Id);
    }

    [TestMethod]
    public void RealizarDosReservasDistintasOk()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);
        
        Reserva reserva1 = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };

        reserva1.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        double costoReserva1 = _logicaCalculoPrecio.CalcularPrecioBase(reserva1.Deposito, reserva1.RangoDeFechas);
        
        Reserva reservaConfirmada1 = _logicaReserva.ConfirmarYAgregarReserva(reserva1, costoReserva1);
        
        RangoDeFechas rango2 = new RangoDeFechas();
        rango2.FechaInicio = new DateTime(2024, 12, 8);
        rango2.FechaFin = new DateTime(2024, 12, 14);

        Reserva reserva2 = new Reserva()
        {
            RangoDeFechas = rango2,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoDos",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };

        reserva2.Deposito.AgregarFechaDeDisponibilidad(rango2, _logicaReserva.GetAll());
        
        double costoReserva2 = _logicaCalculoPrecio.CalcularPrecioBase(reserva2.Deposito, reserva2.RangoDeFechas);
        
        Reserva reservaConfirmada2 = _logicaReserva.ConfirmarYAgregarReserva(reserva2, costoReserva2);
        
        
        Assert.AreEqual(costoReserva1, reservaConfirmada1.Costo);
        Assert.IsTrue(reservaConfirmada1.EstadoAprobacionCliente);
        Assert.AreEqual(1, reserva1.Id);
        
        Assert.AreEqual(costoReserva2, reservaConfirmada2.Costo);
        Assert.IsTrue(reservaConfirmada2.EstadoAprobacionCliente);
        Assert.AreEqual(2, reserva2.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RealizarReservaConCostoNegativoTest()
    {
        Reserva reserva = new Reserva();
        _logicaReserva.ConfirmarReserva(reserva, -1);
    }
    
    [TestMethod]
    public void ObtenerReservasDeUnUsuario ()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);
        
        Reserva reserva1 = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };

        reserva1.Deposito.AgregarFechaDeDisponibilidad(reserva1.RangoDeFechas, _logicaReserva.GetAll());
        
        double costoReserva1 = _logicaCalculoPrecio.CalcularPrecioBase(reserva1.Deposito, reserva1.RangoDeFechas);
        
        Reserva reservaConfirmada1 = _logicaReserva.ConfirmarYAgregarReserva(reserva1, costoReserva1);
        
        RangoDeFechas rangoParaSegundaReserva = new RangoDeFechas();
        rangoParaSegundaReserva.FechaInicio = new DateTime(2024, 12, 8);
        rangoParaSegundaReserva.FechaFin = new DateTime(2024, 12, 15);
        
        Reserva reserva2 = new Reserva()
        {
            RangoDeFechas = rangoParaSegundaReserva,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoDos",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };
        
        reserva2.Deposito.AgregarFechaDeDisponibilidad(reserva2.RangoDeFechas, _logicaReserva.GetAll());

        double costoReserva2 = _logicaCalculoPrecio.CalcularPrecioBase(reserva2.Deposito, reserva2.RangoDeFechas);
        
        Reserva reservaConfirmada2 = _logicaReserva.ConfirmarYAgregarReserva(reserva2, costoReserva2);
        
        List<Reserva> reservas = _logicaReserva.ObtenerReservasDeUsuario(_clienteParaIniciarSesion);
        
        Assert.AreEqual(2, reservas.Count);
        Assert.AreEqual(reservaConfirmada1, reservas[0]);
        Assert.AreEqual(reservaConfirmada2, reservas[1]);
    }
    
    [TestMethod]
    public void RealizarReservaConPagoOkTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };
        
        reserva.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);
        
        Reserva reservaConfirmada = _logicaReserva.ConfirmarYAgregarReserva(reserva, costoReserva);
        
        Assert.AreEqual(costoReserva, reservaConfirmada.Pago.Monto);
    }

    [TestMethod]
    public void RealizarDosReservasYAumentaIdPagoOkTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);

        Reserva reserva1 = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };

        reserva1.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());

        double costoReserva1 = _logicaCalculoPrecio.CalcularPrecioBase(reserva1.Deposito, reserva1.RangoDeFechas);
        
        Reserva reservaConfirmada1 = _logicaReserva.ConfirmarYAgregarReserva(reserva1, costoReserva1);

        RangoDeFechas rango2 = new RangoDeFechas();
        rango2.FechaInicio = new DateTime(2024, 12, 8);
        rango2.FechaFin = new DateTime(2024, 12, 14);

        Reserva reserva2 = new Reserva()
        {
            RangoDeFechas = rango2,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoDos",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };

        reserva2.Deposito.AgregarFechaDeDisponibilidad(rango2, _logicaReserva.GetAll());

        double costoReserva2 = _logicaCalculoPrecio.CalcularPrecioBase(reserva2.Deposito, reserva2.RangoDeFechas);
        
        Reserva reservaConfirmada2 = _logicaReserva.ConfirmarYAgregarReserva(reserva2, costoReserva2);

        Assert.AreEqual(1, reservaConfirmada1.Pago.Id);
        Assert.AreEqual(2, reservaConfirmada2.Pago.Id);
    }
    
    [TestMethod]
    public void RealizarReservaYVerificarEstadoOkTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 7);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = _clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = false
            }
        };
        
        reserva.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);
        
        Reserva reservaConfirmada = _logicaReserva.ConfirmarYAgregarReserva(reserva, costoReserva);
        
        Assert.AreEqual("Reservado", reservaConfirmada.Pago.Estado);
    }

    [TestMethod]
    public void ObtenerReservasDeUnDepositoOk() {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas rangoParaPrimeraReserva = new RangoDeFechas();
        rangoParaPrimeraReserva.FechaInicio = new DateTime(2024, 12, 1);
        rangoParaPrimeraReserva.FechaFin = new DateTime(2024, 12, 7);
        RangoDeFechas rangoParaSegundaReserva = new RangoDeFechas();
        rangoParaSegundaReserva.FechaInicio = new DateTime(2024, 12, 8);
        rangoParaSegundaReserva.FechaFin = new DateTime(2024, 12, 15);

        Deposito primerDeposito = new Deposito()
        {
            Nombre = "PrimerDepo",
            Id = 1
        };
        primerDeposito.AgregarFechaDeDisponibilidad(rangoParaPrimeraReserva, _logicaReserva.GetAll());
        primerDeposito.AgregarFechaDeDisponibilidad(rangoParaSegundaReserva, _logicaReserva.GetAll());
        
        Reserva primeraReserva = new Reserva()
        {
            RangoDeFechas = rangoParaPrimeraReserva,
            Usuario = _clienteParaIniciarSesion,
            Deposito = primerDeposito,
            Pago = new Pago
            {
                Estado = "Reservado"
            }
        };
        
        Reserva segundaReserva = new Reserva()
        {
            RangoDeFechas = rangoParaSegundaReserva,
            Usuario = _clienteParaIniciarSesion,
            Deposito = primerDeposito,
            Pago = new Pago
            {
                Estado = "Reservado"
            }
        };

        _logicaReserva.ConfirmarYAgregarReserva(primeraReserva, _logicaCalculoPrecio.CalcularPrecioBase(primeraReserva.Deposito, primeraReserva.RangoDeFechas));
        _logicaReserva.ConfirmarYAgregarReserva(segundaReserva, _logicaCalculoPrecio.CalcularPrecioBase(segundaReserva.Deposito, segundaReserva.RangoDeFechas));
        
        RangoDeFechas nuevoRango = new RangoDeFechas();
        nuevoRango.FechaInicio = new DateTime(2024, 12, 8);
        nuevoRango.FechaFin = new DateTime(2024, 12, 15);
        Deposito otroDepo = new Deposito()
        {
            Nombre = "OtroDepo",
            Id = 7
        };
        otroDepo.AgregarFechaDeDisponibilidad(nuevoRango, _logicaReserva.GetAll());
        Reserva terceraReserva = new Reserva()
        {
            RangoDeFechas = rangoParaSegundaReserva,
            Usuario = _clienteParaIniciarSesion,
            Deposito = otroDepo,
            Pago = new Pago
            {
                Estado = "Reservado"
            }
        };
        
        _logicaReserva.ConfirmarYAgregarReserva(terceraReserva, _logicaCalculoPrecio.CalcularPrecioBase(terceraReserva.Deposito, terceraReserva.RangoDeFechas));
        
        Assert.AreEqual(2, _logicaReserva.ObtenerReservasDeUnDeposito(primerDeposito.Id).Count);
    }
}