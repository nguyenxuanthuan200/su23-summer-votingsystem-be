using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Models.ResponseModels.FormResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.FormService
{
    public class FormService : IFormService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;
        public FormService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetFormResponse>> CreateForm(CreateFormRequest request)
        {
            APIResponse<GetFormResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId);
            if (checkUser == null)
            {
                response.ToFailedResponse("UserName không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcate = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (checkcate == null)
            {
                response.ToFailedResponse("Category không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Form form = new Form();
            {
                form.FormId = id;
                form.UserId = request.UserId;
                form.Name = request.Name;
                form.Visibility = request.Visibility;
                form.Status = true;
                form.CategoryId = request.CategoryId;

            }
            await dbContext.Forms.AddAsync(form);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetFormResponse>(form);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteForm(Guid formId, DeleteFormRequest request)
        {
            APIResponse<String> response = new();
            var cam = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId && c.Status == true);
            if (cam == null)
            {
                response.ToFailedResponse("User không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == formId && c.Status == true);
            if (checkform == null)
            {
                response.ToFailedResponse("Form không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkus = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == formId && c.UserId == request.UserId);
            if (checkus == null)
            {
                response.ToFailedResponse("UserName này không phải người tạo Form", StatusCodes.Status400BadRequest);
                return response;
            }
            checkform.Status = false;
            dbContext.Forms.Update(checkform);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetFormResponse>>> GetAllForm()
        {
            APIResponse<IEnumerable<GetFormResponse>> response = new();
            var form = await dbContext.Forms.Where(p => p.Visibility == "public" && p.Status == true).ToListAsync();
            if (form == null)
            {
                response.ToFailedResponse("Không có Form nào", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetFormResponse> result = form.Select(
                x =>
                {
                    return new GetFormResponse()
                    {
                        FormId = x.FormId,
                        Name = x.Name,
                        UserId = x.UserId,
                        Visibility = x.Visibility,
                        CategoryId = x.CategoryId,
                    };
                }
                ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetListQuestionFormResponse>> GetFormById(Guid formId)
        {
            APIResponse<GetListQuestionFormResponse> response = new();
            var form = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == formId && c.Status == true);
            if (form == null)
            {
                response.ToFailedResponse("Form không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var map = _mapper.Map<GetListQuestionFormResponse>(form);
            var listQuestion = await dbContext.Questions.Where(p => p.Status == true && p.FormId == form.FormId).ToListAsync();
            List<GetListQuestionResponse> result = new List<GetListQuestionResponse>();
            foreach (var item in listQuestion)
            {
                var checkType = await dbContext.Types.Where(p => p.TypeId == item.TypeId).SingleOrDefaultAsync();
                var question = new GetListQuestionResponse();
                question.QuestionId = item.QuestionId;
                question.Title = item.Title;
                question.Content = item.Content;
                question.TypeId = checkType.TypeId;
                result.Add(question);
            }
            map.Questions = result;
            response.Data = map;
            response.ToSuccessResponse("Yêu cầu thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetFormResponse>>> GetFormByUserId(string id)
        {
            APIResponse<IEnumerable<GetFormResponse>> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == id && p.Status == true)
               .SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("User không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var getById = await dbContext.Forms.Where(p => p.UserId == id && p.Status == true)
                .ToListAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Form không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetFormResponse> result = getById.Select(
               x =>
               {
                   return new GetFormResponse()
                   {
                       CategoryId = x.CategoryId,
                       FormId = x.FormId,
                       Name = x.Name,
                       UserId = x.UserId,
                       Visibility = x.Visibility,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetFormResponse>> UpdateForm(Guid id, UpdateFormByUser request)
        {
            APIResponse<GetFormResponse> response = new();
            var cam = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId && c.Status == true);
            if (cam == null)
            {
                response.ToFailedResponse("User không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == id && c.Status == true);
            if (checkform == null)
            {
                response.ToFailedResponse("Form không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkus = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == id && c.UserId == request.UserId);
            if (checkus == null)
            {
                response.ToFailedResponse("UserName này không phải người tạo Form", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcate = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (checkcate == null)
            {
                response.ToFailedResponse("Category không tồn tại hoặc chưa có", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }

            checkform.Name = request.Name;
            checkform.Visibility = request.Visibility;
            checkform.CategoryId = request.CategoryId;
            dbContext.Forms.Update(checkform);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetFormResponse>(checkform);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
