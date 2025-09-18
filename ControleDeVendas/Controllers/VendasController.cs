using ControleDeVendas.Data;
using ControleDeVendas.Models.ViewModels;
using ControleDeVendas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleDeVendas.Controllers
{
    public class VendasController : Controller
    {
        private readonly VendaService _vendaService;

        public VendasController(VendaService vendaService)
        {
            _vendaService = vendaService;
        }
        public async Task<ActionResult> Index()
        {
            var list = await _vendaService.FindAllAsync();
            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            var vm = new VendaFormViewModel
            {
                Vendedores = (await _vendedorService.ListAsync())
            .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome }),
                Itens = (await _produtoService.ListAtivosAsync())
            .Select(p => new ItemProdutoVM
            {
                ProdutoId = p.Id,
                Nome = p.Nome,
                PrecoAtual = p.Valor,
                Selecionado = false,
                Quantidade = 0
            })
            .ToList()
            };

            return View(vm);
        }
    }
}
