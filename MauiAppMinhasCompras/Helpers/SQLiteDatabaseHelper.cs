using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        
        public async Task<List<Produto>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            return await _conn.Table<Produto>()
                .Where(p => p.DataCadastro >= startDate.Date &&
                           p.DataCadastro <= endDate)
                .OrderByDescending(p => p.DataCadastro)
                .ToListAsync();
        }

        public async Task<double> GetTotalSpentByDateRange(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            var produtos = await _conn.Table<Produto>()
                                   .Where(p => p.DataCadastro >= startDate.Date &&
                                              p.DataCadastro <= endDate)
                                   .ToListAsync();

            return produtos.Sum(p => p.Total);
        }
    }
}