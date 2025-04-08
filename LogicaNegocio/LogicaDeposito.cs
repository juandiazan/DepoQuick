using Dominio;
using Repositorio;

namespace LogicaNegocio;

public class LogicaDeposito
{
    private IRepositorioFindDelete<Deposito> _repositorioDepositos;
    private LogicaSesion _logicaSesion;
    
    public LogicaDeposito(IRepositorioFindDelete<Deposito> repositorioDepositos, LogicaSesion logicaSesion) {
        _repositorioDepositos = repositorioDepositos;
        _logicaSesion = logicaSesion;
    }
    
    
    public Deposito Add(Deposito unDeposito) {
        if (!_logicaSesion.HayUnaSesionIniciada() || _logicaSesion.HayUnClienteEnSesion()) {
            throw new LogicaDepositoException("Solo un administrador puede ingresar depósitos al sistema");
        }
        _repositorioDepositos.Add(unDeposito);
        return unDeposito;
    }
    public List<Deposito> GetAll() {
        return _repositorioDepositos.GetAll();
    }
    public void Delete(List<Reserva> listaReservas, Deposito unDeposito) {
        if (!_logicaSesion.HayUnaSesionIniciada() || _logicaSesion.HayUnClienteEnSesion()) {
            throw new LogicaDepositoException("Solo un administrador puede eliminar un depósito");
        } else if (!ExisteDeposito(unDeposito)) {
            throw new LogicaDepositoException("El depósito que se quiere eliminar no existe");
        } else if (DepositoReservado(listaReservas, unDeposito.Id)) {
            throw new LogicaDepositoException("El depósito a eliminar está reservado o fue reservado previamente");
        }
        
        _repositorioDepositos.Delete(unDeposito);
    }
    public Deposito Find(Func<Deposito, bool> filtro) {
        Deposito depositoBuscado = _repositorioDepositos.Find(filtro);
        
        if (depositoBuscado is null) {
            throw new LogicaDepositoException("El depósito a buscar no existe");
        }
        
        return _repositorioDepositos.Find(filtro);
    }
    public List<Deposito> ObtenerDepositosEnRango(RangoDeFechas unaDisponibilidad) {
        List<Deposito> depositosDisponiblesEnRango = new List<Deposito>();
        foreach (Deposito deposito in GetAll()) {
            if (deposito.EstaDisponibleEnRango(unaDisponibilidad)) {
                depositosDisponiblesEnRango.Add(deposito);
            }   
        }
        return depositosDisponiblesEnRango;
    }
    
    
    private bool ExisteDeposito(Deposito unDeposito) {
        return _repositorioDepositos.GetAll().Any(x => x.Id == unDeposito.Id);
    }
    private bool DepositoReservado(List<Reserva> listaReservas, int unaId) {
        return listaReservas.Any(x => x.Deposito.Id == unaId);
    }
}