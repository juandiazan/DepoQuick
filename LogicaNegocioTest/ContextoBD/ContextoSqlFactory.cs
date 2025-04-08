using Microsoft.EntityFrameworkCore;
using Repositorio;

namespace LogicaNegocioTest.ContextoBD;

public static class ContextoSqlFactory
{
    public static ContextoSql CrearContextoEnMemoria() {
        var builderDeOpciones = new DbContextOptionsBuilder<ContextoSql>();
        builderDeOpciones.UseInMemoryDatabase("TestBD");

        return new ContextoSql(builderDeOpciones.Options);
    }
}