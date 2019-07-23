using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhaApiCompleta.Api.ViewModels;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Api.Controllers
{
    [Route("api/Fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedoresRepository _fornecedoresRepository;
        private readonly IFornecedoresService _fornecedoresService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedoresRepository fornecedoresRepository, IFornecedoresService fornecedoresService, IMapper mapper)
        {
            _fornecedoresRepository = fornecedoresRepository;
            _fornecedoresService = fornecedoresService;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> InsertFornecedor(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedoresRepository.Include(fornecedor);

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateFornecedor([FromRoute]Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedoresRepository.Include(fornecedor);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFornecedor([FromRoute]Guid id)
        {
            var fornecedor = await GetFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();

            var result = await _fornecedoresService.Remover(id);

            return Ok();
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