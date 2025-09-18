using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControleDeVendas.Models.ViewModels
{
    public class VendaFormViewModel
    {
        [Required]
        public int VendedorId { get; set; }
        [Required]
        public DateTime Data { get; set; } = DateTime.Now;
        public IEnumerable<SelectListItem> Vendedores { get; set; } = Enumerable.Empty<SelectListItem>();
        public List<ItemProdutoVM> Itens { get; set; } = new();
    }
    public class ItemProdutoVM
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal PrecoAtual { get; set; }
        public bool Selecionado { get; set; }
        [Range(0, 9999999)]
        public int Quantidade { get; set; }
    }
}
