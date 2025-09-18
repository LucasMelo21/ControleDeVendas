using System.ComponentModel.DataAnnotations;

namespace ControleDeVendas.Models
{
    public class Produto
    {
        [Required(ErrorMessage ="{0} necessário")]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} necessário")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "{0} necessário")]
        [DisplayFormat(DataFormatString = "${0:F2}")]
        public decimal Valor { get; set; }
        public int QuantidadeEstoque { get; set; }
        public ICollection<VendaProduto> VendasProdutos { get; set; } 
        public Produto() { }

        public Produto(int id, string name, decimal valor, int quantidadeEstoque)
        {
            Id = id;
            Nome = name;
            Valor = valor;
            QuantidadeEstoque = quantidadeEstoque;
        }
        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }
        public bool TemEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }
    }
}
