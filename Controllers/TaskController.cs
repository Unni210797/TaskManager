using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Task_Management_System.Data;
using Task_Management_System.Models;
using Task_Management_System.ViewModel;

namespace Task_Management_System.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        string username = "";

        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TaskController(UserManager<IdentityUser> userManager, ApplicationDBContext context)
        {
            _context = context;
            _userManager = userManager;

        }
        List<TaskInfos> filterTaskByStatus = new List<TaskInfos>();
        public async Task<IActionResult> Index(string status, string dueDate)
        {
            username = User.Identity.Name!;
            if (status==null  && dueDate==null)
            {

                FilteredTask.filteredTasks = await _context.TaskInfos.FromSqlRaw<TaskInfos>("GetFilteredTaskByName {0}", username).ToListAsync();
                if (FilteredTask.filteredTasks.Count == 0)
                {
                    return RedirectToAction("Create");
                }

                return View(FilteredTask.filteredTasks);
            }
            else
            {
               
                try
                {

                    filterTaskByStatus = FilteredTask.filteredTasks;
                    if(dueDate!=null && status==null)
                    {
                        filterTaskByStatus = filterTaskByStatus.Where(u=>u.DueDate== DateOnly.Parse(dueDate)).ToList();
                    }
                    else if (dueDate == null && status !=null)
                    {
                        filterTaskByStatus = filterTaskByStatus.Where(u => u.Status == status).ToList();
                    }
                    else if(dueDate != null && status != null)
                    { 
                        filterTaskByStatus = filterTaskByStatus.Where(u=>u.Status==status && u.DueDate == DateOnly.Parse(dueDate)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_TaskTablePartial", filterTaskByStatus);
                }

                return View(filterTaskByStatus);
            }
           
        }
        //public async Task<IActionResult> FilterTasks()
        //{
        //    List<TaskInfos> filterTaskByStatus = new List<TaskInfos>();
        //    try
        //    {

        //        filterTaskByStatus = FilteredTask.filteredTasks;
        //        if (dueDate == null && status != "All")
        //        {

        //            filterTaskByStatus = filterTaskByStatus.Where(u => u.Status == status).ToList();
        //        }
        //        else
        //        {
        //            filterTaskByStatus = filterTaskByStatus.Where(u => u.DueDate == DateOnly.Parse(dueDate)).ToList();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return View(filterTaskByStatus);
        //}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskInfo = await _context.TaskInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskInfo == null)
            {
                return NotFound();
            }

            return View(taskInfo);
        }

        public IActionResult Create()
        {
            string username = User.Identity.Name;

            var viewModel = new TaskViewModel
            {
                Username = username!
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Project,Description,DueDate,CreatedDate,Status")] TaskViewModel taskViewModel)
        {

            if (ModelState.IsValid)
            {
                var taskInfos = new TaskInfos
                {
                    UserName = taskViewModel.Username,
                    Project = taskViewModel.Project,
                    Description = taskViewModel.Description,
                    DueDate = taskViewModel.DueDate,
                    CreatedDate = taskViewModel.CreatedDate,
                    Status = taskViewModel.Status
                };

                _context.Add(taskInfos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskInfos = await _context.TaskInfos.FindAsync(id);
            if (taskInfos == null)
            {
                return NotFound();
            }
            return View(taskInfos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Project,Description,DueDate,CreatedDate,Status")] TaskInfos taskInfos)
        {
            if (id != taskInfos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskInfos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskInfosExists(taskInfos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskInfos);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskInfos = await _context.TaskInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskInfos == null)
            {
                return NotFound();
            }

            return View(taskInfos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskInfos = await _context.TaskInfos.FindAsync(id);
            if (taskInfos != null)
            {
                _context.TaskInfos.Remove(taskInfos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskInfosExists(int id)
        {
            return _context.TaskInfos.Any(e => e.Id == id);
        }


    }
    public static class FilteredTask
    {
       public static List<TaskInfos> filteredTasks { get; set; }
    }
}

