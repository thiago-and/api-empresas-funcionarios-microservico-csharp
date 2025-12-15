using CadastroFuncionarios.Api.Data;
using CadastroFuncionarios.Api.DTOs;
using CadastroFuncionarios.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroFuncionarios.Api.Controllers
{
    [ApiController]
    [Route("api/v1/funcionarios")]
    public class FuncionariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FuncionariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.AsNoTracking().ToListAsync();
            return Ok(funcionarios);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return Ok(funcionario);
        }

        [HttpPost]
        public async Task<ActionResult<Funcionario>> PostFuncionario(FuncionarioDto funcionarioDto)
        {
            var funcionario = new Funcionario
            {
                Nome = funcionarioDto.Nome,
                Cargo = funcionarioDto.Cargo,
                Salario = funcionarioDto.Salario,
                DataAdmissao = funcionarioDto.DataAdmissao
            };

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, funcionario);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutFuncionario(int id, FuncionarioDto funcionario)
        {
            var existente = await _context.Funcionarios.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(funcionario.Nome) || string.IsNullOrWhiteSpace(funcionario.Cargo)
                || funcionario.Salario <= 0 || funcionario.DataAdmissao == default)
            {
                return BadRequest("Todos os campos são obrigatórios");
            }

            existente.Nome = funcionario.Nome;
            existente.Cargo = funcionario.Cargo;
            existente.Salario = funcionario.Salario;
            existente.DataAdmissao = funcionario.DataAdmissao;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}