using ATS_CV.Resourses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ATS_CV.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<Resource> _localizer;

        public AuthenticationController(IMediator mediator, IStringLocalizer<Resource> localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
        }

        [HttpGet("Signup")]
        public IActionResult Signup()
        {
            
            return View();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(SignInCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (result is OkObjectResult)
            {
                TempData["RegistrationMessage"] = "Signup successful!";
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
                return View(command);
            
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            var result = await _mediator.Send(command);

            if (result is OkObjectResult)
            {

                return RedirectToAction("Index", "Home");
            }

            else { ModelState.AddModelError(string.Empty, "Login failed."); }
            
            return View(command);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Authentication");
        }
        [HttpPost("log in")]
        public async Task<IActionResult> Logn(LoginUserCommand command)
        {

            if (String.IsNullOrEmpty(command.Email))
            {
                throw new Exception("tran:noemail");
            }
            var result = await _mediator.Send(command);

                return Ok(result);
            
        }
    }
}
