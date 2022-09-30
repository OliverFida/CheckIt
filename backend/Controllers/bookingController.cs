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
    private checkITContext ctx;

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
            Room room = Helpers.GetRoom(roomId);
            if(room == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new PublicBooking[0];
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
            return new PublicBooking[0];
        }
    }

    [HttpPut("book")]
    [Authorize]
    public ReturnModel book(CreateBookingModel model)
    {
        try
        {
            if(Helpers.GetRoom(model.roomId) == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel(new StatusCodeResult(404))
                {
                    message = "Raum konnte nicht gefunden werden!"
                };
            }

            bool overlapsWithOtherBookings = Helpers.GetRoom(model.roomId)
                                                .GetBookings()
                                                .Any(b =>
                                                    b.StartTime <= model.endTime &&
                                                    model.startTime <= b.EndTime
                                                );

            if(overlapsWithOtherBookings)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new ReturnModel(new StatusCodeResult(400))
                {
                    message = "Die angegebene Buchung überschneidet sich mit einer bereits bestehenden!"
                };
            }

            // TODO : Check if room is active

            Booking book = new Booking
            {
                StartTime = model.startTime,
                EndTime = model.endTime,
                Room = model.roomId,
                UserId = User.GetUser().Id,
                CreateTime = DateTime.Now,
                CreatedBy = User.GetUser().Id
            };
            ctx.Bookings.Add(book);

            ctx.SaveChanges();

            Response.StatusCode = StatusCodes.Status201Created;
            return new ReturnModel
            {
                status = 201,
                message = $"Raum {Helpers.GetRoom(model.roomId).Number} wurde erfolgreich für den {model.startTime.ToString("d")} gebucht.",
                data = book.ToPublicBooking()
            };
        }
        catch(Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel()
            {
                status = 400,
                statusMessage = "error",
                message = "Es ist ein Fehler aufgetreten!"
            };
        }
    }

    [HttpDelete("{bookingId}")]
    [Authorize]
    public ReturnModel remove(int bookingId)
    {
        try
        {
            Booking booking = Helpers.GetBooking(bookingId);

            if(booking is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel(new StatusCodeResult(404))
                {
                    message = "Buchung konnte nicht gefunden werden!"
                };
            }

            if(booking.UserId != User.GetUser().Id && !User.IsInRole("Admin"))
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new ReturnModel(new StatusCodeResult(401))
                {
                    message = "Es können nur eigene Buchungen gelöscht werden!"
                };
            }

            ctx.Bookings.Remove(booking);
            ctx.SaveChanges();

            Response.StatusCode = StatusCodes.Status200OK;
            return new ReturnModel
            {
                message = $"Buchung erfolgreich gelöscht!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel()
            {
                status = 400,
                statusMessage = "error",
                message = "Es ist ein Fehler aufgetreten!"
            };
        }
    }


    [HttpPut("bookAsAdmin")]
    [Authorize(Roles = "Admin")]
    public StatusCodeResult bookAsAdmin(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
    {
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = userHelper.getUserId(authUsername);
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
    public StatusCodeResult edit(DateTime startTime, DateTime newEndTime)
    {
        var db = new checkITContext();
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = db.Bookings.Where(b => b.StartTime == startTime).FirstOrDefault();
        var userId = userHelper.getUserId(authUsername);
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