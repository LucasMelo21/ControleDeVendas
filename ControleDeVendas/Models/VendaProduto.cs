namespace ControleDeVendas.Models
{
    public class VendaProduto
    {
        public int VendaId { get; set; }
        public Venda Venda { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public double Comissao { get; set; }

        public VendaProduto() { }
        public VendaProduto(int vendaId, Venda venda, int produtoId, Produto produto, int quantidade, decimal precoUnitario, double comissao)
        {
            VendaId = vendaId;
            Venda = venda;
            ProdutoId = produtoId;
            Produto = produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Comissao = comissao;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
