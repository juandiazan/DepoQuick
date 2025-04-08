using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaGestionReservaTest
{
    private LogicaUsuario _logicaUsuario;
    private LogicaSesion _logicaSesion;

    private LogicaDeposito _logicaDeposito;
    private LogicaReserva _logicaReserva;
    private LogicaCalculoPrecio _logicaCalculoPrecio;

    private Usuario _adminParaInicioSesion;
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
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };
        _logicaUsuario.Add(_adminParaInicioSesion);
    }
    
    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void TestAceptarReservaOk()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true,
                Nombre = "DepositoUno"
            }
        };
        
        reserva.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());
        
        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);
        
        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmada = _logicaReserva.Add(reserva);
        
        _logicaReserva.AceptarReserva(reservaConfirmada);
        Assert.IsTrue(reservaConfirmada.EstadoAprobacionAdmin);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void TestAceptarReservaConReservaNula()
    {
        _logicaReserva.AceptarReserva(null);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void TestAceptarReservaConReservaNoConfirmada()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            },
            Pago = new Pago
            {
                Estado = "Reservado"
            }
        };
        
        Reserva reservaNoConfirmada = _logicaReserva.Add(reserva);

        _logicaReserva.AceptarReserva(reservaNoConfirmada);

    }

    [TestMethod]
    public void RechazarReservaOkTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };
        
        reserva.Deposito.AgregarFechaDeDisponibilidad(rango, _logicaReserva.GetAll());

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        string motivoRechazo = "No hay disponibilidad";
        _logicaReserva.RechazarReserva(reservaConfirmadaCliente, motivoRechazo);
        Assert.IsFalse(reservaConfirmadaCliente.EstadoAprobacionAdmin);
        Assert.AreEqual(motivoRechazo, reservaConfirmadaCliente.MotivoRechazo);

    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaConReservaNulaTest()
    {
        _logicaReserva.RechazarReserva(null, "No hay disponibilidad");
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaConReservaNoConfirmadaTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Nombre = "DepositoUno",
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            },
            Pago = new Pago
            {
                Estado = "Reservado"
            }
        };
        
        Reserva reservaNoConfirmada = _logicaReserva.Add(reserva);

        _logicaReserva.RechazarReserva(reservaNoConfirmada, "No hay disponibilidad");
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaConMotivoMayorA300CaracteresTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        string motivoRechazo = "a".PadRight(301, 'a');

        _logicaReserva.RechazarReserva(reservaConfirmadaCliente, motivoRechazo);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaConMotivoVacioTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        _logicaReserva.RechazarReserva(reservaConfirmadaCliente, "");

    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void AprobarReservaSinUnAdminConectadoTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(clienteParaIniciarSesion, clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        _logicaReserva.AceptarReserva(reservaConfirmadaCliente);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void AprobarReservaSinNadieConectadoTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        _logicaReserva.AceptarReserva(reservaConfirmadaCliente);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaSinUnAdminConectadoTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(clienteParaIniciarSesion, clienteParaIniciarSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);
        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        _logicaReserva.RechazarReserva(reservaConfirmadaCliente, "No hay disponibilidad");
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RechazarReservaSinNadieConectadoTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };

        double costoReserva = _logicaCalculoPrecio.CalcularPrecioBase(reserva.Deposito, reserva.RangoDeFechas);

        _logicaReserva.ConfirmarReserva(reserva, costoReserva);

        Reserva reservaConfirmadaCliente = _logicaReserva.Add(reserva);

        _logicaReserva.RechazarReserva(reservaConfirmadaCliente, "No hay disponibilidad");
    }

    [TestMethod]
    public void ReservarDepositoConDisponibilidadOkTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 5);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 10);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 15);

        Deposito unDeposito = new Deposito
        {
            Nombre = "DepositoUno",
        };
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            Usuario = _adminParaInicioSesion,
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
      
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
        _logicaReserva.Add(unaReserva);
        
        _logicaReserva.AceptarReserva(unaReserva);
        
        Assert.IsTrue(unaReserva.EstadoAprobacionAdmin);
        Assert.IsFalse(unaReserva.Deposito.EstaDisponibleEnRango(tiempoDisponible));
        Assert.AreEqual(2, unaReserva.Deposito.CantidadFechasDisponibilidad());
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void ReservarDepositoSinDisponibilidadTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 20);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 25);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 15);
      
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void ReservarDepositoConDisponibilidadParcialFechaInicioTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 10);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 25);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 15);
      
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void ReservarDepositoConDisponibilidadParcialFechaFinTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 12);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 10);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 15);
      
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
    }
    
    [TestMethod]
    public void RealizarReservaQueDejeAlDepositoDisponiblePorUnDiaYQueNoEsteDisponibleTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 2);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 11);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 12);
      
        Deposito unDeposito = new Deposito
        {
            Nombre = "UnDeposito"
        };
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            Usuario = _adminParaInicioSesion,
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
        _logicaReserva.Add(unaReserva);
        _logicaReserva.AceptarReserva(unaReserva);
        
        Assert.AreEqual(0, unaReserva.Deposito.CantidadFechasDisponibilidad());
    }
    
    [TestMethod]
    public void VerificarEstadoDePagoCapturadoAlAceptarReservaTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 2);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 11);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 1);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 12);

        Deposito unDeposito = new Deposito
        {
            Nombre = "UnDeposito"
        };
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            Usuario = _adminParaInicioSesion,
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
        _logicaReserva.Add(unaReserva);
        
        _logicaReserva.AceptarReserva(unaReserva);
        
        Assert.AreEqual("Capturado", unaReserva.Pago.Estado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void RealizarReservaSinPagoTest()
    {
        Usuario clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(clienteParaIniciarSesion);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 12, 1);
        rango.FechaFin = new DateTime(2024, 12, 10);
        
        Reserva reserva = new Reserva()
        {
            RangoDeFechas = rango,
            Usuario = clienteParaIniciarSesion,
            Deposito = new Deposito()
            {
                Area = AreasDeDeposito.A,
                Tamanio = TamaniosDeDeposito.Pequeño,
                Climatizacion = true
            }
        };
        
        _logicaReserva.Add(reserva);
    }

    [TestMethod]
    public void ConfirmarReservaYQueNoCambieFechaDisponibilidadDeDepositoReservado() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas tiempoDeReserva = new RangoDeFechas();
        tiempoDeReserva.FechaInicio = new DateTime(2024, 1, 10);
        tiempoDeReserva.FechaFin = new DateTime(2024, 1, 11);

        RangoDeFechas tiempoDisponible = new RangoDeFechas();
        tiempoDisponible.FechaInicio = new DateTime(2024, 1, 10);
        tiempoDisponible.FechaFin = new DateTime(2024, 1, 11);
      
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(tiempoDisponible, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            RangoDeFechas = tiempoDeReserva,
            Deposito = unDeposito
        };
        
        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDeposito, tiempoDeReserva));
        
        Assert.IsTrue(unaReserva.Deposito.EstaDisponibleEnRango(tiempoDisponible));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaReservaException))]
    public void AprobarDosReservasDeMismoDepositoParaMismaFechaTest() {
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        RangoDeFechas unaFecha = new RangoDeFechas();
        unaFecha.FechaInicio = new DateTime(2024, 1, 1);
        unaFecha.FechaFin = new DateTime(2024, 1, 10);

        RangoDeFechas otraFecha = new RangoDeFechas()
        {
            FechaInicio = new DateTime(2024, 1, 4),
            FechaFin = new DateTime(2024, 1, 7)
        };

        Deposito unDepo = new Deposito
        {
            Nombre = "UnDepo"
        };
        unDepo.AgregarFechaDeDisponibilidad(unaFecha, _logicaReserva.GetAll());

        Reserva unaReserva = new Reserva()
        {
            Usuario = _adminParaInicioSesion,
            RangoDeFechas = unaFecha,
            Deposito = unDepo
        };

        Reserva otraReserva = new Reserva()
        {
            Usuario = _adminParaInicioSesion,
            RangoDeFechas = otraFecha,
            Deposito = unDepo
        };

        _logicaReserva.ConfirmarReserva(unaReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDepo, unaFecha));
        _logicaReserva.ConfirmarReserva(otraReserva, _logicaCalculoPrecio.CalcularPrecioBase(unDepo, otraFecha));
        
        _logicaReserva.Add(unaReserva);
        _logicaReserva.Add(otraReserva);
        
        _logicaReserva.AceptarReserva(unaReserva);
        _logicaReserva.AceptarReserva(otraReserva);
    }
}