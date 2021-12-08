Public Class History2DT
    Dim _HMem As New List(Of Line2DT)
    Dim _HBob As New List(Of Line2DT.Node)
    Dim _HZm As Double
    Dim _HMpoint As Point

    Public Property HMem() As List(Of Line2DT)
        Get
            Return _HMem
        End Get
        Set(ByVal value As List(Of Line2DT))

        End Set
    End Property

    Public Property HBob() As List(Of Line2DT.Node)
        Get
            Return _HBob
        End Get
        Set(ByVal value As List(Of Line2DT.Node))

        End Set
    End Property

    Public Property HZm() As Double
        Get
            Return _HZm
        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property HMpoint() As Point
        Get
            Return _HMpoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Sub New(ByVal THMem As List(Of Line2DT), ByVal THBob As List(Of Line2DT.Node), ByVal THZm As Double, ByVal THMpoint As Point)
        _HMem.AddRange(THMem)
        _HBob.AddRange(THBob)
        _HZm = THZm
        _HMpoint = THMpoint
    End Sub
End Class
