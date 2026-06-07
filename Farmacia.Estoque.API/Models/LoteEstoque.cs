using System.ComponentModel.DataAnnotations;

namespace Farmacia.Estoque.API.Models
{
    public class LoteEstoque
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProdutoId { get; set; } 

        [Required(ErrorMessage = "O número do lote é obrigatório.")]
        [MaxLength(50)]
        public string NumeroLote { get; set; } = string.Empty;

        [Required]
        public DateTime DataValidade { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "A quantidade não pode ser negativa.")]
        public int QuantidadeDisponivel { get; set; }
    }
}