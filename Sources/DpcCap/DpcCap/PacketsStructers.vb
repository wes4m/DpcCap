
Imports System.Runtime.InteropServices

Public Class PacketsStructers

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure ip_address
        Public byte1 As Byte
        Public byte2 As Byte
        Public byte3 As Byte
        Public byte4 As Byte
    End Structure


    Public Structure ip_header
        Public ver_ihl As Byte
        Public tos As Byte
        Public tlen As UShort
        Public identification As UShort
        Public flags_fo As UShort
        Public ttl As Byte
        Public proto As Byte
        Public crc As UShort
        Public saddr As ip_address
        Public daddr As ip_address
        Public op_pad As UInteger
    End Structure

    Public Structure udp_header
        Public sport As UShort
        Public dport As UShort
        Public len As UShort
        Public crc As UShort
    End Structure


    Public Structure tcp_header

        Public source_port As UShort
        Public dest_port As UShort
        Public sequence As UInteger
        Public acknowledge As UInteger

        Public ns As Byte
        Public reserved_part1 As Byte
        Public data_offset As Byte

        Public fin As Byte
        Public syn As Byte
        Public rst As Byte
        Public psh As Byte
        Public ack As Byte
        Public urg As Byte

        Public ecn As Byte
        Public cwr As Byte

        Public window As UShort
        Public checksum As UShort
        Public urgent_pointer As UShort

    End Structure


End Class
