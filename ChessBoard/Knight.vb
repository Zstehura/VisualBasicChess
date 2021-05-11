
Imports ChessBoard

<Serializable>
Public Class Knight
    Inherits Piece

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        chletter = "N"
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        chletter = "N"
    End Sub

    Public Sub New()
        MyBase.New()
        chletter = "N"
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'Knights can move in an 'L' with a combonation of 2 and 1
        ' they MUST move 1 or 2 spaces horizontally and then 2 or 1 spaces vertically
        ' They can attack any space they can move to and are not impeded by obstacles

        ' 8 possible spots, no paths
        Dim sMoves(7) As LinkedList(Of Square)
        For i As Integer = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next
        Dim r As Integer = sqPos.Row, c As Integer = sqPos.Column

        'Up 1, Right 2
        If r < 8 And c < 7 Then sMoves(0).AddLast(New Square(r + 1, c + 2))
        'Up 1, Left 2
        If r < 8 And c > 2 Then sMoves(1).AddLast(New Square(r + 1, c - 2))
        'Up 2, Right 1
        If r < 7 And c < 8 Then sMoves(2).AddLast(New Square(r + 2, c + 1))
        'Up 2, Left 1
        If r < 7 And c > 1 Then sMoves(3).AddLast(New Square(r + 2, c - 1))
        'Down 2, Right 1
        If r > 2 And c < 8 Then sMoves(4).AddLast(New Square(r - 2, c + 1))
        'Down 2, Left 1
        If r > 2 And c > 1 Then sMoves(5).AddLast(New Square(r - 2, c - 1))
        'Down 1, Right 2
        If r > 1 And c < 7 Then sMoves(6).AddLast(New Square(r - 1, c + 2))
        'Down 1, Left 2
        If r > 1 And c > 2 Then sMoves(7).AddLast(New Square(r - 1, c - 2))

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New Knight(New Square(Position))
        p.Color = Color
        p.Captured = Captured
        Return p
    End Function
End Class
