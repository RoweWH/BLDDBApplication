CREATE PROCEDURE [dbo].[spCornerCases_GetCaseById]
    @Id int
AS
SELECT
    cc.Id,
    bufferPiece.Piece AS [Buffer],
    firstPiece.Piece AS [First],
    secondPiece.Piece AS [Second]
FROM CornerCycles cc
JOIN CornerPieces bufferPiece
    ON cc.Buffer = bufferPiece.Id
JOIN CornerPieces firstPiece
    ON cc.First = firstPiece.Id
JOIN CornerPieces secondPiece
    ON cc.Second = secondPiece.Id
WHERE cc.Id = @Id;