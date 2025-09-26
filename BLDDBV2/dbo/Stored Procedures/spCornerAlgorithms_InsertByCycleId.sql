CREATE PROCEDURE [dbo].[spCornerAlgorithms_InsertByCycleId]
	@CycleId int,
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into CornerAlgorithms (CycleId, [Algorithm])
	values(@CycleId, @Algorithm);
	set @Id = SCOPE_IDENTITY();
	end
