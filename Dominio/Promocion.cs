namespace Dominio;

public class Promocion
{
    private const int PorcentajeMinimo = 5;
    private const int PorcentajeMaximo = 75;
    private const int LargoMaximoDeEtiqueta = 20;
    
    private string _etiqueta;
    private int _porcentajeDescuento;
    
    
    public int Id { get; set; }
    public string Etiqueta
    {
        get => _etiqueta;
        set
        {
            if (EsNuloOEspacioVacio(value))
            {
                throw new DominioPromocionException("La etiqueta de la promoción no puede estar vacía");
            } else if (EtiquetaTieneMasDelMaximoDeCaracteres(value))
            {
                throw new DominioPromocionException("La etiqueta de la promoción no puede exceder los 20 caracteres");
            }
            _etiqueta = value;
        }
    }
    public int PorcentajeDescuento
    {
        get => _porcentajeDescuento;
        set
        {
            if (PorcentajeMenorACotaInferior(value))
            {
                throw new DominioPromocionException("El porcentaje de descuento no puede ser menor al 5%");
            } else if (PorcentajeMayorACotaSuperior(value)) {
                throw new DominioPromocionException("El porcentaje de descuento no puede ser mayor al 75%");
            }
            _porcentajeDescuento = value;
        }
    }
    public RangoDeFechas rangoFechas { get; set; }
    public virtual List<Deposito> listaDepositos { get; set; } = new List<Deposito>();
    
    
    public override string ToString() {
        return $"{Id} - {Etiqueta} - {PorcentajeDescuento}% - Válida desde: {rangoFechas.FechaInicio.Day}/{rangoFechas.FechaInicio.Month}/{rangoFechas.FechaInicio.Year} - Vence: {rangoFechas.FechaFin.Day}/{rangoFechas.FechaFin.Month}/{rangoFechas.FechaFin.Year}";
    }
    
    
    private bool EsNuloOEspacioVacio(string unaEtiqueta)
    {
        return string.IsNullOrWhiteSpace(unaEtiqueta);
    }
    private bool EtiquetaTieneMasDelMaximoDeCaracteres(string unaEtiqueta)
    {
        return unaEtiqueta.Length > LargoMaximoDeEtiqueta;
    }
    private bool PorcentajeMenorACotaInferior(int unPorcenjateDeDescuento)
    {
        return unPorcenjateDeDescuento < PorcentajeMinimo;
    }
    private bool PorcentajeMayorACotaSuperior(int unPorcenjateDeDescuento)
    {
        return unPorcenjateDeDescuento > PorcentajeMaximo;
    }
}
