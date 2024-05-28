using AppRod.Database;
using AppRod.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppRod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnidadeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadeController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet("teste")]
        public string TesteComunicacao()
        {
            return "Teste de comunicacao";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidade>>> PegarUnidades()
        {
            return await _context.Unidades.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Unidade>> PegarUnidade(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);

            if (unidade == null)
            {
                return NotFound();
            }

            return unidade;
        }

        [HttpPost]
        public async Task<ActionResult<Unidade>> InserirUnidade(Unidade unidade)
        {
            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PegarUnidade), new { id = unidade.pk_Id_Unidade }, unidade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUnidade(int id, Unidade altUnidade)
        {
            var oldUnidade = await _context.Unidades.FindAsync(id);

            if (oldUnidade == null)
            {
                return NotFound();
            }

            oldUnidade.bln_Status = altUnidade.bln_Status;

            _context.Entry(oldUnidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificaUnidade(id))
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

        private bool VerificaUnidade(int id)
        {
            return _context.Unidades.Any(e => e.pk_Id_Unidade == id);
        }

    }
}
