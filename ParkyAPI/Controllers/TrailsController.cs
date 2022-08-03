using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    public class TrailsController : ControllerBase
    {

        private readonly ITrailRepository _trailRepo;
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _trailRepo = trailRepository;
            _npRepo = nationalParkRepository;
            _mapper = mapper;

        }


        /// <summary>
        /// Get List Of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]

        public IActionResult GetTrails()
        {
            var trails = _trailRepo.GetTrails();


            var trailsDto = new List<TrailDto>();

            foreach (var trail in trails)
            {

                trailsDto.Add(_mapper.Map<TrailDto>(trail));
            }
             



            return Ok(trailsDto);

        }
        [HttpGet("GetTrailsByNationalPark/{nationalParkId:int}",Name = "GetTrailsByNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ActionName("GetTrailsByNationalPark")]

        public IActionResult GetTrailsByNationalPark(int nationalParkId)
        {

            if (!_npRepo.NationalParkExists(nationalParkId))
            {

                ModelState.AddModelError("", "National Park Not Exist");

                return StatusCode(404, ModelState);

            }
            var trails = _trailRepo.GetTrailsInNationalPark(nationalParkId);


            var trailsDto = new List<TrailDto>();

            foreach (var trail in trails)
            {

                trailsDto.Add(_mapper.Map<TrailDto>(trail));
            }




            return Ok(trailsDto);

        }
        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}",Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var park=_trailRepo.GetTrail(id);

            if (park==null)
            {
                return NotFound();

            }
            else
            {
                var parkDto = _mapper.Map<TrailDto>(park);
                return Ok(parkDto);
            }


        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type =typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateTrail([FromBody]TrailInsertsDto trailDto)
        {
           

            if (trailDto==null)
            {

                return BadRequest();
            }
            if (!_npRepo.NationalParkExists(trailDto.NationalParkId))
            {
                ModelState.AddModelError("", "National Park Not Exist!");

                return StatusCode(404, ModelState);

            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exist!");

                return StatusCode(404, ModelState);

            }

          

            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {trail.Name}");

                return StatusCode(500, ModelState);
            }
            else
            {
                return CreatedAtRoute("GetTrail",new { trail.Id }, trail);

            }
            //var result=_trailRepo.CreateTrail(nationPark);
            //if (result)
            //{
            //    return Created(Uri)

            //}

            //return Ok(new Uri(Request.GetDisplayUrl()));
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateTrail(int id,[FromBody]TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailDto.Id!=id)
            {

                return BadRequest();
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exist!");

                return StatusCode(404, ModelState);

            }
            var trail = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {trail.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();

            
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public IActionResult DeleteTrail(int id)
        {
            if (id==null)
            {

                return BadRequest();
            }
            if (!_trailRepo.TrailExists(id))
            {
                return NotFound();

            }

            var trail = _trailRepo.GetTrail(id);

            if (!_trailRepo.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {trail.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();


        }






    }
}
