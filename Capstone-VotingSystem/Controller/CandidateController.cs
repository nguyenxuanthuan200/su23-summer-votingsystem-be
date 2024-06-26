﻿using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Services.CandidateService;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/candidates")]
    [ApiController]
    public class CandidateController : BaseApiController
    {
        private readonly ICandidateService candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService;
        }
        // [Authorize(Roles = "User,Admin")]
        [HttpGet("{candidateid}/stage/{stageid}")]
        [SwaggerOperation(summary: "Get candidate by stage id")]
        public async Task<IActionResult> GetCandidateByStageId(Guid candidateid, Guid stageid)
        {
            try
            {
                var result = await candidateService.GetCandidateByStageId(candidateid, stageid);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "User,Admin")]
        [HttpGet("{candidateid}")]
        [SwaggerOperation(summary: "Get candidate by Id")]
        public async Task<IActionResult> GetCandidateById(Guid candidateid)
        {
            try
            {
                var result = await candidateService.GetCandidateById(candidateid);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "User,Admin")]
        [HttpGet("campaign/{id}")]
        [SwaggerOperation(summary: "Get list candidate by campaign id")]
        public async Task<IActionResult> GetListCandidateCampaign(Guid id)
        {
            try
            {
                var result = await candidateService.GetListCandidateCampaign(id);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        //[HttpPost("accounts")]
        //[SwaggerOperation(summary: "Create list account Candidate to Campagin")]
        //public async Task<IActionResult> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        //{
        //    try
        //    {
        //        var response = await candidateService.CreateAccountCandidateCampaign(request);
        //        if (response.Success == false)
        //        {
        //            return BadRequest(response);
        //        }
        //        return Ok(response);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error retrieving data from the database.");
        //    }
        //}
        //[Authorize(Roles = "User")]
        [HttpPost("list-candidate/")]
        [SwaggerOperation(summary: "Create list Candidate to campaign")]
        public async Task<IActionResult> CreateCandidateCampaign(CreateListCandidateRequest request)
        {
            try
            {
                var response = await candidateService.CreateListCandidate(request);
                if (response.Success == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        //[Authorize(Roles = "User")]
        [HttpPost]
        [SwaggerOperation(summary: "Add list Candidate to campaign")]
        public async Task<IActionResult> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            try
            {
                var response = await candidateService.CreateCandidateCampaign(request);
                if (response.Success == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        [SwaggerOperation(summary: "Delete Candidate in campaign")]
        public async Task<IActionResult> DeleteCandidate(Guid id, DeleteCandidateRequest request)
        {
            try
            {
                var result = await candidateService.DeleteCandidateCampaign(id, request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        [HttpGet("stage/{stageid}/user/{userid}")]
        [SwaggerOperation(summary: "Get All Candidate by Stage")]
        public async Task<IActionResult> GetListCandidateByStage(Guid stageid, string userid)
        {
            try
            {
                var result = await candidateService.GetListCandidatStage(stageid, userid);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        //[Authorize(Roles = "User")]
        [HttpGet("user/{userid}")]
        [SwaggerOperation(summary: "Get List Candidate by UserId")]
        public async Task<IActionResult> GetListCandidateByUserId(string userid)
        {
            try
            {
                var result = await candidateService.GetListCandidateByUserId(userid);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        // [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Candidate")]
        public async Task<IActionResult> UpdateCampaign(Guid id, [FromForm] UpdateCandidateProfileRequest request)
        {
            try
            {
                var result = await candidateService.UpdateCandidateProfile(id, request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
    }
}
