using BlogSpace.Domain.DTOs;

namespace BlogSpace.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDTO> AddCommentAsync(Guid postId, Guid authorId, CommentDTO commentDto);
        Task<CommentDTO> UpdateCommentAsync(Guid commentId, Guid authorId, CommentDTO commentDto);
        Task DeleteCommentAsync(Guid commentId, Guid authorId);
        Task<IEnumerable<CommentDTO>> GetCommentsByPostAsync(Guid postId);
    }
}
