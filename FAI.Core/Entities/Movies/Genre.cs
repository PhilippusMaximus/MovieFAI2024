using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    public class Genre : IEntity
    {
        public Genre() 
        {
           this.Movies = new HashSet<Movie>();
        }

        //[Key, Column(Order = 0)] /* Column mit Order bei kombinierten Schlüssel-Attributen */
        public virtual int Id { get; set; }
        [MaxLength(60), MinLength(2)]
        [Required]
        public virtual string Name { get; set; } = string.Empty;

        // bildet beim Ausführen des Konstruktors eine Hashwert-Sammlung aus den Movie-Objekten (Eindeutigkeit, enthält keine Duplikate von Movie-Objekten)
        // Die Eigenschaft Stellt die 1:n Beziehung zwischen Genre und Movie dar.
        public virtual ICollection<Movie> Movies { get; } //= new HashSet<Movie>();
    }
}
