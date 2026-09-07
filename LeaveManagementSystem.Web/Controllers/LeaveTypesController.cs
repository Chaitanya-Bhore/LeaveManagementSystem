
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using System.Linq;
using AutoMapper;
using LeaveManagementSystem.Web.Services;
using LeaveManagementSystem.Web.Common;



[Authorize(Roles = Roles.Administrator)]

public class LeaveTypesController(ILeaveTypesService _leaveTypesService) : Controller
{

    private const string NameExistsValidationMessage
        = "A leave type with the same name already exists in database.";




    // GET: LEAVETYPES
    public async Task<IActionResult> Index()
    {
        var viewData = await _leaveTypesService.GetAllLeaveTypes();
        return View(viewData);
    }
    //this is abstraction

    // GET: LEAVETYPES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leavetype = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);

        if (leavetype == null)
        {
            return NotFound();
        }

        return View(leavetype);
    }

    // GET: LEAVETYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LEAVETYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
    {
        if (!string.IsNullOrWhiteSpace(leaveTypeCreate?.Name))
        {
            if (await _leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(
                    "Name",
                    "A leave type with the same name already exists in database.");
            }
        }

        if (ModelState.IsValid)
        {
            await _leaveTypesService.Create(leaveTypeCreate);
            return RedirectToAction(nameof(Index));
        }

        return View(leaveTypeCreate);
    }





    // GET: LEAVETYPES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leavetype = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);

        if (leavetype == null)
        {
            return NotFound();
        }

        return View(leavetype);

    }

    //"Receive the ID of the LeaveType the user wants to edit.
    //Find that LeaveType in the database using EF Core.
    //If it doesn't exist, return 404.
    //If it exists, send the object to the Edit view so the user can modify it."

    // POST: LEAVETYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
    {
        if (id != leaveTypeEdit.Id)
        {
            return NotFound();
        }

        // Adding custom validation and model state error

        if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
        {
            ModelState.AddModelError("Name", NameExistsValidationMessage);
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _leaveTypesService.Edit(leaveTypeEdit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
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
        return View(leaveTypeEdit);
    }

    // GET: LEAVETYPES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }


        return View(leaveType);
    }

    // POST: LEAVETYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        await _leaveTypesService.Remove(id.Value);
        return RedirectToAction(nameof(Index));

    }

}


        