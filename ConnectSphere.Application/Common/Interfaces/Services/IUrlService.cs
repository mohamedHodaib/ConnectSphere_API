namespace ConnectSphere.Application.Interfaces
{
    public interface IUrlService
    {
        string? GetAbsoluteUrl(string? relativePath);
    }
}
