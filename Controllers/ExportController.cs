// ExportController.cs

// 1. Essential using directives for ASP.NET Core MVC and Authorization
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 

[ApiController]
[Route("export")]
public class ExportController : ControllerBase 
{
    private readonly EmailService _emailService; 


    public ExportController(EmailService emailService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    [Authorize]
    [HttpPost("/send-message")] 
   
    public async Task<IActionResult> SendMessage([FromForm] string email, [FromForm] string message)
    {
       
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { success = false, message = "Recipient email is required." });

        try
        {
            const string subject = "A message about your Family Tree";
            string body = $@"
                <p>Hello! A family member wanted to share this message with you:</p>
                <p>{message}</p>
                <p><a href='http://localhost:5119'>Open the Family Tree Builder</a></p>
                ";

            
          
            await _emailService.SendSimpleEmailAsync(email, subject, body);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Email send failed: {ex.Message}");
            return StatusCode(500, new { success = false, message = "Failed to send message. Check SMTP setup." });
        }
    }
    
    // Helper method for email validation
    private bool IsValidEmail(string email)
    {
        try
        {
            return new MailAddress(email).Address == email;
        }
        catch
        {
            return false;
        }
    }
}