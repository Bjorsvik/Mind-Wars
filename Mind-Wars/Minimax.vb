﻿Option Explicit On

Class Minimax
    Implements IDisposable
    Private FourthNumber As Integer = 0
    Dim LocalInitialList As New ArrayList
    Dim LocalPossibleList As New ArrayList
    Sub New(ByVal InitialList As ArrayList, ByVal PossibleList As ArrayList, ByVal Fourth As Integer)
        FourthNumber = Fourth
        LocalInitialList.Clear()
        LocalPossibleList.Clear()
        LocalInitialList = InitialList.GetRange(0, InitialList.Count)
        LocalPossibleList = PossibleList.GetRange(0, InitialList.Count)
    End Sub

    Public Sub FindBestMove()
        Dim SleepCounter As Integer = 0
        Do Until (LocalPossibleList.Count > 20 AndAlso LocalInitialList.Count > 20 AndAlso Not FourthNumber = 0) OrElse SleepCounter >= 100
            Threading.Thread.Sleep(50)
            SleepCounter += 1
            If SleepCounter >= 100 Then
                Debug.Print("!!!!!!!! - Thread #" & FourthNumber & " is stuck.")
            End If
        Loop

        Dim BWCount(1) As Integer
        Dim HighestMinScoreIndex As Integer = 0
        Dim ScoreForSolution As Integer = 0
        Dim i As Integer
        Dim iMax As Integer
        Dim quarter As Integer = CInt(Math.Round(LocalInitialList.Count / 4))
        Dim SolutionCount As Integer = LocalPossibleList.Count
        Dim InitialCount As Integer = LocalInitialList.Count
        Dim BWList As New ArrayList

        Select Case FourthNumber
            Case 1
                iMax = quarter
                i = 0
            Case 2
                iMax = quarter * 2
                i = quarter - 1
            Case 3
                iMax = quarter * 3
                i = quarter * 2 - 1
            Case 4
                iMax = LocalInitialList.Count
                i = quarter * 3 - 1
            Case > 4
                MsgBox("You need to set FourthNumber (1 to 4).")
        End Select

        Debug.Print("Thread #" & FourthNumber & " starts calculating.")
        Do Until i = iMax
            Dim q As Integer = 0
            Dim score As Integer = Integer.MaxValue
            Dim InitialItemiArray() As Integer = LocalInitialList.Item(i)
            Do
                BWCount = MiniGetBW(LocalPossibleList.Item(q), InitialItemiArray)
                Dim BWint As Integer = BWCount(0) * 10 + BWCount(1)
                If Not BWList.Contains(BWint) Then
                    Dim tempscore As Integer = MiniCalculateEliminated(BWCount(0), BWCount(1), InitialItemiArray)
                    If score > tempscore Then
                        score = tempscore
                    End If
                    BWList.Add(BWint)
                End If
                q += 1
            Loop Until q = SolutionCount
            BWList.Clear()
            q = 0
            If score > ScoreForSolution AndAlso score < Integer.MaxValue Then
                ScoreForSolution = score
                HighestMinScoreIndex = i
            End If
            i += 1
        Loop

        ' Thread locking didn't work out as expected.
        FourBestIndices(FourthNumber - 1) = HighestMinScoreIndex
        FourBestScores(FourthNumber - 1) = ScoreForSolution

        Debug.Print("Thread #" & FourthNumber & " finished.")
    End Sub

    Private Function MiniCalculateEliminated(ByVal B As Integer, ByVal W As Integer, ByVal HypotheticalGuess() As Integer) As Integer
        Dim SolutionsEliminated As Integer = 0
        Dim q As Integer = 0
        Dim ListCount As Integer = LocalPossibleList.Count - 1
        Do Until q = ListCount
            AI_BW_Check = MiniGetBW(LocalPossibleList.Item(q), HypotheticalGuess)
            If Not AI_BW_Check(0) = B OrElse Not AI_BW_Check(1) = W Then
                SolutionsEliminated += 1
            End If
            q += 1
        Loop
        Return SolutionsEliminated
    End Function

    Public Function MiniGetBW(ByVal ail() As Integer, ByVal g() As Integer) As Integer()
        Dim whitepegs As Integer
        Dim blackpegs As Integer
        Dim counted(holes - 1) As Integer
        Dim correct(holes - 1) As Integer

        Dim checkcorrectindex As Integer = 0
        Do
            If ail(checkcorrectindex) = g(checkcorrectindex) Then
                blackpegs += 1
                counted(checkcorrectindex) = 1
                correct(checkcorrectindex) = 1
            End If
            checkcorrectindex += 1
        Loop Until checkcorrectindex = holes

        Dim currentstep As Integer = 0
        Do
            If correct(currentstep) = 0 AndAlso ail.Contains(g(currentstep)) Then
                Dim countedindex As Integer = 0
                Dim foundmatch As Boolean = False
                Do
                    If countedindex = currentstep Then
                        countedindex += 1
                        Continue Do
                    ElseIf ail(countedindex) = g(currentstep) AndAlso counted(countedindex) = 0 Then
                        counted(countedindex) = 1
                        whitepegs += 1
                        foundmatch = True
                    End If
                    countedindex += 1
                Loop Until countedindex = holes Or foundmatch = True
            End If
            currentstep += 1
        Loop Until currentstep = holes

        Dim AntallBW(1) As Integer
        AntallBW = {blackpegs, whitepegs}
        Return AntallBW
    End Function


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            LocalInitialList = Nothing
            LocalPossibleList = Nothing

        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
