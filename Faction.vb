Public Class Faction
    Public Property System As String
    Public Property FactionName As String
    Public Property EntryDate As Date
    Public Property Commander As String

    Public Property Influence As Decimal
    Public Property State As String
    Public Property Found As Boolean = False

    Public Property PrevEntryDate As Date
    Public Property PrevCommander As String
    Public Property PrevInfluence As Decimal
    Public Property PrevState As String

    Public Property Downloaded As Boolean = False
End Class
