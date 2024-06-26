﻿using AutoMapper;
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
                ele.Score = request.Score;
            }
            await dbContext.Elements.AddAsync(ele);
            await dbContext.SaveChangesAsync();
            var mapq = _mapper.Map<GetQuestionResponse>(checkquestion);
            mapq.TypeName = checktypequestion.Name;
            mapq.TypeId = checktypequestion.TypeId;
            var element = await dbContext.Elements.Where(p => p.QuestionId == checkquestion.QuestionId && p.Status == true).ToListAsync();
            List<GetElementResponse> listelement = element.Select(
           x =>
           {
               return new GetElementResponse()
               {
                   ElementId = x.ElementId,
                   Answer = x.Content,
                   QuestionId = x.QuestionId,
                   Score = x.Score,
                   Status = x.Status,
               };
           }
           ).ToList();
            listelement = listelement.OrderBy(x => x.Answer).ToList();
            mapq.Element = listelement;
            response.Data = mapq;
            response.ToSuccessResponse(response.Data, "Thêm câu trả lời vào câu hỏi thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<string>> CreateQuestion(CreateListQuestionRequest request)
        {
            APIResponse<string> response = new();
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == request.FormId);
            if (checkform == null)
            {
                response.ToFailedResponse("Biểu mẫu không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkform.IsApprove == true)
            {
                response.ToFailedResponse("Không thể chỉnh sửa biểu mẫu khi đã xác nhận điều khoản", StatusCodes.Status400BadRequest);
                return response;
            }

            if (request.listQuestion.Count == 0)
            {
                response.ToFailedResponse("Danh sách câu hỏi trống", StatusCodes.Status400BadRequest);
                return response;
            }
            foreach (var item in request.listQuestion)
            {
                var checktype = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == item.TypeId);
                if (checktype == null)
                {
                    response.ToFailedResponse("Loại của câu hỏi không tồn tại", StatusCodes.Status400BadRequest);
                    return response;
                }

                var id = Guid.NewGuid();
                Question ques = new Question();
                {
                    ques.QuestionId = id;
                    ques.FormId = request.FormId;
                    ques.Content = item.Content;
                    ques.Status = true;
                    ques.TypeId = item.TypeId;
                }
                await dbContext.Questions.AddAsync(ques);
                await dbContext.SaveChangesAsync();

                if (checktype.Name == "Bình chọn sao")
                {
                    if (item.Score % 5 != 0)
                    {
                        response.ToFailedResponse("Điểm phải chia hết cho 5", StatusCodes.Status400BadRequest);
                        return response;
                    }
                    var so = item.Score / 5;
                    for (var i = 1; i < 6; i++)
                    {
                        var idEle = Guid.NewGuid();
                        Element ele = new Element();
                        {
                            ele.ElementId = idEle;
                            ele.Content = i + " star";
                            ele.Status = true;
                            ele.QuestionId = id;
                            ele.Score = so * i;
                        }
                        await dbContext.Elements.AddAsync(ele);
                        await dbContext.SaveChangesAsync();
                    }

                }
                // var map = _mapper.Map<GetQuestionResponse>(ques);
                else if (item.Element != null)
                {
                    //  List<GetElementResponse> listelement = new List<GetElementResponse>();
                    foreach (var i in item.Element)
                    {
                        var ide = Guid.NewGuid();
                        Element ele = new Element();
                        {
                            ele.ElementId = ide;
                            ele.Content = i.Answer;
                            ele.Status = true;
                            ele.QuestionId = ques.QuestionId;
                            ele.Score = i.Score;
                        }
                        await dbContext.Elements.AddAsync(ele);
                        await dbContext.SaveChangesAsync();
                        // var map1 = _mapper.Map<GetElementResponse>(ele);
                        // map1.Answer = ele.Content;
                        //  listelement.Add(map1);
                    }
                    // map.Element = listelement;
                }

            }

            // map.TypeName = checktype.Name;
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }

        public async Task<APIResponse<GetQuestionNoElementResponse>> CreateQuestionNoElement(CreateQuestionWithNoElementRequest request)
        {
            APIResponse<GetQuestionNoElementResponse> response = new();
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == request.FormId && c.Status == true);
            if (checkform == null)
            {
                response.ToFailedResponse("Biểu mẫu không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checktype = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == request.TypeId);
            if (checktype == null)
            {
                response.ToFailedResponse("Kiểu của câu hỏi không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!checktype.Name.Equals("Bình chọn sao"))
            {
                response.ToFailedResponse("Kiểu của câu hỏi phải là dạng đánh giá", StatusCodes.Status400BadRequest);
                return response;
            }


            var id = Guid.NewGuid();
            Question ques = new Question();
            {
                ques.QuestionId = id;
                ques.FormId = request.FormId;
                ques.Content = request.Content;
                ques.TypeId = request.TypeId;
                ques.Status = true;
            }
            if (request.Score % 5 != 0)
            {
                response.ToFailedResponse("Điểm phải chia hết cho 5", StatusCodes.Status400BadRequest);
                return response;
            }
            var so = request.Score / 5;
            for (var i = 1; i < 6; i++)
            {
                var idEle = Guid.NewGuid();
                Element ele = new Element();
                {
                    ele.ElementId = idEle;
                    ele.Content = i + " star";
                    ele.Status = true;
                    ele.QuestionId = id;
                    ele.Score = so * i;
                }
                await dbContext.Elements.AddAsync(ele);
                // await dbContext.SaveChangesAsync();
            }

            await dbContext.Questions.AddAsync(ques);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetQuestionNoElementResponse>(ques);

            map.TypeName = checktype.Name;
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteQuestion(Guid id)
        {
            APIResponse<String> response = new();

            var checkQuestion = await dbContext.Questions.SingleOrDefaultAsync(c => c.QuestionId == id && c.Status == true);
            if (checkQuestion == null)
            {
                response.ToFailedResponse("Câu hỏi không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            checkQuestion.Status = false;
            dbContext.Questions.Update(checkQuestion);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa thành công", StatusCodes.Status200OK);
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
                quest.FormId = item.FormId;
                quest.TypeId = item.TypeId;
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
                       Status = x.Status,
                       Score = x.Score,
                   };
               }
               ).ToList();
                listelement = listelement.OrderBy(x => x.Answer).ToList();

                quest.Element = listelement;
                Guid typeRatingAnswer = Guid.Parse("284a18e0-6d3b-4f71-83de-aa79513a3cd7");
                decimal scoreOfRating = 0;
                if (quest.TypeId == typeRatingAnswer)
                {
                    var ele = quest.Element.Where(p => p.Answer.Equals("5 star")).SingleOrDefault();
                    //  var getScore = await dbContext.Elements.Where(p => p.QuestionId == item.QuestionId && p.Content.Equals("5 star")).SingleOrDefaultAsync();
                    scoreOfRating = ele.Score;
                    quest.ScoreOfRatingQuestion = (int)scoreOfRating;
                }

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

        public async Task<APIResponse<int>> GetNumberQuestionInForm(Guid formid)
        {
            APIResponse<int> response = new();
            var question = await dbContext.Questions.Where(p => p.FormId == formid && p.Status == true).ToListAsync();

            response.ToSuccessResponse(response.Data = question.Count, "Lấy số lượng câu hỏi thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetQuestionResponse>> GetQuestionById(Guid questionId)
        {
            APIResponse<GetQuestionResponse> response = new();
            var question = await dbContext.Questions.Where(p => p.QuestionId == questionId && p.Status == true).SingleOrDefaultAsync();
            if (question == null)
            {
                response.ToFailedResponse("Không có câu hỏi nào phù hợp theo yêu cầu hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var questiontype = await dbContext.Types.Where(p => p.TypeId == question.TypeId).SingleOrDefaultAsync();
            if (questiontype == null)
            {
                response.ToFailedResponse("Không có loại câu hỏi nào phù hợp theo yêu cầu ", StatusCodes.Status400BadRequest);
                return response;
            }

            Guid typeRatingAnswer = Guid.Parse("284a18e0-6d3b-4f71-83de-aa79513a3cd7");
            decimal scoreOfRating = 0;
            if (questiontype.TypeId == typeRatingAnswer)
            {
                var getScore = await dbContext.Elements.Where(p => p.QuestionId == question.QuestionId && p.Content.Equals("5 star")).SingleOrDefaultAsync();
                scoreOfRating = getScore.Score;
            }
            var mapq = _mapper.Map<GetQuestionResponse>(question);
            mapq.TypeName = questiontype.Name;
            mapq.TypeId = questiontype.TypeId;
            mapq.ScoreOfRatingQuestion = (int)scoreOfRating;
            List<GetElementResponse> listelement = new();
            var elementt = await dbContext.Elements.Where(p => p.QuestionId == question.QuestionId && p.Status == true).ToListAsync();
            List<GetElementResponse> listelementt = elementt.Select(
           x =>
           {
               return new GetElementResponse()
               {
                   ElementId = x.ElementId,
                   Answer = x.Content,
                   QuestionId = x.QuestionId,
                   Score = x.Score,
                   Status = x.Status,
               };
           }
           ).ToList();
            listelementt = listelementt.OrderBy(x => x.Answer).ToList();
            mapq.Element = listelementt;
            response.Data = mapq;
            response.ToSuccessResponse(response.Data, "Lấy chi tiết câu hỏi và câu trả lời thành công", StatusCodes.Status200OK);
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
            Guid type1Answer = Guid.Parse("283a18e0-6d3b-4f71-83de-aa79513a3cd7");
            Guid typeManyAnswer = Guid.Parse("285a18e0-6d3b-4f71-83de-aa79513a3cd7");
            Guid typeRatingAnswer = Guid.Parse("284a18e0-6d3b-4f71-83de-aa79513a3cd7");
            if (question.TypeId == type1Answer || question.TypeId == typeManyAnswer)
            {
                if (request.TypeId == typeRatingAnswer)
                {
                    response.ToFailedResponse("Không thể thay đổi kiểu câu hỏi bình chọn sang câu hỏi đánh giá ", StatusCodes.Status400BadRequest);
                    return response;
                }
            }
            if (question.TypeId == typeRatingAnswer)
            {
                if (request.TypeId != typeRatingAnswer)
                {
                    response.ToFailedResponse("Không thể thay đổi kiểu câu hỏi đánh giá sang câu hỏi bình chọn ", StatusCodes.Status400BadRequest);
                    return response;
                }
            }

            var questiontypeOfRequest = await dbContext.Types.Where(p => p.TypeId == request.TypeId).SingleOrDefaultAsync();
            if (questiontypeOfRequest == null)
            {
                response.ToFailedResponse("Không có loại câu hỏi nào phù hợp theo yêu cầu ", StatusCodes.Status400BadRequest);
                return response;
            }
            if (questiontypeOfRequest.TypeId == typeRatingAnswer)
            {
                question.Content = request.Content;
                if (request.TotalScore % 5 != 0)
                {
                    response.ToFailedResponse("Điểm phải chia hết cho 5", StatusCodes.Status400BadRequest);
                    return response;
                }
                var ListElementRating = await dbContext.Elements.Where(p => p.QuestionId == question.QuestionId && p.Status == true).ToListAsync();
                ListElementRating = ListElementRating.OrderBy(x => x.Content).ToList();
                var so = request.TotalScore / 5;
                int a = 1;
                foreach (var n in ListElementRating)
                {
                    n.Score = so * a;
                    dbContext.Elements.Update(n);
                    await dbContext.SaveChangesAsync();
                    a = a + 1;
                }
                dbContext.Questions.Update(question);
                await dbContext.SaveChangesAsync();
            }
            question.Content = request.Content;
            question.TypeId = questiontypeOfRequest.TypeId;
            dbContext.Questions.Update(question);
            await dbContext.SaveChangesAsync();

            var mapq = _mapper.Map<GetQuestionResponse>(question);
            mapq.TypeName = questiontypeOfRequest.Name;
            mapq.TypeId = questiontypeOfRequest.TypeId;
            List<GetElementResponse> listelement = new();
            if (request.TypeId != typeRatingAnswer)
            {
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
                        element.Score = item.Rate;
                        dbContext.Elements.Update(element);
                        await dbContext.SaveChangesAsync();
                    }

                    //var map = _mapper.Map<GetElementResponse>(element);
                    //listelement.Add(map);

                }
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
                   Score = x.Score,
                   Status = x.Status,
               };
           }
           ).ToList();
            listelementt = listelementt.OrderBy(x => x.Answer).ToList();
            mapq.Element = listelementt;
            response.Data = mapq;
            response.ToSuccessResponse(response.Data, "Cập nhật câu hỏi và câu trả lời thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
