using Dominio;
using LogicaNegocio;

namespace Controladores;
public class ControladorPromocion
{
    private LogicaPromocion _logicaPromocion;

    public ControladorPromocion(LogicaPromocion logicaPromocion)
    {
        _logicaPromocion = logicaPromocion;
    }

    public Promocion AgregarPromocion(Promocion promocion)
    {
        return _logicaPromocion.Add(promocion);
    }
    
    public List<Promocion> ObtenerTodasLasPromociones()
    {
        return _logicaPromocion.GetAll();
    }
    
    public Promocion ActualizarPromocion(Promocion promocion)
    {
        return _logicaPromocion.Update(promocion);
    }
    
    public void EliminarPromocion(List<Deposito> listaDepositos, Promocion promocion)
    {
        _logicaPromocion.Delete(listaDepositos, promocion);
    }
    
    public Promocion BuscarPromocion(Func<Promocion, bool> filtro)
    {
        return _logicaPromocion.Find(filtro);
    }
}