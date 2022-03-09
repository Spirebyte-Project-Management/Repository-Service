using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Spirebyte.Services.Repositories.Application.Repositories.DTO;

public class TreeDto
{
    public TreeDto(string path)
    {
        Path = path;
        Files = new List<FileDto>();
        ParentCommit = null;
    }
    public TreeDto(Commit commit, List<Commit> ancestors, Tree gitTree, string path)
    {
        Id = gitTree.Id.ToString(7);
        Sha = gitTree.Sha;
        Path = path;

        Files = gitTree.Select(t => new FileDto(t, new Core.Entities.Commit(ancestors.Find(a => a[t.Path].Target.Sha == t.Target.Sha)))).ToList();
        ParentCommit = new Core.Entities.Commit(commit);
    }
    public string Id { get; set; }
    public string Sha { get; set; }
    public string Path { get; set; }
    public List<FileDto> Files { get; set; }
    
    public Core.Entities.Commit ParentCommit { get; set; }
}