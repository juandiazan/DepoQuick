namespace Dominio;
public class DominioPagoException : Exception
{
    public DominioPagoException(string? mensaje) : base(mensaje) {
        
    }
}