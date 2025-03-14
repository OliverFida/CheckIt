import moment from 'moment';
import timesMap from '../api/timesMap.json';

function getNewStartDate(context, weekDay, lesson){
    // Current monday
    var date = moment().weekday(1);

    // Selected weekday
    date.add(weekDay - 1, 'days');

    // Selected weekOffset
    date.add(context.weekSelection.offset, 'weeks');

    // Find lesson by key
    var foundLesson = timesMap.find(e => e.key === lesson.key);

    // Parse config time
    var lessonHour = foundLesson.start.substring(0, 2);
    var lessonMinute = foundLesson.start.substring(3, 5);

    // Set time
    date.set('hours', lessonHour);
    date.set('minutes', lessonMinute);
    date.set('seconds', 0).set('milliseconds', 0);

    return date;
}

function getNewEndDate(startDate){
    var date = moment(startDate);
    
    // Add 50 minutes
    date.add(50, 'minutes');

    return date;
}

function checkLessonOver(endTime){
    var over = false;
    
    // Check
    if(endTime.isBefore(moment())) over = true;

    return over;
}

function findBooking(context, startDate, endDate){
    var booking = null;

    // Find
    booking = context.bookings.bookings.find(booking => {
        var bookingStartDate = getStartDateFromUtc(booking.startTime);
        var bookingEndDate = getEndDateFromUtc(booking.endTime)

        // Check startTime
        if(startDate.add(1, 'second').isBetween(bookingStartDate, bookingEndDate)
        // Check endTime
        && endDate.subtract(1, 'second').isBetween(bookingStartDate, bookingEndDate)){
            return true;
        }

        return false;
    });

    return booking;
}

function getStartDateFromUtc(startDate){
    var date = moment.utc(startDate);

    // Convert to local
    date.local();

    return date;
}

function getEndDateFromUtc(endDate){
    var date = moment.utc(endDate);

    // Convert to local
    date.local();

    // Add 1 second
    if(date.get('seconds') === 59){
        date.add(1, 'second');
    }

    return date;
}

function checkIsOwn(booking){
    var loginUsername = localStorage.getItem('loginUsername');

    if(booking.user.username === loginUsername) return true;
    return false;
}

function getEndDateFromDuration(startDate, duration){
    var date = moment(startDate);

    // Parse startString
    var startString = startDate.format('HH:mm');

    // Find start lesson
    var startLesson = timesMap.find(e => e.start === startString);

    // Calc end lesson key
    var endLessonKey = startLesson.key + parseInt(duration) - 1;
    
    // Find end lesson
    var endLesson = timesMap.find(e => e.key === endLessonKey);

    // Parse endString
    var endHours = endLesson.end.substring(0, 2);
    var endMinutes = endLesson.end.substring(3, 5);
    
    // Set date
    date.set('hours', endHours);
    date.set('minutes', endMinutes);

    return date;
}

function getDurationFromDates(startDate, endDate){
    var duration;

    // Parse startString
    var startString = startDate.format('HH:mm');

    // Find start lesson
    var startLesson = timesMap.find(e => e.start === startString);

    // Parse endString
    var endString = endDate.format('HH:mm');

    // Find end lesson
    var endLesson = timesMap.find(e => e.end === endString);

    // Calculate duration
    duration = endLesson.key - startLesson.key + 1;
    
    return duration;
}

var exports = {
    getNewStartDate,
    getNewEndDate,
    checkLessonOver,
    findBooking,
    getStartDateFromUtc,
    getEndDateFromUtc,
    checkIsOwn,
    getEndDateFromDuration,
    getDurationFromDates
};
export default exports;