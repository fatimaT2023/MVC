using Microsoft.AspNetCore.Mvc;
using day4topic.Models;

namespace day4topic.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new CalculatorModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CalculatorModel model)
        {
            if (!model.Number1.HasValue ||
                !model.Number2.HasValue)
            {
                model.ErrorMessage = "Please enter both numbers";
                return View(model);
            }

            int num1 = model.Number1.Value;
            int num2 = model.Number2.Value;

            switch (model.Operation)
            {
                case "+":
                    model.Result = (double)num1 + num2;
                    break;

                case "-":
                    model.Result = (double)num1 - num2;
                    break;

                case "*":
                    model.Result = (double)num1 * num2;
                    break;
                    
                case "/":
                    if (num2 == 0)
                    {
                        model.ErrorMessage = "Cannot divide by zero";
                    }
                    else
                    {
                        model.Result = (double)num1 / num2;
                    }
                    break;

                default:
                    model.ErrorMessage = "Invalid operation";
                    break;
            }

            return View(model);
        }
    }
}