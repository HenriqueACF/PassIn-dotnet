using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Event.Register;

public class RegisterEventUseCase
{
    public void Execute(RequestEventJson request)
    {
        Validate(request);

        var dbContext = new PassInDbContext();
        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Title,
            Maximum_Attendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-"),
        };

        dbContext.Events.Add(entity);
        dbContext.SaveChanges();
    }
    
    private void Validate(RequestEventJson request)
    {
        if(request.MaximumAttendees <= 0)
        {
            throw new PassInException("The maximum attendees is invalid.");
        }
        
        if(string.IsNullOrWhiteSpace(request.Title))
        {
            throw new PassInException("the title is invalid.");
        }
        
        if(string.IsNullOrWhiteSpace(request.Details))
        {
            throw new PassInException("the details is invalid.");
        }
    }
}