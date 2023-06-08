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
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (checkUser == null)
            {
                response.ToFailedResponse("UserName không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Form form = new Form();
            {
                form.FormId = id;
                form.UserName=request.UserName;
                form.Name = request.Name;
                form.Visibility = request.Visibility;
            }
            await dbContext.Forms.AddAsync(form);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetFormResponse>(form);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetFormResponse>>> GetAllForm()
        {
            APIResponse<IEnumerable<GetFormResponse>> response = new();
            var form = await dbContext.Forms.Where(p=>p.Visibility==true).ToListAsync();
            IEnumerable<GetFormResponse> result = form.Select(
                x =>
                {
                    return new GetFormResponse()
                    {
                        FormId = x.FormId,
                        Visibility = x.Visibility,
                        Name = x.Name,
                        UserName = x.UserName,
                    };
                }
                ).ToList();
            response.Data = result;
            if (response.Data==null)
            {
                response.ToFailedResponse("Không có Form nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetFormResponse>> UpdateForm(Guid id, UpdateFormByUser request)
        {
            APIResponse<GetFormResponse> response = new();
            var cam = await dbContext.Users.SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (cam == null)
            {
                response.ToFailedResponse("User không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == id);
            if (checkform == null)
            {
                response.ToFailedResponse("Form không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            checkform.Name = request.Name;
            checkform.Visibility = request.Visibility;

            dbContext.Forms.Update(checkform);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetFormResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
