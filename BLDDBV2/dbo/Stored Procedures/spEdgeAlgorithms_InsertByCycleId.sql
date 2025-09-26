CREATE PROCEDURE [dbo].[spEdgeAlgorithms_InsertByCycleId]
	@CycleId int,
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into EdgeAlgorithms (CycleId, [Algorithm])
	values(@CycleId, @Algorithm);
	set @Id = SCOPE_IDENTITY();
	end
