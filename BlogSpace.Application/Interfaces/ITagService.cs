using BlogSpace.Domain.DTOs;

namespace BlogSpace.Application.Interfaces
{
    public interface ITagService
    {
        Task<TagDTO> CreateTagAsync(TagDTO tagDto);
        Task<IEnumerable<TagDTO>> GetAllTagsAsync();
        Task<TagDTO> GetTagByIdAsync(Guid tagId);
        Task<TagDTO> UpdateTagAsync(Guid tagId, TagDTO tagDto);
        Task DeleteTagAsync(Guid tagId);
    }
}
