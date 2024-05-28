using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppRod.Model
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int pk_id_usuario {  get; set; }

        public string? str_login { get; set; }

        public string? str_senha { get; set; }

        public bool bln_status { get; set; }

    }
}
