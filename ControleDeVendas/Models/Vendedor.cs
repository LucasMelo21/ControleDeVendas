using System.ComponentModel.DataAnnotations;

namespace ControleDeVendas.Models
{
    public class Vendedor
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nome { get; set; }
        [Range(0, 999999.99)]
        [DisplayFormat(DataFormatString = "${0:F2}")]
        public double Comissao { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataContratacao { get; set; }
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();

        public Vendedor() { }
        public Vendedor(int id, string name, double comissao)
        {
            Id = id;
            Nome = name;
            Comissao = comissao;
            DataContratacao = DateTime.Now;
        }
        public double CalcularTotalVendas()
        {
            return Vendas.Sum(v => v.CalcularComissaoTotal());
        }
    }
}
