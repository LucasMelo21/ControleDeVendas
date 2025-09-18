using ControleDeVendas.Data;
using ControleDeVendas.Models;
using ControleDeVendas.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ControleDeVendas.Services
{
    public class VendedorService
    {
        private readonly ControleDeVendasContext _context;

        public VendedorService(ControleDeVendasContext context)
        {
            _context = context;
        }
        public async Task<List<Vendedor>> FindAllAsync()
        {
            return await _context.Vendedor
                .Include(v => v.Vendas)
                .ThenInclude(vp => vp.VendasProdutos)
                .ToListAsync();
        }
        public async Task InsertAsync(Vendedor obj)
        {
            try
            {
                Console.WriteLine($"Salvando Vendedor: {obj.Nome}");
                foreach (var vendas in obj.Vendas)
                {
                    Console.WriteLine(vendas.Id);
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
        public async Task<Vendedor> FindByIdAsync(int id)
        {
            var venda = await _context.Vendedor.Include(v => v.Vendas).ThenInclude(vp => vp.VendasProdutos).FirstOrDefaultAsync(v => v.Id == id);
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
                var obj = await _context.Vendedor.FindAsync(id);
                if (obj != null)
                {
                    _context.Vendedor.Remove(obj);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task UpdateAsync(Vendedor obj)
        {
            if (!await _context.Vendedor.AnyAsync(v => v.Id == obj.Id))
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
