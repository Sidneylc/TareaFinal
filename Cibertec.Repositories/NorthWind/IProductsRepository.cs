using System.Collections.Generic;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface IProductsRepository : IRepository<Products>
    {
        Products GetById(string id);
        new bool Update(Products product);
        bool Delete(string id);
        IEnumerable<Products> PagedList(int startRow, int endRow);
        int Count();
    }
}