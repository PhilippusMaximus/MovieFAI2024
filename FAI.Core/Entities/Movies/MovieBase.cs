using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    public class MovieBase
    {
        // das ist die Basisklasse für die relevanten Eigenschaften eines jeden Movie-Objekts
        public virtual Guid Id { get; set; }

        [MaxLength(128), MinLength(1)]
        [Required]
        public virtual string Title { get; set; } = string.Empty;

        // [Required]
        public virtual decimal Price { get; set; }

        public virtual DateTime ReleaseDate { get; set; }
                
        public virtual int GenreId { get; set; }   

        public virtual string? MediumTypeCode { get; set; }

    }
}
