CREATE PROCEDURE [dbo].[spCornerCases_GetId]
	@buffer nvarchar(3),
	@first nvarchar(3),
	@second nvarchar(3)
AS
begin
set nocount on;
select Id from CornerCycles where
Buffer = (select Id from CornerPieces where Piece = @buffer) and
First = (select Id from CornerPieces where Piece = @first) and
Second = (select Id from CornerPieces where Piece = @second);
end
