using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CNSVM.Pages.Groups
{
    
    public class CreateModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public CreateModel(CnsvmDbContext db) 
        {
            _cnsvmDbContext = db;
        }
        [BindProperty]
        public MedicalGroup Group { get; set; }
        public IEnumerable<User> Doctors { get; set; }
        

        public async Task<IActionResult> OnGetAsync()
        {
            Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
            return Page();
            
        }
        public async Task<IActionResult> OnPost(string ids)
        {
            if (ids == null)
            {
                ModelState.AddModelError("Group","No puedes crear un grupo sin integrantes");
				Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
				return Page();
			}
            if (!ModelState.IsValid)
            {
				Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
				return Page();
            }
            try
            {
				var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
				int doctorId = int.Parse(doctorIdClaim.Value);
                Group.CreatedBy = doctorId;
                Group.CreatedAt = DateTime.Now;
                Group.Status = 'A';
                await _cnsvmDbContext.MedicalGroup.AddAsync(Group);
				await _cnsvmDbContext.SaveChangesAsync();

				int groupId = Group.Id;
				string[] IdsDoctor = ids.Split(',');
                
                foreach (var id in IdsDoctor)
                {
                    await _cnsvmDbContext.DoctorGroup.AddAsync(new DoctorGroup()
                    {
                        GroupId = groupId,
                        UserId = int.Parse(id)
                    });
                }

                await _cnsvmDbContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //string [] IdsDoctor = ids.Split(',');
            return Page();
        }
    }
}
