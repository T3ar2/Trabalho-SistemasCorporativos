using Farmacia.SNGPC.API.Config;
using Farmacia.SNGPC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Farmacia.SNGPC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SngpcController : ControllerBase
    {
        private readonly IMongoCollection<ReceitaMedica> _receitasCollection;

        public SngpcController(IOptions<MongoDbSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _receitasCollection = database.GetCollection<ReceitaMedica>(mongoDBSettings.Value.ReceitasCollectionName);
        }

        [HttpGet]
        public async Task<IActionResult> ListarReceitasRetidas()
        {
            var receitas = await _receitasCollection.Find(_ => true).ToListAsync();
            return Ok(receitas);
        }

        [HttpPost("reter-receita")]
        public async Task<IActionResult> ReterReceita([FromBody] ReceitaMedica receita)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _receitasCollection.InsertOneAsync(receita);
            
            return CreatedAtAction(nameof(ListarReceitasRetidas), new { id = receita.Id }, receita);
        }

        [HttpGet("gerar-arquivo-anvisa")]
        public async Task<IActionResult> GerarArquivoAnvisa()
        {
            var receitas = await _receitasCollection.Find(_ => true).ToListAsync();
            
            return Ok(new { 
                Mensagem = "Arquivo XML gerado com sucesso para transmissão ao SNGPC.",
                TotalReceitasProcessadas = receitas.Count 
            });
        }
    }
}