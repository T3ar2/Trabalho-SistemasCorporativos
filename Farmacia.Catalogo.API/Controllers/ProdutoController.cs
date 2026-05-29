using Farmacia.Catalogo.API.Data;
using Farmacia.Catalogo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Catalogo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly CatalogoDbContext ctx;

        public ProdutoController(CatalogoDbContext context)
        {
            ctx = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var produtos = await ctx.Produtos.ToListAsync();
            return Ok(produtos);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ctx.Produtos.Add(produto);
            await ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarTodos), new { id = produto.Id }, produto);
        }

        // GET: api/produto/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var produto = await ctx.Produtos.FindAsync(id);
            
            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto não encontrado." }); // Retorna 404
            }
            
            return Ok(produto);
        }

        // PUT: api/produto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] Produto produtoAtualizado)
        {
            if (id != produtoAtualizado.Id)
            {
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do produto." });
            }

            ctx.Entry(produtoAtualizado).State = EntityState.Modified;

            try
            {
                await ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ctx.Produtos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var produto = await ctx.Produtos.FindAsync(id);
            
            if (produto == null)
            {
                return NotFound();
            }

            ctx.Produtos.Remove(produto);
            await ctx.SaveChangesAsync();

            return NoContent(); // Retorna 204
        }
    }
}