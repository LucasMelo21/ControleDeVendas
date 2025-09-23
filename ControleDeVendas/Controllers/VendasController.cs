using ControleDeVendas.Data;
using ControleDeVendas.Models;
using ControleDeVendas.Models.ViewModels;
using ControleDeVendas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ControleDeVendas.Controllers
{
    public class VendasController : Controller
    {
        private readonly VendaService _vendaService;
        private readonly VendedorService _vendedorService;
        private readonly ProdutoService _produtoService;

        public VendasController(VendaService vendaService, VendedorService vendedorService, ProdutoService produtoService)
        {
            _vendaService = vendaService;
            _produtoService = produtoService;
            _vendedorService = vendedorService;
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
            .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome }), Itens = (await _produtoService.ListAtivosAsync())
            .Select(p => new ItemProdutoVM{ ProdutoId = p.Id, Nome = p.Nome, PrecoAtual = p.Valor, Selecionado = false, Quantidade = 0})
            .ToList()
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendaFormViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                vm.Vendedores = (await _vendedorService.ListAsync())
                    .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome, Selected = (v.Id == vm.VendedorId) });
                if (vm.Itens == null || vm.Itens.Count == 0)
                {
                    vm.Itens = (await _produtoService.ListAtivosAsync())
                        .Select(p => new ItemProdutoVM { ProdutoId = p.Id, Nome = p.Nome, PrecoAtual = p.Valor })
                        .ToList();
                }
                return View(vm);
            }

            var itensSelecionados = vm.Itens?.Where(i => i.Selecionado && i.Quantidade > 0).ToList() ?? new();
            if (itensSelecionados.Count == 0)
            {
                ModelState.AddModelError("", "Selecione ao menos um produto com quantidade.");
                vm.Vendedores = (await _vendedorService.ListAsync())
                    .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome, Selected = (v.Id == vm.VendedorId) });
                return View(vm);
            }

            if (!await _vendedorService.ExistsAsync(vm.VendedorId))
            {
                ModelState.AddModelError(nameof(vm.VendedorId), "Vendedor inválido.");
                vm.Vendedores = (await _vendedorService.ListAsync())
                    .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Nome, Selected = (v.Id == vm.VendedorId) });
                return View(vm);
            }

            var idsProdutos = itensSelecionados.Select(i => i.ProdutoId).ToList();
            var produtos = await _produtoService.GetByIdsAsync(idsProdutos);

            var venda = new Venda
            {
                VendedorId = vm.VendedorId,
                Data = vm.Data,
                VendasProdutos = new List<VendaProduto>()
            };

            foreach (var item in itensSelecionados)
            {
                var produto = produtos.First(p => p.Id == item.ProdutoId);
                venda.VendasProdutos.Add(new VendaProduto
                {
                    ProdutoId = produto.Id,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = produto.Valor
                });
            }

            await _vendaService.InsertAsync(venda);

            TempData["Success"] = "Venda registrada com sucesso!";
            return RedirectToAction(nameof(Details), new { id = venda.Id });
        }
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var venda = await _vendaService.FindByIdAsync(id, ct);
            if (id == null)  return NotFound();
            var vendedores = await _vendedorService.ListAsync(ct);
            var produtos = await _produtoService.ListAtivosAsync(ct);

            var itensExistentes = venda.VendasProdutos.ToDictionary(x => x.ProdutoId, x => x);

            var vm = new VendaFormViewModel { Id = venda.Id, VendedorId = venda.VendedorId, Data = venda.Data, Vendedores = new SelectList(vendedores, "Id", "Nome", venda.VendedorId),
            Itens = produtos.Select(p => {
                var existe = itensExistentes.TryGetValue(p.Id, out var vp);
                return new ItemProdutoVM
                {
                    ProdutoId = p.Id,
                    Nome = p.Nome,
                    PrecoAtual = p.Valor,
                    Selecionado = existe && vp!.Quantidade > 0,
                    Quantidade = existe ? vp!.Quantidade : 0
                };
            }).ToList()
            };
            return View(vm);
        }
        public async Task<IActionResult> Details(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            // carrega venda com itens e produtos (FindByIdAsync já faz Include/ThenInclude)
            var venda = await _vendaService.FindByIdAsync(id.Value, ct);
            if (venda == null) return NotFound();

            return View(venda);
        }
        public async Task<IActionResult> Delete(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var venda = await _vendaService.FindByIdAsync(id.Value, ct);
            if (venda == null) return NotFound();

            return View(venda);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            try
            {
                await _vendaService.RemoveAsync(id);
                TempData["Success"] = "Venda removida com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Não foi possível remover a venda: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
