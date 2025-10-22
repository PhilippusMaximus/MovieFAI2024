using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    public class Movie : MovieBase, IEntity
    {
        // Movie erbt von MovieBase, MovieBase enthält die Grunddaten, Movie die Beziehungen zu anderern Entitäten
        // Hier sind die Navigation Properties zur Verknüpfung der Movies mit dem MediumType und Genre
        // somit Navigation-Properties von den Movie-Properties der Basisklasse getrennt

        // Genre muss nicht als ForeignKey deklariert werden,
        // da die Konventionen von EF Core den Fremdschlüssel (GenreId in Basisklasse) automatisch erkennen
        public virtual Genre Genre { get; set; }

        [ForeignKey(nameof(MediumTypeCode))]
        public virtual MediumType MediumType { get; set; }
              

    }
}
