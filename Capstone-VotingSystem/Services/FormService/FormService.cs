using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;
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

        public async Task<APIResponse<string>> ApproveForm(Guid id)
        {
            APIResponse<string> response = new();
            var form = await dbContext.Forms.Where(p => p.Status == true && p.IsApprove == false && p.FormId == id).SingleOrDefaultAsync();
            if (form == null)
            {
                response.ToFailedResponse("Biểu mẫu đã bị xóa hoặc đã được duyệt", StatusCodes.Status400BadRequest);
                return response;
            }
            //check condition form 
            var checkQuestion = await dbContext.Questions.Where(p => p.Status == true && p.FormId == id).ToListAsync();
            if (checkQuestion.Count <= 0)
            {
                response.ToFailedResponse("Biểu mẫu phải có ít nhất 1 câu hỏi", StatusCodes.Status400BadRequest);
                return response;
            }
            form.IsApprove = true;
            dbContext.Forms.Update(form);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Duyệt biểu mẫu thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetFormResponse>> CreateForm(CreateFormRequest request)
        {
            APIResponse<GetFormResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId && c.Status == true);
            if (checkUser == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcate = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (checkcate == null)
            {
                response.ToFailedResponse("Thể loại không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Hiển thị không đúng định dạng!! (công khai hoặc riêng tư)", StatusCodes.Status400BadRequest);
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
                form.IsApprove = false;
                form.CategoryId = request.CategoryId;

            }
            await dbContext.Forms.AddAsync(form);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetFormResponse>(form);
            response.ToSuccessResponse("Tạo biểu mẫu thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteForm(Guid formId, DeleteFormRequest request)
        {
            APIResponse<String> response = new();
            var user = await dbContext.Accounts.SingleOrDefaultAsync(c => c.UserName == request.UserId && c.Status == true);
            if (user == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == formId && c.Status == true);
            if (checkform == null)
            {
                response.ToFailedResponse("Biểu mẫu không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkrole = await dbContext.Roles.Where(p => p.RoleId == user.RoleId).SingleOrDefaultAsync();
            if (checkform.UserId != request.UserId && checkrole.Name != "admin")
            {
                response.ToFailedResponse("Bạn không đủ quyền để xóa chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkform.IsApprove == true)
            {
                response.ToFailedResponse("Không thể thực hiện khi biểu mẫu đã được duyệt", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkrole.Name == "admin")
            {
                TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
                Guid idNoti = Guid.NewGuid();
                Notification noti = new Notification()
                {
                    NotificationId = idNoti,
                    Title = "Thông báo biểu mẫu",
                    Message = "Biểu mẫu - " + checkform.Name + "của bạn vừa bị xóa bởi admin vì vi phạm điều lệ, vui lòng liên hệ để biết thêm thông tin chi tiết.",
                    CreateDate = currentDateTimeVn,
                    IsRead = false,
                    Status = true,
                    Username = checkform.UserId,
                    CampaignId = null,
                };
                await dbContext.Notifications.AddAsync(noti);
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
            var form = await dbContext.Forms.Where(p => p.Visibility == "public" && p.Status == true && p.IsApprove == true).ToListAsync();
            if (form == null)
            {
                response.ToFailedResponse("Không có biểu mẫu nào phù hợp", StatusCodes.Status400BadRequest);
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
                question.Content = item.Content;
                question.TypeId = checkType.TypeId;
                result.Add(question);
                var listElement = await dbContext.Elements.Where(p => p.Status == true && p.QuestionId == item.QuestionId).ToListAsync();
                List<ListElementQuestionResponse> resultElement = new List<ListElementQuestionResponse>();
                foreach (var element in listElement)
                {
                    var elements = new ListElementQuestionResponse();
                    elements.ElementId = element.ElementId;
                    elements.Answer = element.Content;
                    elements.Rate = element.Score;
                    elements.Status = element.Status;
                    resultElement.Add(elements);
                }
                question.Elements = resultElement;
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
                       IsApprove = x.IsApprove,
                       UserId = x.UserId,
                       Visibility = x.Visibility,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetFormResponse>>> GetFormNeedApprove()
        {
            APIResponse<IEnumerable<GetFormResponse>> response = new();
            var getAll = await dbContext.Forms.Where(p => p.IsApprove == false && p.Status == true)
                .ToListAsync();
            if (getAll.Count == 0)
            {
                response.ToFailedResponse("Không có biểu mẫu nào cần được duyệt", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetFormResponse> result = getAll.Select(
               x =>
               {
                   return new GetFormResponse()
                   {
                       CategoryId = x.CategoryId,
                       FormId = x.FormId,
                       Name = x.Name,
                       IsApprove = x.IsApprove,
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
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == id && c.Status == true);
            if (checkform == null)
            {
                response.ToFailedResponse("Biểu mẫu này không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkus = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == id && c.UserId == request.UserId);
            if (checkus == null)
            {
                response.ToFailedResponse("Bạn không có đủ quyền hạn để thay đổi biểu mẫu này", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcate = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (checkcate == null)
            {
                response.ToFailedResponse("Thể loại không tồn tại hoặc chưa có", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkform.IsApprove == true)
            {
                response.ToFailedResponse("Không thể thay đổi khi biểu mẫu đã được duyệt", StatusCodes.Status400BadRequest);
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
