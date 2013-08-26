

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




Imports System.Runtime.InteropServices

'' Importing DpcCap 
Imports DpcCap.DPCODERS_CAP

Module DpcCapExamples



    ''' Getting all adapters devices information by number
    Sub GetDevicesExample()

        Dim CurrAdapter As New AdapterDevice

        For i = 1 To MAXIMUM_DEVICES

            CurrAdapter = GetDeviceAdapter(i)

            ' Check if error occurred
            If CurrAdapter.Err = NO_ERROR Then

                ' Print Current adapter information
                Console.WriteLine("\n {0} - {1} ({2})", i, CurrAdapter.Name, CurrAdapter.Description)

            Else
                ' Break the loop if error occurred
                Exit For
            End If

        Next

    End Sub

    ''' Sending raw packet example
    Sub SendingRawPacket()

        Dim packet() As Byte = New Byte() {&H0, &H0, &H0, &H0} '' The packet bytes

        '' Getting device adapter to send using it ( getting it by number ) , number used here = 2
        Dim Adapter As AdapterDevice = GetDeviceAdapter(2)

        '' checking no errors 
        If Adapter.Err = NO_ERROR Then

            '' Get handle to the adapter
            '' OpenAdapter(Adapter name , packet size , flags , read time out )
            Dim AdapterHandle As IntPtr = OpenAdapter(Adapter.Name, packet.Length + 1, OPENFLAG_PROMISCUOUS, 1000)

            '' checking the handle 
            If AdapterHandle <> IntPtr.Zero Then

                '' Sending packet
                '' Sendpacket(Adapter handle , packet , packet size )
                If Sendpacket(AdapterHandle, packet, packet.Length) = NO_ERROR Then
                    '' Packet send successfully
                End If

            End If

        End If

    End Sub

    ''' Capturing network traffic
    Sub CaptureTraffic()

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
                    If MakeFilter(AdHandleLive, "tcp", NetMask) = NO_ERROR Then

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
    '' Declaring new byte array to save the new packet bytes in it
    Dim buff() As Byte
    Public Function NewPacket(PacketDataPointer As IntPtr, Size As Integer) As IntPtr

        '' Resize the array with the new packet size
        Array.Resize(buff, Size)
        '' Copy the new packet bytes to ( buff ) 
        Marshal.Copy(PacketDataPointer, buff, 0, Size)


        '' Do whatever with the packet (buff) headering it and else ...

    End Function



    Sub Main()

        ' DpcCap Exmaples <> '
        ' Test it :P

        ' GetDevicesExample()
        ' SendingRawPacket()
        ' CapturingTraffic()

    End Sub
    


End Module
