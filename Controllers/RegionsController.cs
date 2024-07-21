using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbcontext;
        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            _dbcontext = nZWalksDbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain=_dbcontext.Regions.ToList();
            
            var regionsDto= new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto){
                    Id= regionDomain.Id,
                        Code= regionDomain.Code,
                        Name= regionDomain.Name,
                        RegionImageUrl= regionDomain.RegionImageUrl
                };
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            var region = _dbcontext.Regions.FirstOrDefault(x=>x.Id==id);
            if (region == null)
            {
                return NotFound();
            }

            var regionsDto = new List<RegionDto>{
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionsDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody]AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            _dbcontext.Regions.Add(regionDomainModel);
            _dbcontext.SaveChanges();


            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return CreatedAtAction(nameof(GetById), new { id=regionDomainModel.Id}, regionDto);
        }
    }
}
