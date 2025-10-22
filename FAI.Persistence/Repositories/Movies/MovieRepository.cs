using FAI.Core.Attributes;
using FAI.Core.Repositories.Movies;
using FAI.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Repositories.Movies
{
    [MapServiceDependency(nameof(MovieRepository))]
    public class MovieRepository : BaseRepository, IMovieRepository
    {
    }
}
