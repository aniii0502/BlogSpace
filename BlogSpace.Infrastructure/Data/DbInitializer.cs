using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSpace.Domain.Entities;
using BlogSpace.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager
        )
        {
            // Apply migrations
            await context.Database.MigrateAsync();

            // -----------------------------
            // Seed Roles
            // -----------------------------
            if (!roleManager.Roles.Any())
            {
                foreach (var roleName in Enum.GetNames(typeof(UserRole)))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>
                        {
                            Id = Guid.NewGuid(),
                            Name = roleName,
                            NormalizedName = roleName.ToUpper(),
                        }
                    );
                }
            }

            // -----------------------------
            // Seed Admin User
            // -----------------------------
            if (!userManager.Users.Any())
            {
                var admin = new User
                {
                    FullName = "Admin User",
                    UserName = "admin@blogspace.com",
                    Email = "admin@blogspace.com",
                    Role = UserRole.Admin,
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            }

            // -----------------------------
            // Seed Categories
            // -----------------------------
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Technology" },
                    new Category { Id = Guid.NewGuid(), Name = "Science" },
                    new Category { Id = Guid.NewGuid(), Name = "Lifestyle" },
                    new Category { Id = Guid.NewGuid(), Name = "Education" },
                };
                await context.Categories.AddRangeAsync(categories);
            }

            // -----------------------------
            // Seed Tags
            // -----------------------------
            if (!context.Tags.Any())
            {
                var tags = new List<Tag>
                {
                    new Tag { Id = Guid.NewGuid(), Name = "C#" },
                    new Tag { Id = Guid.NewGuid(), Name = "ASP.NET" },
                    new Tag { Id = Guid.NewGuid(), Name = "Blazor" },
                    new Tag { Id = Guid.NewGuid(), Name = "EF Core" },
                    new Tag { Id = Guid.NewGuid(), Name = "PostgreSQL" },
                };
                await context.Tags.AddRangeAsync(tags);
            }

            await context.SaveChangesAsync();

            // -----------------------------
            // Seed 20–30 Users
            // -----------------------------
            if (userManager.Users.Count() < 25)
            {
                var random = new Random();
                for (int i = 1; i <= 25; i++)
                {
                    var user = new User
                    {
                        FullName = $"User {i}",
                        UserName = $"user{i}@blogspace.com",
                        Email = $"user{i}@blogspace.com",
                        Role = UserRole.User,
                    };
                    await userManager.CreateAsync(user, $"User@123{i}");
                }
            }

            var allUsers = userManager.Users.ToList();
            var allCategories = await context.Categories.ToListAsync();
            var allTags = await context.Tags.ToListAsync();

            // -----------------------------
            // Seed 100 Posts
            // -----------------------------
            if (!context.Posts.Any())
            {
                var random = new Random();
                var posts = new List<Post>();
                for (int i = 1; i <= 100; i++)
                {
                    var author = allUsers[random.Next(allUsers.Count)];
                    var category = allCategories[random.Next(allCategories.Count)];

                    var post = new Post
                    {
                        Title = $"Sample Post {i}",
                        Content = $"This is the content of sample post {i}.",
                        AuthorId = author.Id,
                        CategoryId = category.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    // Assign 1–3 random tags
                    var selectedTags = allTags
                        .OrderBy(x => random.Next())
                        .Take(random.Next(1, 4))
                        .ToList();
                    foreach (var tag in selectedTags)
                    {
                        post.PostTags.Add(new PostTag { Post = post, TagId = tag.Id });
                    }

                    posts.Add(post);
                }
                await context.Posts.AddRangeAsync(posts);
                await context.SaveChangesAsync();
            }

            var allPosts = await context.Posts.Include(p => p.PostTags).ToListAsync();

            // -----------------------------
            // Seed Likes & Comments
            // -----------------------------
            var random2 = new Random();
            foreach (var post in allPosts)
            {
                // Add 0–10 likes per post
                int likesCount = random2.Next(0, 11);
                var likedUsers = allUsers.OrderBy(x => random2.Next()).Take(likesCount).ToList();
                foreach (var user in likedUsers)
                {
                    if (!context.Likes.Any(l => l.PostId == post.Id && l.UserId == user.Id))
                    {
                        context.Likes.Add(
                            new Like
                            {
                                PostId = post.Id,
                                UserId = user.Id,
                                CreatedAt = DateTime.UtcNow,
                            }
                        );
                    }
                }

                // Add 0–5 comments per post
                int commentsCount = random2.Next(0, 6);
                var commentUsers = allUsers
                    .OrderBy(x => random2.Next())
                    .Take(commentsCount)
                    .ToList();
                foreach (var user in commentUsers)
                {
                    context.Comments.Add(
                        new Comment
                        {
                            PostId = post.Id,
                            AuthorId = user.Id,
                            Content = $"Sample comment by {user.FullName}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        }
                    );
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedAdminAsync(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager
        )
        {
            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Admin" });

            // Create default admin user
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User",
                    Role = Domain.Enums.UserRole.Admin,
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
