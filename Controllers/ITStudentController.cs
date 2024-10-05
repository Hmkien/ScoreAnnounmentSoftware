using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreAnnouncementSoftware.Data;
using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Controllers
{
    public class ITStudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ITStudentController(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
