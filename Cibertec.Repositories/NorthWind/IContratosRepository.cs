using Cibertec.Models;
using System.Collections.Generic;

namespace Cibertec.Repositories.NorthWind
{
    public interface IContratosRepository : IRepository<Contrato>
    {
        new Contrato GetById(int id);
        new int Insert(Contrato contrato);
        new bool Update(Contrato contrato);
        bool Delete(int id);
        IEnumerable<Contrato> PagedList(int startRow, int endRow);
        int Count();
    }
}