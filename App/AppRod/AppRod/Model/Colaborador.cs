using System.ComponentModel.DataAnnotations;

namespace AppRod.Model
{
    public class Colaborador
    {
        [Key]
        public int pk_Id_Colaborador {  get; set; }
        public string? str_Nome { get; set; }
        public int fk_Unidade { get; set; }
        public int fk_Usuario { get; set; }
        public Unidade Unidade { get; set; }
        public Usuario Usuario { get; set; }
    }
}
