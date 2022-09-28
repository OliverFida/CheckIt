namespace awl_raumreservierung.classes
{
	public static class bookingHelper
	{
			public static DateTime StartOfWeek(DateTime dt)
			{
				int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
				return dt.AddDays(-1 * diff).Date;
			}
			public static DateTime endNextWeek(DateTime dt)
			{
				var diff = (DayOfWeek.Friday - dt.DayOfWeek + 7);
				return dt.AddDays(diff);
			}
	}
}
