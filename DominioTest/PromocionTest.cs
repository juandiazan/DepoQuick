using Dominio;

namespace DominioTest;
    
[TestClass]
public class PromocionTest
{
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void CrearPromocionConEtiquetaVaciaTest() {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(10);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void CrearPromocionConEtiquetaNullTest()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(10);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = null,
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void CrearPromocionConEtiquetaExcede20CaracteresTest()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(10);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta que excede los 20 caracteres",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void CrearPromocionConPorcentajeDescuentoMenorA5Test()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(10);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta",
            PorcentajeDescuento = 4,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void CrearPromocionConPorcentajeDescuentoMayorA75Test()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(10);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta",
            PorcentajeDescuento = 76,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioException))]
    public void CrearPromocionConFechaInicioPosteriorAFechaFinTest()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now.AddDays(10);
        rango.FechaFin = DateTime.Now;
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioException))]
    public void CrearPromocionConFechaInicioIgualAFechaFin()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Today;
        rango.FechaFin = DateTime.Today;
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
    }
    
    [TestMethod]
    public void CrearPromocionOkTest()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = DateTime.Now;
        rango.FechaFin = DateTime.Now.AddDays(12);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Una etiqueta",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };

        Assert.AreEqual("Una etiqueta", unaPromocion.Etiqueta);
        Assert.AreEqual(10, unaPromocion.PorcentajeDescuento);
        Assert.AreEqual(DateTime.Now.Date, unaPromocion.rangoFechas.FechaInicio.Date);
        Assert.AreEqual(DateTime.Now.AddDays(12).Date, unaPromocion.rangoFechas.FechaFin.Date);
    }
}