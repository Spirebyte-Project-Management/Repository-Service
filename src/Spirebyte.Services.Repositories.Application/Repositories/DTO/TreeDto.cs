using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Spirebyte.Services.Repositories.Application.Repositories.DTO;

public class TreeDto
{
    public TreeDto(string path)
    {
        Path = path;
        TreeEntries = new List<TreeEntryDto>();
        ParentCommit = null;
    }

    public TreeDto(Commit commit, List<Commit> ancestors, Tree gitTree, string path)
    {
        Id = gitTree.Id.ToString(7);
        Sha = gitTree.Sha;
        Path = string.IsNullOrEmpty(path) ? "/" : path;

        TreeEntries = getEntriesWithHistory(gitTree, ancestors);
        ParentCommit = new Core.Entities.Commit(commit);
    }

    public string Id { get; set; }
    public string Sha { get; set; }
    public string Path { get; set; }
    public List<TreeEntryDto> TreeEntries { get; set; }

    public Core.Entities.Commit ParentCommit { get; set; }

    private List<TreeEntryDto> getEntriesWithHistory(Tree gitTree, List<Commit> ancestors)
    {
        return gitTree.Select(t =>
                new TreeEntryDto(
                    t,
                    new Core.Entities.Commit(
                        ancestors.Find(a =>
                            a[t.Path] != null && a[t.Path].Target.Sha == t.Target.Sha)
                    )
                )
            )
            .OrderByDescending(e => e.Mode == Mode.Directory)
            .ThenBy(e => e.Path)
            .ToList();
    }
}