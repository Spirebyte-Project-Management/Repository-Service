using LibGit2Sharp;
using Commit = Spirebyte.Services.Repositories.Core.Entities.Commit;

namespace Spirebyte.Services.Repositories.Application.Repositories.DTO;

public class TreeEntryDto
{
    public TreeEntryDto(TreeEntry treeEntry, Commit commit)
    {
        Name = treeEntry.Name;
        Path = treeEntry.Path;
        Mode = treeEntry.Mode;
        Commit = commit;
    }
    public string Name { get; set; }
    public string Path { get; set; }
    public Mode Mode { get; set; }
    
    public Commit Commit { get; set; }
}