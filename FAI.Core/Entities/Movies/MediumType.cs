using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    // [Table(name: "XY")] Table Attribute, um den SQL-Tabellen Namen zu übersteuern
    public class MediumType : IEntity
    {
        public MediumType() 
        { 
            this.Movies = new HashSet<Movie>();
        }

        [MaxLength(8), MinLength(2)]
        [Key]
        public virtual string Code { get; set; }

        [MaxLength(32),MinLength(2)]
        [Required]
        public virtual string Name { get; set; }

        // bildet beim Ausführen des Konstruktors eine Hashwert-Sammlung aus den Movie-Objekten (Eindeutigkeit, enthält keine Duplikate von Movie-Objekten)
        // Die Eigenschaft Stellt die 1:n Beziehung zwischen MediumType und Movie dar.
        public virtual ICollection<Movie> Movies { get; } //= new HashSet<Movie>();
    }
}
