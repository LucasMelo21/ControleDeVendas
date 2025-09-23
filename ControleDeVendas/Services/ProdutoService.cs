using ControleDeVendas.Data;
using ControleDeVendas.Models;
using ControleDeVendas.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ControleDeVendas.Services
{
    public class ProdutoService
    {
        private readonly ControleDeVendasContext _context;

        public ProdutoService(ControleDeVendasContext context) 
        { 
            _context = context; 
        }
        public async Task<List<Produto>> ListAsync(CancellationToken ct = default)
        {
            return await _context.Produto
                .AsNoTracking()
                .OrderBy(p => p.Nome)
                .ToListAsync(ct);
        }
        public async Task<List<Produto>> ListAtivosAsync(CancellationToken ct = default)
        {
            return await _context.Produto
                .AsNoTracking()
                .Where(p => p.Ativo)     
                .OrderBy(p => p.Nome)
                .ToListAsync(ct);
        }

        public async Task<List<Produto>> FindAllAsync()
        {
            return await _context.Produto
                .Include(v => v.VendasProdutos)
                .ThenInclude(vp => vp.Produto)
                .ToListAsync();
        }
        public async Task InsertAsync(Produto obj)
        {
            try
            {
                Console.WriteLine($"Salvando Produto: {obj.Nome}");
                _context.Add(obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar: {ex.Message} - {ex.InnerException?.Message}");
                throw;
            }
        }
        public async Task<List<Produto>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            var idList = ids?.Distinct().ToList() ?? new List<int>();
            if (idList.Count == 0) return new List<Produto>();

            return await _context.Produto
                .AsNoTracking()
                .Where(p => idList.Contains(p.Id))
                .ToListAsync(ct);
        }

        public async Task<Produto> FindByIdAsync(int id, CancellationToken ct = default)
        {
            var produto = await _context.Produto.Include(v => v.VendasProdutos).ThenInclude(vp => vp.Produto).FirstOrDefaultAsync(v => v.Id == id);
            if (produto == null)
            {
                Console.WriteLine($"null reference");
            }
            return produto;
        }
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Produto.FindAsync(id);
                if (obj != null)
                {
                    _context.Produto.Remove(obj);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task UpdateAsync(Produto obj)
        {
            if (!await _context.Produto.AnyAsync(v => v.Id == obj.Id))
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
