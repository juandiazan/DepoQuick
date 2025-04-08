using Dominio;
using Repositorio;

namespace LogicaNegocio;

public class LogicaPromocion
{
    private IRepositorioCRUDFind<Promocion> _repositorioCrudFindPromocion;
    private LogicaSesion _logicaSesion;
    
    public LogicaPromocion(IRepositorioCRUDFind<Promocion> repositorioCrudFindPromocion, LogicaSesion logicaSesion)
    {
        _repositorioCrudFindPromocion = repositorioCrudFindPromocion;
        _logicaSesion = logicaSesion;
    }
    
    
    public Promocion Add(Promocion promocion)
    {
        if (!_logicaSesion.HayUnaSesionIniciada() || _logicaSesion.HayUnClienteEnSesion()) {
            throw new LogicaPromocionException("Solo un administrador puede ingresar nuevas promociones");
        }
        _repositorioCrudFindPromocion.Add(promocion);
        return promocion;
    }
    public List<Promocion> GetAll()
    {
        return _repositorioCrudFindPromocion.GetAll();
    }
    public Promocion Update(Promocion unaPromocion)
    {
        if (!_logicaSesion.HayUnaSesionIniciada() || _logicaSesion.HayUnClienteEnSesion()) {
            throw new LogicaPromocionException("Solo un administrador puede modificar una promoción");
        } else if (!ExistePromocion(unaPromocion.Id))
        {
            throw new LogicaPromocionException("No se encontro la promocion a modificar");
        }
        return _repositorioCrudFindPromocion.Update(unaPromocion);
    }
    public void Delete(List<Deposito> listaDepositos, Promocion unaPromocion)
    {
        if (!_logicaSesion.HayUnaSesionIniciada() || _logicaSesion.HayUnClienteEnSesion()) {
            throw new LogicaPromocionException("Solo un administrador puede eliminar una promoción");
        } else if (!ExistePromocion(unaPromocion.Id)) {
            throw new LogicaPromocionException("No se encontro la promocion a eliminar");
        } else if (ExisteDepositoQueUsaPromocion(listaDepositos, unaPromocion.Id)) {
            throw new LogicaPromocionException("La promoción a eliminar está aplicada a un depósito para reservar");
        }
        _repositorioCrudFindPromocion.Delete(unaPromocion);
    }
    public Promocion Find(Func <Promocion, bool> filtro) {
        Promocion promoRetorno = _repositorioCrudFindPromocion.Find(filtro);
        
        if (promoRetorno is null) {
            throw new LogicaPromocionException("No se encontró la promoción");
        }
        
        return _repositorioCrudFindPromocion.Find(filtro);
    }
    
    
    private bool ExistePromocion(int idPromocion)
    {
        return _repositorioCrudFindPromocion.GetAll().Any(x => x.Id == idPromocion);
    }
    private bool ExisteDepositoQueUsaPromocion(List<Deposito> listaDepositos, int idPromocion) {
        return listaDepositos.Any(unDeposito => unDeposito.ListaPromociones.Any(promocion => promocion.Id == idPromocion));
    }
}