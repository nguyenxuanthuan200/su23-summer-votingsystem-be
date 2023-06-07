﻿using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Services.CampaignService;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/campaign")]
    [ApiController]
    public class CampaignController : BaseController
    {
        private readonly ICampaignService campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCampaign()
        {
            try
            {
                var result = await campaignService.GetCampaign();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCampaign(CreateCampaignRequest request)
        {
            try
            {
                if (request == null)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }
                var create = await campaignService.CreateCampaign(request);
                if (create == null)
                {
                    return CustomResult("Campaign da ton tai", HttpStatusCode.Accepted);
                }
                //var result = _mapper.Map<CreateAccountResponse>(create);
                return CustomResult("Success", create, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);


            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(Guid id, UpdateCampaignRequest request)
        {
            try
            {
                if (request == null || id != request.CampaignId)
                {
                    return CustomResult("Cu Phap Sai", HttpStatusCode.BadRequest);
                }

                var update = await campaignService.UpdateCampaign(request);

                if (update == null)
                {
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                }
                //var result = _mapper.Map<CreateAccountResponse>(update);
                return CustomResult("Success", update, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return CustomResult("Fail", HttpStatusCode.InternalServerError);
            }
        }
        //[HttpGet("campus")]
        //public async Task<IActionResult> GetCampaignByCampus(Guid id)
        //{
        //    try
        //    {
        //        var result = await campaignRepositories.GetCampaignByCampus(id);
        //        if (result == null)
        //        {
        //            return CustomResult("Not Found", HttpStatusCode.NotFound);
        //        }
        //        return CustomResult("Success", result, HttpStatusCode.OK);
        //    }
        //    catch (Exception)
        //    {
        //        return CustomResult("Fail", HttpStatusCode.InternalServerError);

        //    }
        //}
        //[HttpGet("campaigntype")]
        //public async Task<IActionResult> GetCampaignByType(Guid id)
        //{
        //    try
        //    {
        //        var result = await campaignRepositories.GetCampaignByType(id);
        //        if (result == null)
        //        {
        //            return CustomResult("Not Found", HttpStatusCode.NotFound);
        //        }
        //        return CustomResult("Success", result, HttpStatusCode.OK);
        //    }
        //    catch (Exception)
        //    {
        //        return CustomResult("Fail", HttpStatusCode.InternalServerError);

        //    }
        //}
    }
}
