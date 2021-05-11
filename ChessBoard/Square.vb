Imports System.Text.RegularExpressions


Public Class InvalidMoveException
    Inherits Exception
    Sub New()
        MyBase.New("Invalid declaration for Piece Class attempted")
    End Sub
End Class


'Class representing Squares
<Serializable>
Public Class Square
    Private intRow As Integer
    Private intCol As Integer

    Public Property Row As Integer
        Get
            Return intRow
        End Get
        Set(value As Integer)
            If value < 1 Or value > 8 Then
                Throw New InvalidMoveException
            Else
                intRow = value
            End If
        End Set
    End Property

    Public Property Column As Integer
        Get
            Return intCol
        End Get
        Set(value As Integer)
            If value < 1 Or value > 8 Then
                Throw New InvalidMoveException
            Else
                intCol = value
            End If
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return ColumnIntToChar(intCol) & intRow.ToString
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim bReturn As Boolean = MyBase.Equals(obj)
        If TypeOf obj Is Square Then
            If obj.ToString = ToString() Then
                bReturn = True
            Else
                bReturn = False
            End If
        End If
        Return bReturn
    End Function

    Public Sub New(iRow As Integer, iColumn As Integer)
        Row = iRow
        Column = iColumn
    End Sub

    Public Sub New(sq As String)
        If sq.Length <> 2 Then
            Throw New InvalidMoveException
        End If

        If Regex.IsMatch(sq, "[A-H][1-8]") Then
            Row = Convert.ToInt32(Right(sq, 1))
            Column = ColumnCharToInt(sq(0))
        Else
            Throw New InvalidMoveException
        End If
    End Sub

    Public Sub New()
        Me.New(1, 1)
    End Sub

    Private Function ColumnIntToChar(col As Integer, Optional add As Integer = 0) As Char
        Dim c As Char = Chr(col - 1 + Asc("A"c) + add)
        Return c
    End Function

    Private Function ColumnCharToInt(col As Char, Optional add As Integer = 0) As Integer
        Dim i As Integer = Asc(col) - Asc("A"c) + 1
        i += add
        Return i
    End Function
End Class