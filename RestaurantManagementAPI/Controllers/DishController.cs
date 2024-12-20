using Microsoft.AspNetCore.Mvc;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services.DishServiceFolder;

namespace RestaurantManagementAPI.Controllers
{
    [Route("/api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public ActionResult Post([FromRoute] string restaurantId, [FromBody] CreateDishDto dishDto)
        {
            var newDishId = _dishService.Create(restaurantId, dishDto);
            return Created($"/api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }
        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] string restaurantId, [FromRoute] string dishId)
        {
            var dish = _dishService.GetById(restaurantId, dishId);
            return Ok(dish);
        }
        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] string restaurantId)
        {
            var result = _dishService.GetAll(restaurantId);
            return Ok(result);
        }
        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] string restaurantId)
        {
            _dishService.RemoveAll(restaurantId);
            return NoContent();
        }
        [HttpDelete("{dishId}")]
        public ActionResult DeleteDishById([FromRoute] string restaurantId, [FromRoute] string dishId)
        {
            _dishService.RemoveById(restaurantId, dishId);
            return NoContent();
        }
    }
}
