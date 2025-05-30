using JV_Calculator.Models;

namespace JV_Calculator.Services
{
    public class JVCalculator : IJVCalculator
    {
        public JVResult CalculateJV(List<Partner> partners)
        {
            return BusinessRules.Validate(partners);
        }
    }
}
