using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using MinhaApiCompleta.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Services
{
    public class FornecedoresService : BaseService, IFornecedoresService
    {
        private readonly IFornecedoresRepository _fornecedoresRepository;
        private readonly IEnderecosRepository _enderecosRepository;

        public FornecedoresService(IFornecedoresRepository fornecedoresRepository, IEnderecosRepository enderecosRepository, INotificador notificador)
            :base(notificador)
        {
            _fornecedoresRepository = fornecedoresRepository;
            _enderecosRepository = enderecosRepository;
        }

        public async Task<bool> Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return false;

            if(_fornecedoresRepository.Get(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado");
                return false;
            }

            await _fornecedoresRepository.Include(fornecedor);
            return true;
        }

        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;

            if (_fornecedoresRepository.Get(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado");
                return false;
            }

            await _fornecedoresRepository.Update(fornecedor);
            return true;
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecosRepository.Update(endereco);
        }

        public async Task<bool> Remover(Guid id)
        {
            if (_fornecedoresRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return false;
            }

            var endereco = await _enderecosRepository.ObterEnderecoPorFornecedor(id);

            if (endereco != null)
            {
                await _enderecosRepository.Delete(endereco.Id);
            }

            await _fornecedoresRepository.Delete(id);
            return true;
        }

        public void Dispose()
        {
            _fornecedoresRepository?.Dispose();
            _enderecosRepository?.Dispose();
        }
    }
}