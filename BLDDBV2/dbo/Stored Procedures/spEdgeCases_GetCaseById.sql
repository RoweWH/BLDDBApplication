CREATE PROCEDURE [dbo].[spEdgeCases_GetCaseById]
    @Id int
AS
SELECT
    ec.Id,
    bufferPiece.Piece AS [Buffer],
    firstPiece.Piece AS [First],
    secondPiece.Piece AS [Second]
FROM EdgeCycles ec
JOIN EdgePieces bufferPiece
    ON ec.Buffer = bufferPiece.Id
JOIN EdgePieces firstPiece
    ON ec.First = firstPiece.Id
JOIN EdgePieces secondPiece
    ON ec.Second = secondPiece.Id
WHERE ec.Id = @Id;