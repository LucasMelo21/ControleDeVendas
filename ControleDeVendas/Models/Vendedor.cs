using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataContratacao { get; set; }
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
        public Vendedor() 
        {

        }
        public Vendedor(int id, string nome, double comissao)
        {
            Id = id;
            Nome = nome;
            Comissao = comissao;
            DataContratacao = DateTime.Today;
        }
        public double CalcularTotalVendas()
        {
            return Vendas.Sum(v => v.CalcularComissaoTotal());
        }
    }
}
