using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.DateTime)] //puede ser date
        public DateTime FechaTransaccion { get; set; } = DateTime.Parse( DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
        public decimal Monto { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage="Debe Seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede pasar de {1} caracteres")]
        public string Nota { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe Seleccionar una Cuenta")]
        [Display(Name = "Cuenta")]
        public int CuentaId { get; set; }

        [Display(Name = "Tipo Operación")]
        public TipoOperaciones TipoOperacionId { get; set; } = TipoOperaciones.Ingreso;

        public String Cuenta { get; set; }
        public String  Categoria { get; set; }

    }
}
