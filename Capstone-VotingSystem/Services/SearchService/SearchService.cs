using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.SearchRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Models.ResponseModels.SearchReponse;
using Capstone_VotingSystem.Models.ResponseModels.StageResponse;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace Capstone_VotingSystem.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public SearchService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<APIResponse<PagedListCampaignResponse>> SearchFilterCampaign(SearchCampaignRequest request)
        {
            APIResponse<PagedListCampaignResponse> response = new();
            PagedListCampaignResponse paged = new();
            List<GetCampaignAndStageResponse> listCamn = new List<GetCampaignAndStageResponse>();
            var listCampaign = dbContext.Campaigns.AsQueryable();
           
            listCampaign = listCampaign.Where(p => p.Status == true && p.Visibility == "public");
            //check keyword
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                listCampaign = listCampaign.Where(p => p.Title.Contains(request.Keyword));
            }
            foreach (var item in listCampaign)
            {
                var map = _mapper.Map<GetCampaignAndStageResponse>(item);
                var stage = await dbContext.Stages.Where(p => p.CampaignId == item.CampaignId).ToListAsync();
                if (stage.Count != 0)
                {
                    List<GetStageResponse> listStage = stage.Select(
                   x =>
                   {
                       return new GetStageResponse()
                       {
                           StageId = x.StageId,
                           Content = x.Content,
                           Title = x.Title,
                           Description = x.Description,
                           StartTime = x.StartTime,
                           EndTime = x.EndTime,
                           CampaignId = x.CampaignId,
                           FormId = x.FormId,
                       };
                   }
                   ).ToList();
                    map.Stage = listStage;

                    listCamn.Add(map);
                }
            }
            paged.Total = listCamn.Count();
            listCamn = listCamn.ToPagedList((int)request.Page, (int)request.PageSize).ToList();
            paged.Campaign = listCamn;

            
            response.Data = paged;
            if (listCamn.Count()==0)
            {
                response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách Campaign thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<PagedListCandidateResponse>> SearchFilterCandidate(SearchCandidateRequest request)
        {
            APIResponse<PagedListCandidateResponse> response = new();
            PagedListCandidateResponse paged = new();
            List<GetListCandidateCampaignResponse> listCandi = new List<GetListCandidateCampaignResponse>();
            if (request.CampaignId==null || request.CampaignId==Guid.Empty)
            {
                response.ToFailedResponse("CampaignId không hợp lệ", StatusCodes.Status400BadRequest);
                return response;
            }
            //danh sach candidate thuoc campaign
            var listCandidate =  dbContext.Candidates.Where(p => p.Status == true && p.CampaignId == request.CampaignId).AsQueryable();

            if (string.IsNullOrEmpty(request.Keyword))
            {
               foreach(var i in listCandidate)
                {
                    var us = await dbContext.Users.Where(p => p.UserId == i.UserId && p.Status==true).SingleOrDefaultAsync();
                    var mapp = _mapper.Map<GetListCandidateCampaignResponse>(us);
                    mapp.CandidateId = i.CandidateId;
                    mapp.Description = i.Description;
                    mapp.CampaignId = i.CampaignId;
                    listCandi.Add(mapp);
                }
            }
            //if (request.GroupId != null || request.CampaignId != Guid.Empty)
            //{
            //    listCandidate = listCandidate.Where(p => p.GroupCandidateId==request.GroupId);
            //}
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var user = await dbContext.Users.Where(p => p.FullName.Contains(request.Keyword) &&p.Status==true).ToListAsync();
                //listCandidate = listCandidate.Where(p => p.Title.Contains(request.Keyword));
                foreach (var item in user)
                {
                    var checkCandidate= await dbContext.Candidates.Where(p=>p.UserId==item.UserId && p.CampaignId==request.CampaignId && p.Status==true).SingleOrDefaultAsync();
                    if (checkCandidate != null)
                    {
                        var map = _mapper.Map<GetListCandidateCampaignResponse>(item);
                        map.CandidateId=checkCandidate.CandidateId;
                        map.Description=checkCandidate.Description;
                        map.CampaignId=checkCandidate.CampaignId;
                        listCandi.Add(map);
                    }
                }
            }

            paged.Total = listCandi.Count();
            listCandi = listCandi.ToPagedList((int)request.Page, (int)request.PageSize).ToList();
            paged.Candidate = listCandi;


            response.Data = paged;
            if (listCandi.Count() == 0)
            {
                response.ToFailedResponse("Không có Candidate nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách Candiate thành công", StatusCodes.Status200OK);
            return response;

        }
    }
}
