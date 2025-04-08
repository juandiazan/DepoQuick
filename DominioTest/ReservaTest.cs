using Dominio;

namespace DominioTest;

[TestClass]
public class ReservaTest
{
   [TestMethod]
   public void CrearReservaOkTest() {
      Usuario unUsuario = new Usuario()
      {
         Nombre = "unUsuario",
         Apellido = "unApellido",
         Email = "correoElectronico@gmail.com",
         Contrasena = "Passw123@",
         VerificacionContrasena = "Passw123@",
         RolDeUsuario = Rol.Cliente
      };

      RangoDeFechas rango = new RangoDeFechas();
      rango.FechaInicio = new DateTime(2024, 12, 1);
      rango.FechaFin = new DateTime(2024, 12, 10);
      Reserva unaReserva = new Reserva()
      {
         RangoDeFechas = rango,
         Usuario = unUsuario,
         Deposito = new Deposito()
         {
            Area = AreasDeDeposito.A,
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
         }
      };
   }
   
   [TestMethod]
   [ExpectedException(typeof(DominioException))]
   public void CrearReservaConFechaFinInvalidaTest() {
      Usuario unUsuario = new Usuario()
      {
         Nombre = "unUsuario",
         Apellido = "unApellido",
         Email = "correoElectronico@gmail.com",
         Contrasena = "Passw123@",
         VerificacionContrasena = "Passw123@",
         RolDeUsuario = Rol.Cliente
      };
      
      RangoDeFechas rango = new RangoDeFechas();
      rango.FechaInicio = new DateTime(2024, 12, 10);
      rango.FechaFin = new DateTime(2024, 12, 1);
      Reserva unaReserva = new Reserva()
      {
         RangoDeFechas = rango,
         Usuario = unUsuario,
         Deposito = new Deposito()
         {
            Area = AreasDeDeposito.A,
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
         }
      };
   }
   
   [TestMethod]
   public void CrearReservaConUsuarioNoClienteOkTest() {
      Usuario unUsuario = new Usuario()
      {
         Nombre = "unUsuario",
         Apellido = "unApellido",
         Email = "correoElectronico@gmail.com",
         Contrasena = "Passw123@",
         VerificacionContrasena = "Passw123@",
         RolDeUsuario = Rol.Administrador
      };
      
      RangoDeFechas rango = new RangoDeFechas();
      rango.FechaInicio = new DateTime(2024, 12, 1);
      rango.FechaFin = new DateTime(2024, 12, 10);
      Reserva unaReserva = new Reserva()
      {
         RangoDeFechas = rango,
         Usuario = unUsuario,
         Deposito = new Deposito()
         {
            Area = AreasDeDeposito.A,
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
         }
      };
   }

   [TestMethod]
   public void ProbarToStringDeReservaTest()
   {
      Usuario unUsuario = new Usuario()
      {
         Nombre = "unUsuario",
         Apellido = "unApellido",
         Email = "correoElectronico@gmail.com",
         Contrasena = "Passw123@",
         VerificacionContrasena = "Passw123@",
         RolDeUsuario = Rol.Cliente
      };

      RangoDeFechas rango = new RangoDeFechas();
      rango.FechaInicio = new DateTime(2024, 12, 1);
      rango.FechaFin = new DateTime(2024, 12, 10);
      Reserva unaReserva = new Reserva()
      {
         RangoDeFechas = rango,
         Usuario = unUsuario,
         Deposito = new Deposito()
         {
            Area = AreasDeDeposito.A,
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
         }
      };

      string esperado = "Usuario: unUsuario unApellido, Rango de fechas: 01/12/2024 - 10/12/2024"; 

      Assert.AreEqual(esperado, unaReserva.ToString());
   }
}