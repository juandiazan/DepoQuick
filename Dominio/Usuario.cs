namespace Dominio;

public enum Rol
{
    Administrador,
    Cliente
}
public class Usuario
{
    private const int CantidadMaximaCaracteresNombreYApellido = 100;
    private const int CantidadMinimaCaracteresContrasena = 8;
    
    private string _nombre;
    private string _apellido;
    private string _correo;
    private string _contrasena;
    private string _verificacionContrasena;
    
    
    public string Nombre
    {
        get => _nombre;
        set
        {
            if (EsNuloOVacio(value)) {
                throw new DominioUsuarioException("El nombre del usuario no puede ser vacío");
            }
            if (TieneFormatoIncorrecto(value)) {
                throw new DominioUsuarioException("El formato del nombre de usuario es incorrecto");
            } 
            _nombre = value;
        }
    }
    public string Apellido
    {
        get => _apellido;
        set
        {
            if (EsNuloOVacio(value)) {
                throw new DominioUsuarioException("El apellido del usuario no puede ser vacío");
            } else if (TieneFormatoIncorrecto(value)) {
                throw new DominioUsuarioException("El formato del apellido del usuario es incorrecto");    
            } else if (NombreCompletoSuperaCantidadMaximaCaracteres(Nombre, value)){
                throw new DominioUsuarioException("El nombre completo del usuario es demasiado largo");
            } 
            _apellido = value;
        }
    }
    public string Email
    {
        get => _correo;
        set
        {
            if (EsNuloOVacio(value) ) {
                throw new DominioUsuarioException("El correo del usuario no puede ser vacío");    
            } else if (EsCorreoIncorrecto(value)) {
                throw new DominioUsuarioException("El formato del correo del usuario es incorrecto");
            }
            _correo = value;
        }
    }
    public string Contrasena
    {
        get => _contrasena;
        set
        {
            if (EsNuloOVacio(value)) {
                throw new DominioUsuarioException("La contraseña del usuario no puede ser vacía");
            } else if (EsContraseñaIncorrecta(value)) {
                throw new DominioUsuarioException("El formato de la contraseña del usuario es incorrecto");   
            }
            _contrasena = value;
        }
    }
    public string VerificacionContrasena
    {
        get => _verificacionContrasena;
        set
        {
            if (ContrasenasSonDistintas(Contrasena, value)) {
                throw new DominioUsuarioException("La contraseña y su verificación son distintas");   
            }
            _verificacionContrasena = value;
        }
    }
    public Rol RolDeUsuario { get; set; }
    
    
    private bool EsNuloOVacio(string unTexto) {
        return string.IsNullOrWhiteSpace(unTexto);
    }
    private bool TieneFormatoIncorrecto(string unTexto) {
        return ContieneNumeros(unTexto) || ContieneSimbolos(unTexto) || ContieneEspacios(unTexto);
    }
    private bool ContieneNumeros(string unTexto) {
        return unTexto.Any(unCaracter => char.IsNumber(unCaracter));
    }
    private bool ContieneSimbolos(string unTexto) {
        return unTexto.Contains('!') || unTexto.Contains('@') || unTexto.Contains('#')
               || unTexto.Contains('$') || unTexto.Contains('%') || unTexto.Contains('.') || unTexto.Contains(',');
    }
    private bool ContieneEspacios(string unTexto) {
        return unTexto.Contains(' ');
    }
    private bool NombreCompletoSuperaCantidadMaximaCaracteres(string unNombre, string unApellido) {
        return unNombre.Length + unApellido.Length > CantidadMaximaCaracteresNombreYApellido;
    }
    private bool EsCorreoIncorrecto(string unCorreo) {
        return NoTieneArroba(unCorreo) || NoTieneTextoAntesDeArroba(unCorreo) ||
               NoTieneTextoDespuesDeArroba(unCorreo) || TieneMasDeUnArroba(unCorreo);
    }
    private bool NoTieneArroba(string unCorreo) {
        return !unCorreo.Contains('@');
    }
    private bool NoTieneTextoAntesDeArroba(string unCorreo) {
        return unCorreo.Split('@')[0].Length == 0;
    }
    private bool NoTieneTextoDespuesDeArroba(string unCorreo) {
        return unCorreo.Split('@')[1].Length == 0;
    }
    private bool TieneMasDeUnArroba(string unCorreo) {
        return unCorreo.Split('@').Length > 2;
    }
    private bool EsContraseñaIncorrecta(string unaContrasena) {
        return NoTieneMasDelMinimoDeCaracteres(unaContrasena) || !ContieneSimbolos(unaContrasena) || !ContieneNumeros(unaContrasena)
               || NoTieneLetrasMinusculas(unaContrasena) || NoTieneLetrasMayusculas(unaContrasena);
    }
    private bool NoTieneMasDelMinimoDeCaracteres(string unaContrasena) {
        return unaContrasena.Length < CantidadMinimaCaracteresContrasena;
    }
    private bool NoTieneLetrasMinusculas(string unaContrasena) {
        return !unaContrasena.Any(unCaracter => char.IsLower(unCaracter));
    }
    private bool NoTieneLetrasMayusculas(string unaContrasena) {
        return !unaContrasena.Any(unCaracter => char.IsUpper(unCaracter));
    }
    private bool ContrasenasSonDistintas(string unaContrasena, string verificacionContrasena) {
        return !unaContrasena.Equals(verificacionContrasena);
    }
}

