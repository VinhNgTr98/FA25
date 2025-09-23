using CarManagement_API.Data;
using CarManagement_API.DTOs;
using CarManagement_API.Models;
using CarManagement_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _service;

        public CarController(ICarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarReadDto>>> GetAllCars()
        {
            var cars = await _service.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarReadDto>> GetCar(Guid id)
        {
            var car = await _service.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CarReadDto>>> GetFilteredCars(
            [FromQuery] string? transmission,
            [FromQuery] string? fuel,
            [FromQuery] string? carBrand,
            [FromQuery] string? carName,
            [FromQuery] int? engineCc)
        {
            var cars = await _service.GetFilteredCarsAsync(transmission, fuel, carBrand, carName, engineCc);
            return Ok(cars);
        }

        [HttpPost]
        public async Task<ActionResult<CarReadDto>> CreateCar([FromBody] CarCreateDto dto)
        {
            var car = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(Guid id, [FromBody] CarUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
