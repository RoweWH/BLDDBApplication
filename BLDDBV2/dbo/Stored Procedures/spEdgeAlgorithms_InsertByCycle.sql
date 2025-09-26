CREATE PROCEDURE [dbo].[spEdgeAlgorithms_InsertByCycle]
	@Buffer nvarchar(2),
	@First nvarchar(2),
	@Second nvarchar(2),
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into EdgeAlgorithms (CycleId, [Algorithm])
	values(
	(select Id from EdgeCycles
	where Buffer = (select Id from EdgePieces where Piece = @Buffer) and First = (select Id from EdgePieces where Piece = @First) and Second = (select Id from EdgePieces where Piece = @Second)),
	@Algorithm);
	set @Id = SCOPE_IDENTITY();
	end