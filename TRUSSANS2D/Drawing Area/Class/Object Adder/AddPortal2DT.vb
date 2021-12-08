Public Class AddPortal2DT
    '0 - SplitH
    '1 - ClickH
    '2 - Pan
    '3 - Select
    '4 - Move
    '5 - Clone
    '6 -  Mirror
    Dim Spoint As Point
    Dim Epoint As Point

    Public Sub New(ByRef Sp As Point, ByRef Ep As Point, ByVal I As Integer)
        Spoint = Sp
        Epoint = Ep
        WA2dt.PD = New SnapPredicate2DT
        CPredictSnap(Spoint, Epoint)
        If I = 0 Then
            SplitH()
        ElseIf I = 1 Then
            ClickH()
        ElseIf I = 2 Then
            PanH()
        ElseIf I = 3 Then
            SelectH()
        ElseIf I = 4 Then
            MoveH()
        ElseIf I = 5 Then
            CloneH()
        ElseIf I = 6 Then
            MirrorH()
        ElseIf I = 7 Then
            ZoomToFit()
        End If
        WA2dt.PD = New SnapPredicate2DT
    End Sub

#Region "Snap Predicate"
    Private Sub MPredictSnap(ByRef Sp As Point)
        WA2dt.PD.Her = New Line2DT.Node(Sp)
    End Sub

    Private Sub CPredictSnap(ByRef Sp As Point, ByRef Ep As Point)
        WA2dt.PD.Her = New Line2DT.Node(Sp)
        If OptionD.MSnap = True Then
            Sp = If(WA2dt.Mem.Exists(WA2dt.PD.MidptSnapPD) = True, _
                    New Point( _
                        (WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).SN.Coord.X + _
                            WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).EN.Coord.X) / 2, _
                        (WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).SN.Coord.Y + _
                            WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).EN.Coord.Y) / 2), _
                    Sp)
            If WA2dt.Mem.Exists(WA2dt.PD.LineSnapPD) = True Then _
            SnaptoLine(Sp, WA2dt.Mem.Find(WA2dt.PD.LineSnapPD))
        End If
        If OptionD.HSnap = True Then
            Sp.Y = If(WA2dt.Bob.Exists(WA2dt.PD.HorizSnapPD) = True, _
                      WA2dt.Bob.Find(WA2dt.PD.HorizSnapPD).Coord.Y, _
                      Sp.Y)
            Sp.X = If(WA2dt.Bob.Exists(WA2dt.PD.VertSnapPD) = True, _
                      WA2dt.Bob.Find(WA2dt.PD.VertSnapPD).Coord.X, _
                      Sp.X)
        End If
        If OptionD.NSnap = True Then
            Sp = If(WA2dt.Bob.Exists(WA2dt.PD.NodeSnapPD) = True, _
                    WA2dt.Bob.Find(WA2dt.PD.NodeSnapPD).Coord, Sp)
        End If
        WA2dt.PD.Her = New Line2DT.Node(Ep)
        Ep.X = If((Ep.X < Sp.X + (2 / WA2dt.Zm) And Ep.X > Sp.X - (2 / WA2dt.Zm)), Sp.X, Ep.X)
        Ep.Y = If((Ep.Y < Sp.Y + (2 / WA2dt.Zm) And Ep.Y > Sp.Y - (2 / WA2dt.Zm)), Sp.Y, Ep.Y)
        If OptionD.MSnap = True Then
            Ep = If(WA2dt.Mem.Exists(WA2dt.PD.MidptSnapPD) = True, _
                    New Point( _
                        (WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).SN.Coord.X + _
                            WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).EN.Coord.X) / 2, _
                        (WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).SN.Coord.Y + _
                            WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD).EN.Coord.Y) / 2), _
                    Ep)
            If WA2dt.Mem.Exists(WA2dt.PD.LineSnapPD) = True Then _
                SnaptoLine(Ep, WA2dt.Mem.Find(WA2dt.PD.LineSnapPD))
        End If
        If OptionD.HSnap = True Then
            Ep.Y = If(WA2dt.Bob.Exists(WA2dt.PD.HorizSnapPD) = True, _
                      WA2dt.Bob.Find(WA2dt.PD.HorizSnapPD).Coord.Y, _
                      Ep.Y)
            Ep.X = If(WA2dt.Bob.Exists(WA2dt.PD.VertSnapPD) = True, _
                      WA2dt.Bob.Find(WA2dt.PD.VertSnapPD).Coord.X, _
                      Ep.X)
        End If
        If OptionD.NSnap = True Then
            Ep = If(WA2dt.Bob.Exists(WA2dt.PD.NodeSnapPD) = True, _
                    WA2dt.Bob.Find(WA2dt.PD.NodeSnapPD).Coord, _
                    Ep)
        End If
    End Sub

    Private Sub SnaptoLine(ByRef pt As Point, ByVal SnapEL As Line2DT)
        Dim ReTx, ReTy As Double
        ReTx = ((pt.X * (SnapEL.Lcos)) + (pt.Y * (-1 * SnapEL.Msin)))
        ReTy = ((SnapEL.SN.Coord.Y * (SnapEL.Lcos)) + (SnapEL.SN.Coord.X * (SnapEL.Msin)))
        pt.X = ((ReTx * (SnapEL.Lcos)) + (ReTy * (SnapEL.Msin)))
        pt.Y = ((ReTy * (SnapEL.Lcos)) + (ReTx * (-1 * SnapEL.Msin)))
    End Sub

    Private Sub MCMPredictSnap(ByRef Sp As Point, ByRef Ep As Point)
        WA2dt.PD.Her = New Line2DT.Node(Sp)
        Sp = If(WA2dt.Bob.Exists(WA2dt.PD.NodeSnapPD) = True, _
                WA2dt.Bob.Find(WA2dt.PD.NodeSnapPD).Coord, _
                Sp)
        WA2dt.PD.Her = New Line2DT.Node(Ep)
        Ep = If(WA2dt.Bob.Exists(WA2dt.PD.NodeSnapPD) = True, _
                WA2dt.Bob.Find(WA2dt.PD.NodeSnapPD).Coord, _
                Ep)
    End Sub
#End Region

#Region "ClickH"
    Private Sub SplitH()
        WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
        SplitElt(Spoint)
    End Sub

    Private Sub ClickH()
        SplitElt(Epoint)
        '-------Check for zero length
        If ZeroLength(Spoint, Epoint) = False Then
            WA2dt.Mem.Add( _
                    New Line2DT( _
                        New Line2DT.Node(Spoint), _
                        New Line2DT.Node(Epoint)))
        End If
    End Sub

    Private Sub SplitElt(ByVal Tpt As Point)
        Dim S, E As Point
        Dim SnapEL As Line2DT

        Dim B As New SnapPredicate2DT(True)
        B.Her = New Line2DT.Node(Tpt)
        If WA2dt.Mem.Exists(B.LineSnapPD) = True Then
            SnapEL = WA2dt.Mem.Find(B.LineSnapPD)
            S = SnapEL.SN.Coord
            E = SnapEL.EN.Coord
            SnaptoLine(Tpt, SnapEL)
            WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
            If ZeroLength(S, Tpt) = False And ZeroLength(Tpt, E) = False Then
                WA2dt.Mem.Add(New Line2DT( _
                                            New Line2DT.Node(S), _
                                            New Line2DT.Node(Tpt)))
                WA2dt.Mem.Add(New Line2DT( _
                                    New Line2DT.Node(Tpt), _
                                    New Line2DT.Node(E)))
                WA2dt.Mem.Remove(SnapEL)
            End If

        End If
    End Sub
#End Region

#Region "ArcH"
    Public Sub New(ByRef APts() As Point, ByRef seg As Integer) '--Arc Adding
        Dim NoPts As Integer = APts.Length - 1
        Dim ss As Integer = NoPts / seg
        If ss = 0 Then
            MsgBox("Splits as minimum to" & NoPts)
            ss = 1
        End If
        WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
        Dim SPts As New List(Of Point)
        Dim EPts As New List(Of Point)
        Dim i As Integer = 0
        While i < NoPts
            SPts.Add(APts(i))
            i = i + ss
            EPts.Add(APts(If(i < NoPts, i, NoPts)))
        End While
        i = 0
        For Each P In SPts
            If ZeroLength(New Point(SPts(i).X - WA2dt.Mpoint.X, SPts(i).Y - WA2dt.Mpoint.Y), New Point(EPts(i).X - WA2dt.Mpoint.X, EPts(i).Y - WA2dt.Mpoint.Y)) = False Then
                WA2dt.Mem.Add( _
                         New Line2DT( _
                             New Line2DT.Node( _
                                 New Point(SPts(i).X, SPts(i).Y)), _
                             New Line2DT.Node( _
                                 New Point(EPts(i).X, EPts(i).Y))))
                i = i + 1
            End If
        Next
        WA2dt.PD = New SnapPredicate2DT
    End Sub
#End Region

#Region "PanH"
    Private Sub PanH()
        WA2dt.Mpoint = New Point _
        (WA2dt.Mpoint.X + (Epoint.X - Spoint.X), WA2dt.Mpoint.Y + (Epoint.Y - Spoint.Y))
    End Sub
#End Region

#Region "Zoom To Fit"
    Private Sub ZoomToFit()
        Dim Xavg As Double
        Dim Yavg As Double
        For Each Nd In WA2dt.Bob
            Xavg = Xavg + Nd.Coord.X
            Yavg = Yavg + Nd.Coord.Y
        Next
        Xavg = Xavg / WA2dt.Bob.Count
        Yavg = Yavg / WA2dt.Bob.Count
        WA2dt.Mpoint = New Point( _
                            WA2dt.Mpoint.X + (Xavg - WA2dt.Mpoint.X), _
                            WA2dt.Mpoint.Y + (Yavg - WA2dt.Mpoint.Y))
    End Sub
#End Region

#Region "SelectH"
    Public Sub New(ByRef Pt As Point)
        Dim DupPD As New Predicate(Of Integer)(AddressOf FindDuplicates)
        Dim Tpd As New SnapPredicate2DT(2, True)
        Tpd.Her = New Line2DT.Node(New Point(Pt.X, Pt.Y))
        If WA2dt.Mem.Exists(Tpd.SelectPD) = True Then
            B = WA2dt.Mem.FindIndex(Tpd.SelectPD)
            If WA2dt.selLine.Exists(DupPD) = False Then
                WA2dt.selLine.Add(B)
            Else
                WA2dt.selLine.Remove(B)
            End If
        End If
        Exit Sub
    End Sub

    Private Sub SelectH()
        Dim DupPD As New Predicate(Of Integer)(AddressOf FindDuplicates)
        Dim rect As System.Drawing.Rectangle
        FindRect(rect)
        For Each El In WA2dt.Mem
            B = WA2dt.Mem.IndexOf(El)
            Dim Pt0, Pt12, Pt25, Pt37, Pt50, Pt62, Pt75, Pt87, Pt100 As Point
            Pt0 = New Point(El.SN.Coord.X, El.SN.Coord.Y)
            Pt100 = New Point(El.EN.Coord.X, El.EN.Coord.Y)
            Pt50 = New Point((El.EN.Coord.X + El.SN.Coord.X) / 2, (El.EN.Coord.Y + El.SN.Coord.Y) / 2)
            Pt25 = New Point((El.SN.Coord.X + _
                                ((El.EN.Coord.X + El.SN.Coord.X) / 2)) / 2, _
                             (El.SN.Coord.Y + _
                                ((El.EN.Coord.Y + El.SN.Coord.Y) / 2)) / 2)
            Pt75 = New Point((El.EN.Coord.X + _
                                ((El.EN.Coord.X + El.SN.Coord.X) / 2)) / 2, _
                            (El.EN.Coord.Y + _
                                ((El.EN.Coord.Y + El.SN.Coord.Y) / 2)) / 2)
            Pt12 = New Point((Pt0.X + Pt25.X) / 2, (Pt0.Y + Pt25.Y) / 2)
            Pt37 = New Point((Pt25.X + Pt50.X) / 2, (Pt25.Y + Pt50.Y) / 2)
            Pt62 = New Point((Pt50.X + Pt75.X) / 2, (Pt50.Y + Pt75.Y) / 2)
            Pt87 = New Point((Pt75.X + Pt100.X) / 2, (Pt75.Y + Pt100.Y) / 2)
            If (rect.Contains(Pt0) _
                Or rect.Contains(Pt100)) _
                Or rect.Contains(Pt50) _
                Or rect.Contains(Pt25) _
                Or rect.Contains(Pt75) _
                Or rect.Contains(Pt12) _
                Or rect.Contains(Pt37) _
                Or rect.Contains(Pt62) _
                Or rect.Contains(Pt87) _
                AndAlso WA2dt.selLine.Exists(DupPD) = False Then
                WA2dt.selLine.Add(WA2dt.Mem.IndexOf(El))
            End If
        Next
    End Sub

    Dim B As Integer

    Private Function FindDuplicates(ByVal A As Integer)
        FindDuplicates = If(A = B, True, False)
    End Function

    Private Sub FindRect(ByRef R As Rectangle)
        Dim pt1, pt2 As PointF
        If (Epoint.X - Spoint.X) > 0 And (Epoint.Y - Spoint.Y) > 0 Then
            pt1 = New PointF(Spoint.X, Spoint.Y)
            pt2 = New PointF(Epoint.X, Epoint.Y)
        ElseIf (Epoint.X - Spoint.X) > 0 Then
            pt1 = New PointF(Spoint.X, Epoint.Y)
            pt2 = New PointF(Epoint.X, Spoint.Y)
        ElseIf (Epoint.Y - Spoint.Y) > 0 Then
            pt1 = New PointF(Epoint.X, Spoint.Y)
            pt2 = New PointF(Spoint.X, Epoint.Y)
        Else
            pt1 = New PointF(Epoint.X, Epoint.Y)
            pt2 = New PointF(Spoint.X, Spoint.Y)
        End If
        R = New Rectangle(New Point(pt1.X, pt1.Y), New Size(pt2.X - pt1.X, pt2.Y - pt1.Y))
    End Sub
#End Region

#Region "DeleteH" ' -- Special handler included for Load ADD & DELETE -- To find maximum load and scaling all load according to that
    Public Sub New(ByRef SL As List(Of Integer))
        Try
            WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
            WA2dt.Bob.Clear()
            SL.Reverse()
            For Each ind In SL
                WA2dt.Mem.RemoveAt(ind)
            Next
            For Each El In WA2dt.Mem
                AttachNode(El.SN)
                AttachNode(El.EN)
            Next
            'FixMaxload(0)
            SL.Clear()
            WA2dt.MTPic.Refresh()
        Catch ex As Exception
            MsgBox("Error found in Delete sequence -- Index error -- Code DELETEINDEX", MsgBoxStyle.Critical, "TrussANS")
            SL.Clear()
            WA2dt.MTPic.Refresh()
        End Try
    End Sub

    Public Sub New(ByVal currentload As Double)
        FixMaxload(currentload)
    End Sub

    Private Sub AttachNode(ByRef Nd As Line2DT.Node)
        Dim Chk As Boolean = False
        If WA2dt.Bob.Count <> 0 Then
            For Each TNd In WA2dt.Bob
                If TNd.Coord.X = Nd.Coord.X And TNd.Coord.Y = Nd.Coord.Y Then
                    Nd.ModifyIndex(TNd.index)
                    Chk = True
                End If
            Next
        End If
        If Chk = False Then
            WA2dt.Bob.Add(New Line2DT.Node(Nd.Coord, WA2dt.Bob.Count))
            WA2dt.Bob(WA2dt.Bob.Count - 1).Load = New Line2DT.Node.LoadCondition(Nd.Load.Loadintensity, Nd.Load.Loadinclination)
            WA2dt.Bob(WA2dt.Bob.Count - 1).Support = New Line2DT.Node.SupportCondition(Nd.Support.PJ, Nd.Support.PS, Nd.Support.RS, Nd.Support.supportinclination, Nd.Support.settlementdx, Nd.Support.settlementdy)
            Nd.ModifyIndex(WA2dt.Bob.Count - 1)
        End If

        'WA2dt.PD.Her = New Line2DT.Node(Nd.Coord)
        'If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
        '    Nd.Coord = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD).Coord
        '    Nd.index = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD).index
        '    Nd.Load = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD).Load
        '    Nd.Support = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD).Support
        'Else
        '    WA2dt.Bob.Add(New Line2DT.Node(Nd.Coord, WA2dt.Bob.Count))
        '    WA2dt.Bob(WA2dt.Bob.Count - 1).Load = New Line2DT.Node.LoadCondition(Nd.Load.Loadintensity, Nd.Load.Loadinclination)
        '    WA2dt.Bob(WA2dt.Bob.Count - 1).Support = New Line2DT.Node.SupportCondition(Nd.Support.PJ, Nd.Support.PS, Nd.Support.RS, Nd.Support.supportinclination, Nd.Support.settlementdx, Nd.Support.settlementdy)
        '    Nd.ModifyIndex(WA2dt.Bob.Count - 1)
        'End If
    End Sub

    Public Sub FixMaxload(ByVal curload As Double)
        If WA2dt.Mem.Count <> 0 Then
            Dim LengthAvg As Double
            WA2dt.Maxload = If(Math.Abs(curload) <> 0, Math.Abs(curload), 0.000001)
            WA2dt.Loadhtfactor = 1

            For Each EL In WA2dt.Mem
                WA2dt.MaxLength = If(EL.Length > WA2dt.MaxLength, EL.Length, WA2dt.MaxLength)
                LengthAvg = LengthAvg + EL.Length
                If Math.Abs(EL.SN.Load.Loadintensity) > WA2dt.Maxload Then
                    WA2dt.Maxload = Math.Abs(EL.SN.Load.Loadintensity)
                End If
                If Math.Abs(EL.EN.Load.Loadintensity) > WA2dt.Maxload Then
                    WA2dt.Maxload = Math.Abs(EL.EN.Load.Loadintensity)
                End If
            Next
            WA2dt.MinLength = WA2dt.MaxLength
            For Each EL In WA2dt.Mem
                WA2dt.MinLength = If(EL.Length < WA2dt.MinLength, EL.Length, WA2dt.MinLength)
            Next
            LengthAvg = LengthAvg / WA2dt.Mem.Count
            WA2dt.Loadhtfactor = (LengthAvg * MDIMain.Nappdefaults.defaultScaleFactor) * 0.3
            WA2dt.Loadhtfactor = 50
            WA2dt.UStreamGap = WA2dt.MaxLength
            WA2dt.MTPic.Refresh()
        End If
    End Sub
#End Region

#Region "MoveH"
    Private Sub MoveH()
        Dim Dx, Dy As Double
        Dx = Epoint.X - Spoint.X
        Dy = Epoint.Y - Spoint.Y
        WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
        Dim Spt, Ept As Point
        For Each ind In WA2dt.selLine
            Spt = New Point(WA2dt.Mem(ind).SN.Coord.X + Dx, _
                            WA2dt.Mem(ind).SN.Coord.Y + Dy)
            Ept = New Point(WA2dt.Mem(ind).EN.Coord.X + Dx, _
                            WA2dt.Mem(ind).EN.Coord.Y + Dy)
            MCMPredictSnap(Spt, Ept)
            If ZeroLength(Spt, Ept) = False Then
                WA2dt.Mem.Add(New Line2DT( _
                                               New Line2DT.Node(Spt), _
                                              New Line2DT.Node(Ept)))
            End If
        Next


        WA2dt.Bob.Clear()
        WA2dt.selLine.Reverse()
        For Each ind In WA2dt.selLine
            WA2dt.Mem.RemoveAt(ind)
        Next
        For Each El In WA2dt.Mem
            AttachNode(El.SN)
            AttachNode(El.EN)
        Next
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MTPic.Refresh()
    End Sub
#End Region

#Region "CloneH"
    Private Sub CloneH()
        Dim Dx, Dy As Double
        Dx = Epoint.X - Spoint.X
        Dy = Epoint.Y - Spoint.Y
        WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
        Dim Spt, Ept As Point
        For Each ind In WA2dt.selLine
            Spt = New Point(WA2dt.Mem(ind).SN.Coord.X + Dx, WA2dt.Mem(ind).SN.Coord.Y + Dy)
            Ept = New Point(WA2dt.Mem(ind).EN.Coord.X + Dx, WA2dt.Mem(ind).EN.Coord.Y + Dy)
            MCMPredictSnap(Spt, Ept)
            If ZeroLength(Spt, Ept) = False Then
                WA2dt.Mem.Add(New Line2DT( _
                                              New Line2DT.Node(Spt), _
                                              New Line2DT.Node(Ept)))
            End If
        Next
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MTPic.Refresh()
    End Sub
#End Region

#Region "MirrorH"
    Private Sub MirrorH()
        Dim Ext1 As New Point(WA2dt.Spoint.X, WA2dt.Spoint.Y)
        Dim Ext2 As New Point(WA2dt.Epoint.X, WA2dt.Epoint.Y)
        WA2dt.HistU.Add(New History2DT(WA2dt.Mem, WA2dt.Bob, WA2dt.Zm, WA2dt.Mpoint))
        FlipTempLine(Ext1, Ext2)
        If Ext1.X = Ext2.X Then
            AddHorizFlip(Ext1.X)
        ElseIf Ext1.Y = Ext2.Y Then
            AddVertFlip(Ext1.Y)
        End If
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MTPic.Refresh()
    End Sub

    Private Sub FlipTempLine(ByRef e1 As Point, ByRef e2 As Point)
        If Math.Abs(e2.X - e1.X) > Math.Abs(e2.Y - e1.Y) Then
            e2.Y = e1.Y
        Else
            e2.X = e1.X
        End If
    End Sub

    Private Sub AddHorizFlip(ByRef MirX As Double)
        For Each ind In WA2dt.selLine
            Dim Spt As New Point(-(WA2dt.Mem(ind).SN.Coord.X) + (2 * MirX), (WA2dt.Mem(ind).SN.Coord.Y))
            Dim Ept As New Point(-(WA2dt.Mem(ind).EN.Coord.X) + (2 * MirX), (WA2dt.Mem(ind).EN.Coord.Y))
            MCMPredictSnap(Spt, Ept)

            'Dim Pl As New List(Of Line2DT.P)
            'For Each L In WA2dt.Mem(ind).Pload
            '    Pl.Add(New Line2DT.P(L.pload, (WA2dt.Mem(ind).Length - L.plocation), L.PCoordS, L.PCoordE, L.MAlign, L.HAlign, L.VAlign))
            'Next
            'WA2dt.Mem(ind).Pload.Clear()
            'WA2dt.Mem(ind).Pload.AddRange(Pl)

            'Dim Ul As New List(Of Line2DT.U)
            'For Each L In WA2dt.Mem(ind).Uload
            '    Ul.Add(New Line2DT.U(L.uload1, L.uload2, (WA2dt.Mem(ind).Length - L.ulocation2), (WA2dt.Mem(ind).Length - L.ulocation1), L.uCoordS1, L.uCoordE1, L.uCoordS2, L.uCoordE2, L.MAlign, L.HAlign, L.VAlign, L.uStream1, L.uStream2))
            'Next
            'WA2dt.Mem(ind).Uload.Clear()
            'WA2dt.Mem(ind).Uload.AddRange(Ul)

            'Dim Ml As New List(Of Line2DT.M)
            'For Each L In WA2dt.Mem(ind).Mload
            '    Ml.Add(New Line2DT.M(L.mload, (WA2dt.Mem(ind).Length - L.mlocation), L.mCoord, L.mClockwise, L.mAnticlockwise))
            'Next
            'WA2dt.Mem(ind).Mload.Clear()
            'WA2dt.Mem(ind).Mload.AddRange(Ml)
            If ZeroLength(Spt, Ept) = False Then
                WA2dt.Mem.Add(New Line2DT( _
                                               New Line2DT.Node(Spt), _
                                               New Line2DT.Node(Ept)))
            End If
        Next
    End Sub

    Private Sub AddVertFlip(ByRef MirY As Double)
        For Each ind In WA2dt.selLine
            Dim Spt As New Point((WA2dt.Mem(ind).SN.Coord.X), -(WA2dt.Mem(ind).SN.Coord.Y) + (2 * MirY))
            Dim Ept As New Point((WA2dt.Mem(ind).EN.Coord.X), -(WA2dt.Mem(ind).EN.Coord.Y) + (2 * MirY))
            MCMPredictSnap(Spt, Ept)

            'Dim Pl As New List(Of Line2DT.P)
            'For Each L In WA2dt.Mem(ind).Pload
            '    Pl.Add(New Line2DT.P(L.pload, (WA2dt.Mem(ind).Length - L.plocation), L.PCoordS, L.PCoordE, L.MAlign, L.HAlign, L.VAlign))
            'Next
            'WA2dt.Mem(ind).Pload.Clear()
            'WA2dt.Mem(ind).Pload.AddRange(Pl)

            'Dim Ul As New List(Of Line2DT.U)
            'For Each L In WA2dt.Mem(ind).Uload
            '    Ul.Add(New Line2DT.U(L.uload1, L.uload2, (WA2dt.Mem(ind).Length - L.ulocation2), (WA2dt.Mem(ind).Length - L.ulocation1), L.uCoordS1, L.uCoordE1, L.uCoordS2, L.uCoordE2, L.MAlign, L.HAlign, L.VAlign, L.uStream1, L.uStream2))
            'Next
            'WA2dt.Mem(ind).Uload.Clear()
            'WA2dt.Mem(ind).Uload.AddRange(Ul)

            'Dim Ml As New List(Of Line2DT.M)
            'For Each L In WA2dt.Mem(ind).Mload
            '    Ml.Add(New Line2DT.M(L.mload, (WA2dt.Mem(ind).Length - L.mlocation), L.mCoord, L.mClockwise, L.mAnticlockwise))
            'Next
            'WA2dt.Mem(ind).Mload.Clear()
            'WA2dt.Mem(ind).Mload.AddRange(Ml)
            If ZeroLength(Spt, Ept) = False Then
                WA2dt.Mem.Add(New Line2DT( _
                                              New Line2DT.Node(Spt), _
                                              New Line2DT.Node(Ept)))
            End If
        Next
    End Sub
#End Region

#Region "Add Load"
    '---Public Sub FixMaxload  (Find in DeleteH)
#End Region

    Private Function ZeroLength(ByVal Spoint As Point, ByVal Epoint As Point) As Boolean
        If Math.Sqrt(Math.Pow((Epoint.X - Spoint.X), 2) + Math.Pow((Epoint.Y - Spoint.Y), 2)) < 2 Then
            Return True
        End If
        Return False
    End Function
End Class
