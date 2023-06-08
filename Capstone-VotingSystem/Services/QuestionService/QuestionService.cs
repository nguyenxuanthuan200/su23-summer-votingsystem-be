using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Models.RequestModels.QuestionRequest;
using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;
        public QuestionService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<APIResponse<GetQuestionResponse>> CreateQuestion(CreateQuestionRequest request)
        {
            APIResponse<GetQuestionResponse> response = new();
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == request.FormId);
            if (checkform == null)
            {
                response.ToFailedResponse("Form không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checktype = await dbContext.QuestionTypes.SingleOrDefaultAsync(c => c.QuestionTypeId == request.QuestionTypeId);
            if (checktype == null)
            {
                response.ToFailedResponse("Type của question không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.Element == null)
            {
                response.ToFailedResponse("Element của question trống", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Question ques = new Question();
            {
                ques.QuestionId = id;
                ques.QuestionName = request.QuestionName;
                ques.FormId = request.FormId;
                ques.QuestionTypeId = request.QuestionTypeId;
            }
            await dbContext.Questions.AddAsync(ques);
            await dbContext.SaveChangesAsync();
            if (request.Element == null)
            {
                response.ToFailedResponse("Element của question trống", StatusCodes.Status400BadRequest);
                return response;
            }
            List<GetElementResponse> listelement = new List<GetElementResponse>();
            foreach (var i in request.Element)
            {
                var ide = Guid.NewGuid();
                Element ele=new Element ();
                {
                    ele.ElementId = ide;
                    ele.Text = i.Text;
                    ele.QuestionId = ques.QuestionId;
                }
                await dbContext.Elements.AddAsync(ele);
                await dbContext.SaveChangesAsync();
                var map1 = _mapper.Map<GetElementResponse>(ele);
                listelement.Add(map1);
            }
            var map = _mapper.Map<GetQuestionResponse>(ques);
            map.Element = listelement;
            var type = await dbContext.QuestionTypes.SingleOrDefaultAsync(c => c.QuestionTypeId == request.QuestionTypeId);
            map.TypeName = type.TypeName;
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public Task<APIResponse<IEnumerable<GetQuestionResponse>>> GetListQuestionForm(Guid formid)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<GetQuestionResponse>> UpdateQuestion(Guid id, UpdateQuestionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
