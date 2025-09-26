CREATE PROCEDURE [dbo].[spEdgeCases_GetId]
	@buffer nvarchar(3),
	@first nvarchar(3),
	@second nvarchar(3)
AS
begin
set nocount on;
select Id from EdgeCycles where
Buffer = (select Id from EdgePieces where Piece = @buffer) and
First = (select Id from EdgePieces where Piece = @first) and
Second = (select Id from EdgePieces where Piece = @second);
end
