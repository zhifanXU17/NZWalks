﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
  // https://localhost:1234/api/regions
  [Route("/api/[controller]")]
  [ApiController]
  public class RegionsController : ControllerBase
  {
    private readonly NZWalksDbContext dbContext;
    private readonly IRegionRepository regionRepository;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
    {
      this.dbContext = dbContext;
      this.regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var regionsDomain = await regionRepository.GetAllAsync();

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
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
      var regionDomain = await regionRepository.GetByIdAsync(id);

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
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
      var regionDomainModel = new Region
      {
        Code = addRegionRequestDto.Code,
        Name = addRegionRequestDto.Name,
        RegionImageUrl = addRegionRequestDto.RegionImageUrl,
      };

      await regionRepository.CreateAsync(regionDomainModel);

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
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
      var regionDomainModel = new Region
      {
        Code = updateRegionRequestDto.Code,
        Name = updateRegionRequestDto.Name,
        RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
      };

      regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
      
      if (regionDomainModel == null)
      {
        return NotFound();
      }

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
    public async Task<IActionResult> Delete([FromRoute] Guid id) 
    {
      var regionDomainModel = await regionRepository.DeleteAsync(id);

      if (regionDomainModel == null) { return NotFound(); }

      return Ok();
    }
  }
}
