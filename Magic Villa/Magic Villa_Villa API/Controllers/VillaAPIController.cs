using Microsoft.AspNetCore.Mvc;
using Magic_Villa_VillaAPI.Models;
using Magic_Villa_VillaAPI.Models.Dto;
using Magic_Villa_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
//in the video for this we tested the endpoints in Postman around 1:23:54
namespace Magic_Villa_VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    //when you add an endpoint to api controller you need to define the http verb of that endpoint
    //[ApiController] //adding this means we have to have an attribute route. Also helps with validation we have in DTO  
    public class VillaAPIController : ControllerBase
    {
        //in order to log we have to use dependency injection
        
        private readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }


        //creating an endpoint here
        [HttpGet] //defining http verb for the endpoint GetVillas
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            return Ok(VillaStore.villaList);
        }
        /*
         DTOs(data transfer objects) provide a wrapper between the entity
         or the database model and what is being exposed from the API
         */
        
        //the follwing statements are for GetVilla
        [HttpGet("{id:int}", Name ="GetVilla")] //have to specify that this httpget takes a parameter
        //more explanatory way of writing the response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200)]
        /*you could remove the VillaDTO return type beside ActionResult
          and write the following instead:
          [ProducesResponseType(200, Type =typeof(VillaDTO))]         
         */
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]

        //this endpoint does not have an explicit name in the controller so we add something to the httpget above it
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Villa Error with Id" + id);
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) 
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost] //the http verb we use when we're making a resource
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower())!= null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            //return Ok(villaDTO);
            //below is so when a resource is created you give them the URL where it was created
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //With ActionResult you have to add the return type but with IActionResult you do not use return type
        //and since with deletion we don't want to return any data it works
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO) 
        { 
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u =>u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, [FromBody]JsonPatchDocument<VillaDTO> patchDTO)
        {
            //Look up Json Patch for more info on patch operations
            //patchDTO is a Json patch document
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return NoContent();
            //when you run this, in the request body you only need the path, op and value
        }

    }
}
