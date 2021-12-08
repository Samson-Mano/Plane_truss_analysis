Module Indexes
    Public WA2dt As WorkArea2DT
    Public RA2dt As ResultArea2DT
   
    Public Sub CreateElement2DT()
        WA2dt = New WorkArea2DT
        WA2dt.MdiParent = MDIMain
        WA2dt.Dock = DockStyle.Fill
    End Sub

    Public Sub StartAnalysis()
        If MDIMain._analyze.Checked = True Then
            MDIMain._analyze.Checked = False
            '--Tool Bar
            MDIMain._line.Enabled = True
            MDIMain._arc.Enabled = True
            MDIMain._selectM.Enabled = True

            MDIMain._line.Checked = False
            MDIMain._arc.Checked = False
            MDIMain._selectM.Checked = False

            MDIMain._delete.Enabled = False

            MDIMain._move.Enabled = False
            MDIMain._clone.Enabled = False
            MDIMain._mirror.Enabled = False

            MDIMain._move.Checked = False
            MDIMain._clone.Checked = False
            MDIMain._mirror.Checked = False


            '--Context Menu
            MDIMain._memberproperties.Enabled = False
            MDIMain._AddLoad.Enabled = False
            MDIMain._Addsupport.Enabled = False

            MDIMain.LineToolStripMenuItem.Enabled = True
            MDIMain.ArcToolStripMenuItem.Enabled = True
            MDIMain.SelectMToolStripMenuItem.Enabled = True

            MDIMain.LineToolStripMenuItem.Checked = True
            MDIMain.ArcToolStripMenuItem.Checked = False
            MDIMain.SelectMToolStripMenuItem.Checked = False


            MDIMain.DeleteToolStripMenuItem.Enabled = False
            MDIMain.MoveToolStripMenuItem.Enabled = False
            MDIMain.CloneToolStripMenuItem.Enabled = False
            MDIMain.MirrorToolStripMenuItem.Enabled = False
            MDIMain.MemberpropertiesToolStripMenuItem.Enabled = False
            MDIMain.AddloadToolStripMenuItem.Enabled = False
            MDIMain.AddsupportToolStripMenuItem.Enabled = False

            MDIMain.MemberForceToolStripMenuItem.Enabled = False
            MDIMain.MemberStressToolStripMenuItem.Enabled = False
            MDIMain.DeformationToolStripMenuItem.Enabled = False
            MDIMain.MemberStrainToolStripMenuItem.Enabled = False

            MDIMain.MemberForceToolStripMenuItem.Checked = False
            MDIMain.MemberStressToolStripMenuItem.Checked = False
            MDIMain.DeformationToolStripMenuItem.Checked = False
            MDIMain.MemberStrainToolStripMenuItem.Checked = False

            MDIMain._memberforce.Enabled = False
            MDIMain._memberstress.Enabled = False
            MDIMain._memberdeformation.Enabled = False
            MDIMain._memberstrain.Enabled = False
            MDIMain._matrix.Enabled = False

            MDIMain.MemberForceToolStripMenuItem.Checked = False
            MDIMain._memberforce.Checked = False
            MDIMain._memberstress.Checked = False
            MDIMain._memberdeformation.Checked = False
            MDIMain._memberstrain.Checked = False

            MDIMain.Cursorimg = My.Resources.line

            MDIMain._line.Checked = True

            MDIMain._analyze.Image = My.Resources.analyze

            RA2dt.Dispose()
            RA2dt.Close()
            WA2dt.Show()
        Else
            MDIMain._analyze.Checked = True

            WA2dt.selLine.Clear()
            WA2dt.Linflg = False
            WA2dt.Arcflg = False

            '--Tool Bar
            MDIMain._line.Enabled = False
            MDIMain._arc.Enabled = False
            MDIMain._selectM.Enabled = False

            MDIMain._line.Checked = False
            MDIMain._arc.Checked = False
            MDIMain._selectM.Checked = False

            MDIMain._delete.Enabled = False

            MDIMain._move.Enabled = False
            MDIMain._clone.Enabled = False
            MDIMain._mirror.Enabled = False

            MDIMain._move.Checked = False
            MDIMain._clone.Checked = False
            MDIMain._mirror.Checked = False


            '--Context Menu
            MDIMain._memberproperties.Enabled = False
            MDIMain._AddLoad.Enabled = False
            MDIMain._Addsupport.Enabled = False

            MDIMain.LineToolStripMenuItem.Enabled = False
            MDIMain.ArcToolStripMenuItem.Enabled = False
            MDIMain.SelectMToolStripMenuItem.Enabled = False

            MDIMain.LineToolStripMenuItem.Checked = False
            MDIMain.ArcToolStripMenuItem.Checked = False
            MDIMain.SelectMToolStripMenuItem.Checked = False

            MDIMain.DeleteToolStripMenuItem.Enabled = False
            MDIMain.MoveToolStripMenuItem.Enabled = False
            MDIMain.CloneToolStripMenuItem.Enabled = False
            MDIMain.MirrorToolStripMenuItem.Enabled = False
            MDIMain.MemberpropertiesToolStripMenuItem.Enabled = False
            MDIMain.AddloadToolStripMenuItem.Enabled = False
            MDIMain.AddsupportToolStripMenuItem.Enabled = False

            MDIMain.Cursorimg = My.Resources.Force

            MDIMain.MemberForceToolStripMenuItem.Enabled = True
            MDIMain.MemberStressToolStripMenuItem.Enabled = True
            MDIMain.DeformationToolStripMenuItem.Enabled = True
            MDIMain.MemberStrainToolStripMenuItem.Enabled = True

            MDIMain._memberforce.Enabled = True
            MDIMain._memberstress.Enabled = True
            MDIMain._memberdeformation.Enabled = True
            MDIMain._memberstrain.Enabled = True
            MDIMain._matrix.Enabled = True

            MDIMain.MemberForceToolStripMenuItem.Checked = True
            MDIMain._memberforce.Checked = True


            MDIMain._analyze.Image = My.Resources.create
            WA2dt.Hide()

            '-----Main Analysis Call <Nicholas Tesla is a honourable scientist and my favourite>
            CreateResult2DT()
            RA2dt.Analyzer = New Tesla
            If RA2dt.Analyzer.Failed = False Then
                RA2dt.Show()
            ElseIf RA2dt.Analyzer.Failed = True Then
                MDIMain._analyze.Checked = True
                StartAnalysis()
            End If
        End If
    End Sub

    Private Sub CreateResult2DT()
        RA2dt = New ResultArea2DT
        RA2dt.MdiParent = MDIMain
        RA2dt.Dock = DockStyle.Fill
    End Sub
End Module
