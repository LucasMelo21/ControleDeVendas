using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ControleDeVendas.Models;

namespace ControleDeVendas.Data
{
    public class ControleDeVendasContext : DbContext
    {
        public ControleDeVendasContext (DbContextOptions<ControleDeVendasContext> options)
            : base(options)
        {
        }

        public DbSet<ControleDeVendas.Models.Vendedor> Vendedor { get; set; } = default!;
        public DbSet<ControleDeVendas.Models.Produto> Produto { get; set; } = default!;
        public DbSet<ControleDeVendas.Models.VendaProduto> VendaProduto { get; set; } = default!;
        public DbSet<ControleDeVendas.Models.Venda> Venda {  get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VendaProduto>()
                .HasKey(vp => new { vp.VendaId, vp.ProdutoId });

            modelBuilder.Entity<VendaProduto>()
                .HasOne(vp => vp.Venda)
                .WithMany(v => v.VendasProdutos)
                .HasForeignKey(vp => vp.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VendaProduto>()
                .HasOne(vp => vp.Produto)
                .WithMany(p => p.VendasProdutos)
                .HasForeignKey(vp => vp.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Produto>().Property(p => p.Valor).HasPrecision(10,2);
            modelBuilder.Entity<VendaProduto>().Property(vp => vp.PrecoUnitario).HasPrecision(10,2);
        }

    }
}
