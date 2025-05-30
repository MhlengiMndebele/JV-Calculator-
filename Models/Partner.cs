namespace JV_Calculator.Models
{
    public class Partner
    {
        public string? Company { get; set; }
        public decimal Turnover1 { get; set; }
        public int Year1 { get; set; }
        public decimal Turnover2 { get; set; }
        public int Year2 { get; set; }
        public string? Grading { get; set; }
        public decimal CapitalContribution { get; set; }
        public bool IsLeadPartner { get; set; }
        public bool IsActive { get; set; }
    }

    public class JVResult
    {
        public bool IsApproved { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }

}
