using System;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;

public interface IBranchRequestStorage
{
    void SetBranch(Guid referenceId, Branch branch);
    Branch GetBranch(Guid referenceId);
}