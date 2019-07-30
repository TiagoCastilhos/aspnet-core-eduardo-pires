using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApiCompleta.Api.ViewModels;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Api.Controllers
{
    [Authorize]
    [Route("api/Fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedoresRepository _fornecedoresRepository;
        private readonly IEnderecosRepository _enderecosRepository;
        private readonly IFornecedoresService _fornecedoresService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedoresRepository fornecedoresRepository,
                                      IEnderecosRepository enderecosRepository,
                                      IFornecedoresService fornecedoresService,
                                      IMapper mapper,
                                      INotificador notificador)
            : base(notificador)
        {
            _fornecedoresRepository = fornecedoresRepository;
            _enderecosRepository = enderecosRepository;
            _fornecedoresService = fornecedoresService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FornecedorViewModel[]), 200)]
        public async Task<IActionResult> GetAllFornecedores()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedoresRepository.GetAll());

            return Ok(fornecedores);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FornecedorViewModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFornecedor(Guid id)
        {
            var fornecedores = await GetFornecedorProdutosEndereco(id);

            if (fornecedores == null)
                return NotFound();

            return Ok(fornecedores);
        }

        [HttpGet("{id:guid}/Endereco")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        {
            var enderecoViewModel = _mapper.Map<EnderecoViewModel>(await _enderecosRepository.GetById(id));

            return enderecoViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> InsertFornecedor(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _fornecedoresRepository.Include(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateFornecedor([FromRoute]Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotificarErro("O id informado nao e o mesmo que foi passado na query");
                return CustomResponse(fornecedorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(fornecedorViewModel);

            await _fornecedoresService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [HttpPut("{id:guid}/Endereco")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado nao e o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedoresService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFornecedor([FromRoute]Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedoresService.Remover(id);

            return CustomResponse(fornecedorViewModel);
        }

        public async Task<FornecedorViewModel> GetFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedoresRepository.ObterFornecedorEndereco(id));
        }

        public async Task<FornecedorViewModel> GetFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedoresRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}