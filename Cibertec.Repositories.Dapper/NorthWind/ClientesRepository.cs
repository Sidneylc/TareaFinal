﻿using Dapper;
using Dapper.Contrib.Extensions;
using Cibertec.Models;
using Cibertec.Repositories.NorthWind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    public class ClientesRepository : Repository<Cliente>, IClientesRepository
    {
        public ClientesRepository(string connectionString) : base(connectionString)
        {

        }

        public new Cliente GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Cliente>().Where(cliente => cliente.Codigo.Equals(id)).First();
            }
        }

        public new int Insert(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("insert Cliente (Nombre, Apellido, Dni, Telefono, Correo) " +
                                                "values (@nombre, @apellido, @dni, @telefono, @correo)",
                                            new
                                            {
                                                nombre = cliente.Nombre,
                                                apellido = cliente.Apellido,
                                                dni = cliente.Dni,
                                                telefono = cliente.Telefono,
                                                correo = cliente.Correo
                                            });
                return result;
            }
        }

        public new bool Update(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("update cliente " +
                        "set Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono = @telefono, Correo = @correo" +
                                            "where Codigo = @codigo",
                                            new
                                            {
                                                nombre = cliente.Nombre,
                                                apellido = cliente.Apellido,
                                                dni = cliente.Dni,
                                                telefono = cliente.Telefono,
                                                correo = cliente.Correo,
                                                codigo = cliente.Codigo
                                            });
                return Convert.ToBoolean(result);
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from cliente " +
                    "where Codigo = @myid ", new { myid = id });
                return Convert.ToBoolean(result);
                //if (result==1) return true; else return false;   
            }
        }

        public IEnumerable<Cliente> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Cliente>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Cliente>("dbo.uspClientePagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(Codigo) from dbo.Cliente");
            }
        }
    }
}