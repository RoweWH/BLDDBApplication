CREATE PROCEDURE [dbo].[spParityAlgorithms_InsertByCaseId]
	@CaseId int,
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into ParityAlgorithms (CaseId, [Algorithm])
	values(@CaseId, @Algorithm);
	set @Id = SCOPE_IDENTITY();
	end
