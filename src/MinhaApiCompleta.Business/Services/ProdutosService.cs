using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using MinhaApiCompleta.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Services
{
    public class ProdutosService : BaseService, IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;

        public ProdutosService(IProdutosRepository produtosRepository, INotificador notificador)
            :base(notificador)
        {
            _produtosRepository = produtosRepository;
        }

        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtosRepository.Include(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtosRepository.Update(produto);
        }

        public async Task Remover(Guid id)
        {
            await _produtosRepository.Delete(id);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}