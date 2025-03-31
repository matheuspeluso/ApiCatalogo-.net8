using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")] //não precisaria pois no contexto já esta sendo mapeada
    public class Categoria
    {
        public Categoria() {
            Produtos = new Collection<Produto>(); //inicializando a nossa propriedade produto que é uma coleção de produto
        }

        [Key] 
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public ICollection<Produto>? Produtos { get; set; } //categoria contem uma coleção de produtos
        //OBS: isso já seria suficiente para o EF Core criar uma conexão de ONE TO MANY porém vamos explicitar
        //e refiniar mais para isso vamos na entidade produto.
    }
}
