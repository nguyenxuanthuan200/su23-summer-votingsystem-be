using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.TypeRequest;
using Capstone_VotingSystem.Models.ResponseModels.TypeResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.TypeService
{
    public class TypeService : ITypeService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public TypeService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            _mapper = mapper;
        }
        public async Task<APIResponse<TypeResponse>> CreateType(CreateTypeRequest request)
        {
            APIResponse<TypeResponse> response = new();
           
            var checkcate = await  dbContext.Types.Where(x=>x.Name.ToUpper().Equals(request.Name.ToUpper())).SingleOrDefaultAsync();
            if (checkcate != null)
            {
                response.ToFailedResponse("Name Type đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Entities.Type type = new Entities.Type();
            {
                type.TypeId = id;
                type.Name = request.Name;
                type.Description = request.Description;

            }
            await dbContext.Types.AddAsync(type);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<TypeResponse>(type);
            response.ToSuccessResponse("Tạo Type thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<TypeResponse>>> GetListType()
        {
            APIResponse<IEnumerable<TypeResponse>> response = new();
            var type = await dbContext.Types.ToListAsync();
            IEnumerable<TypeResponse> result = type.Select(x =>
            {
                return new TypeResponse()
                {
                    TypeId = x.TypeId,
                    Name = x.Name,
                    Description = x.Description,
                };
            }).ToList();
                        response.ToSuccessResponse(response.Data= result, "Lấy danh sách Type thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<TypeResponse>> UpdateType(Guid id,UpdateTypeRequest request)
        {
            APIResponse<TypeResponse> response = new();
            var typecheck = await dbContext.Types.SingleOrDefaultAsync(c => c.TypeId == id);
            if (typecheck == null)
            {
                response.ToFailedResponse("Type không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var typeNameCheck = await dbContext.Types.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim().Equals(request.Name.ToUpper().Trim()));
            if (typeNameCheck!=null)
            {
                response.ToFailedResponse("Type Name đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            

            typecheck.Name = request.Name;
            typecheck.Description = request.Description;
            dbContext.Types.Update(typecheck);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<TypeResponse>(typecheck);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
