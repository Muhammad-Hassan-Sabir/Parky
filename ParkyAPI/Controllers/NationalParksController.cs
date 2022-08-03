using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    public class NationalParksController : ControllerBase
    {

        private readonly INationalParkRepository _npRepo;

        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalPark,IMapper mapper)
        {
            _npRepo = nationalPark;
            _mapper = mapper;

        }


        /// <summary>
        /// Get List Of Nation Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]

        public IActionResult GetNationalParks()
        {
            var parks = _npRepo.GetNationalParks();


            var parksDto = new List<NationalParkDto>();

            foreach (var park in parks)
            {

                parksDto.Add(_mapper.Map<NationalParkDto>(park));
            }
             



            return Ok(parksDto);

        }

        /// <summary>
        /// Get Individual National Park
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}",Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var park=_npRepo.GetNationalPark(id);

            if (park==null)
            {
                return NotFound();

            }
            else
            {
                var parkDto = _mapper.Map<NationalParkDto>(park);
                return Ok(parkDto);
            }


        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type =typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
           

            if (nationalParkDto==null || nationalParkDto.Id!=0)
            {

                return BadRequest();
            }

            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exist!");

                return StatusCode(404, ModelState);

            }

          

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }
            else
            {
                return CreatedAtRoute("GetNationalPark",new { nationalPark.Id }, nationalPark);

            }




            //var result=_npRepo.CreateNationalPark(nationPark);
            //if (result)
            //{
            //    return Created(Uri)

            //}

            //return Ok(new Uri(Request.GetDisplayUrl()));






        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateNationalPark(int id,[FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkDto.Id!=id)
            {

                return BadRequest();
            }
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exist!");

                return StatusCode(404, ModelState);

            }
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();

            
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public IActionResult DeleteNationalPark(int id)
        {
            if (id==null)
            {

                return BadRequest();
            }
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();

            }

            var nationalPark = _npRepo.GetNationalPark(id);

            if (!_npRepo.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somethin went wrong when saving the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();


        }



    }
}
