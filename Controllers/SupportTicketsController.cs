using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AiHelpdeskAgent.Data;
using AiHelpdeskAgent.Models;

namespace AiHelpdeskAgent.Controllers;
public class SupportTicketsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? status)
    {
        var query = db.SupportTickets.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        ViewBag.Search = search; ViewBag.Status = status;
        return View(await query.OrderByDescending(x => x.CreatedAt).ToListAsync());
    }
    public IActionResult Create() => View(new SupportTicket());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupportTicket item)
    { if (!ModelState.IsValid) return View(item); db.SupportTickets.Add(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record created successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) => id is null ? NotFound() : (await db.SupportTickets.FindAsync(id) is SupportTicket item ? View(item) : NotFound());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SupportTicket item)
    { if (id != item.Id) return NotFound(); if (!ModelState.IsValid) return View(item); db.Update(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record updated successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Delete(int? id) => id is null ? NotFound() : (await db.SupportTickets.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id) is SupportTicket item ? View(item) : NotFound());
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { var item = await db.SupportTickets.FindAsync(id); if (item is not null) { db.SupportTickets.Remove(item); await db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
}
