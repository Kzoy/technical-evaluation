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
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet("teste")]
        public string TesteComunicacao()
        {
            return "Teste de comunicacao";
        }

        // Método para obter todos os usuários
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> PegarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // Novo método para obter usuários por status
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> PegarUsuariosPorStatus(bool status)
        {
            var usuarios = await _context.Usuarios.Where(u => u.bln_status == status).ToListAsync();

            if (usuarios == null || usuarios.Count() == 0)
            {
                return NotFound();
            }

            return usuarios;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> PegarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> InserirUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PegarUsuario), new { id = usuario.pk_id_usuario }, usuario);
        }

        //Atualização usuario (Senha, Status)
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, Usuario altUsuario)
        {
            var oldUsuario = await _context.Usuarios.FindAsync(id);

            if (oldUsuario == null)
            {
                return NotFound();
            }

            oldUsuario.str_senha = altUsuario.str_senha;
            oldUsuario.bln_status = altUsuario.bln_status;  

            _context.Entry(oldUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificarUsuario(id))
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

        private bool VerificarUsuario(int id)
        {
            return _context.Usuarios.Any(e => e.pk_id_usuario == id);
        }

    }
}
