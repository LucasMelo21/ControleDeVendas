using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleDeVendas.Data;
using ControleDeVendas.Models;
using ControleDeVendas.Services;
using ControleDeVendas.Services.Exceptions;

namespace ControleDeVendas.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _produtoService.ListAsync(ct));
        }

        public async Task<IActionResult> Details(int? id, CancellationToken ct)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoService
                .FindByIdAsync(id.Value, ct);
            if (produto == null)return NotFound();
            
            return View(produto);
        }

        public IActionResult Create()
        {
            return View(new Produto { Ativo = true});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Where(x => x.Value?.Errors.Count > 0))
                    Console.WriteLine($"{e.Key}: {string.Join(", ", e.Value!.Errors.Select(er => er.ErrorMessage))}");
                return View(produto);
            }
            try
            {
                await _produtoService.InsertAsync(produto);
                TempData["Success"] = "Produto criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar produto: {ex.Message}");
                return View(produto);
            }
        }

        public async Task<IActionResult> Edit(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var produto = await _produtoService.FindByIdAsync(id.Value, ct);
            if (produto == null) return NotFound();

            return View(produto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto, CancellationToken ct)
        {
            if (id != produto.Id) return NotFound();
            if (!ModelState.IsValid) return View(produto);

            try
            {
                await _produtoService.UpdateAsync(produto);
                TempData["Success"] = "Produto atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar produto: {ex.Message}");
                return View(produto);
            }
        }

        public async Task<IActionResult> Delete(int? id, CancellationToken ct)
        {
            if (id == null) return NotFound();

            var produto = await _produtoService.FindByIdAsync(id.Value, ct);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            try
            {
                await _produtoService.RemoveAsync(id);
                TempData["Success"] = "Produto excluído com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Não foi possível excluir o produto: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

