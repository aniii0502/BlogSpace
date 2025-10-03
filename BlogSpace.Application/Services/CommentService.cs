using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Common.Exceptions;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // ADD COMMENT
        // -----------------------------
        public async Task<CommentDTO> AddCommentAsync(Guid postId, Guid authorId, CommentDTO commentDto)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) throw new NotFoundException("Post not found.");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                AuthorId = authorId,
                Content = commentDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return await GetCommentByIdAsync(comment.Id);
        }

        // -----------------------------
        // UPDATE COMMENT
        // -----------------------------
        public async Task<CommentDTO> UpdateCommentAsync(Guid commentId, Guid authorId, CommentDTO commentDto)
        {
            var comment = await _context.Comments.Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null) throw new NotFoundException("Comment not found.");
            if (comment.AuthorId != authorId) throw new ForbiddenException("You are not the author of this comment.");

            comment.Content = commentDto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetCommentByIdAsync(comment.Id);
        }

        // -----------------------------
        // DELETE COMMENT
        // -----------------------------
        public async Task DeleteCommentAsync(Guid commentId, Guid authorId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null) throw new NotFoundException("Comment not found.");
            if (comment.AuthorId != authorId) throw new ForbiddenException("You are not the author of this comment.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        // -----------------------------
        // GET COMMENTS BY POST
        // -----------------------------
        public async Task<IEnumerable<CommentDTO>> GetCommentsByPostAsync(Guid postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return comments.Select(c => new CommentDTO
            {
                Id = c.Id,
                PostId = c.PostId,
                AuthorId = c.AuthorId,
                AuthorName = c.Author.FullName,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }

        // -----------------------------
        // HELPER: GET COMMENT BY ID
        // -----------------------------
        private async Task<CommentDTO> GetCommentByIdAsync(Guid commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null) throw new NotFoundException("Comment not found.");

            return new CommentDTO
            {
                Id = comment.Id,
                PostId = comment.PostId,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author.FullName,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
