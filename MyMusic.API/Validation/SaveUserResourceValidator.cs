﻿using FluentValidation;
using MyMusic.API.Ressources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Validation
{
    public class SaveUserResourceValidator:AbstractValidator<UserResource>
    {
        public SaveUserResourceValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.LastName)
               .NotEmpty()
               .MaximumLength(50);

            RuleFor(m => m.UserName)
               .NotEmpty()
               .MaximumLength(50);

            RuleFor(m => m.Password)
               .NotEmpty()
               .MaximumLength(50);
        }
    }
}
