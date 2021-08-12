using FluentValidation;
using MyMusic.API.Ressources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Validation
{
    public class SaveArtistResourceValidator:AbstractValidator<SaveArtistResourse>
    {
        public SaveArtistResourceValidator()
        {

            RuleFor(m => m.Name)
               .NotEmpty()
               .MaximumLength(50);
        }
    }
}
