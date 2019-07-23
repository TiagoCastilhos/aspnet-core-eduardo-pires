using MinhaApiCompleta.Business.Models;
using System;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Interfaces
{
    public interface IFornecedoresRepository : IRepository<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);

        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
    }
}