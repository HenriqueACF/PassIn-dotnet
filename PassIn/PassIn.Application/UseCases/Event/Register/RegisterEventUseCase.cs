using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Event.Register;

public class RegisterEventUseCase
{
    public ResponseRegisterJson Execute(RequestEventJson request)
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

        return new ResponseRegisterJson()
        {
            Id = entity.Id,
        };
    }
    
    private void Validate(RequestEventJson request)
    {
        if(request.MaximumAttendees <= 0)
        {
            throw new ErrorOnValidationException("The maximum attendees is invalid.");
        }
        
        if(string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ErrorOnValidationException("the title is invalid.");
        }
        
        if(string.IsNullOrWhiteSpace(request.Details))
        {
            throw new ErrorOnValidationException("the details is invalid.");
        }
    }
}