﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GymPlanner.Areas.Identity.Data;

public class User : IdentityUser
{
    public List<Training>? Trainings { get; set; }
}

