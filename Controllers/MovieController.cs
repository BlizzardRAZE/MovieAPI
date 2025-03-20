using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Models;
using MovieApi.Services;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private readonly ILogger<MovieController> _logger;

        private IMovieService _service;

        public MovieController(ILogger<MovieController> logger, IMovieService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {   
            IEnumerable<Movie> list = _service.GetMovies();
            // Check to see if movies is not null
            if (list != null) {
                // Status 200
                return Ok(list);
            }
            else
            {
                // Error 400
                return BadRequest();
            }
        }

        [HttpGet("{name}", Name="GetMovie")]

        public IActionResult GetMoviesByName(string name) {
            Movie obj = _service.GetMovieByName(name);
            
            if (obj != null) {
                return Ok(obj);
            }
            
            return BadRequest();
        }

        [HttpGet("year/")]
        public IActionResult GetMovieByYear (int year) {
            Movie obj = _service.GetMoviesByYear(year);

            if (obj != null) {
                return Ok(obj);
            }

            return BadRequest();
        }


        [HttpPost]
        public IActionResult CreateMovie(Movie m) {

            _service.CreateMovie(m);
            // ADD RETURN FOR SUCCESS

            // Status 201
            return CreatedAtRoute("GetMovie", new {name=m.Name}, m);
        }

        [HttpPut("{name}")]
        public IActionResult UpdateMovie(string name, Movie movieIn) {
            _service.UpdateMovie(name, movieIn);
            
            return NoContent();
        }


        [HttpDelete("{name}")]
        public IActionResult DeleteMovie(string name) {
            _service.DeleteMovie(name);
            
            return NoContent();
        }

    }
}
