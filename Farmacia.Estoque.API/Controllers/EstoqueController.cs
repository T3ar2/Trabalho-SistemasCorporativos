
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

        // POST: api/estoque/entrada
        [HttpPost("entrada")]
        public async Task<IActionResult> EntradaDeEstoque([FromBody] LoteEstoque lote)
        {
            _context.Lotes.Add(lote);
            await _context.SaveChangesAsync();
            return Ok(lote);
        }

        [HttpPut("abater")]
        public async Task<IActionResult> AbaterEstoque(Guid produtoId, string numeroLote, int quantidadeVendida)
        {
            var lote = await _context.Lotes
                .FirstOrDefaultAsync(l => l.ProdutoId == produtoId && l.NumeroLote == numeroLote);

            if (lote == null)
            {
                return NotFound(new { mensagem = "Lote não encontrado no estoque." });
            }

            if (lote.QuantidadeDisponivel < quantidadeVendida)
            {
                return BadRequest(new { mensagem = "Quantidade insuficiente em estoque." });
            }

            lote.QuantidadeDisponivel -= quantidadeVendida;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Estoque abatido com sucesso!", estoqueAtual = lote.QuantidadeDisponivel });
        }
    }
}