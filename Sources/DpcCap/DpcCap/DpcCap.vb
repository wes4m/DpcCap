Imports System.Runtime.InteropServices


Public Class DPCODERS_CAP

    ' Structers '

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure AdapterDevice
        Dim Err As Integer
        Dim Name As String
        Dim Description As String
        Dim Flags As UInt32
    End Structure

  
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure LastError
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)> _
        Public x As String
    End Structure

    ' Imports '
    <DllImport("DpcCapLib.dll")>
    Private Shared Function GetAdapterDevice(ByVal DeviceNumber As Integer) As IntPtr
    End Function
    Public Shared Function GetDeviceAdapter(ByVal DeviceNumber As Integer) As AdapterDevice
        Dim DeviceStructer As New AdapterDevice
        Dim StructPointer As IntPtr = GetAdapterDevice(DeviceNumber)
        Return Marshal.PtrToStructure(StructPointer, GetType(AdapterDevice))
    End Function

    <DllImport("DpcCapLib.dll")>
    Public Shared Function OpenAdapter(AdapterName As String, SnapLen As Integer, Flags As Integer, ReadTimeOut As Integer) As IntPtr
    End Function
    <DllImport("DpcCapLib.dll")>
    Public Shared Function OpenAdapterLive(AdapterName As String, SnapLen As Integer, Flags As Integer, ReadTimeOut As Integer) As IntPtr
    End Function
    <DllImport("DpcCapLib.dll")>
    Public Shared Function DataLink(LiveAdapterHandle As IntPtr) As Integer
    End Function
    <DllImport("DpcCapLib.dll")>
    Public Shared Function GetAdapterNetMask(AdapterName As String) As UInteger
    End Function
    <DllImport("DpcCapLib.dll")>
    Public Shared Function MakeFilter(LiveAdapterHandle As IntPtr, filter As String, netmask As UInteger) As Integer
    End Function
    <DllImport("DpcCapLib.dll")>
    Public Shared Function CaptureLoop(LiveAdapterHandle As IntPtr, Count As Integer, CallBack As IntPtr) As Integer
    End Function

    Public Delegate Function AdapterDelg(PacketDataPointer As IntPtr, Size As Integer) As IntPtr

    <DllImport("DpcCapLib.dll")>
    Public Shared Function Sendpacket(AdapterHandle As IntPtr, packet() As Byte, size As Integer) As Integer
    End Function

    <DllImport("DpcCapLib.dll")>
    Private Shared Function GetLastErrorBuff() As IntPtr
    End Function
    Public Shared Function GetLastError() As String
        Dim x As LastError = Marshal.PtrToStructure(GetLastErrorBuff(), GetType(LastError))
        Return x.x
    End Function




    ' Error - Info - Success codes defines ' 
    Public Const NO_ERROR = &H0

    Public Const DLT_EN10MB = &H1
    Public Const MAXIMUM_DEVICES = 100

    ' flags for open
    Public Const OPENFLAG_PROMISCUOUS = 1
    Public Const OPENFLAG_DATATX_UDP = 2
    Public Const OPENFLAG_NOCAPTURE_RPCAP = 4
    Public Const OPENFLAG_NOCAPTURE_LOCAL = 8
    Public Const OPENFLAG_MAX_RESPONSIVENESS = 16

End Class

