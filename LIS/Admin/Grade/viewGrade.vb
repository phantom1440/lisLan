Imports System.Data.SqlClient
Public Class viewGrade
    Dim stdno, name, course, yrlevel As String

    Private Sub comboboxsemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboboxsemester.SelectedIndexChanged
        loadsubjects()
    End Sub

    Sub New(stdno As String, name As String, course As String, yrlevel As String)
        InitializeComponent()
        Me.stdno = stdno
        Me.name = name
        Me.course = course
        Me.yrlevel = yrlevel
    End Sub
    Private Sub viewGrade_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        stdno_lbl.Text = stdno
        name_lbl.Text = name
        comboboxsemester.SelectedIndex = 0
        loadsubjects()
    End Sub
    Sub loadsubjects()
        subjectBox.Controls.Clear()
        Using con As New SqlConnection(My.Settings.connection)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.CommandText = $"select subjects.code, subjects.subject, grades.id, grade from grades inner join subjects on subjects.id = grades.subjectid where grades.studentid='{stdno}' and  subjects.semester = '{comboboxsemester.SelectedItem}'"
                Using reader As SqlDataReader = cmd.ExecuteReader
                    While reader.Read
                        subjectBox.Controls.Add(New subject(reader.Item(0).ToString, reader.Item(1).ToString, reader.Item(2).ToString, reader.Item(3).ToString))
                    End While
                End Using
            End Using
            con.Close()
        End Using
    End Sub
End Class