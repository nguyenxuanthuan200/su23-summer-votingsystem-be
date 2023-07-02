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

        public async Task<APIResponse<GetQuestionResponse>> CreateElementQuestion(Guid questionId, CreateElementRequest request)
        {
            APIResponse<GetQuestionResponse> response = new();
            var checkquestion = await dbContext.Questions.SingleOrDefaultAsync(c => c.QuestionId == questionId);
            if (checkquestion == null)
            {
                response.ToFailedResponse("Question không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checktypequestion = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == checkquestion.TypeId);
            var id = Guid.NewGuid();
            Element ele = new Element();
            {
                ele.ElementId = id;
                ele.Content = request.Answer;
                ele.Status = true;
                ele.QuestionId = questionId;
                ele.Rate = request.Rate;
            }
            await dbContext.Elements.AddAsync(ele);
            await dbContext.SaveChangesAsync();
            var mapq = _mapper.Map<GetQuestionResponse>(checkquestion);
            mapq.TypeName = checktypequestion.Name;
            var element = await dbContext.Elements.Where(p => p.QuestionId == checkquestion.QuestionId && p.Status == true).ToListAsync();
            List<GetElementResponse> listelement = element.Select(
           x =>
           {
               return new GetElementResponse()
               {
                   ElementId = x.ElementId,
                   Answer = x.Content,
                   QuestionId = x.QuestionId,
                   Rate = x.Rate,
               };
           }
           ).ToList();
            mapq.Element = listelement;
            response.Data = mapq;
            response.ToSuccessResponse(response.Data, "Thêm câu trả lời vào câu hỏi thành công", StatusCodes.Status200OK);
            return response;

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
            var checktype = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == request.TypeId);
            if (checktype == null)
            {
                response.ToFailedResponse("Type của question không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }

            var id = Guid.NewGuid();
            Question ques = new Question();
            {
                ques.QuestionId = id;
                ques.Title = request.Title;
                ques.FormId = request.FormId;
                ques.Content = request.Content;
                ques.TypeId = request.TypeId;
            }
            await dbContext.Questions.AddAsync(ques);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetQuestionResponse>(ques);
            if (request.Element != null)
            {
                List<GetElementResponse> listelement = new List<GetElementResponse>();
                foreach (var i in request.Element)
                {
                    var ide = Guid.NewGuid();
                    Element ele = new Element();
                    {
                        ele.ElementId = ide;
                        ele.Content = i.Answer;
                        ele.Status = true;
                        ele.QuestionId = ques.QuestionId;
                        ele.Rate = i.Rate;
                    }
                    await dbContext.Elements.AddAsync(ele);
                    await dbContext.SaveChangesAsync();
                    var map1 = _mapper.Map<GetElementResponse>(ele);
                    map1.Answer = ele.Content;
                    listelement.Add(map1);
                }
                map.Element = listelement;
            }

            map.TypeName = checktype.Name;
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<GetQuestionNoElementResponse>> CreateQuestionNoElement(CreateQuestionWithNoElementRequest request)
        {
            APIResponse<GetQuestionNoElementResponse> response = new();
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == request.FormId);
            if (checkform == null)
            {
                response.ToFailedResponse("Form không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checktype = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == request.TypeId);
            if (checktype == null)
            {
                response.ToFailedResponse("Type của question không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }

            var id = Guid.NewGuid();
            Question ques = new Question();
            {
                ques.QuestionId = id;
                ques.Title = request.Title;
                ques.FormId = request.FormId;
                ques.Content = request.Content;
                ques.TypeId = request.TypeId;
            }
            await dbContext.Questions.AddAsync(ques);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetQuestionNoElementResponse>(ques);
            
            map.TypeName = checktype.Name;
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetQuestionResponse>>> GetListQuestionForm(Guid formid)
        {
            APIResponse<IEnumerable<GetQuestionResponse>> response = new();
            var question = await dbContext.Questions.Where(p => p.FormId == formid).ToListAsync();
            List<GetQuestionResponse> listquestion = new List<GetQuestionResponse>();
            foreach (var item in question)
            {
                var type = await dbContext.Types.Where(p => p.TypeId == item.TypeId).SingleOrDefaultAsync();
                GetQuestionResponse quest = new GetQuestionResponse();
                quest.QuestionId = item.QuestionId;
                quest.Content = item.Content;
                quest.Title = item.Title;
                quest.FormId = item.FormId;
                quest.TypeName = type.Name;
                var element = await dbContext.Elements.Where(p => p.QuestionId == item.QuestionId).ToListAsync();
                List<GetElementResponse> listelement = element.Select(
               x =>
               {
                   return new GetElementResponse()
                   {
                       ElementId = x.ElementId,
                       Answer = x.Content,
                       QuestionId = x.QuestionId,
                       Rate = x.Rate,
                   };
               }
               ).ToList();
                quest.Element = listelement;

                listquestion.Add(quest);
            }
            response.Data = listquestion;
            if (response.Data == null)
            {
                response.ToFailedResponse("Không có câu hỏi nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách câu hỏi thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetQuestionResponse>> UpdateQuestion(Guid id, UpdateQuestionRequest request)
        {
            APIResponse<GetQuestionResponse> response = new();
            var question = await dbContext.Questions.Where(p => p.QuestionId == id).SingleOrDefaultAsync();
            if (question == null)
            {
                response.ToFailedResponse("Không có câu hỏi nào phù hợp theo yêu cầu ", StatusCodes.Status400BadRequest);
                return response;
            }
            var questiontype = await dbContext.Types.Where(p => p.TypeId == request.TypeId).SingleOrDefaultAsync();
            if (questiontype == null)
            {
                response.ToFailedResponse("Không có loại câu hỏi nào phù hợp theo yêu cầu ", StatusCodes.Status400BadRequest);
                return response;
            }
            question.Title = request.Title;
            question.Content = request.Content;
            question.TypeId = request.TypeId;
            dbContext.Questions.Update(question);
            await dbContext.SaveChangesAsync();

            var mapq = _mapper.Map<GetQuestionResponse>(question);
            mapq.TypeName = questiontype.Name;
            List<GetElementResponse> listelement = new();
            foreach (var item in request.Element)
            {
                var element = await dbContext.Elements.Where(p => p.QuestionId == id && p.ElementId == item.ElementId).SingleOrDefaultAsync();
                if (element == null)
                {
                    response.ToFailedResponse("Câu trả lời không nằm trong câu hỏi heo yêu cầu ", StatusCodes.Status400BadRequest);
                    return response;
                }
                else
                {
                    element.Content = item.Answer;
                    element.Rate = item.Rate;
                    dbContext.Elements.Update(element);
                    await dbContext.SaveChangesAsync();
                }

                //var map = _mapper.Map<GetElementResponse>(element);
                //listelement.Add(map);

            }
            var elementt = await dbContext.Elements.Where(p => p.QuestionId == question.QuestionId && p.Status == true).ToListAsync();
            List<GetElementResponse> listelementt = elementt.Select(
           x =>
           {
               return new GetElementResponse()
               {
                   ElementId = x.ElementId,
                   Answer = x.Content,
                   QuestionId = x.QuestionId,
                   Rate = x.Rate,
               };
           }
           ).ToList();
            mapq.Element = listelementt;
            response.Data = mapq;
            response.ToSuccessResponse(response.Data, "Cập nhật câu hỏi và câu trả lời thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
