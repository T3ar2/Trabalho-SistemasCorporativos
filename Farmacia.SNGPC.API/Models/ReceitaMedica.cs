using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Farmacia.SNGPC.API.Models
{
    public class ReceitaMedica
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("produtoId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProdutoId { get; set; }

        [Required]
        [BsonElement("nomeMedico")]
        public string NomeMedico { get; set; } = string.Empty;

        [Required]
        [BsonElement("crm")]
        public string Crm { get; set; } = string.Empty;

        [Required]
        [BsonElement("tipoReceita")]
        public string TipoReceita { get; set; } = string.Empty;

        [BsonElement("dataRetencao")]
        public DateTime DataRetencao { get; set; } = DateTime.UtcNow;
    }
}
