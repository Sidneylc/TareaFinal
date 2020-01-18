using System.Collections.Generic;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface ICustomersRepository: IRepository<Customers>
    {
        Customers GetById(string id);
        new bool Update(Customers customer);
        bool Delete(string id);
        IEnumerable<Customers> PagedList(int startRow, int endRow);
        int Count();
    }
}