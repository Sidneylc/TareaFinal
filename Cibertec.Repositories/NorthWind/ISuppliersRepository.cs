using System.Collections.Generic;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface ISuppliersRepository : IRepository<Suppliers>
    {
        new Suppliers GetById(int id);
        new bool Update(Suppliers supplier);
        bool Delete(int id);
        IEnumerable<Suppliers> PagedList(int startRow, int endRow);
        int Count();
    }
}