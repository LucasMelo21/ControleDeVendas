using ControleDeVendas.Data;
using ControleDeVendas.Models;
using ControleDeVendas.Services.Exceptions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ControleDeVendas.Services
{
    public class VendaService
    {
        private readonly ControleDeVendasContext _context;

        public VendaService(ControleDeVendasContext context)
        {
            _context = context;
        }
        public async Task<List<Venda>> FindAllAsync()
        {
            return await _context.Venda
                .Include(v => v.VendasProdutos)
                .ThenInclude(vp => vp.Produto)
                .ToListAsync();
        }
        public async Task InsertAsync(Venda obj)
        {
            try
            {
                Console.WriteLine($"Salvando Venda: Vendedor: {obj.Vendedor.Nome} Produtos: ");
                foreach (var produtos in obj.VendasProdutos)
                {
                    Console.WriteLine(produtos.Produto.Nome);
                }
                _context.Add(obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar: {ex.Message} - {ex.InnerException?.Message}");
                throw;
            }
        }
        public async Task<Venda> FindByIdAsync(int id)
        {
            var venda = await _context.Venda.Include(v => v.VendasProdutos).ThenInclude(vp => vp.Produto).FirstOrDefaultAsync(v => v.Id == id);
            if (venda == null)
            {
                Console.WriteLine($"null reference");
            }
            return venda;
        }
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Venda.FindAsync(id);
                if (obj != null)
                {
                    _context.Venda.Remove(obj);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task UpdateAsync(Venda obj)
        {
            if (!await _context.Venda.AnyAsync(v => v.Id == obj.Id)) 
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
