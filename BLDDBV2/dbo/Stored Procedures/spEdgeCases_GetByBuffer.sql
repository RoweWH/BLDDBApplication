CREATE PROCEDURE [dbo].[spEdgeCases_GetByBuffer]
	@Buffer nvarchar(2),
	@FlippedBuffer nvarchar(2)
AS

select * from dbo.vwEdgeCases_All
where Buffer IN (@Buffer, @FlippedBuffer)
or First IN (@Buffer, @FlippedBuffer)
or Second IN (@Buffer, @FlippedBuffer);
