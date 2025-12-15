using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroFuncionarios.Api.DTOs
{
    public class FuncionarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        [DefaultValue("João da Silva")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O cargo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O cargo pode ter no máximo 100 caracteres.")]
        [DefaultValue("Analista de Sistemas")]
        public required string Cargo { get; set; }

        [Required(ErrorMessage = "O salário é obrigatório.")]
        [Range(1412, 100000, ErrorMessage = "O salário deve ser maior ou igual ao salário mínimo.")]
        [Column(TypeName = "decimal(10,2)")]
        [DefaultValue(3500.00)]
        public required decimal Salario { get; set; }

        [Required(ErrorMessage = "A data de admissão é obrigatória.")]
        [DataType(DataType.Date)]
        [DefaultValue("2024-01-15")]
        public required DateTime DataAdmissao { get; set; }
    }
}