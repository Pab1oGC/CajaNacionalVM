using System.ComponentModel.DataAnnotations;

namespace VeriMedCNS.Models
{
    public class Votes
    {
        [Key]
        public int VoteId { get; set; }
        public int RequestId { get; set; }
        public int DoctorId { get; set; }

        public string Vote { get; set; }
        public DateTime VoteDate { get; set; }
        public string VoteReazon { get; set; }

    }
}