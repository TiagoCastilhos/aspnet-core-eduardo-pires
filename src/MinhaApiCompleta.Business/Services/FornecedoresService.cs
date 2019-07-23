using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using MinhaApiCompleta.Business.Models.Validations;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Services
{
    public class FornecedoresService : BaseService, IFornecedoresService
    {
        private readonly IFornecedoresRepository _fornecedoresRepository;
        private readonly IEnderecosRespository _enderecosRespository;

        public FornecedoresService(IFornecedoresRepository fornecedoresRepository, IEnderecosRespository enderecosRespository, INotificador notificador)
            :base(notificador)
        {
            _fornecedoresRepository = fornecedoresRepository;
            _enderecosRespository = enderecosRespository;
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

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if (_fornecedoresRepository.Get(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado");
                return;
            }

            await _fornecedoresRepository.Update(fornecedor);
        }        
    }
}