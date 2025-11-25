// ExportController.cs

// 1. Essential using directives for ASP.NET Core MVC and Authorization
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Good practice for controllers

[ApiController]
[Route("export")]
public class ExportController : ControllerBase // 2. Must inherit from ControllerBase
{
    private readonly EmailService _emailService; // 3. Field must be inside the class

    // Constructor
    public ExportController(EmailService emailService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    [Authorize]
    [HttpPost("/send-message")] 
    // This method must be INSIDE the ExportController class
    public async Task<IActionResult> SendMessage([FromForm] string email, [FromForm] string message)
    {
        // Compiler can now find BadRequest(), Ok(), and StatusCode()
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { success = false, message = "Recipient email is required." });

        try
        {
            const string subject = "A message about your Family Tree";
            string body = $"Hello! A family member wanted to share this message with you: {message}";
            
            // Compiler can now find _emailService
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