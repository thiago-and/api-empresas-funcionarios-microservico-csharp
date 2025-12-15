using CadastroEmpresas.Api.Data;
using CadastroEmpresas.Api.Models;
using CadastroEmpresas.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresas.Api.Controllers
{
    [ApiController]
    [Route("api/v1/empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            var empresas = await _context.Empresas.AsNoTracking().ToListAsync();
            return Ok(empresas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(EmpresaDto empresaDto)
        {
            var empresa = new Empresa
            {
                Nome = empresaDto.Nome,
                Cnpj = empresaDto.Cnpj,
                Endereco = empresaDto.Endereco,
                Telefone = empresaDto.Telefone
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEmpresa(int id, EmpresaDto empresa)
        {
            var existente = await _context.Empresas.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(empresa.Nome) || string.IsNullOrWhiteSpace(empresa.Cnpj)
                || string.IsNullOrWhiteSpace(empresa.Endereco) || string.IsNullOrWhiteSpace(empresa.Telefone))
            {
                return BadRequest("Todos os campos são obrigatórios");
            }

            existente.Nome = empresa.Nome;
            existente.Cnpj = empresa.Cnpj;
            existente.Endereco = empresa.Endereco;
            existente.Telefone = empresa.Telefone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}