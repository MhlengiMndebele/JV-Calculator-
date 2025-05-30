using JV_Calculator.Models;
using JV_Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JV_Calculator.Controllers
{
    public class JVController : Controller
    {

        private readonly IJVCalculator _service;
        public JVController(IJVCalculator service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }


        [HttpGet]
        public IActionResult Index()
        {
            Partner pPartners = new Partner();
            return View(pPartners);
        }

        [HttpPost]
        public IActionResult GetCompanies(string CompGrid)
        {
            if (string.IsNullOrEmpty(CompGrid))
            {
                return BadRequest("No data received.");
            }

            var companies = JsonConvert.DeserializeObject<List<Partner>>(CompGrid);

            List<Partner>? partners = companies;

            if(partners.Count <=2)
            {
                TempData["Result"] = "❌ At least two partners are required.";
                return View("Index");
            }

            int bLeader = 0;

            foreach(var company in partners)
            {

                if(company.IsLeadPartner)
                {
                    bLeader = bLeader + 1;
                }

            }


            if(bLeader >1 || bLeader==0)
            {
                TempData["Result"] = "❌ Exactly one lead partner must be selected.";
                return View("Index");
            }

            var result = this._service.CalculateJV(partners);

            TempData["Result"] = result.Messages[0];

            TempData["Approve"] = result.IsApproved;

            return View("Index");
        }
    }
}
