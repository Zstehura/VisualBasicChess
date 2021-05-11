Imports ChessBoard

<Serializable>
Public Class Bishop
    Inherits Piece

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        chletter = "b"
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        chletter = "b"
    End Sub

    Public Sub New()
        MyBase.New()
        chletter = "b"
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'Bishops can move diagonally in any direction. Each subsequent square in a path
        ' must increase/decrease by 1 vertically AND 1 horizontally

        ' 4 lists for the four diagonal paths
        Dim sMoves(3) As LinkedList(Of Square)
        For i As Integer = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next

        Dim newRow As Integer, newCol As Integer

        ' Upper right path: ++
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 8 Or newCol = 8
            newRow += 1
            newCol += 1
            sMoves(0).AddLast(New Square(newRow, newCol))
        Loop

        'Lower right path: -+
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 8 Or newCol = 1
            newRow += 1
            newCol -= 1
            sMoves(1).AddLast(New Square(newRow, newCol))
        Loop

        'Lower left path: --
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 1 Or newCol = 1
            newRow -= 1
            newCol -= 1
            sMoves(2).AddLast(New Square(newRow, newCol))
        Loop

        'Upper left path: +-
        newRow = sqPos.Row
        newCol = sqPos.Column
        Do Until newRow = 1 Or newCol = 8
            newRow -= 1
            newCol += 1
            sMoves(3).AddLast(New Square(newRow, newCol))
        Loop

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New Bishop(New Square(Position))
        p.Color = Color
        p.Captured = Captured
        Return p
    End Function
End Class
