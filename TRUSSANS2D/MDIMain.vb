Imports System.Windows.Forms
Imports System.IO
Imports System.Collections.Specialized
Imports System.Runtime.Serialization.Formatters.Binary

Public Class MDIMain
    Private _O As Boolean = False
    Private _Cursorimg As Image
    Private _Napplication As Newapp

    Public Property Oindex() As Integer
        Get
            Return _O
        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property Cursorimg() As Image
        Get
            Return _Cursorimg
        End Get
        Set(ByVal value As Image)
            _Cursorimg = value
        End Set
    End Property

    Public Property Nappdefaults() As Newapp
        Get
            Return _Napplication
        End Get
        Set(ByVal value As Newapp)
            _Napplication = value
        End Set
    End Property

#Region "Oval Events"
    Private Sub _filemenu_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _filemenu.MouseLeave
        _filemenu.Image = My.Resources.File
    End Sub

    Private Sub _filemenu_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles _filemenu.MouseHover
        _filemenu.Image = My.Resources.FileH
    End Sub

    Private Sub _filemenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _filemenu.Click
        Dim pt As System.Drawing.Point
        pt.X = 0
        pt.Y = 0 + _filemenu.Height
        ContextMenuStrip1.Show(Me.PointToScreen(pt))
    End Sub
#End Region '-------------------Show Off

#Region "File Menu Events"
#Region "New"
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Try
            If WA2dt.HistU.Count <> 0 Then
                Dim R As MsgBoxResult = MsgBox("Do You Wanna Save Your Progress ?", MsgBoxStyle.YesNoCancel, "TrussANS")
                If R = MsgBoxResult.Yes Then
                    Dim SW As New SaveFileDialog
                    SW.DefaultExt = ".2dt"
                    SW.Filter = "Samson Mano's 2D - Object Files (*.2dt)|*.2dt"
                    DialogResult = SW.ShowDialog()
                    ClosingCurrent(True)
                ElseIf R = MsgBoxResult.No Then
                    ClosingCurrent(True)
                ElseIf R = MsgBoxResult.Cancel Then
                    Exit Sub
                    ToolStateChange()
                End If
            Else
                ClosingCurrent(True)
            End If
        Catch ex As Exception
            ClosingCurrent(True)
            ToolStateChange()
        End Try
        ToolStateChange()
    End Sub

    Private Sub ClosingCurrent(ByVal NewOPening As Boolean)
        'If _O = True Then
        Try
            WA2dt.HistU.Clear()
            WA2dt.Dispose()
            WA2dt.Close()
            RA2dt.Dispose()
            RA2dt.Close()

            _analyze.Checked = True
            StartAnalysis()
        Catch ex As Exception

        End Try
        Me._Commandtxtbox.Visible = False
        Me._Commandtxtbox.Text = ""
        'End If

        ToolStrip1.Enabled = True
        _line.Checked = False
        _arc.Checked = False
        _selectM.Checked = False
        _move.Checked = False
        _clone.Checked = False
        _mirror.Checked = False

        _line.Enabled = False
        _arc.Enabled = False
        _selectM.Enabled = False
        _move.Enabled = False
        _clone.Enabled = False
        _mirror.Enabled = False
        _analyze.Enabled = False
        _viewOption.Enabled = False

        If NewOPening = True Then
            _Napplication = New Newapp
            _Napplication.ShowDialog()
        End If

    End Sub

    Public Sub StartAPP(ByVal i As Boolean)
        _O = i
        ToolStrip1.Enabled = True

        '--ToolBar
        _line.Checked = True
        _arc.Checked = False
        _selectM.Checked = False
        _move.Checked = False
        _clone.Checked = False
        _mirror.Checked = False

        _line.Enabled = True
        _arc.Enabled = True
        _selectM.Enabled = True
        _move.Checked = False
        _clone.Checked = False
        _mirror.Checked = False
        _analyze.Enabled = True
        _viewOption.Enabled = True

        '--Context Menu
        LineToolStripMenuItem.Checked = True
        ArcToolStripMenuItem.Checked = False
        SelectMToolStripMenuItem.Checked = False
        MoveToolStripMenuItem.Enabled = False
        CloneToolStripMenuItem.Enabled = False
        MirrorToolStripMenuItem.Enabled = False

        _Cursorimg = My.Resources.line
        If _O = True Then
            CreateElement2DT()
            Me._Commandtxtbox.Visible = False
            Me._Commandtxtbox.Text = ""
            WA2dt.Show()
        End If        '_DMCMenable()

    End Sub
#End Region

#Region "Open "
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Try
            If WA2dt.HistU.Count <> 0 Then
                Dim R As MsgBoxResult = MsgBox("Do You Wanna Save Your Progress ?", MsgBoxStyle.YesNoCancel, "TrussANS")
                If R = MsgBoxResult.Yes Then
                    Dim SW As New SaveFileDialog
                    SW.DefaultExt = ".2dt"
                    SW.Filter = "Samson Mano's 2D - Object Files (*.2dt)|*.2dt"
                    DialogResult = SW.ShowDialog()
                    ClosingCurrent(False)
                ElseIf R = MsgBoxResult.No Then
                    ClosingCurrent(False)
                    OpeningExisting()
                ElseIf R = MsgBoxResult.Cancel Then
                    Exit Sub
                End If
            Else
                ClosingCurrent(False)
                OpeningExisting()
            End If
        Catch ex As Exception
            ClosingCurrent(False)
            OpeningExisting()
        End Try
    End Sub

    Private Sub OpeningExisting()
        Dim OW As New OpenFileDialog
        OW.DefaultExt = ".2dt"
        OW.Filter = "Samson Mano's 2D - Object Files (*.2dt)|*.2dt"
        OW.ShowDialog()
        If File.Exists(OW.FileName) Then
            Dim trobject As New List(Of Object)
            Dim gsf As Stream = File.OpenRead(OW.FileName)
            Dim deserializer As New BinaryFormatter
            Try
                trobject = CType(deserializer.Deserialize(gsf), Object)
                StartAPP(True)
            Catch ex As Exception
                MessageBox.Show("Sorry!!!!! Unable to Open.. File Reading Error", "Samson Mano", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            Dim instance1 As List(Of Line2DT) = trobject(0)
            WA2dt.Mem = instance1
            Dim instance2 As List(Of Line2DT.Node) = trobject(1)
            WA2dt.Bob = instance2
            Dim instance3 As Double = trobject(2)
            WA2dt.Zm = instance3
            Dim instance4 As Point = trobject(3)
            WA2dt.Mpoint = instance4
            _Napplication = New Newapp
            Dim instance5 As String = trobject(4)
            _Napplication.Filename = instance5
            Dim instance6 As Boolean = trobject(5)
            _Napplication.defaultPJ = instance6
            Dim instance7 As Double = trobject(6)
            _Napplication.defaultE = instance7
            Dim instance8 As Double = trobject(7)
            _Napplication.defaultA = instance8
            Dim instance9 As Double = trobject(8)
            _Napplication.defaultScaleFactor = instance9


            WA2dt.HistU.Clear()
            WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))

            Dim A As New AddPortal2DT(0)
            WA2dt.MainPic.Refresh()
        End If
        ToolStateChange()
    End Sub
#End Region

#Region "Save"
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Dim SW As New SaveFileDialog
        SW.DefaultExt = ".2dt"
        SW.Filter = "Samson Mano's 2D - Object Files (*.2dt)|*.2dt"
        SW.FileName = Nappdefaults.Filename
        DialogResult = SW.ShowDialog()

        If SW.FileName <> "" Then
            Dim trobject As New List(Of Object)
            trobject.Add(WA2dt.Mem)
            trobject.Add(WA2dt.Bob)
            trobject.Add(WA2dt.Zm)
            trobject.Add(WA2dt.Mpoint)
            trobject.Add(_Napplication.Filename)
            trobject.Add(_Napplication.defaultPJ)
            trobject.Add(_Napplication.defaultE)
            trobject.Add(_Napplication.defaultA)
            trobject.Add(_Napplication.defaultScaleFactor)

            Dim psf As Stream = File.Create(SW.FileName)
            Dim serializer As New BinaryFormatter
            serializer.Serialize(psf, trobject)
            psf.Close()
        End If
    End Sub
#End Region

#Region "Close"
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If ExitHandler() = True Then
            Me.Dispose()
            Me.Close()
        Else
            Exit Sub
        End If
    End Sub

    Private Function ExitHandler() As Boolean
        Try
            If WA2dt.HistU.Count <> 0 Then
                Dim R As MsgBoxResult = MsgBox("Do You Wanna Save Your Progress ?", MsgBoxStyle.YesNoCancel, "TrussANS")
                If R = MsgBoxResult.Yes Then
                    Dim SW As New SaveFileDialog
                    SW.DefaultExt = ".2dt"
                    SW.Filter = "Samson Mano's 2D - Object Files (*.2dt)|*.2dt"
                    DialogResult = SW.ShowDialog()
                    Return True
                ElseIf R = MsgBoxResult.No Then
                    Return True
                ElseIf R = MsgBoxResult.Cancel Then
                    Return False
                    Exit Function
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            Return True
        End Try
        Return True
    End Function
#End Region
#End Region

#Region "Tool Bar controls"
    Private Sub ToolStateChange()
        If _line.Checked = True Then
            _HelpStatusLabel.Text = "Click to Locate Start Point and Click again to Locate End Point"
        ElseIf _arc.Checked = True Then
            _HelpStatusLabel.Text = "Click - Click to Locate start point & end point of BaseLengh and Click again to Locate the Rise, Input Box will appear asking for number of segments"
        ElseIf _selectM.Checked = True Then
            _HelpStatusLabel.Text = "Click and drag to multiselect, select specific Double member - enables load adding for that particular member"
        ElseIf _selectM.Checked = True Then
            If _move.Checked = True Then
                _HelpStatusLabel.Text = "Click to Locate Start Point and Click again to Locate End Point for Cut - Paste (Move)"
            ElseIf _clone.Checked = True Then
                _HelpStatusLabel.Text = "Click to Locate Start Point and Click again to Locate End Point for Copy - Paste (Clone)"
            ElseIf _mirror.Checked = True Then
                _HelpStatusLabel.Text = "Click - Click for Horizontal or Vertical flip (Flip)"
            End If
        ElseIf _analyze.Checked = True Then
            If _memberforce.Checked = True Then
                _HelpStatusLabel.Text = "Analysis successful - Result - Member forces"
            ElseIf _memberstress.Checked Then
                _HelpStatusLabel.Text = "Analysis successful - Result - Member stress"
            ElseIf _memberstrain.Checked = True Then
                _HelpStatusLabel.Text = "Analysis successful - Result - Member strain"
            ElseIf _memberdeformation.Checked = True Then
                _HelpStatusLabel.Text = "Analysis successful - Elastic deformation of structure & Reaction in the supports"
            End If
        Else
            _HelpStatusLabel.Text = "Email to <saminnx@gmail.com> Samson Mano"
        End If
        _Commandtxtbox.Visible = False
        _commandtxtlabel.Visible = False
    End Sub
#End Region

#Region "Zone 1"
    Private Sub _line_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _line.Click, LineToolStripMenuItem.Click
        '--Tool Bar
        _line.Checked = True
        _arc.Checked = False
        _selectM.Checked = False
        '--Context Menu
        LineToolStripMenuItem.Checked = True
        ArcToolStripMenuItem.Checked = False
        SelectMToolStripMenuItem.Checked = False
        _DMCMenable()
        _Cursorimg = My.Resources.line
    End Sub

    Private Sub _arc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _arc.Click, ArcToolStripMenuItem.Click
        '--Tool Bar
        _line.Checked = False
        _arc.Checked = True
        _selectM.Checked = False

        '--Context Menu
        LineToolStripMenuItem.Checked = False
        ArcToolStripMenuItem.Checked = True
        SelectMToolStripMenuItem.Checked = False
        _DMCMenable()
        _Cursorimg = My.Resources.arc
    End Sub

    Private Sub _selectM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _selectM.Click, SelectMToolStripMenuItem.Click
        '--Tool Bar
        _line.Checked = False
        _arc.Checked = False
        _selectM.Checked = True
        '--Context Menu
        LineToolStripMenuItem.Checked = False
        ArcToolStripMenuItem.Checked = False
        SelectMToolStripMenuItem.Checked = True
        _Cursorimg = My.Resources.selectmember
    End Sub

    Private Sub unselectcoztabhandler() Handles _line.Click, LineToolStripMenuItem.Click, _arc.Click, ArcToolStripMenuItem.Click, _selectM.Click, SelectMToolStripMenuItem.Click
        WA2dt.selLine.Clear()
        ToolStateChange()
    End Sub
#End Region

#Region "Zone 2"
    Public Sub _DMCMenable()
        If _O = True Then
            If WA2dt.selLine.Count = 0 Then
                '--Tool Bar
                _delete.Enabled = False
                _move.Enabled = False
                _clone.Enabled = False
                _mirror.Enabled = False

                _move.Checked = False
                _clone.Checked = False
                _mirror.Checked = False

                _memberproperties.Enabled = False
                _AddLoad.Enabled = False
                _Addsupport.Enabled = False

                '--Context Menu
                DeleteToolStripMenuItem.Enabled = False
                MoveToolStripMenuItem.Enabled = False
                CloneToolStripMenuItem.Enabled = False
                MirrorToolStripMenuItem.Enabled = False

                MoveToolStripMenuItem.Enabled = False
                CloneToolStripMenuItem.Enabled = False
                MirrorToolStripMenuItem.Enabled = False
                MemberpropertiesToolStripMenuItem.Enabled = False
                AddloadToolStripMenuItem.Enabled = False
                AddsupportToolStripMenuItem.Enabled = False

                _Cursorimg = My.Resources.selectmember
            Else
                '--Tool Bar
                _delete.Enabled = True
                _move.Enabled = True
                _clone.Enabled = True
                _mirror.Enabled = True

                If WA2dt.selLine.Count = 1 Then
                    _memberproperties.Enabled = True
                    _AddLoad.Enabled = True
                    _Addsupport.Enabled = True

                    MemberpropertiesToolStripMenuItem.Enabled = True
                    AddloadToolStripMenuItem.Enabled = True
                    AddsupportToolStripMenuItem.Enabled = True
                Else
                    _memberproperties.Enabled = False
                    _AddLoad.Enabled = False
                    _Addsupport.Enabled = False

                    MemberpropertiesToolStripMenuItem.Enabled = False
                    AddloadToolStripMenuItem.Enabled = False
                    AddsupportToolStripMenuItem.Enabled = False
                End If


                '--Context Menu
                DeleteToolStripMenuItem.Enabled = True
                MoveToolStripMenuItem.Enabled = True
                CloneToolStripMenuItem.Enabled = True
                MirrorToolStripMenuItem.Enabled = True
            End If
        ElseIf _O = 2 Then

        End If
        ToolStateChange()
    End Sub

    Private Sub _delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _delete.Click, DeleteToolStripMenuItem.Click
        If _O = True Then
            If WA2dt.selLine.Count <> 0 Then
                WA2dt.AP = New AddPortal2DT(WA2dt.selLine)
                WA2dt.selLine.Clear()
            End If
            _DMCMenable()
        ElseIf _O = 2 Then

        End If
    End Sub

    Private Sub _move_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _move.Click, MoveToolStripMenuItem.Click
        '--Tool Bar
        _move.Checked = True
        _clone.Checked = False
        _mirror.Checked = False
        '--Context Menu
        MoveToolStripMenuItem.Enabled = True
        CloneToolStripMenuItem.Enabled = False
        MirrorToolStripMenuItem.Enabled = False
        _Cursorimg = My.Resources.move
    End Sub

    Private Sub _clone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _clone.Click, CloneToolStripMenuItem.Click
        '--Tool Bar
        _move.Checked = False
        _clone.Checked = True
        _mirror.Checked = False
        '--Context Menu
        MoveToolStripMenuItem.Enabled = False
        CloneToolStripMenuItem.Enabled = True
        MirrorToolStripMenuItem.Enabled = False
        _Cursorimg = My.Resources.clone
    End Sub

    Private Sub _mirror_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _mirror.Click, MirrorToolStripMenuItem.Click
        '--Tool Bar
        _move.Checked = False
        _clone.Checked = False
        _mirror.Checked = True
        '--Context Menu
        MoveToolStripMenuItem.Enabled = False
        CloneToolStripMenuItem.Enabled = False
        MirrorToolStripMenuItem.Enabled = True
        _Cursorimg = My.Resources.Mirror
    End Sub
#End Region

#Region "Zone 3"
    Private Sub _Memberproperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberproperties.Click, MemberpropertiesToolStripMenuItem.Click
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New Member_Properties(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Member Properties Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try

    End Sub

    Private Sub _AddLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _AddLoad.Click, AddloadToolStripMenuItem.Click
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New LoadWindow(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("AddLoad Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try

    End Sub

    Private Sub _Addsupport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _Addsupport.Click, AddsupportToolStripMenuItem.Click
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New SupportBoundaryConditionWindow(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Support - Boundary Condition Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try
    End Sub
#End Region

#Region "Zone 4"
    Private Sub _analyze_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _analyze.Click, AnalyzeToolStripMenuItem.Click
        'Start Analysis
        StartAnalysis()
        ToolStateChange()
    End Sub

    Private Sub _memberforce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberforce.Click, MemberForceToolStripMenuItem.Click
        If _memberforce.Checked = False Then
            _memberforce.Checked = True
            _memberstress.Checked = False
            _memberstrain.Checked = False
            _memberdeformation.Checked = False

            MemberForceToolStripMenuItem.Checked = True
            MemberStressToolStripMenuItem.Checked = False
            DeformationToolStripMenuItem.Checked = False
            MemberStrainToolStripMenuItem.Checked = False
            Me.Cursorimg = My.Resources.Force
            RA2dt.MainPic.Refresh()
            ToolStateChange()
        End If
    End Sub

    Private Sub _memberstress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberstress.Click, MemberStressToolStripMenuItem.Click
        If _memberstress.Checked = False Then
            _memberforce.Checked = False
            _memberstress.Checked = True
            _memberstrain.Checked = False
            _memberdeformation.Checked = False

            MemberForceToolStripMenuItem.Checked = False
            MemberStressToolStripMenuItem.Checked = True
            DeformationToolStripMenuItem.Checked = False
            MemberStrainToolStripMenuItem.Checked = False
            Me.Cursorimg = My.Resources.stress
            RA2dt.MainPic.Refresh()
            ToolStateChange()
        End If
    End Sub

    Private Sub _memberstrain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberstrain.Click, MemberStrainToolStripMenuItem.Click
        If _memberstrain.Checked = False Then
            _memberforce.Checked = False
            _memberstress.Checked = False
            _memberstrain.Checked = True
            _memberdeformation.Checked = False

            MemberForceToolStripMenuItem.Checked = False
            MemberStressToolStripMenuItem.Checked = False
            DeformationToolStripMenuItem.Checked = False
            MemberStrainToolStripMenuItem.Checked = True
            Me.Cursorimg = My.Resources.strain
            RA2dt.MainPic.Refresh()
            ToolStateChange()
        End If
    End Sub

    Private Sub _memberdeformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberdeformation.Click, DeformationToolStripMenuItem.Click
        If _memberdeformation.Checked = False Then
            _memberforce.Checked = False
            _memberstress.Checked = False
            _memberstrain.Checked = False
            _memberdeformation.Checked = True

            MemberForceToolStripMenuItem.Checked = False
            MemberStressToolStripMenuItem.Checked = False
            DeformationToolStripMenuItem.Checked = True
            MemberStrainToolStripMenuItem.Checked = False
            Me.Cursorimg = My.Resources.displacement
            RA2dt.MainPic.Refresh()
            ToolStateChange()
        End If
    End Sub

    Private Sub _matrix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _matrix.Click
        RA2dt.Analyzer.MatrixUpdater.ShowDialog()
    End Sub
#End Region

#Region "Command Box"
    Dim _txtinputflg As Boolean = False

    Public Property txtinputflg() As Boolean
        Get
            Return _txtinputflg
        End Get
        Set(ByVal value As Boolean)
            _txtinputflg = value
        End Set
    End Property

    Private Sub _Commandtxtbox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _Commandtxtbox.KeyDown
        If IsNumeric(_Commandtxtbox.Text) Then
            _txtinputflg = True
        End If

        If e.KeyCode = Keys.Enter Then
            'MsgBox(_Commandtxtbox.Text)
            _txtinputflg = False
            If (Me._line.Checked = True And WA2dt.Linflg = True) Or (Me._arc.Checked = True And WA2dt.Arcflg = True) Then
                Dim Length As Double
                Dim Angle As Double
                Dim GL As String = ""
                Dim GT As String = ""
                Dim i, j As Integer

                For i = 0 To (_Commandtxtbox.TextLength - 1)
                    If _Commandtxtbox.Text.Chars(i) = " " Then
                        GoTo s1
                    End If
                    GL = GL + _Commandtxtbox.Text.Chars(i)
                Next
s1:
                For j = i To (_Commandtxtbox.TextLength - 1)
                    GT = GT + _Commandtxtbox.Text.Chars(j)
                Next

                Length = Val(GL)
                Angle = Val(GT)

                If Length = 0 Then
                    Exit Sub
                End If

                _txtinputflg = False
                'MsgBox("Length = " & Length)
                'MsgBox("Theta = " & Angle)

                '----To Find EPOINT and pass it to AddPortal
                Dim Epoint As Point
                Dim C As Double = Math.Cos(Angle * (Math.PI / 180))
                Dim S As Double = Math.Sin(Angle * (Math.PI / 180))


                Epoint = New Point(WA2dt.Spoint.X + (Length * Me.Nappdefaults.defaultScaleFactor * C + 0 * Me.Nappdefaults.defaultScaleFactor * S), WA2dt.Spoint.Y + (-Length * Me.Nappdefaults.defaultScaleFactor * S + 0 * Me.Nappdefaults.defaultScaleFactor * C))
                If Me._line.Checked = True Then
                    Dim _AP As AddPortal2DT = New AddPortal2DT(WA2dt.Spoint, Epoint, 1)
                    WA2dt.Mem(WA2dt.Mem.Count - 1).Length = Length
                    WA2dt.Linflg = False
                ElseIf Me._arc.Checked = True Then
                    WA2dt.Epoint = Epoint
                    WA2dt.Linflg = True
                    WA2dt.Arcflg = False
                End If

                _Commandtxtbox.Visible = False
                _commandtxtlabel.Visible = False

                WA2dt.MainPic.Refresh()
            End If
        End If
    End Sub

    Private Sub _Commandtxtbox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _Commandtxtbox.KeyPress
        If e.KeyChar = ControlChars.Cr Then
            e.Handled = True
        End If
    End Sub

    Private Sub _Commandtxtbox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Commandtxtbox.LostFocus
        _txtinputflg = False
        _Commandtxtbox.SelectionLength = _Commandtxtbox.TextLength
    End Sub

    Private Sub _Commandtxtbox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _Commandtxtbox.MouseWheel
        WA2dt.MainPic.Focus()
    End Sub

    Private Sub _Commandtxtbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _Commandtxtbox.TextChanged
        'If IsNumeric(_Commandtxtbox.Text) Then

        'Else
        '    _Commandtxtbox.Text = ""
        'End If
        _Commandtxtbox.SelectionLength = If(_txtinputflg = False, _Commandtxtbox.TextLength, Nothing)
        '_Commandtxtbox.Visible = If(WA2dt.Linflg = False Or Me._line.Checked = False, False, True)
    End Sub
#End Region

    Private Sub MDIMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If ExitHandler() = True Then
            Exit Sub
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub MDIMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WA2dt = New WorkArea2DT
        RA2dt = New ResultArea2DT

        ToolStrip1.Enabled = True
        _line.Checked = False
        _arc.Checked = False
        _selectM.Checked = False
        _AddLoad.Checked = False
        _Addsupport.Checked = False
        _memberforce.Checked = False
        _memberstress.Checked = False
        _memberstrain.Checked = False
        _memberdeformation.Checked = False

        _line.Enabled = False
        _arc.Enabled = False
        _selectM.Enabled = False
        _AddLoad.Enabled = False
        _Addsupport.Enabled = False
        _memberforce.Enabled = False
        _memberstress.Enabled = False
        _memberstrain.Enabled = False
        _memberdeformation.Enabled = False
        _analyze.Enabled = False
        _viewOption.Enabled = False

        AddHandler _line.CheckedChanged, AddressOf ToolStateChange
        AddHandler _arc.CheckedChanged, AddressOf ToolStateChange
        AddHandler _selectM.CheckedChanged, AddressOf ToolStateChange
        AddHandler _move.CheckedChanged, AddressOf ToolStateChange
        AddHandler _clone.CheckedChanged, AddressOf ToolStateChange
        AddHandler _mirror.CheckedChanged, AddressOf ToolStateChange


        _move.Checked = False
        _clone.Checked = False
        _mirror.Checked = False

        ToolStateChange()
    End Sub

End Class
