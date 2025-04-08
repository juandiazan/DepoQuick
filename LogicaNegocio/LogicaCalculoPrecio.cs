using Dominio;

namespace LogicaNegocio;

public class LogicaCalculoPrecio
{
    private const double PorcentajeADecimal = 0.01;
    private const int MinPromociones = 0;
    private const int CotaInferiorDias = 0;
    
    private const int PrecioPorClimatizacion = 20;
    
    private const int DiasDescuento = 7;
    private const int DiasDescuento14 = 14;
    private const double PorcentajeDescuento7Y14 = 0.05;
    private const double PorcentajeDescuentoMas14 = 0.1;
    
    private const double PrecioBasePequenio = 50;
    private const double PrecioBaseMediano = 75;
    private const double PrecioBaseGrande = 100;
    
    
    public double CalcularPrecioBase(Deposito unDeposito, RangoDeFechas rangoFechas) {
        double precioBase = 0;
        int duracionAlquiler = (rangoFechas.FechaFin - rangoFechas.FechaInicio).Days;
        if (AlquilerDuraMenosDeUnDia(duracionAlquiler)) {
            throw new LogicaDepositoException("La fecha de fin no puede ser menor a la fecha de inicio");
        }
        
        if (EstaClimatizado(unDeposito)) {
            precioBase += PrecioPorClimatizacion * duracionAlquiler;
        }
        
        if (EsDepositoPequenio(unDeposito)) {
            precioBase += PrecioBasePequenio * duracionAlquiler;
        }
        else if (EsDepositoMediano(unDeposito)) {
            precioBase += PrecioBaseMediano * duracionAlquiler;
        }
        else if (EsDepositoGrande(unDeposito)) {
            precioBase += PrecioBaseGrande * duracionAlquiler;
        }
        
        if (AlquilerEntreSieteYCatorceDias(duracionAlquiler)) {
            precioBase = precioBase - (precioBase * PorcentajeDescuento7Y14);
        }
        else if (AlquilerDeMasDeCatorceDias(duracionAlquiler)) {
            precioBase = precioBase - (precioBase * PorcentajeDescuentoMas14);
        }
        
        if (HayPromociones(unDeposito))
        {
            precioBase = AplicarDescuentoPromocion(unDeposito, rangoFechas, precioBase);
        }
        
        return precioBase;
    }
    
    
    private bool AlquilerDuraMenosDeUnDia(int duracionAlquiler)
    {
        return duracionAlquiler <= CotaInferiorDias;
    }
    private bool EstaClimatizado(Deposito unDeposito)
    {
        return unDeposito.Climatizacion;
    }
    private bool EsDepositoPequenio(Deposito unDeposito)
    {
        return unDeposito.Tamanio == TamaniosDeDeposito.Pequeño;
    }
    private bool EsDepositoMediano(Deposito unDeposito)
    {
        return unDeposito.Tamanio == TamaniosDeDeposito.Mediano;
    }
    private bool EsDepositoGrande(Deposito unDeposito)
    {
        return unDeposito.Tamanio == TamaniosDeDeposito.Grande;
    }
    private bool AlquilerEntreSieteYCatorceDias(int duracionAlquiler)
    {
        return duracionAlquiler >= DiasDescuento && duracionAlquiler <= DiasDescuento14;
    }
    private bool AlquilerDeMasDeCatorceDias(int duracionAlquiler)
    {
        return duracionAlquiler > DiasDescuento14;
    }
    private bool HayPromociones(Deposito unDeposito)
    {
        return unDeposito.ListaPromociones.Count > MinPromociones;
    }
    private double AplicarDescuentoPromocion(Deposito unDeposito, RangoDeFechas rangoFechas, double precioBase)
    {
        foreach (Promocion promocion in unDeposito.ListaPromociones) {
            if (PromocionValida(rangoFechas, promocion)) {
                precioBase = precioBase - (precioBase * (promocion.PorcentajeDescuento * PorcentajeADecimal));
            }
        }
        return precioBase;
    }
    private bool PromocionValida(RangoDeFechas rangoFechas, Promocion promocion)
    {
        return rangoFechas.FechaInicio >= promocion.rangoFechas.FechaInicio && rangoFechas.FechaFin <= promocion.rangoFechas.FechaFin;
    }
}