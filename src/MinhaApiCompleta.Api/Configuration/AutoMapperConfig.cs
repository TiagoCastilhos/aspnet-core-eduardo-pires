using AutoMapper;
using MinhaApiCompleta.Api.ViewModels;
using MinhaApiCompleta.Business.Models;

namespace MinhaApiCompleta.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<ProdutoViewModel, Produto>();

            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, o => o.MapFrom(s => s.Fornecedor.Nome));
        }
    }
}