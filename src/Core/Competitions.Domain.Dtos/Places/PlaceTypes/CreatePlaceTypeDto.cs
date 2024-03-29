﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Places.PlaceTypes
{
	public class CreatePlaceTypeDto
	{
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public String Title { get; set; }

        public String? Description { get; set; }
    }
}
