CREATE PROCEDURE [dbo].[spCornerCases_GetByBuffer]
	@Buffer nvarchar(3),
	@TwistedBufferOne nvarchar(3),
	@TwistedBufferTwo nvarchar(3)
AS
select * from dbo.vwCornerCases_All
where Buffer IN (@Buffer, @TwistedBufferOne, @TwistedBufferTwo)
or First IN (@Buffer, @TwistedBufferOne, @TwistedBufferTwo)
or Second IN (@Buffer, @TwistedBufferOne, @TwistedBufferTwo);
