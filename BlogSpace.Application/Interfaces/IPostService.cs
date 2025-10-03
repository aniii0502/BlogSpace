using BlogSpace.Domain.DTOs;

namespace BlogSpace.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO> CreatePostAsync(PostDTO postDto, Guid authorId);
        Task<PostDTO> UpdatePostAsync(Guid postId, PostDTO postDto, Guid authorId);
        Task DeletePostAsync(Guid postId, Guid authorId);
        Task<PostDTO> GetPostByIdAsync(Guid postId);
        Task<IEnumerable<PostDTO>> GetAllPostsAsync();
        Task<IEnumerable<PostDTO>> GetPostsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<PostDTO>> GetPostsByTagAsync(Guid tagId);
        Task<IEnumerable<PostDTO>> SearchPostsAsync(string query);
    }
}
