using JV_Calculator.Models;
using NUnit.Framework;
using Xunit;

namespace JV_Calculator.Services
{
    public class BusinessRules
    {
        private const decimal RequiredTurnover = 20000000;
        private const decimal RequiredCapital = 4000000;
        private const decimal RequiredLeadTurnover = 3000000;
        private const int MinYear = 2012;

        public static JVResult Validate(List<Partner> partners)
        {
            var result = new JVResult();

            if (partners == null || partners.Count < 2)
            {
                result.Messages.Add("Please add more partner(s) to form a JV");
                return result;
            }

            if (partners.Any(p => !p.IsActive))
            {
                result.Messages.Add("One or more partner has expired registration status");
                return result;
            }

            var lead = partners.FirstOrDefault(p => p.IsLeadPartner);
            if (lead == null)
            {
                result.Messages.Add("Lead partner not found.");
                return result;
            }

            int leadGrade = int.Parse(lead.Grading.ToUpper().Replace("CE", ""));
            if (leadGrade < 3 || leadGrade < 6)
            {
                result.Messages.Add("The JV does not meet the lead partner minimum grading requirement");
                return result;
            }

            decimal totalTurnover = 0;
            foreach (var p in partners)
            {
                if (p.Year1 >= MinYear)
                    totalTurnover += p.Turnover1;
                if (p.Year2 >= MinYear)
                    totalTurnover += p.Turnover2;
            }

            if (totalTurnover < RequiredTurnover)
            {
                result.Messages.Add("Total for Partners’ turnover is below minimum requirements for Requirement A");
            }

            decimal leadMaxTurnover = Math.Max(
                lead.Year1 >= MinYear ? lead.Turnover1 : 0,
                lead.Year2 >= MinYear ? lead.Turnover2 : 0
            );

            if (leadMaxTurnover < RequiredLeadTurnover)
            {
                result.Messages.Add("The lead partner’s biggest turnover is below Requirement B");
            }

            decimal totalCapital = partners.Sum(p => p.CapitalContribution);
            if (totalCapital < RequiredCapital)
            {
                result.Messages.Add("The Partner’s turnover is below minimum Requirement C");
            }

            if (!result.Messages.Any())
            {
                result.IsApproved = true;
                result.Messages.Add("The calculated grading and class of work for this JV is Met.");
            }

            return result;
        }
    }

    public class BusinessRulesValidatorTests
    {
        [Fact]
        public void ValidJV_ShouldPass()
        {
            var partners = new List<Partner>
        {
            new Partner { Company = "XCP", Turnover1 = 5_000_000, Year1 = 2021, Turnover2 = 13_000_000, Year2 = 2023, Grading = "7CE", CapitalContribution = 3_500_000, IsLeadPartner = true, IsActive = true },
            new Partner { Company = "ZYZ", Turnover1 = 7_000_000, Year1 = 2023, Turnover2 = 0, Year2 = 2022, Grading = "3CE", CapitalContribution = 1_500_000, IsLeadPartner = false, IsActive = true },
            new Partner { Company = "ABX", Turnover1 = 180_000, Year1 = 2010, Turnover2 = 80_000, Year2 = 2022, Grading = "1CE", CapitalContribution = 0, IsLeadPartner = false, IsActive = true },
        };

            var result = BusinessRules.Validate(partners);
            Assert.That(result.IsApproved);
        }
    }
}
