Public Class Newapp
    Dim _Filename As String
    Dim _defaultE As Double
    Dim _defaultA As Double
    Dim _defaultPJ As Boolean = True
    Dim _defaultScaleFactor As Double

    Public Property Filename() As String
        Get
            Return _Filename
        End Get
        Set(ByVal value As String)
            _Filename = value
        End Set
    End Property

    Public Property defaultE() As Double
        Get
            Return _defaultE
        End Get
        Set(ByVal value As Double)
            _defaultE = value
        End Set
    End Property

    Public Property defaultA() As Double
        Get
            Return _defaultA
        End Get
        Set(ByVal value As Double)
            _defaultA = value
        End Set
    End Property

    Public Property defaultPJ() As Boolean
        Get
            Return _defaultPJ
        End Get
        Set(ByVal value As Boolean)
            _defaultPJ = value
        End Set
    End Property

    Public Property defaultScaleFactor()
        Get
            Return _defaultScaleFactor
        End Get
        Set(ByVal value)
            _defaultScaleFactor = value
        End Set
    End Property


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        _Filename = TextBox1.Text
        _defaultE = Val(TextBox2.Text)
        _defaultA = Val(TextBox3.Text)
        _defaultScaleFactor = Val(TextBox4.Text)
        _defaultPJ = True
        MDIMain.StartAPP(True)
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text = "" Then
            TextBox1.Text = "Truss1"
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IsNumeric(TextBox2.Text) And Val(TextBox2.Text) <> 0 Then

        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If IsNumeric(TextBox3.Text) And Val(TextBox3.Text) <> 0 Then

        Else
            TextBox3.Text = ""
        End If
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        If IsNumeric(TextBox4.Text) And Val(TextBox4.Text) <> 0 Then

        Else
            TextBox4.Text = ""
        End If
    End Sub
End Class