using Microsoft.EntityFrameworkCore;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using MinhaApiCompleta.Data.Contexts;
using System;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Data.Repositories
{
    public class EnderecosRepository : Repository<Endereco>, IEnderecosRepository
    {
        public EnderecosRepository(MeuDbContext context) 
            :base(context)
        {
        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        }
    }
}