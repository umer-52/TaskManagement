using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Common;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
        => await _context.Tasks
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int userId,
        TaskQueryParameters query,
        bool isAdmin)
    {
        // Base query with navigation for AssignedUserName in DTO
        IQueryable<TaskItem> dbQuery = _context.Tasks
            .Include(t => t.User)
            .AsNoTracking();

        // Non-admins only see their own tasks (LINQ filtering)
        if (!isAdmin)
            dbQuery = dbQuery.Where(t => t.UserId == userId);

        if (query.IsCompleted.HasValue)
            dbQuery = dbQuery.Where(t => t.IsCompleted == query.IsCompleted.Value);

        if (query.Priority.HasValue)
            dbQuery = dbQuery.Where(t => t.Priority == query.Priority.Value);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLower();
            dbQuery = dbQuery.Where(t =>
                t.Title.ToLower().Contains(term) ||
                t.Description.ToLower().Contains(term));
        }

        var totalCount = await dbQuery.CountAsync();

        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 50);

        var items = await dbQuery
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
