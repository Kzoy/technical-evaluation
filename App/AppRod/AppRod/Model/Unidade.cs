using System.ComponentModel.DataAnnotations;

namespace AppRod.Model
{
    public class Unidade
    {
        [Key]
        public int pk_Id_Unidade {  get; set; }
        public string? str_Nome { get; set; }
        public bool bln_Status { get; set; }
        public ICollection<Colaborador> Colaboradores { get; set; }

    }
}
