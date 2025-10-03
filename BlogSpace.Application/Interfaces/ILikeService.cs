using BlogSpace.Domain.DTOs;

namespace BlogSpace.Application.Interfaces
{
    public interface ILikeService
    {
        Task<LikeDTO> AddLikeAsync(Guid postId, Guid userId);
        Task RemoveLikeAsync(Guid postId, Guid userId);
        Task<int> GetLikesCountAsync(Guid postId);
        Task<bool> HasUserLikedAsync(Guid postId, Guid userId);
    }
}
