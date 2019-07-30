using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinhaApiCompleta.Api.ViewModels;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Api.Controllers
{
    [Route("api/Produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutosRepository _produtosRepository;
        private readonly IProdutosService _produtosService;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutosRepository produtosRepository,
                                  IProdutosService produtosService,
                                  IMapper mapper,
                                  INotificador notificador) 
            :base(notificador)
        {
            _produtosService = produtosService;
            _produtosRepository = produtosRepository;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ProdutoViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtosRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return Ok(produtoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(produtoViewModel);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                return CustomResponse();

            produtoViewModel.Imagem = imagemNome;

            await _produtosService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> AlternateInsert(ProdutoImagemViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";

            if (! await UploadArquivoAlternativo(produtoViewModel.ImagemUpload))
                return CustomResponse();

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtosService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            await _produtosService.Remover(id);

            return CustomResponse(produtoViewModel);
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtosRepository.ObterProdutoFornecedor(id));
        }

        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length <= 0)
            {
                NotificarErro("Forneca uma imagem para este produto");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", arquivo.FileName);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Ja existe um arquivo com esse nome");
                return false;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
                await arquivo.CopyToAsync(stream);

            return true;
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {        
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneca uma imagem para este produto");
                return false;
            }

            var data = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Ja existe um arquivo com esse nome");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, data);

            return true;
        }
    }
}