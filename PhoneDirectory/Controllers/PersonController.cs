using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Data;
using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Extensions;
using PhoneDirectory.Mappers;
using PhoneDirectory.Models;
using PhoneDirectory.Repostory;
using PhoneDirectory.Services;
using System;

namespace PhoneDirectory.Controllers
{
    [Route("person/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class PersonController:ControllerBase
    {
        private readonly PersonService _personService;
        private readonly UserManager<AppUser> _userManager;


        public PersonController(UserManager<AppUser> userManager, PersonService personService)
        {
            _userManager = userManager;
            _personService = personService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> getAll()
        {
            var username=User.GetEmail();
            var user =await _userManager.FindByEmailAsync(username);
            var userList=await _personService.getAll(user);
            return Ok(userList);
        }

        [HttpPost]
        [Route("CreatePerson")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto createPersonDto)
        {


            try
            {
                var username = User.GetEmail();
                var user = await _userManager.FindByEmailAsync(username);
                var person = createPersonDto.ToPersonFromCreatePersonDto();
                person.Appuser = user;
                person.AppUserId = user.Id;
                await _personService.createPerson(person);
                return Ok(person.ToPersonDtoFromPerson());
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(createPersonDto.FullName))
            {
                return BadRequest(new { ErrorCode = "INVALID_NAME", message = ex.Message });
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(createPersonDto.PhoneNumber))
            {
                return BadRequest(new { ErrorCode = "PERSON_NUMBER_MUST_BE_VALID", message = ex.Message });
            }

            catch
            {
                return StatusCode(500, new { ErrorCode = "SERVER_ERROR", Message = "An unexpected error occurred." });
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person.ToPersonDtoFromPerson());
        }
        [HttpPut]
        [Route("UpdatePerson/{personId}")]
        public async Task<IActionResult> UpdateAddress([FromRoute]int personId, [FromBody] UpdatePersonDto updatePersonDto)
        {
            if (updatePersonDto == null)
            {
                return BadRequest("Data not found.");
            }
          

            try
            {
                var username = User.GetEmail();
                var user = await _userManager.FindByEmailAsync(username);

                var updatedPerson = await _personService.updatePerson(updatePersonDto,personId);
                var persondto =updatedPerson.ToPersonDtoFromPerson();

                return Ok(persondto);
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(updatePersonDto.FullName))
            {
                return BadRequest(new { ErrorCode = "INVALID_NAME", message = ex.Message });
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(updatePersonDto.PhoneNumber))
            {
                return BadRequest(new { ErrorCode = "PERSON_NUMBER_MUST_BE_VALID", message = ex.Message });
            }
         
            catch 
            {
                return StatusCode(500, new { ErrorCode = "SERVER_ERROR", Message = "An unexpected error occurred." });
            }
        }
        [HttpDelete]
        [Route("deletePerson/{personId}")]
        public async Task<IActionResult> DeletePersonById([FromRoute]int personId ){

            try
            {
        var person =await _personService.GetPersonById(personId);
         
                if (person == null)
                {
                    return NotFound();
                }  
                 await _personService.DeletePerson(person);
                return Ok(person.ToPersonDtoFromPerson());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    } 
}
