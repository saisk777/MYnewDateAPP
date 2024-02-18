using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("not-found")] // for this one we are going to return 401 Unauthorized 

        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if(thing == null) return NotFound();

            return thing;

        }

        [HttpGet("server-error")] 
        
        public ActionResult<string> GetServerError()
        {
  
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
            
        }

        [HttpGet("bad-request")] // for this one we are going to return 400 Unauthorized 

        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request ");
        }
    }
}