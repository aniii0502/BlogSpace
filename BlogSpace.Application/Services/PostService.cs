using BlogSpace.Application.Interfaces;
using BlogSpace.Common.Exceptions;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Application.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // CREATE POST
        // -----------------------------
        public async Task<PostDTO> CreatePostAsync(PostDTO postDto, Guid authorId)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = postDto.Title,
                Content = postDto.Content,
                AuthorId = authorId,
                CategoryId = postDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Add tags if provided
            if (postDto.TagIds != null && postDto.TagIds.Any())
            {
                post.PostTags = postDto
                    .TagIds.Select(tagId => new PostTag { PostId = post.Id, TagId = tagId })
                    .ToList();
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return await GetPostByIdAsync(post.Id);
        }

        // -----------------------------
        // UPDATE POST
        // -----------------------------
        public async Task<PostDTO> UpdatePostAsync(Guid postId, PostDTO postDto, Guid authorId)
        {
            var post = await _context
                .Posts.Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId != authorId)
                throw new ForbiddenException("You are not the author of this post.");

            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.CategoryId = postDto.CategoryId;
            post.UpdatedAt = DateTime.UtcNow;

            // Update tags
            if (postDto.TagIds != null)
            {
                // Remove old tags
                _context.PostTags.RemoveRange(post.PostTags);

                // Add new tags
                post.PostTags = postDto
                    .TagIds.Select(tagId => new PostTag { PostId = post.Id, TagId = tagId })
                    .ToList();
            }

            await _context.SaveChangesAsync();
            return await GetPostByIdAsync(post.Id);
        }

        // -----------------------------
        // DELETE POST
        // -----------------------------
        public async Task DeletePostAsync(Guid postId, Guid authorId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                throw new NotFoundException("Post not found.");
            if (post.AuthorId != authorId)
                throw new ForbiddenException("You are not the author of this post.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        // -----------------------------
        // GET POST BY ID
        // -----------------------------
        public async Task<PostDTO> GetPostByIdAsync(Guid postId)
        {
            var post = await _context
                .Posts.Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new NotFoundException("Post not found.");

            return new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.FullName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                TagIds = post.PostTags.Select(pt => pt.TagId).ToList(),
                TagNames = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            };
        }

        // -----------------------------
        // GET ALL POSTS
        // -----------------------------
        public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
        {
            var posts = await _context
                .Posts.Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(post => new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.FullName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                TagIds = post.PostTags.Select(pt => pt.TagId).ToList(),
                TagNames = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            });
        }

        // -----------------------------
        // GET POSTS BY CATEGORY
        // -----------------------------
        public async Task<IEnumerable<PostDTO>> GetPostsByCategoryAsync(Guid categoryId)
        {
            var posts = await _context
                .Posts.Where(p => p.CategoryId == categoryId)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(post => new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.FullName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                TagIds = post.PostTags.Select(pt => pt.TagId).ToList(),
                TagNames = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            });
        }

        // -----------------------------
        // GET POSTS BY TAG
        // -----------------------------
        public async Task<IEnumerable<PostDTO>> GetPostsByTagAsync(Guid tagId)
        {
            var posts = await _context
                .Posts.Where(p => p.PostTags.Any(pt => pt.TagId == tagId)) // filter posts that have the tag
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(post => new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.FullName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                TagIds = post.PostTags.Select(pt => pt.TagId).ToList(),
                TagNames = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            });
        }

        // -----------------------------
        // SEARCH POSTS
        // -----------------------------
        public async Task<IEnumerable<PostDTO>> SearchPostsAsync(string query)
        {
            var posts = await _context
                .Posts.Where(p => p.Title.Contains(query) || p.Content.Contains(query))
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(post => new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = post.Author.FullName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name,
                TagIds = post.PostTags.Select(pt => pt.TagId).ToList(),
                TagNames = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
            });
        }
    }
}
