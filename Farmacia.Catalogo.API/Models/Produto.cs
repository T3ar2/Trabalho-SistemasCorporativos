
using System.ComponentModel.DataAnnotations;

namespace Farmacia.Catalogo.API.Models
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PrincipioAtivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do produto deve ser informada.")]
        public CategoriaProduto Categoria { get; set; }

        [Required]
        public bool ExigeReceita { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal PrecoBase { get; set; }
    }
}