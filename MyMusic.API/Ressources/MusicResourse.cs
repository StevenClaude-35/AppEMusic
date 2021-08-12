using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Ressources
{
    public class MusicResourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ArtistResourse Artist { get; set; }
    }
}
