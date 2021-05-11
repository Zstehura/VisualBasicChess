Imports ChessBoard

<Serializable>
Public Class Queen
    Inherits Piece

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        chletter = "q"
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        chletter = "q"
    End Sub

    Public Sub New()
        MyBase.New()
        chletter = "q"
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'Queens move in straight lines in any direction, direct or diagonal

        '8 possible paths
        Dim sMoves(7) As LinkedList(Of Square)
        For i As Integer = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next
        Dim newRow As Integer, newCol As Integer

        'Vertical paths
        'Forward
        newRow = sqPos.Row
        Do Until newRow = 8
            newRow += 1
            sMoves(0).AddLast(New Square(newRow, sqPos.Column))
        Loop
        'Backward
        newRow = sqPos.Row
        Do Until newRow = 1
            newRow -= 1
            sMoves(1).AddLast(New Square(newRow, sqPos.Column))
        Loop

        'Horizontal
        'Right
        newCol = sqPos.Column
        Do Until newCol = 8
            newCol += 1
            sMoves(2).AddLast(New Square(sqPos.Row, newCol))
        Loop
        'Left
        newCol = sqPos.Column
        Do Until newCol = 1
            newCol -= 1
            sMoves(3).AddLast(New Square(sqPos.Row, newCol))
        Loop

        ' Upper right path: ++
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 8 Or newCol = 8
            newRow += 1
            newCol += 1
            sMoves(4).AddLast(New Square(newRow, newCol))
        Loop

        'Lower right path: -+
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 8 Or newCol = 1
            newRow += 1
            newCol -= 1
            sMoves(5).AddLast(New Square(newRow, newCol))
        Loop

        'Lower left path: --
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 1 Or newCol = 1
            newRow -= 1
            newCol -= 1
            sMoves(6).AddLast(New Square(newRow, newCol))
        Loop

        'Upper left path: +-
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 1 Or newCol = 8
            newRow -= 1
            newCol += 1
            sMoves(7).AddLast(New Square(newRow, newCol))
        Loop

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New Queen(New Square(Position))
        p.Color = Color
        p.Captured = Captured
        Return p
    End Function
End Class
