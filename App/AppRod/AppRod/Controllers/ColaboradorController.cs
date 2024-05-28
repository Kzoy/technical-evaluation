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
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColaboradorController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet("teste")]
        public string TesteComunicacao()
        {
            return "Teste de comunicacao";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colaborador>>> PegarColaboradores()
        {
            return await _context.Colaboradores.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> PegarColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return colaborador;
        }

        [HttpPost]
        public async Task<ActionResult<Colaborador>> InserirColaborador(Colaborador colaborador)
        {
            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PegarColaborador), new { id = colaborador.pk_Id_Colaborador }, colaborador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarColaborador(int id, Colaborador altColaborador)
        {
            var oldColaborador = await _context.Colaboradores.FindAsync(id);

            if (oldColaborador == null)
            {
                return NotFound();
            }

            oldColaborador.str_Nome = altColaborador.str_Nome;
            oldColaborador.fk_Unidade = altColaborador.fk_Unidade;

            _context.Entry(oldColaborador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificarColaborador(id))
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
        public async Task<IActionResult> DeletarColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VerificarColaborador(int id)
        {
            return _context.Colaboradores.Any(e => e.pk_Id_Colaborador == id);
        }

    }
}
