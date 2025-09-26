CREATE PROCEDURE [dbo].[spCornerAlgorithms_InsertByCycle]
	@Buffer nvarchar(3),
	@First nvarchar(3),
	@Second nvarchar(3),
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into CornerAlgorithms (CycleId, [Algorithm])
	values(
	(select Id from CornerCycles
	where Buffer = (select Id from CornerPieces where Piece = @Buffer) and First = (select Id from CornerPieces where Piece = @First) and Second = (select Id from CornerPieces where Piece = @Second)),
	@Algorithm);
	set @Id = SCOPE_IDENTITY();
	end
