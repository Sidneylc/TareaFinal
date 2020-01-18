using System.Collections.Generic;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface ISuppliersRepository : IRepository<Suppliers>
    {
        Suppliers GetById(string id);
        new bool Update(Suppliers supplier);
        bool Delete(string id);
        IEnumerable<Suppliers> PagedList(int startRow, int endRow);
        int Count();
    }
}