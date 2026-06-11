
using Farmacia.Estoque.API.Data;
using Farmacia.Estoque.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly EstoqueDbContext _context;

        public EstoqueController(EstoqueDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarEstoque()
        {
            return Ok(await _context.Lotes.ToListAsync());
        }

        [HttpPost("entrada")]
        public async Task<IActionResult> EntradaDeEstoque([FromBody] LoteEstoque lote)
        {
            _context.Lotes.Add(lote);
            await _context.SaveChangesAsync();
            return Ok(lote);
        }

       [HttpPut("abater")]
        public async Task<IActionResult> AbaterEstoque([FromBody] AbaterEstoqueRequest request)
        {
            // Agora o .NET sabe que deve ler o JSON do Body!
            var lote = await _context.Lotes
                .FirstOrDefaultAsync(l => l.ProdutoId == request.ProdutoId && l.NumeroLote == request.NumeroLote);

            if (lote == null)
            {
                return NotFound(new { mensagem = "Lote não encontrado no estoque." });
            }

            if (lote.QuantidadeDisponivel < request.QuantidadeVendida)
            {
                return BadRequest(new { mensagem = "Quantidade insuficiente em estoque." });
            }

            lote.QuantidadeDisponivel -= request.QuantidadeVendida;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Estoque abatido com sucesso!", estoqueAtual = lote.QuantidadeDisponivel });
        }

    }

    public class AbaterEstoqueRequest
{
    public Guid ProdutoId { get; set; }
    public string NumeroLote { get; set; } = string.Empty;
    public int QuantidadeVendida { get; set; }
}
}