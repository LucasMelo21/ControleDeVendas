using System.ComponentModel.DataAnnotations;

namespace ControleDeVendas.Models
{
    public class Venda
    {
        [Required(ErrorMessage = "{0} necessário")]
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime Data { get; set; }
        public int VendedorId { get; set; }
        public Vendedor Vendedor { get; set; }
        public ICollection<VendaProduto> VendasProdutos { get; set; } = new List<VendaProduto>();

        public Venda() { }
        public Venda(int id, DateTime date)
        {
            Id = id;
            Data = date;
        }
        public void AdicionarProduto(Produto produto, int quantidade, double comissao)
        {
            var vendaProduto = new VendaProduto
            {
                Produto = produto,
                ProdutoId = produto.Id,
                Venda = this,
                VendaId = this.Id,
                Quantidade = quantidade,
                PrecoUnitario = produto.Valor,
                Comissao = comissao
            };

            VendasProdutos.Add(vendaProduto);

            produto.QuantidadeEstoque -= quantidade;
        }
        public decimal CalcularTotal()
        {
            return VendasProdutos.Sum(vp => vp.Quantidade * vp.PrecoUnitario);
        }
        public double CalcularComissaoTotal()
        {
            return VendasProdutos.Sum(v => v.Comissao);
        }
        public override string ToString()
        {
            return Data.ToString("dd/MM/yyyy")
                + ", "
                + Vendedor.Nome
                + ", Total de Produtos:"
                + VendasProdutos.Count;
        }
    }
}
