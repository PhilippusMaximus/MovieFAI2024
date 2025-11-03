using FAI.Resources;
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
        //[Required(ErrorMessageResourceName = "Err_IsRequired", ErrorMessageResourceType = typeof(BasicRes))]
        [Display(Name = nameof(Title), ResourceType = typeof(BasicRes))]
        public virtual string Title { get; set; } = string.Empty;

        // [Required]
        [Display(Name = nameof(Price), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public virtual decimal Price { get; set; }
        
        [Display(Name = nameof(ReleaseDate), ResourceType = typeof(BasicRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public virtual DateTime ReleaseDate { get; set; }

        [Display(Name = nameof(Genre), ResourceType = typeof(BasicRes))]
        public virtual int GenreId { get; set; }

        [Display(Name = nameof(MediumType), ResourceType = typeof(BasicRes))]
        public virtual string? MediumTypeCode { get; set; }

    }
}
