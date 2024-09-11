using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
  // https://localhost:1234/api/regions
  [Route("/api/[controller]")]
  [ApiController]
  public class RegionsController : ControllerBase
  {
    private readonly NZWalksDbContext dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var regionsDomain = dbContext.Regions.ToList();

      var regionsDto = new List<RegionDto>();
      foreach (var regionDomain in regionsDomain)
      {
        regionsDto.Add(new RegionDto
        {
          Id = regionDomain.Id,
          Code = regionDomain.Code,
          Name = regionDomain.Name,
          RegionImageUrl = regionDomain.RegionImageUrl,
        });
      }

      return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
      var regionDomain = dbContext.Regions.Find(id);

      if (regionDomain == null)
      {
        return NotFound();
      }

      var regionDto = new RegionDto
      {
        Id = regionDomain.Id,
        Code = regionDomain.Code,
        Name = regionDomain.Name,
        RegionImageUrl = regionDomain.RegionImageUrl,
      };

      return Ok(regionDto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
      var regionDomainModel = new Region
      {
        Code = addRegionRequestDto.Code,
        Name = addRegionRequestDto.Name,
        RegionImageUrl = addRegionRequestDto.RegionImageUrl,
      };

      dbContext.Regions.Add(regionDomainModel);
      dbContext.SaveChanges();

      var regionDto = new RegionDto
      {
        Id = regionDomainModel.Id,
        Code = regionDomainModel.Code,
        Name = regionDomainModel.Name,
        RegionImageUrl = regionDomainModel.RegionImageUrl,
      };


      return CreatedAtAction(nameof(GetById), new { regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
      var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

      if (regionDomainModel == null)
      {
        return NotFound();
      }


      regionDomainModel.Name = updateRegionRequestDto.Name;
      regionDomainModel.Code = updateRegionRequestDto.Code;
      regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

      dbContext.SaveChanges();

      var regionDto = new RegionDto
      {
        Id = regionDomainModel.Id,
        Code = regionDomainModel.Code,
        Name = regionDomainModel.Name,
        RegionImageUrl = regionDomainModel.RegionImageUrl,
      };

      return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id) 
    {
      var regionDomainModel = dbContext.Regions.FirstOrDefault(y => y.Id == id);

      if (regionDomainModel == null) { return NotFound(); }

      dbContext.Regions.Remove(regionDomainModel);
      dbContext.SaveChanges();

      return Ok();
    }
  }
}
