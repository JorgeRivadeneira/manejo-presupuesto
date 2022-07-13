﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task<int> Contar(int usuarioId);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperaciones tipoOperacionId);
        Task<Categoria> ObtenerPodId(int id, int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                Values(@Nombre, @TipoOperacionId, @UsuarioId);
                SELECT SCOPE_IDENTITY();
            ", categoria);
            //categoria.Id = id; //solo para hacer la prueba
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(@$"
                SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId
                ORDER BY Nombre
                OFFSET {paginacion.RecordsSaltar} ROWS FETCH NEXT {paginacion.RecordsPorPagina}
                ROWS ONLY", new {usuarioId });
        }

        public async Task<int> Contar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                @"SELECT count(*) FROM CATEGORIAS WHERE UsuarioId = @usuarioId", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId,
            TipoOperaciones tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(@"
                SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId
                AND TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });
        }

        public async Task<Categoria> ObtenerPodId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"
                SELECT * FROM Categorias WHERE Id = @Id and UsuarioId = @UsuarioId", 
                new {id, usuarioId});
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                DELETE Categorias WHERE Id = @Id", new {id});
        }
    }
}
