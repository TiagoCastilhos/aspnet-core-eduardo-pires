using Microsoft.EntityFrameworkCore;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using MinhaApiCompleta.Data.Contexts;
using System;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Data.Repositories
{
    public class FornecedoresRepository : Repository<Fornecedor>, IFornecedoresRepository
    {
        public FornecedoresRepository(MeuDbContext dbContext)
            :base(dbContext)
        {
        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Produtos)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}