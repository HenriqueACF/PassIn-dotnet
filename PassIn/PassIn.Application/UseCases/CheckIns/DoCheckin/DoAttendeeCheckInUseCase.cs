using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.DoCheckin;

public class DoAttendeeCheckInUseCase
{
    private readonly PassInDbContext _dbContext;

    public DoAttendeeCheckInUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    
    public ResponseRegisterJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = new CheckIn
        {
            Attendee_Id = attendeeId,
            Created_at = DateTime.UtcNow
        };

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();
        
        return new ResponseRegisterJson
        {
            Id = entity.Id,
        };
    }
    
    // VALIDAÇÕES
    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(attendee => attendee.Id == attendeeId);

        if (existAttendee == false)
            throw new NotFoundException("The attende with this Id was not found");
        
        var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);
        
        if(existCheckIn)
            throw new ConflictException("The attende can not do checkin twice in the same event");
    }
}