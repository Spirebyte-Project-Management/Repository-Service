using LibGit2Sharp;
using MimeTypes;

namespace Spirebyte.Services.Repositories.Application.Repositories.DTO;

public class BlobDto
{
    public BlobDto(Blob blob, Commit commit, string path)
    {
        Id = blob.Id.ToString();
        Sha = blob.Sha;

        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Extension = System.IO.Path.GetExtension(path);
        Size = blob.Size;

        MimeType = MimeTypeMap.GetMimeType(Extension);
        IsBinary = blob.IsBinary;
        Data = blob.GetContentText();

        ParentCommit = new Core.Entities.Commit(commit);
    }

    public string Id { get; set; }
    public string Sha { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public string MimeType { get; set; }
    public bool IsBinary { get; set; }
    public string Data { get; set; }

    public Core.Entities.Commit ParentCommit { get; set; }
}