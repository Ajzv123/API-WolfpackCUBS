using EncuestasAPI2.Models;
using EncuestasAPI2.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EncuestasAPI2.Controllers
{
    [ApiController]
    [Route("api/Usuarios")]
    public class UsuariosController : ControllerBase
    {   
        private readonly ILogger<UsuariosController> _logger;
        private readonly EncuestasAppContext _db;

        public UsuariosController(EncuestasAppContext db, ILogger<UsuariosController> logger)
        {            
            _logger = logger;
            _db = db;

        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            if (_db.Usuarios == null) {
                return NotFound();
            }
            // return await _db.Usuarios.ToListAsync();
            return await _db.Usuarios.Select(u => UsuarioDTO.UsuarioToDTO(u)).ToListAsync();

        }
        // GET: api/Usuarios2
        [HttpGet]
        [Route("~/api/Usuarios2")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios2()
        {
            if (_db.Usuarios == null)
            {
                return NotFound();
            }
            return await _db.Usuarios.ToListAsync();
            }
        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            if(_db.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return UsuarioDTO.UsuarioToDTO(usuario);
        }
        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if(id != usuarioDTO.Id)
            {
                return BadRequest();
            }
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(id);
            }
            usuario.Nombre = usuarioDTO.Nombre;
            usuario.ApellidoPaterno = usuarioDTO.ApellidoPaterno;
            usuario.ApellidoMaterno = usuarioDTO.ApellidoMaterno;
            usuario.NumeroTelefono = usuarioDTO.NumeroTelefono;
            usuario.Nss = usuarioDTO.NSS;
            usuario.Correo = usuarioDTO.Correo;
            usuario.Password = usuarioDTO.Password;
            usuario.FechaNacimiento = usuarioDTO.FechaNacimiento;
            usuario.IdTipoUsuario = usuarioDTO.IdTipoUsuario;
            usuario.IdRegion = usuarioDTO.IdRegion;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UsuarioExists(id))
            {
                return NotFound();
            }
            return NoContent();
        }
        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                ApellidoPaterno = usuarioDTO.ApellidoPaterno,
                ApellidoMaterno = usuarioDTO.ApellidoMaterno,
                NumeroTelefono = usuarioDTO.NumeroTelefono,
                Nss = usuarioDTO.NSS,
                Correo = usuarioDTO.Correo,
                Password = usuarioDTO.Password,
                FechaNacimiento = usuarioDTO.FechaNacimiento,
                IdTipoUsuario = usuarioDTO.IdTipoUsuario,
                IdRegion = usuarioDTO.IdRegion
            };
            if(_db.Usuarios == null)
            {
                return Problem("Entity set 'EncuestasDBContext.Usuario' is null");
            }
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, UsuarioDTO.UsuarioToDTO(usuario));
        }
        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if(_db.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        private bool UsuarioExists(int id)
        {
            return (_db.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
