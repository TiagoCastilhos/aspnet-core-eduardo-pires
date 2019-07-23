using MinhaApiCompleta.Business.Models;
using System;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Interfaces
{
    public interface IFornecedoresService : IDisposable
    {
        Task<bool> Adicionar(Fornecedor fornecedor);

        Task Atualizar(Fornecedor fornecedor);
    }
}