﻿using Capstone_VotingSystem.Models.ResponseModels.TeacherResponse;
namespace Capstone_VotingSystem.Repositories.TeacherRepo
{
    public interface ITeacherRepositories
    {
        Task<IEnumerable<GetListTeacherResponse>> GetListTeacher();
    }
}