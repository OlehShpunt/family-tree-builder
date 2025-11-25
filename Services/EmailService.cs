// EmailService.cs
using FluentEmail.Core;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IFluentEmailFactory _emailFactory;
    
    // Inject the factory instead of the single IFluentEmail instance if you haven't already
    public EmailService(IFluentEmailFactory emailFactory)
    {
        _emailFactory = emailFactory;
    }

    // New, simple method to send a text email without attachments
    public async Task SendSimpleEmailAsync(string toEmail, string subject, string body)
    {
        // Use the factory to create a new email instance
        var email = _emailFactory.Create(); 

        await email
            // .From() is often inherited from configuration, but can be set explicitly
            .To(toEmail)
            .Subject(subject)
            // Use .Body() to set the text or HTML content
            .Body(body, isHtml: true) 
            .SendAsync();
    }
}