
using BpstEdu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitProSoftTech.Controllers
{
    public class BaseController : Controller
    {
        private readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

       
       

     
 

    }
}
