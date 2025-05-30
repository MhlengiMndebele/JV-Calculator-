using JV_Calculator.Models;

namespace JV_Calculator.Services
{
    public interface IJVCalculator
    {
        public JVResult CalculateJV(List<Partner> partners);
    }
}
