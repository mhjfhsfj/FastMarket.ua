using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastMarketBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NovaPostController : ControllerBase
    {
        private readonly NovaPostService _novaPostService;
        
        public NovaPostController(NovaPostService novaPostService)
        {
            _novaPostService = novaPostService;
        }

        [HttpGet(nameof(GetRegionNP))]
        public async Task<ActionResult<IEnumerable<NpRegion>>> GetRegionNP()
        {
            try
            {
                return await _novaPostService.GetRegionsAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet(nameof(GetCityByRegion))]
        public async Task<ActionResult<IEnumerable<NpCity>>> GetCityByRegion(string regionRef)
        {
            try
            {
                return await _novaPostService.GetCitiesByRegionAsync(regionRef);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet(nameof(GetWarehouses))]
        public async Task<ActionResult<IEnumerable<NpWarehouse>>> GetWarehouses(string cityRef,
            string typeOfWarehouse = "All")
        {
            try
            {
                return await _novaPostService.GetWarehousesAsync(cityRef, typeOfWarehouse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
