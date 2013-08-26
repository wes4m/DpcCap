
' DpcCap ( DPOCDERS CAP ) a .net framework vb.net language small and simple winpcap wrapper .
' Coded by unCoder form dpcoders team
' What dose it do ? : it can send raw packets simply and capture network traffic with winpcap fillter options
' in a fast and easy way to use .
' there's examples of using it which you can find in this code .
'                                                                                                                             
' Note    : don't forget to import the wrapper ( DpcCap.DPCODERS_CAP ) , and put it beside the project and the c++ dll too .
' Coder   : unCoder ( uncodersc@gmail.com ) ( Team : dpcoders@gmail.com ) ( Twitter : @DPCoders )
' Website : wwww.dpcoders.com
' DpcCap  : (.Net Refrences - c++ dll) wwww.dpcoders.com/projects/DpcCap.rar
' Winpcap : www.winpcap.org/install/bin/WinPcap_4_1_3.exe

Imports DpcCap.DPCODERS_CAP
Imports System.Runtime.InteropServices

'' Some packets protocols structers
Imports DpcCap.PacketsStructers

Module HeadringPackets

    ''' Capturing network traffic ( tcp traffic filter )
    Sub Main()

        '' Getting device adapter to send using it ( getting it by number ) , number used here = 2
        Dim Adapter As AdapterDevice = GetDeviceAdapter(2)

        '' checking no errors 
        If Adapter.Err = NO_ERROR Then

            '' Open adpater live handle 
            '' OpenAdapterLive(Adapter name,capturing size,flags, read time out)
            Dim AdHandleLive As IntPtr = OpenAdapterLive(Adapter.Name, 65534, OPENFLAG_PROMISCUOUS, 1000)

            '' checking handle
            If AdHandleLive <> IntPtr.Zero Then


                '' Checking if the adpater is an ethernet adapter 
                If (DataLink(AdHandleLive) = DLT_EN10MB) Then

                    '' Getting adapter netmask
                    Dim NetMask As UInteger = GetAdapterNetMask(Adapter.Name)

                    '' Making Filter ( " tcp - udp - .... " ) and check if no errors
                    If MakeFilter(AdHandleLive, "ip and tcp", NetMask) = NO_ERROR Then

                        '' Getting NewPacket function address , to callback it when new packet captured
                        Dim AdDg As New AdapterDelg(AddressOf NewPacket)

                        '' Capturing traffic
                        '' CaptureLoop(Adapter handle live, count to loop (0 if infinity),CallBack func address)
                        CaptureLoop(AdHandleLive, 0, Marshal.GetFunctionPointerForDelegate(AdDg))

                    End If


                End If

            End If

        End If


    End Sub

    '' ^ NewPacket fucntion ( called back when new packet captured using the preivous method )
    Public Function NewPacket(PacketData As IntPtr, Size As Integer)

        '' Headring the pakcet , the first of the packet ( IP Header )

        Dim ih As ip_header

        '' Calculating the ip header postion in the packet = ( The packet begining + 14 )
        Dim calc As IntPtr = PacketData.ToInt32() + 14

        '' point the structer to the ip header address
        ih = Marshal.PtrToStructure(calc, GetType(ip_header))

        '' getting information ( ip address ) ( source ip -> destnation ip )
        Console.WriteLine(" {0}.{1}.{2}.{3} -> {4}.{5}.{6}.{7} ", ih.saddr.byte1, ih.saddr.byte2, ih.saddr.byte3, ih.saddr.byte4,
                                                                  ih.daddr.byte1, ih.daddr.byte2, ih.daddr.byte3, ih.daddr.byte4)

    End Function

End Module
