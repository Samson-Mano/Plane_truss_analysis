Imports System.Math
Public Class Truss_FE_Solver
    '-------- Class Equiped with FEA Truss Analyzer ---------
    '------------- Programmed by Samson Mano ----------------
    '--------------------------------------------------------
    Dim _Failed As Boolean = False
    Dim _ResultMem As List(Of AnalysisResultMem)
    Dim _updater As AnalyzerUpdate
    Dim _updateStr As String = ""

    Public ReadOnly Property Failed() As Boolean
        Get
            Return _Failed
        End Get
    End Property

    Public ReadOnly Property MatrixUpdater() As AnalyzerUpdate
        Get
            Return _updater
        End Get
    End Property

#Region "Material Holding Class"
    Public Class AnalysisResultMem
        Public _DegreeOFFreedomMatrix(3) As Double
        Public _SettlementMatrix(3) As Double
        'Public _LLoadVectorMatrix(5) As Double
        Public _GLoadVectorMatrix(3) As Double
        'Public _LElementStiffnessMatrix(5, 5) As Double
        Public _GElementStiffnessMatrix(3, 3) As Double
        'Public _LResultMatrix(5) As Double
        Public _GResultMatrix(3) As Double
        Public _GNodeResultMatrix(3) As Double

        Dim _StartNodeNo As Integer
        Dim _EndNodeNo As Integer
        Dim _Transformation_Mat(3, 3) As Double '-- Transformation matrix for support condition
        Dim _Transformation_MatT(3, 3) As Double
        Dim ElDetails As Line2DT

        Public ReadOnly Property EL() As Line2DT
            Get
                Return ElDetails
            End Get
        End Property

        Public Property StartNodeNo() As Integer
            Get
                Return _StartNodeNo
            End Get
            Set(ByVal value As Integer)
                '-----------
            End Set
        End Property

        Public Property EndNodeNo() As Integer
            Get
                Return _EndNodeNo
            End Get
            Set(ByVal value As Integer)
                '-----------
            End Set
        End Property

        Public Sub New(ByVal EL As Line2DT)
            ElDetails = EL
            FixDegreeOfFreedom(EL)
            FixLoadVector(EL)
            FixElementStiffness(EL)
            For Each Node In WA2dt.Bob
                If EL.SN.Coord.Equals(Node.Coord) Then
                    _StartNodeNo = WA2dt.Bob.IndexOf(Node)
                End If
                If EL.EN.Coord.Equals(Node.Coord) Then
                    _EndNodeNo = WA2dt.Bob.IndexOf(Node)
                End If
            Next
        End Sub

        Private Sub FixDegreeOfFreedom(ByRef EL As Line2DT)
            '-----------START NODE BOUNDARY CONDITION
            If EL.SN.Support.PJ = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 1

                _SettlementMatrix(0) = 0
                _SettlementMatrix(1) = 0
            ElseIf EL.SN.Support.PS = True Then
                _DegreeOFFreedomMatrix(0) = 0
                _DegreeOFFreedomMatrix(1) = 0

                If EL.SN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(0) = EL.SN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(0) = 0
                End If
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
            ElseIf EL.SN.Support.RS = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 0

                _SettlementMatrix(0) = 0
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
            End If
            '-------------END NODE BOUNDARY CONDITION
            If EL.EN.Support.PJ = True Then
                _DegreeOFFreedomMatrix(2) = 1
                _DegreeOFFreedomMatrix(3) = 1

                _SettlementMatrix(2) = 0
                _SettlementMatrix(3) = 0
            ElseIf EL.EN.Support.PS = True Then
                _DegreeOFFreedomMatrix(2) = 0
                _DegreeOFFreedomMatrix(3) = 0

                If EL.EN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(2) = EL.EN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(2) = 0
                End If
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(3) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(3) = 0
                End If
            ElseIf EL.EN.Support.RS = True Then
                _DegreeOFFreedomMatrix(2) = 1
                _DegreeOFFreedomMatrix(3) = 0

                _SettlementMatrix(2) = 0
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(3) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(3) = 0
                End If
            End If
        End Sub

        Private Sub FixLoadVector(ByVal EL As Line2DT)
            _GLoadVectorMatrix(0) = (EL.SN.Load.Loadintensity * Math.Cos(EL.SN.Load.Loadinclination))
            _GLoadVectorMatrix(1) = (EL.SN.Load.Loadintensity * Math.Sin(EL.SN.Load.Loadinclination))

            _GLoadVectorMatrix(2) = (EL.EN.Load.Loadintensity * Math.Cos(EL.EN.Load.Loadinclination))
            _GLoadVectorMatrix(3) = (EL.EN.Load.Loadintensity * Math.Sin(EL.EN.Load.Loadinclination))
        End Sub

        Private Sub FixElementStiffness(ByRef EL As Line2DT)
            '-----------Element Stiffness matrix - Global Coordinates --------------
            Dim K1 As Double = (EL.A * EL.E) / EL.Length
            Dim Lcos As Double = (EL.EN.Coord.X - EL.SN.Coord.X) _
                        / Math.Round(
                                Math.Sqrt(
                                    Math.Pow((EL.SN.Coord.X - EL.EN.Coord.X), 2) +
                                    Math.Pow((EL.SN.Coord.Y - EL.EN.Coord.Y), 2)))
            Dim Msin As Double = (EL.EN.Coord.Y - EL.SN.Coord.Y) _
                        / Math.Round(
                                Math.Sqrt(
                                    Math.Pow((EL.SN.Coord.X - EL.EN.Coord.X), 2) +
                                    Math.Pow((EL.SN.Coord.Y - EL.EN.Coord.Y), 2)))
            Dim V1 As Double = K1 * (Lcos ^ 2)
            Dim V2 As Double = K1 * (Msin ^ 2)
            Dim V3 As Double = K1 * (Msin * Lcos)

            '-----------START NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If EL.SN.Support.PJ = True Then
                _Transformation_Mat(0, 0) = 1
                _Transformation_Mat(0, 1) = 0
                _Transformation_Mat(1, 0) = 0
                _Transformation_Mat(1, 1) = 1

                _Transformation_MatT(0, 0) = 1
                _Transformation_MatT(0, 1) = 0
                _Transformation_MatT(1, 0) = 0
                _Transformation_MatT(1, 1) = 1
            ElseIf EL.SN.Support.PS = True Then
                _Transformation_Mat(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(0, 1) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 0) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))

                _Transformation_MatT(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(0, 1) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 0) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
            ElseIf EL.SN.Support.RS = True Then
                _Transformation_Mat(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(0, 1) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 0) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))

                _Transformation_MatT(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(0, 1) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 0) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
            End If
            '-------------END NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If EL.EN.Support.PJ = True Then
                _Transformation_Mat(2, 2) = 1
                _Transformation_Mat(2, 3) = 0
                _Transformation_Mat(3, 2) = 0
                _Transformation_Mat(3, 3) = 1

                _Transformation_MatT(2, 2) = 1
                _Transformation_MatT(2, 3) = 0
                _Transformation_MatT(3, 2) = 0
                _Transformation_MatT(3, 3) = 1
            ElseIf EL.EN.Support.PS = True Then
                _Transformation_Mat(2, 2) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(2, 3) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 2) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))

                _Transformation_MatT(2, 2) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(2, 3) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 2) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
            ElseIf EL.EN.Support.RS = True Then
                _Transformation_Mat(2, 2) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(2, 3) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 2) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))

                _Transformation_MatT(2, 2) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(2, 3) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 2) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
            End If

            '--- Row1
            _GElementStiffnessMatrix(0, 0) = V1
            _GElementStiffnessMatrix(0, 1) = V3
            _GElementStiffnessMatrix(0, 2) = -V1
            _GElementStiffnessMatrix(0, 3) = -V3

            '--- Row2
            _GElementStiffnessMatrix(1, 0) = V3
            _GElementStiffnessMatrix(1, 1) = V2
            _GElementStiffnessMatrix(1, 2) = -V3
            _GElementStiffnessMatrix(1, 3) = -V2

            '--- Row3
            _GElementStiffnessMatrix(2, 0) = -V1
            _GElementStiffnessMatrix(2, 1) = -V3
            _GElementStiffnessMatrix(2, 2) = V1
            _GElementStiffnessMatrix(2, 3) = V3

            '--- Row4
            _GElementStiffnessMatrix(3, 0) = -V3
            _GElementStiffnessMatrix(3, 1) = -V2
            _GElementStiffnessMatrix(3, 2) = V3
            _GElementStiffnessMatrix(3, 3) = V2


            '---Matrix Multiplication KT
            Dim KTmatrix(3, 3) As Double
            MatrixMultiplication(_GElementStiffnessMatrix, 3, 3, _Transformation_Mat, 3, 3, KTmatrix)
            '---Matrix Multiplication TtKT
            MatrixMultiplication(_Transformation_MatT, 3, 3, KTmatrix, 3, 3, _GElementStiffnessMatrix)

        End Sub

        Public Sub FixGlobalMatrices(ByRef DOFmatrix() As Double, ByRef FERmatrix() As Double, ByRef Settlementmatrix() As Double, ByRef GSmatrix(,) As Double)
            '-----------Degree of Freedom Matrix
            DOFmatrix((_StartNodeNo * 2) + 0) = _DegreeOFFreedomMatrix(0)
            DOFmatrix((_StartNodeNo * 2) + 1) = _DegreeOFFreedomMatrix(1)
            DOFmatrix((_EndNodeNo * 2) + 0) = _DegreeOFFreedomMatrix(2)
            DOFmatrix((_EndNodeNo * 2) + 1) = _DegreeOFFreedomMatrix(3)

            '-----------Nodal Reaction Matrix
            FERmatrix((_StartNodeNo * 2) + 0) = _GLoadVectorMatrix(0)
            FERmatrix((_StartNodeNo * 2) + 1) = _GLoadVectorMatrix(1)
            FERmatrix((_EndNodeNo * 2) + 0) = _GLoadVectorMatrix(2)
            FERmatrix((_EndNodeNo * 2) + 1) = _GLoadVectorMatrix(3)

            '-----------Settlement Matrix
            Settlementmatrix((_StartNodeNo * 2) + 0) = _SettlementMatrix(0)
            Settlementmatrix((_StartNodeNo * 2) + 1) = _SettlementMatrix(1)
            Settlementmatrix((_EndNodeNo * 2) + 0) = _SettlementMatrix(2)
            Settlementmatrix((_EndNodeNo * 2) + 1) = _SettlementMatrix(3)

            '-----------Global Stiffness matrix --------------
            '--- Column1
            GSmatrix((_StartNodeNo * 2) + 0, (_StartNodeNo * 2) + 0) = GSmatrix((_StartNodeNo * 2) + 0, (_StartNodeNo * 2) + 0) + _GElementStiffnessMatrix(0, 0)
            GSmatrix((_StartNodeNo * 2) + 0, (_StartNodeNo * 2) + 1) = GSmatrix((_StartNodeNo * 2) + 0, (_StartNodeNo * 2) + 1) + _GElementStiffnessMatrix(0, 1)
            GSmatrix((_StartNodeNo * 2) + 0, (_EndNodeNo * 2) + 0) = GSmatrix((_StartNodeNo * 2) + 0, (_EndNodeNo * 2) + 0) + _GElementStiffnessMatrix(0, 2)
            GSmatrix((_StartNodeNo * 2) + 0, (_EndNodeNo * 2) + 1) = GSmatrix((_StartNodeNo * 2) + 0, (_EndNodeNo * 2) + 1) + _GElementStiffnessMatrix(0, 3)

            '--- Column2
            GSmatrix((_StartNodeNo * 2) + 1, (_StartNodeNo * 2) + 0) = GSmatrix((_StartNodeNo * 2) + 1, (_StartNodeNo * 2) + 0) + _GElementStiffnessMatrix(1, 0)
            GSmatrix((_StartNodeNo * 2) + 1, (_StartNodeNo * 2) + 1) = GSmatrix((_StartNodeNo * 2) + 1, (_StartNodeNo * 2) + 1) + _GElementStiffnessMatrix(1, 1)
            GSmatrix((_StartNodeNo * 2) + 1, (_EndNodeNo * 2) + 0) = GSmatrix((_StartNodeNo * 2) + 1, (_EndNodeNo * 2) + 0) + _GElementStiffnessMatrix(1, 2)
            GSmatrix((_StartNodeNo * 2) + 1, (_EndNodeNo * 2) + 1) = GSmatrix((_StartNodeNo * 2) + 1, (_EndNodeNo * 2) + 1) + _GElementStiffnessMatrix(1, 3)

            '--- Column3
            GSmatrix((_EndNodeNo * 2) + 0, (_StartNodeNo * 2) + 0) = GSmatrix((_EndNodeNo * 2) + 0, (_StartNodeNo * 2) + 0) + _GElementStiffnessMatrix(2, 0)
            GSmatrix((_EndNodeNo * 2) + 0, (_StartNodeNo * 2) + 1) = GSmatrix((_EndNodeNo * 2) + 0, (_StartNodeNo * 2) + 1) + _GElementStiffnessMatrix(2, 1)
            GSmatrix((_EndNodeNo * 2) + 0, (_EndNodeNo * 2) + 0) = GSmatrix((_EndNodeNo * 2) + 0, (_EndNodeNo * 2) + 0) + _GElementStiffnessMatrix(2, 2)
            GSmatrix((_EndNodeNo * 2) + 0, (_EndNodeNo * 2) + 1) = GSmatrix((_EndNodeNo * 2) + 0, (_EndNodeNo * 2) + 1) + _GElementStiffnessMatrix(2, 3)

            '--- Column4
            GSmatrix((_EndNodeNo * 2) + 1, (_StartNodeNo * 2) + 0) = GSmatrix((_EndNodeNo * 2) + 1, (_StartNodeNo * 2) + 0) + _GElementStiffnessMatrix(3, 0)
            GSmatrix((_EndNodeNo * 2) + 1, (_StartNodeNo * 2) + 1) = GSmatrix((_EndNodeNo * 2) + 1, (_StartNodeNo * 2) + 1) + _GElementStiffnessMatrix(3, 1)
            GSmatrix((_EndNodeNo * 2) + 1, (_EndNodeNo * 2) + 0) = GSmatrix((_EndNodeNo * 2) + 1, (_EndNodeNo * 2) + 0) + _GElementStiffnessMatrix(3, 2)
            GSmatrix((_EndNodeNo * 2) + 1, (_EndNodeNo * 2) + 1) = GSmatrix((_EndNodeNo * 2) + 1, (_EndNodeNo * 2) + 1) + _GElementStiffnessMatrix(3, 3)
        End Sub

        Public Sub FixResultMatrix(ByRef ResMatrix() As Double)
            'Dim GRmatrix(0, 3) As Double
            'GRmatrix(0, 0) = ResMatrix((_StartNodeNo * 2) + 0)
            'GRmatrix(0, 1) = ResMatrix((_StartNodeNo * 2) + 1)
            'GRmatrix(0, 2) = ResMatrix((_EndNodeNo * 2) + 0)
            'GRmatrix(0, 3) = ResMatrix((_EndNodeNo * 2) + 1)

            ''---Matrix Multiplication KT
            'Dim T1Gresult(0, 3) As Double
            'MatrixMultiplication(GRmatrix, 0, 3, _Transformation_MatT, 3, 3, T1Gresult)
            ''---Matrix Multiplication TtKT
            'Dim T2Gresult(0, 3) As Double
            'MatrixMultiplication(_Transformation_Mat, 3, 3, T1Gresult, 0, 3, T2Gresult)

            '_GResultMatrix(0) = T2Gresult(0, 0)
            '_GResultMatrix(1) = T2Gresult(0, 1)
            '_GResultMatrix(2) = T2Gresult(0, 2)
            '_GResultMatrix(3) = T2Gresult(0, 3)

            _GResultMatrix(0) = ResMatrix((_StartNodeNo * 2) + 0)
            _GResultMatrix(1) = ResMatrix((_StartNodeNo * 2) + 1)
            _GResultMatrix(2) = ResMatrix((_EndNodeNo * 2) + 0)
            _GResultMatrix(3) = ResMatrix((_EndNodeNo * 2) + 1)
        End Sub

        Public Sub FixNodeResultMatrix(ByRef ResMatrix() As Double)
            _GNodeResultMatrix(0) = ResMatrix((_StartNodeNo * 2) + 0)
            _GNodeResultMatrix(1) = ResMatrix((_StartNodeNo * 2) + 1)
            _GNodeResultMatrix(2) = ResMatrix((_EndNodeNo * 2) + 0)
            _GNodeResultMatrix(3) = ResMatrix((_EndNodeNo * 2) + 1)
        End Sub

        Private Sub MatrixMultiplication(ByVal A(,) As Double, ByVal arow As Integer, ByVal acolumn As Integer, ByVal B(,) As Double, ByVal brow As Integer, ByVal bcolumn As Integer, ByRef C(,) As Double)
            ReDim C(arow, bcolumn)
            For i = 0 To arow
                For j = 0 To bcolumn
                    C(i, j) = 0
                    For k = 0 To brow
                        C(i, j) = C(i, j) + (A(i, k) * B(k, j))
                    Next
                Next
            Next
        End Sub
    End Class
#End Region

    Public Sub New()
        _updater = New AnalyzerUpdate
        _updater.Button1.Visible = False
        _updater.Button2.Visible = False
        _updater.Button3.Visible = False

        _updateStr = "------------- Finite Element Analyzer ----------------" & vbNewLine &
                     "----------------- Programmed by ----------------------" & vbNewLine &
                     "------------------ Samson Mano -----------------------" & vbNewLine &
                     "------------------------------------------------------" & vbNewLine &
                     "------------------------------------------------------" & vbNewLine &
                     "------------------------------------------------------" & vbNewLine &
                     "------------------------------------------------------" & vbNewLine &
                     "------------------------------------------------------" & vbNewLine &
                     vbNewLine &
                     vbNewLine


        '---------Check Structure Feasibility for Analysis

        _Failed = PreliminaryCheck()
        _updater._ResultTxtBox.Text = _updateStr

        If Failed = True Then
            _updater.Button1.Visible = False
            _updater.Button2.Text = "Back"
            _updater.Button2.Visible = True
            _updater.Button3.Visible = False
            _updater.ShowDialog()
            Exit Sub
        End If

        '----Starting Analysis
        _ResultMem = New List(Of AnalysisResultMem)
        For Each E1 In WA2dt.Mem
            _ResultMem.Add(New AnalysisResultMem(E1))
        Next
        _updater._ResultTxtBox.Text = ""

        _updateStr = "------------- Finite Element Analyzer ----------------" & vbNewLine &
                          "----------------- Programmed by ----------------------" & vbNewLine &
                          "------------------ Samson Mano -----------------------" & vbNewLine &
                          "------------------------------------------------------" & vbNewLine &
                          vbNewLine

        '_____________________________________________________________________________________________
        _updateStr = _updateStr & "----> Element - Degree Of Freedom Matrix <----" & vbNewLine & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member :" & (_ResultMem.IndexOf(itm) + 1) _
                                        & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                        & vbTab & " =  |   " & vbTab & itm._DegreeOFFreedomMatrix(0) _
                                        & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(1) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(2) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(3) & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element Stiffness Matrix <----" & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & vbNewLine & "Member " & (_ResultMem.IndexOf(itm) + 1) _
                                         & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                         & vbNewLine
            For p = 0 To 3
                _updateStr = _updateStr & "|"
                For t = 0 To 3
                    _updateStr = _updateStr & vbTab & Math.Round(itm._GElementStiffnessMatrix(p, t), 2)
                Next
                _updateStr = _updateStr & vbTab & "|" & vbNewLine
            Next
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element - Fixed End Reaction Matrix <----" & vbNewLine & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member " & (_ResultMem.IndexOf(itm) + 1) _
                                           & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                           & vbTab & " =  |   " & vbTab & itm._GLoadVectorMatrix(0) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(1) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(2) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(3) & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Dim GlobalStiffnessMatrix((WA2dt.Bob.Count * 2) - 1, (WA2dt.Bob.Count * 2) - 1) As Double
        Dim GlobalDOFMatrix((WA2dt.Bob.Count * 2) - 1) As Double
        Dim GlobalForceMatrix((WA2dt.Bob.Count * 2) - 1) As Double
        Dim GlobalSettlementMatrix((WA2dt.Bob.Count * 2) - 1) As Double
        For Each resultM In _ResultMem
            resultM.FixGlobalMatrices(GlobalDOFMatrix, GlobalForceMatrix, GlobalSettlementMatrix, GlobalStiffnessMatrix)
        Next

        '____ Settlement and Sliding of support - redistribution to global force matrix______
        Call SlidingRedistribution(GlobalStiffnessMatrix, GlobalSettlementMatrix, GlobalForceMatrix)


        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global - Degree Of Freedom Matrix <----" _
                                                     & vbTab & vbTab _
                                                     & "----> Global - Fixed End Reaction Matrix <----" _
                                                     & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            _updateStr = _updateStr & vbTab & "|  " & vbTab & GlobalDOFMatrix(p) & vbTab & "  |" & vbTab & vbTab & vbTab
            _updateStr = _updateStr & vbTab & "|  " & vbTab & GlobalForceMatrix(p) & vbTab & "  |" & vbTab & vbTab & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global Stiffness Matrix <----" & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            _updateStr = _updateStr & "|"
            For t = 0 To ((WA2dt.Bob.Count * 2) - 1)
                _updateStr = _updateStr & vbTab & Math.Round(GlobalStiffnessMatrix(p, t), 2)
            Next
            _updateStr = _updateStr & vbTab & "|" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        Dim CBound As Integer
        Call Curtailment(GlobalDOFMatrix, GlobalForceMatrix, GlobalStiffnessMatrix, CBound)

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global Stiffness Matrix After Curtailment <----" & vbNewLine
        For p = 0 To (CBound - 1)
            _updateStr = _updateStr & "|"
            For t = 0 To (CBound - 1)
                _updateStr = _updateStr & vbTab & Math.Round(GlobalStiffnessMatrix(p, t), 2)
            Next
            _updateStr = _updateStr & vbTab & "|" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Dim ResultMatrix(CBound - 1) As Double
        Call GElimination(GlobalStiffnessMatrix, GlobalForceMatrix, ResultMatrix, CBound - 1)

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Result Matrix <----" & vbNewLine & vbNewLine
        For p = 0 To (CBound - 1)
            _updateStr = _updateStr & "|  " & vbTab & ResultMatrix(p) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        ReDim GlobalStiffnessMatrix((WA2dt.Bob.Count * 2) - 1, (WA2dt.Bob.Count * 2) - 1)
        ReDim GlobalDOFMatrix((WA2dt.Bob.Count * 2) - 1)
        ReDim GlobalForceMatrix((WA2dt.Bob.Count * 2) - 1)
        ReDim GlobalSettlementMatrix((WA2dt.Bob.Count * 2) - 1)
        For Each resultM In _ResultMem
            resultM.FixGlobalMatrices(GlobalDOFMatrix, GlobalForceMatrix, GlobalSettlementMatrix, GlobalStiffnessMatrix)
        Next
        '____ Settlement and Sliding of support has to be redistirbuted to global force matrix______
        'Call SlidingRedistribution(GlobalStiffnessMatrix, GlobalSettlementMatrix, GlobalForceMatrix)

        Call Welding(ResultMatrix, GlobalSettlementMatrix, GlobalDOFMatrix)
        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Result Matrix After Welding <----" & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            _updateStr = _updateStr & "|  " & vbTab & ResultMatrix(p) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        For Each resultM In _ResultMem
            resultM.FixResultMatrix(ResultMatrix)
        Next


        Dim GlobalNodalResultantForce((WA2dt.Bob.Count * 2) - 1) As Double
        Call GMultiplier(GlobalNodalResultantForce, ResultMatrix, GlobalStiffnessMatrix, GlobalForceMatrix)
        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Nodal Resultant Matrix <----" & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            _updateStr = _updateStr & "|  " & vbTab & Round(GlobalNodalResultantForce(p), 2) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        For Each resultM In _ResultMem
            resultM.FixNodeResultMatrix(GlobalNodalResultantForce)
        Next

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        Dim XResultant As Double
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1) Step 2
            XResultant = XResultant + GlobalNodalResultantForce(p) + GlobalForceMatrix(p)
        Next
        Dim YResultant As Double
        For p = 1 To ((WA2dt.Bob.Count * 2) - 1) Step 2
            YResultant = YResultant + GlobalNodalResultantForce(p) + GlobalForceMatrix(p)
        Next
        _updateStr = _updateStr & "----> Resultant Check <----" & vbNewLine & vbNewLine
        _updateStr = _updateStr & "X Resultant:  " & vbTab & XResultant & vbNewLine
        _updateStr = _updateStr & "Y Resultant:  " & vbTab & YResultant & vbNewLine
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        _updater.ResultStatus.Text = "Status: Success"
        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            If GlobalDOFMatrix(i) = 1 Then
                If Round(GlobalNodalResultantForce(i), 1) <> 0 Then
                    _Failed = True
                    _updater.ResultStatus.Text = "Status: Failure"
                    _updater.Button1.Visible = False
                    _updater.Button2.Text = "Back To Design"
                    _updater.Button2.Visible = True
                    _updater.Button3.Visible = False
                    _updater.ShowDialog()
                    Exit Sub
                End If
            End If
        Next
        Dim maxdisplacement As Double = 0 ' ---- Maximum displacement for scaling
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            If Math.Abs(ResultMatrix(p)) > maxdisplacement Then
                maxdisplacement = Math.Abs(ResultMatrix(p))
            End If
        Next
        If Val(maxdisplacement) = 0 Or IsNumeric(maxdisplacement) = False Or Double.IsInfinity(maxdisplacement) = True Then
            _Failed = True
            _updater.ResultStatus.Text = "Status: Failure"
            _updater.Button1.Visible = False
            _updater.Button2.Text = "Back"
            _updater.Button2.Visible = True
            _updater.Button3.Visible = False
            _updater.ShowDialog()
            Exit Sub
        End If

        Call SetCoordinates(maxdisplacement)
        '------------------------------- Analysis Complete

        _updater.Button1.Visible = True
        _updater.Button2.Text = "View Result"
        _updater.Button2.Visible = True
        _updater.Button3.Visible = True
        _updater.ShowDialog()
    End Sub

#Region "Preliminary Checking For Structure, Load & Support"
    Private Function PreliminaryCheck()
        Dim ChkResult As Boolean = False
        CorrectNodeSupportError()
        CheckStructure(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        CheckSupport(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        CheckLoad(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Return ChkResult
    End Function

#Region "Correct Node Support Error"
    Private Structure Repetation
        Dim NdNumber As Integer
        Dim ReOccurMember As List(Of Integer)
    End Structure

    Private Sub CorrectNodeSupportError()
        '----Sort nodes based on Repetation count
        Dim RCount As New List(Of Repetation)
        For Each Nd In WA2dt.Bob
            Dim TReap As New Repetation
            TReap.ReOccurMember = New List(Of Integer)
            For Each El In WA2dt.Mem
                If Nd.index = El.SN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
                If Nd.index = El.EN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
            Next
            TReap.NdNumber = WA2dt.Bob.IndexOf(Nd)
            RCount.Add(TReap)
        Next
        Dim PooF As New List(Of Integer)
        For Each Rc In RCount
            Dim SupportRevised As Line2DT.Node.SupportCondition
            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).SN.Support.PS = True Or
                        WA2dt.Mem(ReO).SN.Support.RS = True Then
                        SupportRevised = New _
                                    Line2DT.Node.SupportCondition(False,
                                                WA2dt.Mem(ReO).SN.Support.PS,
                                                WA2dt.Mem(ReO).SN.Support.RS,
                                                WA2dt.Mem(ReO).SN.Support.supportinclination,
                                                WA2dt.Mem(ReO).SN.Support.settlementdx,
                                                WA2dt.Mem(ReO).SN.Support.settlementdy)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).SN.Support.PJ = True Then
                        SupportRevised = New _
                                    Line2DT.Node.SupportCondition(
                                                WA2dt.Mem(ReO).SN.Support.PJ,
                                                 False,
                                                False,
                                                Nothing,
                                                Nothing,
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).EN.Support.PS = True Or
                        WA2dt.Mem(ReO).EN.Support.RS = True Then
                        SupportRevised = New _
                                    Line2DT.Node.SupportCondition(False,
                                                WA2dt.Mem(ReO).EN.Support.PS,
                                                WA2dt.Mem(ReO).EN.Support.RS,
                                                WA2dt.Mem(ReO).EN.Support.supportinclination,
                                                WA2dt.Mem(ReO).EN.Support.settlementdx,
                                                WA2dt.Mem(ReO).EN.Support.settlementdy)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).EN.Support.PJ = True Then
                        SupportRevised = New _
                                    Line2DT.Node.SupportCondition(
                                                WA2dt.Mem(ReO).EN.Support.PJ,
                                                False,
                                                False,
                                                Nothing,
                                                Nothing,
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                End If
            Next

            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).SN.Support = SupportRevised
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).EN.Support = SupportRevised
                End If
            Next
            WA2dt.Bob(Rc.NdNumber).Support = SupportRevised
        Next

        For Each leftOut In PooF
            If WA2dt.Mem(leftOut).SN.Support.PJ = False And
                WA2dt.Mem(leftOut).SN.Support.PS = False And
                WA2dt.Mem(leftOut).SN.Support.RS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "TrussANS")
            ElseIf WA2dt.Mem(leftOut).EN.Support.PJ = False And
                WA2dt.Mem(leftOut).EN.Support.PS = False And
                WA2dt.Mem(leftOut).EN.Support.RS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "TrussANS")
            End If
        Next
    End Sub
#End Region

    Private Sub CheckStructure(ByRef ChkFlg As Boolean)

    End Sub

    Private Sub CheckSupport(ByRef ChkFlg As Boolean)
        Dim I As Integer = 0
        For Each node In WA2dt.Bob
            If node.Support.PS = True Or node.Support.RS = True Then
                I = I + 1
            End If
        Next

        If I < 2 Then
            _updateStr = _updateStr & vbNewLine & " Checking Support" &
             " ........................" &
             " Failed" &
                vbNewLine
            ChkFlg = True
            Exit Sub
        End If

        _updateStr = _updateStr & vbNewLine & " Checking Support" &
        " ........................" &
        " Ok" &
           vbNewLine
    End Sub

    Private Sub CheckLoad(ByRef ChkFlg As Boolean)
        Dim I As Integer = 0

        For Each itm In WA2dt.Mem
            If itm.SN.Load.Loadintensity <> 0 Or itm.EN.Load.Loadintensity <> 0 Then
                I = I + 1
            End If
        Next

        If I = 0 Then
            _updateStr = _updateStr & vbNewLine & " Checking Load" &
             " ........................" &
             " Failed" &
                vbNewLine
            ChkFlg = True
            Exit Sub
        End If

        _updateStr = _updateStr & vbNewLine & " Checking Load" &
        " ........................" &
        " Ok" &
           vbNewLine
    End Sub
#End Region

#Region "Gauss Elimination Method"
    Private Sub GElimination(ByRef A(,) As Double, ByRef B() As Double, ByRef re() As Double, ByVal cb As Integer)
        '----Check For Uncertainity :)
        If WA2dt.Mem.Count - 1 <= 0 Then
            Exit Sub
        ElseIf cb = 0 Then
            re(0) = B(0) / A(0, 0)
            Exit Sub
        End If

        Dim Triangular_A(cb, cb + 1), line_1, temporary_1, multiplier_1, sum_1 As Double
        Dim soln(cb + 1) As Double
        For n = 0 To cb
            For m = 0 To cb
                Triangular_A(m, n) = A(m, n)
            Next
        Next

        '.... substituting the force to triangularmatrics....
        For n = 0 To cb
            Triangular_A(n, cb + 1) = B(n)
        Next

        '...............soving the triangular matrics.............
        For k = 0 To cb
            '......Bring a non-zero element first by changes lines if necessary
            If Triangular_A(k, k) = 0 Then
                For n = k To cb
                    If Triangular_A(n, k) <> 0 Then line_1 = n : Exit For 'Finds line_1 with non-zero element
                Next n
                '..........Change line k with line_1
                For m = k To cb
                    temporary_1 = Triangular_A(k, m)
                    Triangular_A(k, m) = Triangular_A(line_1, m)
                    Triangular_A(line_1, m) = temporary_1
                Next m
            End If
            '....For other lines, make a zero element by using:
            '.........Ai1=Aij-A11*(Aij/A11)
            '.....and change all the line using the same formula for other elements
            For n = k + 1 To cb
                If Triangular_A(n, k) <> 0 Then 'if it is zero, stays as it is
                    multiplier_1 = Triangular_A(n, k) / Triangular_A(k, k)
                    For m = k To cb + 1
                        Triangular_A(n, m) = Triangular_A(n, m) - Triangular_A(k, m) * multiplier_1
                    Next m
                End If
            Next n
        Next k


        '..... calculating the dof value..........

        'First, calculate last xi (for i = System_DIM)
        soln(cb + 1) = Triangular_A(cb, cb + 1) / Triangular_A(cb, cb)

        '................
        For n = 1 To cb
            sum_1 = 0
            For m = 1 To n
                sum_1 = sum_1 + soln(cb + 2 - m) * Triangular_A(cb - n, cb + 1 - m)
            Next m
            soln(cb + 1 - n) = (Triangular_A(cb - n, cb + 1) - sum_1) / Triangular_A(cb - n, cb - n)

        Next n

        For n = 0 To cb
            re(n) = soln(n + 1)
        Next
    End Sub
#End Region

#Region "Curtailment & Welding of Matrices"
    Public Sub Curtailment(ByRef DOFmatrix() As Double, ByRef FERmatrix() As Double, ByRef GSmatrix(,) As Double, ByRef CBound As Integer)
        Dim tgm((WA2dt.Bob.Count * 2) - 1, (WA2dt.Bob.Count * 2) - 1) As Double
        Dim tdofm((WA2dt.Bob.Count * 2) - 1) As Integer
        Dim tferm((WA2dt.Bob.Count * 2) - 1) As Double

        Dim r, s As Integer
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            If DOFmatrix(p) = 0 Then
                Continue For
            Else
                s = 0
                For t = 0 To ((WA2dt.Bob.Count * 2) - 1)
                    If DOFmatrix(t) = 0 Then
                        Continue For
                    Else
                        tferm(s) = FERmatrix(t)
                        tdofm(s) = DOFmatrix(t)
                        tgm(r, s) = GSmatrix(p, t)
                        s = s + 1
                    End If
                Next
                r = r + 1
            End If
        Next

        ReDim GSmatrix(r - 1, r - 1)
        ReDim DOFmatrix(r - 1)
        ReDim FERmatrix(r - 1)

        For p = 0 To r - 1
            DOFmatrix(p) = tdofm(p)
            FERmatrix(p) = tferm(p)
            For t = 0 To r - 1
                GSmatrix(p, t) = tgm(p, t)
            Next
        Next
        CBound = r
    End Sub

    Public Sub Welding(ByRef re() As Double, ByVal SettlementMatrix() As Double, ByRef DOFmatrix() As Double)
        Dim tres((WA2dt.Bob.Count * 2) - 1) As Double
        Dim j As Integer
        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            If DOFmatrix(i) = 0 Then
                If SettlementMatrix(i) <> 0 Then
                    tres(i) = SettlementMatrix(i)
                End If
                Continue For
            End If
            tres(i) = tres(i) + re(j)
            j = j + 1
        Next

        ReDim re((WA2dt.Bob.Count * 2) - 1)
        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            re(i) = tres(i)
        Next
    End Sub

    Public Sub GMultiplier(ByRef Nre() As Double, ByRef re() As Double, ByRef GSmatrix(,) As Double, ByRef FERmatrix() As Double)
        Dim teR((WA2dt.Bob.Count * 2) - 1) As Double

        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            teR(i) = 0
            For j = 0 To ((WA2dt.Bob.Count * 2) - 1)
                teR(i) = teR(i) + (GSmatrix(i, j) * re(j))
            Next
        Next

        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            Nre(i) = teR(i) - FERmatrix(i)
        Next
    End Sub

    Public Sub SlidingRedistribution(ByVal GStiffnessMatrix(,) As Double, ByVal SettlementMatrix() As Double, ByRef ForceMatrix() As Double)
        Dim MatrixCount As Integer = -1 '---Note the number of force redistribution matrix required
        Dim SettlementValue As New List(Of Double)
        Dim Ivalue As New List(Of Integer)
        For i = 0 To ((WA2dt.Bob.Count * 2) - 1)
            If SettlementMatrix(i) <> 0 Then
                Ivalue.Add(i)
                SettlementValue.Add(SettlementMatrix(i))
                MatrixCount = MatrixCount + 1
            End If
        Next
        If MatrixCount = -1 Then
            Exit Sub
        End If
        Dim RedistributionMatrix(MatrixCount, (WA2dt.Bob.Count * 2) - 1)
        For i = 0 To MatrixCount
            For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
                If Ivalue(i) = p Then
                    RedistributionMatrix(i, p) = SettlementValue(i)
                    Continue For
                End If
                RedistributionMatrix(i, p) = SettlementValue(i) * GStiffnessMatrix(Ivalue(i), p)
            Next
        Next
        '-------Force Redistribution Matrix
        Dim CummulativeForceReDistribution((WA2dt.Bob.Count * 2) - 1) As Double
        For i = 0 To MatrixCount
            For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
                CummulativeForceReDistribution(p) = CummulativeForceReDistribution(p) + RedistributionMatrix(i, p)
            Next
        Next
        For p = 0 To ((WA2dt.Bob.Count * 2) - 1)
            ForceMatrix(p) = ForceMatrix(p) - CummulativeForceReDistribution(p)
        Next
    End Sub
#End Region

#Region "Set Co-ordinates"
    Private Sub SetCoordinates(ByVal MaxDisp As Double)
        RA2dt.Rmem.Clear()

        For Each R In _ResultMem
            RA2dt.Rmem.Add(New Rslt2DT(R._GResultMatrix(0),
                                       R._GResultMatrix(1),
                                       R._GResultMatrix(2),
                                       R._GResultMatrix(3),
                                       R._GNodeResultMatrix(0),
                                       R._GNodeResultMatrix(1),
                                       R._GNodeResultMatrix(2),
                                       R._GNodeResultMatrix(3), MaxDisp, R.EL))

        Next

        RA2dt.Rbob.Clear()
        Dim Hr As New System.Predicate(Of Rslt2DT.Node)(AddressOf ExistingNodes)
        For Each EL In RA2dt.Rmem
            _Her = EL.SN
            If RA2dt.Rbob.Exists(Hr) = False Then
                RA2dt.Rbob.Add(EL.SN)
            End If
            _Her = EL.EN
            If RA2dt.Rbob.Exists(Hr) = False Then
                RA2dt.Rbob.Add(EL.EN)
            End If
        Next

        Dim Max, Mid, Min As Double
        FixMaxMidMin(Max, Mid, Min, 0)
        For Each El In RA2dt.Rmem
            El.FixStressColorVariation(Max, Mid, Min)
        Next
        FixMaxMidMin(Max, Mid, Min, 1)
        For Each El In RA2dt.Rmem
            El.FixStrainColorVariation(Max, Mid, Min)
        Next
        FixMaxMidMin(Max, Mid, Min, 2)
        For Each El In RA2dt.Rmem
            El.FixForceColorVariation(Max, Mid, Min)
        Next
    End Sub

    Private _Her As Rslt2DT.Node

    Private Function ExistingNodes(ByVal His As Rslt2DT.Node) As Boolean
        If _Her.Coord.X = His.Coord.X And _Her.Coord.Y = His.Coord.Y Then
            Return True
        End If
        Return False
    End Function

    Private Sub FixMaxMidMin(ByRef _max As Double, ByRef _mid As Double, ByRef _min As Double, ByVal S As Integer)
        Dim Values As New List(Of Double)
        If S = 0 Then
            '-----Stress Max
            For Each EL In RA2dt.Rmem
                Values.Add(EL.Stress)
            Next
            Values.Sort()
            Values.Reverse()
            _max = Values(0)
            _min = Values(Values.Count - 1)
            _mid = (_max + _min) / 2
        ElseIf S = 1 Then
            '-----Strain Max
            For Each EL In RA2dt.Rmem
                Values.Add(EL.Strain)
            Next
            Values.Sort()
            Values.Reverse()
            _max = Values(0)
            _min = Values(Values.Count - 1)
            _mid = (_max + _min) / 2
        ElseIf S = 2 Then
            '------Force Max
            For Each EL In RA2dt.Rmem
                Values.Add(EL.Force)
            Next
            Values.Sort()
            Values.Reverse()
            _max = Values(0)
            _min = Values(Values.Count - 1)
            _mid = (_max + _min) / 2
        End If
    End Sub
#End Region
End Class
