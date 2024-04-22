Imports System.IO
Imports System.Data.SqlClient
Public Class root
    Dim rootPath As String = Application.StartupPath & "\LisData"
    Dim path As String
    Dim nextPath As String
    Dim filename As String

    Private Sub root_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        On Error Resume Next
        CreateDirectoryInRoot()
        createCoursesDirectory()
        path = rootPath & "\LIS"
        LoadDirectory(path)
    End Sub

    Private Sub createCoursesDirectory()
        Try
            Dim mpath = rootPath & "\LIS"
            Using con As New SqlConnection(My.Settings.connection)
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    cmd.CommandText = "select Course from course"
                    Using reader As SqlDataReader = cmd.ExecuteReader

                        While reader.Read
                            If Not Directory.Exists(mpath & "\" & reader.Item("Course")) Then
                                Directory.CreateDirectory(mpath & "\" & reader.Item("Course"))
                                CreateYrlevelDirectory(mpath & "\" & reader.Item("Course"))
                            Else
                                CreateYrlevelDirectory(mpath & "\" & reader.Item("Course"))
                            End If
                        End While
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CreateYrlevelDirectory(path As String)
        Try
            Using con As New SqlConnection(My.Settings.connection)
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    cmd.CommandText = "select Yearlevel from yearlevel"
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        While reader.Read
                            If Not Directory.Exists(path & "\" & reader.Item("Yearlevel")) Then
                                Directory.CreateDirectory(path & "\" & reader.Item("Yearlevel"))
                                createSectionDirectory(path & "\" & reader.Item("Yearlevel"))
                            Else
                                createSectionDirectory(path & "\" & reader.Item("Yearlevel"))
                            End If
                        End While
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub createSectionDirectory(path As String)
        If Not Directory.Exists(path & "\" & "A") Then
            Directory.CreateDirectory(path & "\" & "A")
        End If
        If Not Directory.Exists(path & "\" & "B") Then
            Directory.CreateDirectory(path & "\" & "B")
        End If
    End Sub
    Private Sub CreateDirectoryInRoot()
        Try
            If Not Directory.Exists(rootPath) Then
                Directory.CreateDirectory(rootPath)
            End If
        Catch ex As Exception
            MessageBox.Show("Error creating directory: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadDirectory(ByVal directoryPath As String)
        On Error Resume Next
        path = directoryPath
        ListView1.Clear()
        For Each dir As String In My.Computer.FileSystem.GetDirectories(path)
            ListView1.Items.Add(dir.Substring(dir.LastIndexOf("\") + 1), ImageList1.Images.Count() - 2)
        Next
        For Each file As String In My.Computer.FileSystem.GetFiles(path)
            ListView1.Items.Add(file.Substring(file.LastIndexOf("\") + 1), ImageList1.Images.Count() - 1)
        Next
    End Sub

    Private Sub ListView1_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles ListView1.ItemSelectionChanged
        nextPath = path + "\" + e.Item.Text
        filename = ListView1.FocusedItem.Text
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        On Error Resume Next

        If (My.Computer.FileSystem.DirectoryExists(nextPath)) Then
            path = nextPath
            LoadDirectory(path)
        Else
            Diagnostics.Process.Start(path + "\" + filename)
        End If
    End Sub

    Private Sub backBtn_Click(sender As Object, e As EventArgs) Handles backBtn.Click
        Dim parentDirectory As String = GetParentDirectory(path)

        If parentDirectory IsNot Nothing AndAlso Not parentDirectory.Equals(rootPath) Then
            path = parentDirectory
            LoadDirectory(path)
        Else
            MessageBox.Show("You are already at the root directory.")
        End If
    End Sub

    Private Function GetParentDirectory(ByVal directoryPath As String) As String
        If Not String.IsNullOrEmpty(directoryPath) Then
            Dim lastIndex As Integer = directoryPath.LastIndexOf("\")
            If lastIndex >= 0 Then
                Return directoryPath.Substring(0, lastIndex)
            End If
        End If
        Return Nothing
    End Function
End Class
