Public Class ResultPaint
    Private Zmfactor As Double

    Public Sub New(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal PaintNull As Boolean, ByVal Mpt As Point)
        If PaintNull = False Then
            Zmfactor = RA2dt.Zm
            e.Graphics.TranslateTransform(Mpt.X, Mpt.Y)
            If MDIMain._memberstress.Checked = True Then
                _stress_Paint(e)
            ElseIf MDIMain._memberforce.Checked = True Then
                _force_Paint(e)
            ElseIf MDIMain._memberstrain.Checked = True Then
                _strain_Paint(e)
            ElseIf MDIMain._memberdeformation.Checked = True Then
                _deformation_Paint(e)
            End If
        End If
    End Sub

    Private Sub _stress_Paint(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim StressColor As New System.Drawing.Color
        For Each El In RA2dt.Rmem
            StressColor = Color.FromArgb(El.SsertsRed, El.SsertsGreen, El.SsertsBlue)
            e.Graphics.DrawLine(New Pen(StressColor, 2 / Zmfactor), El.SN.Coord.X, El.SN.Coord.Y, El.EN.Coord.X, El.EN.Coord.Y)
            If MDIMain.ResultviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Stress, 6), New Font("Verdana", (8 / Zmfactor)), New Pen(StressColor, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2), ((El.SN.Coord.Y + El.EN.Coord.Y) / 2))
            End If
        Next
        For Each Nd In RA2dt.Rbob
            e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(Nd.Coord.X - (2 / Zmfactor)), CInt(Nd.Coord.Y - (2 / Zmfactor)), CInt((4 / Zmfactor)), CInt((4 / Zmfactor)))
        Next
    End Sub

    Private Sub _force_Paint(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim ForceColor As New System.Drawing.Color
        For Each El In RA2dt.Rmem
            ForceColor = Color.FromArgb(El.EcorfRed, El.EcorfGreen, El.EcorfBlue)
            e.Graphics.DrawLine(New Pen(ForceColor, 2 / Zmfactor), El.SN.Coord.X, El.SN.Coord.Y, El.EN.Coord.X, El.EN.Coord.Y)
            If MDIMain.ResultviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Force, 6), New Font("Verdana", (8 / Zmfactor)), New Pen(ForceColor, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2), ((El.SN.Coord.Y + El.EN.Coord.Y) / 2))
            End If
        Next
        For Each Nd In RA2dt.Rbob
            e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(Nd.Coord.X - (2 / Zmfactor)), CInt(Nd.Coord.Y - (2 / Zmfactor)), CInt((4 / Zmfactor)), CInt((4 / Zmfactor)))
        Next
    End Sub

    Private Sub _strain_Paint(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim StrainColor As New System.Drawing.Color
        For Each El In RA2dt.Rmem
            StrainColor = Color.FromArgb(El.NiartsRed, El.NiartsGreen, El.NiartsBlue)
            e.Graphics.DrawLine(New Pen(StrainColor, 2 / Zmfactor), El.SN.Coord.X, El.SN.Coord.Y, El.EN.Coord.X, El.EN.Coord.Y)
            If MDIMain.ResultviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Strain, 6), New Font("Verdana", (8 / Zmfactor)), New Pen(StrainColor, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2), ((El.SN.Coord.Y + El.EN.Coord.Y) / 2))
            End If
        Next
        For Each Nd In RA2dt.Rbob
            e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(Nd.Coord.X - (2 / Zmfactor)), CInt(Nd.Coord.Y - (2 / Zmfactor)), CInt((4 / Zmfactor)), CInt((4 / Zmfactor)))
        Next
    End Sub

    Private Sub _deformation_Paint(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim dispColor As New System.Drawing.Pen(Color.Chocolate, 2 / Zmfactor)
        dispColor.DashStyle = Drawing2D.DashStyle.Dot
        For Each El In RA2dt.Rmem
            e.Graphics.DrawLine(dispColor, El.SN.DisplacedCoord.X, El.SN.DisplacedCoord.Y, El.EN.DisplacedCoord.X, El.EN.DisplacedCoord.Y)
        Next
        For Each Nd In RA2dt.Rbob
            e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(Nd.DisplacedCoord.X - (2 / Zmfactor)), CInt(Nd.DisplacedCoord.Y - (2 / Zmfactor)), CInt((4 / Zmfactor)), CInt((4 / Zmfactor)))
            If MDIMain.ResultviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Nd.reactionStr, New System.Drawing.Font("Verdana", CInt(8 / Zmfactor), FontStyle.Bold), Brushes.Green, Nd.DisplacedCoord.X - (2 / Zmfactor), Nd.DisplacedCoord.Y - (2 / Zmfactor))
                e.Graphics.DrawString(Nd.displacementStr, New System.Drawing.Font("Verdana", CInt(8 / Zmfactor), FontStyle.Bold), Brushes.Chocolate, Nd.DisplacedCoord.X - (2 / Zmfactor), Nd.DisplacedCoord.Y - (2 / Zmfactor))
            End If
        Next
    End Sub
End Class
