using System.Collections.Generic;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class Project
{
    public Project(string id)
    {
        Id = id;
    }

    public string Id { get; }
    public ICollection<string> Issues { get; set; }
}