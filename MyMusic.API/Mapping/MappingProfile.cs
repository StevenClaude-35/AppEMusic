using AutoMapper;
using MyMusic.API.Ressources;
using MysMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Mapping
{
    public class MappingProfile:Profile
    {
        

        public MappingProfile()
        {
            //Domain(base de donnée vers Resource)
            CreateMap<Music, MusicResourse>();
            CreateMap<Artist, ArtistResourse>();
            CreateMap<Music, SaveMusicResourse>();
            CreateMap<Artist, SaveArtistResourse>();
            CreateMap<Composer, ComposerResourse>();
            CreateMap<Composer, SaveComposerResourse>();
            CreateMap<User, UserResource>();


            //Resource vers Domaine

            CreateMap<MusicResourse, Music>();
            CreateMap<ArtistResourse, Artist>();
            CreateMap<SaveMusicResourse,Music>();
            CreateMap<SaveArtistResourse, Artist>();
            CreateMap<ComposerResourse, Composer>();
            CreateMap<SaveComposerResourse,Composer>();
            CreateMap<UserResource,User>();



        }
    }
}
