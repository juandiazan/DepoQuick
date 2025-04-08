using Dominio;

namespace DominioTest;

[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuarioOkTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConNombreVacioTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "",
            Apellido = "unApellido",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConNombreConNumerosTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre12345",
            Apellido = "unApellido",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConNombreConSimbolosTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre!@#.$%,",
            Apellido = "unApellido",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConApellidoVacioTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))] 
    public void CrearUsuarioConApellidoNullTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = null,
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConApellidoConNumerosTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido12345",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConApellidoConSimbolosTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido!@#$%.,",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConNombreApellidoSuperaMaximoCaracteresTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombreUsu-unNombreUsu-unNombreUsu-unNombreUsu-unNombreUsu-unNombreUsu",
            Apellido = "unNombreUsu-unNombreUsu-unNombreUsu-unNombreUsu-unNombreUsu",
            Email = "correoElectronico@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoVacioTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoNullTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = null,
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoSinArrobaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreoSinArroba",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoSinTextoAntesDeArrobaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "@correo",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoSinTextoDespuesDeArrobaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConCorreoConMasDeUnArrobaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@unCorreo@unCorreo",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaVaciaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "",
            VerificacionContrasena = ""
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaNullTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = null,
            VerificacionContrasena = null
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaConLargoMenorAlMinimoTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "123Aa#!",
            VerificacionContrasena = "123Aa#!"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaSinSimboloTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "12345AaBb",
            VerificacionContrasena = "12345AaBb"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaSinLetraMinusculaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "12345A!$%",
            VerificacionContrasena = "12345A!$%"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaSinLetraMayusculaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "12345a!$%",
            VerificacionContrasena = "12345a!$%"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConContraseñaSinNumeroTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "ABCabc!$%",
            VerificacionContrasena = "ABCabc!$%"
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConVerificacionDeContrasenaErroneaTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "123ABCabc!$%",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConEspaciosEnNombreTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "   unNombre ",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioUsuarioException))]
    public void CrearUsuarioConEspaciosEnApellidoTest() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "  unApellido  ",
            Email = "unCorreo@gmail.com",
            Contrasena = "Passw123@",
            VerificacionContrasena = "Passw123@"
        };
    }
}
