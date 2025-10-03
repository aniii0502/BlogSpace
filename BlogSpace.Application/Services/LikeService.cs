using BlogSpace.Application.Interfaces;
using BlogSpace.Common.Exceptions;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // ADD LIKE
        // -----------------------------
        public async Task<LikeDTO> AddLikeAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");

            var existingLike = await _context.Likes.FirstOrDefaultAsync(l =>
                l.PostId == postId && l.UserId == userId
            );
            if (existingLike != null)
                throw new ApplicationException("User has already liked this post.");

            var like = new Like
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return new LikeDTO
            {
                Id = like.Id,
                PostId = like.PostId,
                UserId = like.UserId,
                CreatedAt = like.CreatedAt,
            };
        }

        // -----------------------------
        // REMOVE LIKE
        // -----------------------------
        public async Task RemoveLikeAsync(Guid postId, Guid userId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(l =>
                l.PostId == postId && l.UserId == userId
            );
            if (like == null)
                throw new NotFoundException("Like not found.");

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        // -----------------------------
        // GET LIKES COUNT
        // -----------------------------
        public async Task<int> GetLikesCountAsync(Guid postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

        // -----------------------------
        // CHECK IF USER LIKED
        // -----------------------------
        public async Task<bool> HasUserLikedAsync(Guid postId, Guid userId)
        {
            return await _context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
        }
    }
}
