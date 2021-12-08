Public Class OptionD
    Dim _NSint As Integer = 6
    Dim _HSint As Integer = 3
    Dim _MSint As Integer = 3

    Dim _NSnap As Boolean = True
    Dim _HSnap As Boolean = True
    Dim _MSnap As Boolean = True
    Dim _LLeg As Boolean = True
    Dim _NLeg As Boolean = True
    Dim _GLeg As Boolean = True
    Dim _Prec As Integer = 2

    Dim _Mc As System.Drawing.Color = Color.SteelBlue
    Dim _Nc As System.Drawing.Color = Color.Brown
    Dim _Sc As System.Drawing.Color = Color.Aqua
    Dim _Lc As System.Drawing.Color = Color.Blue
    Dim _NNc As System.Drawing.Color = Color.SandyBrown
    Dim _Bc As System.Drawing.Color = Color.WhiteSmoke
    Dim _Gc As System.Drawing.Color = Color.LightGray
    Dim _SSc As System.Drawing.Color = Color.Gold

    Dim Th As String = "Default"

#Region "Get Set property"
#Region "Scale"
    Public ReadOnly Property Prec() As Integer
        Get
            Return _Prec
        End Get
    End Property
#End Region

#Region "Snap"
    Public Property NSnap() As Boolean
        Get
            Return _NSnap
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property

    Public Property HSnap() As Boolean
        Get
            Return _HSnap
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property

    Public Property MSnap() As Boolean
        Get
            Return _MSnap
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property

    Public Property NSint() As Integer
        Get
            Return _NSint
        End Get
        Set(ByVal value As Integer)
            '-----
        End Set
    End Property

    Public Property HSint() As Integer
        Get
            Return _HSint
        End Get
        Set(ByVal value As Integer)
            '-----
        End Set
    End Property

    Public Property MSint() As Integer
        Get
            Return _MSint
        End Get
        Set(ByVal value As Integer)
            '-----
        End Set
    End Property
#End Region

#Region "Legends"
    Public Property LLeg() As Boolean
        Get
            Return _LLeg
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property

    Public Property NLeg() As Boolean
        Get
            Return _NLeg
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property

    Public Property GLeg() As Boolean
        Get
            Return _GLeg
        End Get
        Set(ByVal value As Boolean)
            '-----
        End Set
    End Property
#End Region

#Region "Color"
    Public Property Mc() As System.Drawing.Color
        Get
            Return _Mc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property Nc() As System.Drawing.Color
        Get
            Return _Nc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property Sc() As System.Drawing.Color
        Get
            Return _Sc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property Lc() As System.Drawing.Color
        Get
            Return _Lc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property NNc() As System.Drawing.Color
        Get
            Return _NNc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property Bc() As System.Drawing.Color
        Get
            Return _Bc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property Gc() As System.Drawing.Color
        Get
            Return _Gc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property

    Public Property SSc() As System.Drawing.Color
        Get
            Return _SSc
        End Get
        Set(ByVal value As System.Drawing.Color)
            '-----
        End Set
    End Property
#End Region
#End Region

    Private Sub OptionD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckBox1.Checked = _NSnap
        CheckBox2.Checked = _HSnap
        CheckBox3.Checked = _MSnap
        DomainUpDown1.SelectedIndex = _NSint
        DomainUpDown2.SelectedIndex = _HSint
        DomainUpDown3.SelectedIndex = _MSint
        DomainUpDown5.SelectedIndex = _Prec
        CheckBox4.Checked = _LLeg
        CheckBox5.Checked = _NLeg
        CheckBox6.Checked = _GLeg
        Button1.BackColor = _Mc
        Button2.BackColor = _Nc
        Button3.BackColor = _Sc
        Button4.BackColor = _Lc
        Button5.BackColor = _NNc
        Button7.BackColor = _Bc
        Button6.BackColor = _Gc
        Button8.BackColor = _SSc
        DomainUpDown4.SelectedItem = Th
    End Sub

#Region "Precision Group Box"
    Private Sub DomainUpDown5_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown5.SelectedItemChanged
        _Prec = DomainUpDown5.SelectedItem
    End Sub
#End Region

#Region "Snap Group Box"
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        _NSnap = CheckBox1.Checked
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        _HSnap = CheckBox2.Checked
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        _MSnap = CheckBox3.Checked
    End Sub

    Private Sub DomainUpDown1_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown1.SelectedItemChanged
        _NSint = DomainUpDown1.SelectedItem
    End Sub

    Private Sub DomainUpDown2_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown2.SelectedItemChanged
        _HSint = DomainUpDown2.SelectedItem
    End Sub

    Private Sub DomainUpDown3_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown3.SelectedItemChanged
        _MSint = DomainUpDown3.SelectedItem
    End Sub
#End Region

#Region "Legends Group Box"
    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        _LLeg = CheckBox4.Checked
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        _NLeg = CheckBox5.Checked
    End Sub

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        _GLeg = CheckBox6.Checked
    End Sub
#End Region

#Region "Color Group Box"
    Private Sub Button1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseClick
        ColorDialog1.ShowDialog()
        _Mc = ColorDialog1.Color
        Button1.BackColor = _Mc
    End Sub

    Private Sub Button2_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseClick
        ColorDialog1.ShowDialog()
        _Nc = ColorDialog1.Color
        Button2.BackColor = _Nc
    End Sub

    Private Sub Button4_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button4.MouseClick
        ColorDialog1.ShowDialog()
        _Lc = ColorDialog1.Color
        Button4.BackColor = _Lc
    End Sub

    Private Sub Button5_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button5.MouseClick
        ColorDialog1.ShowDialog()
        _NNc = ColorDialog1.Color
        Button5.BackColor = _NNc
    End Sub

    Private Sub Button3_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button3.MouseClick
        ColorDialog1.ShowDialog()
        _Sc = ColorDialog1.Color
        Button3.BackColor = _Sc
    End Sub

    Private Sub Button7_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button7.MouseClick
        ColorDialog1.ShowDialog()
        _Bc = ColorDialog1.Color
        Button7.BackColor = _Bc
    End Sub

    Private Sub Button6_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button6.MouseClick
        ColorDialog1.ShowDialog()
        _Gc = ColorDialog1.Color
        Button6.BackColor = _Gc
    End Sub

    Private Sub Button8_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button8.MouseClick
        ColorDialog1.ShowDialog()
        _SSc = ColorDialog1.Color
        Button7.BackColor = _SSc
    End Sub

    Private Sub DomainUpDown4_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown4.SelectedItemChanged
        Th = DomainUpDown4.SelectedItem
        FixTheme()
    End Sub

    Private Sub FixTheme()
        If Th = "Default" Then
            _Mc = Color.DarkBlue
            _Nc = Color.Brown
            _Sc = Color.Aqua
            _Lc = Color.Blue
            _NNc = Color.BurlyWood
            _Bc = Color.GhostWhite
            _Gc = Color.MistyRose
            _SSc = Color.Gold
        ElseIf Th = "Theme1" Then

        ElseIf Th = "Theme2" Then

        ElseIf Th = "Theme3" Then

        ElseIf Th = "Theme4" Then

        ElseIf Th = "Theme5" Then

        End If
        Button1.BackColor = _Mc
        Button2.BackColor = _Nc
        Button3.BackColor = _Sc
        Button4.BackColor = _Lc
        Button5.BackColor = _NNc
        Button7.BackColor = _Bc
        Button6.BackColor = _Gc
        Button8.BackColor = _SSc
    End Sub
#End Region

End Class