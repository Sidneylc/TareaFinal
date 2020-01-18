using System.Collections.Generic;
using Cibertec.Models;

namespace Cibertec.Repositories.NorthWind
{
    public interface IProductsRepository : IRepository<Products>
    {
        new Products GetById(int id);
        new bool Update(Products product);
        bool Delete(int id);
        IEnumerable<Products> PagedList(int startRow, int endRow);
        int Count();
    }
}