using awl_raumreservierung.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;


[ApiController]
[Route("[controller]")]
public class bookingController : ControllerBase
{
    private readonly ILogger<bookingController> _logger;
    private readonly checkITContext ctx;

    public bookingController(ILogger<bookingController> logger)
    {
        ctx = new checkITContext();
        _logger = logger;
    }

    [HttpGet("room/{roomId}")]
    [Authorize]
    public PublicBooking[] Get(int roomId, DateTime? date)
    {
        try
        {
            Room? room = Helpers.GetRoom(roomId);
            if(room == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return Array.Empty<PublicBooking>();
            }

            if(date == null)
            {
                date = DateTime.Now.StartOfWeek();
            }
            date = date.Value.StartOfWeek();

            return room.GetBookings()
                       .Where(b => 
                            b.StartTime >= date.Value &&
                            b.EndTime <= date.Value.AddDays(6)
                        ).Select(b => b.ToPublicBooking()).ToArray();
        }
        catch(Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return Array.Empty<PublicBooking>();
        }
    }

    [HttpPut("book")]
    [Authorize]
    public ReturnModel Book(CreateBookingModel model)
    {
        try
        {
			Room? room = Helpers.GetRoom(model.RoomID);
			if (room == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel(new StatusCodeResult(404))
                {
                    Message = "Raum konnte nicht gefunden werden!"
                };
            }

            bool overlapsWithOtherBookings = room
																.GetBookings()
                                                .Any(b =>
                                                    b.StartTime <= model.EndTime &&
                                                    model.StartTime <= b.EndTime
                                                );

            if(overlapsWithOtherBookings)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new ReturnModel(new StatusCodeResult(400))
                {
                    Message = "Die angegebene Buchung �berschneidet sich mit einer bereits bestehenden!"
                };
            }

            // TODO : Check if room is active

            Booking book = new()
            {
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Room = model.RoomID,
                UserId = User.GetUser()?.Id ?? 0L,
                CreateTime = DateTime.Now,
                CreatedBy = User.GetUser()?.Id
            };
            ctx.Bookings.Add(book);

            ctx.SaveChanges();

            Response.StatusCode = StatusCodes.Status201Created;
            return new ReturnModel
				{
                Status = 201,
                Message = $"Raum {room.Number} wurde erfolgreich f�r den {model.StartTime:d} gebucht.",
                Data = book.ToPublicBooking()
            };
        }
        catch(Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel()
            {
                Status = 400,
                StatusMessage = "error",
                Message = "Es ist ein Fehler aufgetreten!"
            };
        }
    }

    [HttpDelete("{bookingId}")]
    [Authorize]
    public ReturnModel Remove(int bookingId)
    {
        try
        {
            Booking? booking = Helpers.GetBooking(bookingId);

            if(booking is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel(new StatusCodeResult(404))
                {
                    Message = "Buchung konnte nicht gefunden werden!"
                };
            }

            if(booking.UserId != User.GetUser()?.Id && !User.IsInRole("Admin"))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new ReturnModel(new StatusCodeResult(401))
                {
                    Message = "Es k�nnen nur eigene Buchungen gel�scht werden!"
                };
            }

            ctx.Bookings.Remove(booking);
            ctx.SaveChanges();

            Response.StatusCode = StatusCodes.Status200OK;
            return new ReturnModel
            {
                Message = $"Buchung erfolgreich gel�scht!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel()
            {
                Status = 400,
                StatusMessage = "error",
                Message = "Es ist ein Fehler aufgetreten!"
            };
        }
    }


    [HttpPut("bookAsAdmin")]
    [Authorize(Roles = "Admin")]
    public StatusCodeResult BookAsAdmin(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
    {
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = UserHelper.GetUserId(authUsername);
        if (userId < 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        var db = new checkITContext();
        var booking = new Booking(startTime, endTime, roomId, userId, createTime, createdBy);
        db.Bookings.Add(booking);
        db.SaveChanges();
        return StatusCode(StatusCodes.Status201Created);
    }
    [HttpPost("edit")]
    public StatusCodeResult Edit(DateTime startTime, DateTime newEndTime)
    {
        var db = new checkITContext();
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = db.Bookings.Where(b => b.StartTime == startTime).FirstOrDefault();
        var userId = UserHelper.GetUserId(authUsername);
        if (userId < 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        if (booking == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        // user auth
        var isAdmin = User.FindAll(ClaimTypes.Role).Any(c => c is { Type: ClaimTypes.Role } and { Value: "Admin" });

        if (userId == booking.UserId || isAdmin)
        {
            // booking in future check
            if (booking != null && booking.EndTime > DateTime.Now)
            {
                // no overlap check
                if (db.Bookings.Where(b => b.StartTime > startTime && b.StartTime < startTime).Any())
                {
                    booking.EndTime = newEndTime;
                    db.SaveChanges();
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return StatusCode(StatusCodes.Status200OK);
        }
        return StatusCode(StatusCodes.Status200OK);
    }
}