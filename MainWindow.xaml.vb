Imports System.Text.RegularExpressions
Imports System.Xaml
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports ChessBoard

Class MainWindow

    'Store coordinates of each label
    ' (0, x) = Columns
    ' (1, x) = Rows
    Private coord(1, 7) As Double
    Private gameBoard As Board
    Private strLastMove As String
    Private strMiscMsg As String

    Private pecSelected As Piece
    Private brOption As New Border
    Private brNorm As New Border
    Private brAttack As New Border
    Private brSelected As New Border

    Private ToXcoord As Double, ToYcoord As Double
    Private bf As New BinaryFormatter
    Private ioFs As FileStream


    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        'Initializes the Window/Class variables

        'Find all coordinates
        For Each c As Object In MainGrid.Children
            If TypeOf (c) Is Controls.Label Then
                Dim lbl As Controls.Label = c
                If Regex.IsMatch(lbl.Content, "[A-H1-8]") And lbl.Content.ToString.Length = 1 Then
                    'Determine whether label is column or row
                    If Regex.IsMatch(lbl.Content, "[A-H]") Then
                        'Its a letter, meaning its a column
                        coord(0, Asc(lbl.Content.ToString(0)) - Asc("A"c)) = lbl.Margin.Left - 17
                    Else
                        'Its a number, meaning its a row
                        coord(1, Convert.ToInt32(lbl.Content.ToString) - 1) = lbl.Margin.Top - 12
                    End If 'Regex.IsMatch(lbl.Content, "[A-H]")
                End If 'Regex.IsMatch(lbl.Content, "[A-H1-8]") And lbl.Content.ToString.Length = 1
            End If 'TypeOf (c) Is Label
        Next c

        'Start new game automatically
        newGame()
        txtGameMsg.IsHitTestVisible = False
        txtGameMsg.IsReadOnly = True
        txtStatus.IsHitTestVisible = False
        txtStatus.IsReadOnly = True
        brOption.BorderBrush = Brushes.LimeGreen
        brOption.BorderThickness = New Thickness(3)
        brNorm.BorderBrush = btnNewGame.BorderBrush
        brNorm.BorderThickness = btnNewGame.BorderThickness
        brAttack.BorderBrush = Brushes.Maroon
        brAttack.BorderThickness = New Thickness(3)
        brSelected.BorderBrush = Brushes.Gold
        brSelected.BorderThickness = New Thickness(3)

    End Sub 'MainWindow_Loaded

    Private Sub SquareBtnClick(sender As Object, e As RoutedEventArgs) Handles btnA1.Click, btnB1.Click, btnC1.Click, btnD1.Click, btnE1.Click, btnF1.Click, btnG1.Click, btnH1.Click,
            btnA2.Click, btnB2.Click, btnC2.Click, btnD2.Click, btnE2.Click, btnF2.Click, btnG2.Click, btnH2.Click,
            btnA3.Click, btnB3.Click, btnC3.Click, btnD3.Click, btnE3.Click, btnF3.Click, btnG3.Click, btnH3.Click,
            btnA4.Click, btnB4.Click, btnC4.Click, btnD4.Click, btnE4.Click, btnF4.Click, btnG4.Click, btnH4.Click,
            btnA5.Click, btnB5.Click, btnC5.Click, btnD5.Click, btnE5.Click, btnF5.Click, btnG5.Click, btnH5.Click,
            btnA6.Click, btnB6.Click, btnC6.Click, btnD6.Click, btnE6.Click, btnF6.Click, btnG6.Click, btnH6.Click,
            btnA7.Click, btnB7.Click, btnC7.Click, btnD7.Click, btnE7.Click, btnF7.Click, btnG7.Click, btnH7.Click,
            btnA8.Click, btnB8.Click, btnC8.Click, btnD8.Click, btnE8.Click, btnF8.Click, btnG8.Click, btnH8.Click
        'e.Source.Name = Name of button clicked

        'Determine which button was clicked
        Dim row As Integer = Convert.ToInt32(Right(e.Source.name, 1)) - 1
        Dim col As Integer = Asc(e.Source.Name.ToString(3)) - Asc("A"c)

        'Check if the player is moving or selecting
        If IsNothing(pecSelected) Then
            'No Selected Piece, the player is selecting
            pecSelected = gameBoard.GetPieceAt(New Square(row + 1, col + 1))

            If IsNothing(pecSelected) Then
                'Player clicked an empty space
                strMiscMsg = "Click one of your pieces to see its moves"
                UpdateMessages()
            ElseIf pecSelected.Color <> gameBoard.playerTurn Then
                'Player clicked on a piece that wasn't theirs
                strMiscMsg = "Please select one of your own pieces"
                UpdateMessages()
                pecSelected = Nothing
            Else
                Dim b As Controls.Button
                GetButton(New Square(pecSelected.Position), b)
                b.BorderBrush = brSelected.BorderBrush
                b.BorderThickness = brSelected.BorderThickness

                Dim moves = gameBoard.GetPieceOptions(pecSelected)
                For Each ll In moves
                    If ll IsNot Nothing Then
                        For Each sq As Square In ll
                            'iterate through each path and highlight the squares
                            GetButton(sq, b)

                            If gameBoard.GetPieceAt(sq) IsNot Nothing Then
                                'There's already a piece here, means its an attack
                                b.BorderBrush = brAttack.BorderBrush
                                b.BorderThickness = brAttack.BorderThickness
                            Else
                                'Empty space
                                b.BorderBrush = brOption.BorderBrush
                                b.BorderThickness = brOption.BorderThickness
                            End If

                        Next sq
                    End If
                Next ll

            End If
        Else
            Try
                'Player is moving, check moves
                Dim toSq As New Square(row + 1, col + 1)
                Dim fromSq As New Square(pecSelected.Position)

                ' Move piece
                gameBoard.MovePiece(New Square(pecSelected.Position), toSq)
                Dim img As Image
                GetImg(toSq, img)
                If img IsNot Nothing Then MainGrid.Children.Remove(img)
                GetImg(fromSq, img)
                MoveImg(img, col, row)  'Move Piece Image

                'Update messages
                Dim sq As New Square(row + 1, col + 1)
                strLastMove = pecSelected.Letter & sq.ToString
                UpdateMessages()

                'Check to see if there's a winner
                If gameBoard.IsMate <> Piece.PieceColor.None Then
                    'Disable buttons
                    ButtonsEnabled(False)
                End If

            Catch ex As InvalidMoveException
                strMiscMsg = "Invalid square selected"
                UpdateMessages()
            End Try
            pecSelected = Nothing
            ClearButtonOptions()
        End If

    End Sub 'SquareBtnClick

    Private Sub btnNewGame_Click(sender As Object, e As RoutedEventArgs) Handles btnNewGame.Click
        ' User started a new game
        newGame()
    End Sub 'btnNewGame_Click

    Private Sub newGame()
        gameBoard = New Board(True)
        DrawNewBoard()
        txtStatus.Text = "White always gets the first move"
        ButtonsEnabled(True)
    End Sub

    Private Sub MoveImg(ByRef img As Image, newCol As Double, newRow As Double)

        Dim fromX = img.RenderTransformOrigin.X
        Dim fromY = img.RenderTransformOrigin.Y
        Dim newX As Double = coord(0, newCol)
        Dim newY As Double = coord(1, newRow)
        Dim offset As Vector = VisualTreeHelper.GetOffset(img)
        Dim top = offset.Y
        Dim left = offset.X
        Dim trans As TranslateTransform = New TranslateTransform()
        img.RenderTransform = trans
        Dim anim1 As Animation.DoubleAnimation = New Animation.DoubleAnimation(fromY - top, newY - top, TimeSpan.FromSeconds(1))
        Dim anim2 As Animation.DoubleAnimation = New Animation.DoubleAnimation(fromX - left, newX - left, TimeSpan.FromSeconds(1))
        Debug.Print("Move to " & newX & ", " & newY)

        trans.BeginAnimation(TranslateTransform.YProperty, anim1)
        trans.BeginAnimation(TranslateTransform.XProperty, anim2)

        img.Name = "img" & New Square(newRow + 1, newCol + 1).ToString
        img.RenderTransformOrigin = New Point(newX, newY)
    End Sub

    Private Sub btnSaveGame_Click(sender As Object, e As RoutedEventArgs) Handles btnSaveGame.Click
        Dim saveFile As String = ""

        Using fd As New SaveFileDialog()
            fd.OverwritePrompt = False
            fd.DefaultExt = ".chess"
            fd.Filter = "Chess File|*.chess"
            fd.AddExtension = True
            If fd.ShowDialog() <> Forms.DialogResult.Cancel Then
                saveFile = fd.FileName
            End If
        End Using

        If saveFile = "" Then Exit Sub
        ioFs = New FileStream(saveFile, FileMode.Create)
        bf.Serialize(ioFs, gameBoard)
        ioFs.Close()

    End Sub

    Private Sub UpdateMessages()
        If gameBoard.IsMate <> Piece.PieceColor.None Then
            txtGameMsg.Text = "Checkmate" & vbNewLine
            If gameBoard.IsMate = Piece.PieceColor.Black Then
                'Black lost
                txtGameMsg.Text &= "White wins!"
            Else
                txtGameMsg.Text &= "Black wins!"
            End If
        ElseIf gameBoard.IsCheck <> Piece.PieceColor.None Then
            txtGameMsg.Text = "Check" & vbNewLine & "You must protect your King"
        Else
            txtGameMsg.Text = ""
        End If

        txtStatus.Text = "Last move: " & gameBoard.LastMove & vbNewLine
        If gameBoard.playerTurn = Piece.PieceColor.Black Then
            txtStatus.Text &= "Black's Move" & vbNewLine
        Else
            txtStatus.Text &= "White's Move" & vbNewLine
        End If
        txtStatus.Text &= strMiscMsg
        strMiscMsg = ""

    End Sub

    Private Sub GetImg(sq As Square, ByRef imgFill As Controls.Image)
        For Each c In MainGrid.Children
            If TypeOf c Is Controls.Image Then
                If c.Name = "img" & sq.ToString Then
                    imgFill = c
                End If
            End If
        Next
    End Sub

    Private Sub GetButton(sq As Square, ByRef bFill As Controls.Button)
        For Each b In MainGrid.Children
            If TypeOf b Is Controls.Button Then
                If b.Name = "btn" & sq.ToString Then
                    bFill = b
                End If
            End If
        Next
    End Sub

    Private Sub DelImgs()
        For Each c In MainGrid.Children
            If TypeOf c Is Controls.Image Then
                MainGrid.Children.Remove(c)
                DelImgs()
                Exit For
            End If
        Next
    End Sub

    Private Sub DrawNewBoard()
        Dim pieces() As Piece = gameBoard.GetPieces()
        DelImgs()

        For Each p In pieces
            If p.Captured = False Then
                'create image
                Dim img As New Image
                Dim imgSrc As String = "pack://application:,,,/Images/"
                ' Source="Images/whitePawn.png"  IsHitTestVisible="False"
                img.Name = "img" & p.Position
                img.HorizontalAlignment = Windows.HorizontalAlignment.Left
                img.Height = 50
                img.VerticalAlignment = VerticalAlignment.Top
                img.Width = 50
                img.RenderTransformOrigin = New Point(0.5, 0.5)
                img.IsHitTestVisible = False
                img.Stretch = Stretch.Fill

                If p.Color = Piece.PieceColor.Black Then
                    imgSrc &= "black"
                Else
                    imgSrc &= "white"
                End If
                'C:\Users\Zack\Google Drive\School\Adv. VB\AdvVBChess\AdvVBChess\Images\blackKnight.png
                If TypeOf p Is Pawn Then
                    imgSrc &= "Pawn.png"
                ElseIf TypeOf p Is Knight Then
                    imgSrc &= "Knight.png"
                ElseIf TypeOf p Is Bishop Then
                    imgSrc &= "Bishop.png"
                ElseIf TypeOf p Is Rook Then
                    imgSrc &= "Rook.png"
                ElseIf TypeOf p Is Queen Then
                    imgSrc &= "Queen.png"
                ElseIf TypeOf p Is King Then
                    imgSrc &= "King.png"
                End If

                img.Source = New BitmapImage(New Uri(imgSrc))

                MainGrid.Children.Add(img)
                MoveImg(img, p.Column - 1, p.Row - 1)
            End If
        Next p

    End Sub

    Private Sub ButtonsEnabled(bYn As Boolean)
        For Each c In MainGrid.Children
            If TypeOf c Is Controls.Button Then
                Dim b As Controls.Button = c
                b.IsEnabled = bYn
            End If
        Next
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As RoutedEventArgs) Handles btnAbout.Click
        Dim ab As New AboutBox1
        ab.Show()
    End Sub

    Private Sub btnLoadGame_Click(sender As Object, e As RoutedEventArgs) Handles btnLoadGame.Click
        Dim fd As New OpenFileDialog
        fd.Filter = "Chess file|*chess"
        If fd.ShowDialog <> Forms.DialogResult.Cancel Then
            Try
                ioFs = New FileStream(fd.FileName, FileMode.Open)
                gameBoard = bf.Deserialize(ioFs)
                DrawNewBoard()
                UpdateMessages()
                ioFs.Close()
            Catch ex As Exception
                MessageBox.Show("ERROR: An error ocurred opening the selected file")
            End Try
        End If

    End Sub

    Private Sub ClearButtonOptions()
        For Each c In MainGrid.Children
            If TypeOf c Is Controls.Button Then
                c.BorderBrush = brNorm.BorderBrush
                c.BorderThickness = brNorm.BorderThickness
            End If
        Next
    End Sub

End Class
