using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using VeriMedCNS.Data;
using VeriMedCNS.Models;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace VeriMedCNS.Pages.Patients
{
    public class VoteModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        public VoteModel(CnsvmDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int RequestId { get; set; }

        [BindProperty]
        public string Vote { get; set; }

        [BindProperty]
        public string VoteReazon { get; set; }

        public int DoctorId { get; set; }
        public MedicationRequest MedicationRequest { get; set; }
        //public IEnumerable<Votes> Votes { get; set; }
        public async Task OnGet(int id)
        {
            RequestId = id;
            DoctorId = 1;//int.Parse(HttpContext.Request.Cookies["DoctorId"] ?? "0");

            MedicationRequest = await _context.MedicationRequest.FindAsync(id);
            //Votes = await _context.Votes.Where(v=>v.RequestId == id).ToListAsync();
        }

        public async Task<IActionResult> OnPost(int RequestId)
        {
            // Recuperar el ID del médico desde las cookies
            DoctorId = 1;//int.Parse(HttpContext.Request.Cookies["DoctorId"] ?? "0");

            if (DoctorId == 0)
            {
                ModelState.AddModelError("", "No se pudo obtener el ID del médico.");
                return Page();
            }

            try
            {
                var newVote = new Votes
                {
                    RequestId = RequestId,
                    DoctorId = DoctorId,
                    Vote = Vote == "yes" ? "A" : "R", // Cambia a Aprobado (A) si el voto es "Sí", o Pendiente (P) si es "No"
                    VoteDate = DateTime.Now,
                    VoteReazon = Vote == "no" ? VoteReazon : null // Asigna la justificación solo si el voto es "No"
                };
                await _context.Votes.AddAsync(newVote);
                if (newVote.Vote == "R")
                {
                    var medicationRequest = _context.MedicationRequest.FirstOrDefault(mr => mr.RequestId == newVote.RequestId);
                    if (medicationRequest != null)
                    {
                        medicationRequest.Status = 'R';
                        _context.SaveChanges();
                    }
                }
                
                await _context.SaveChangesAsync();
                
                return RedirectToPage("Index"); // Redirigir a una página de éxito (modifícala según necesites)
            }
            catch (Exception ex)
            {
                // Maneja el error (puedes registrar el error o retornar un mensaje)
                ModelState.AddModelError("", $"Error al guardar el voto: {ex.Message}");
                return Page();
            }
        }
    }
}
