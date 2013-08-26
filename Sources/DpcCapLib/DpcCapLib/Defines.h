#include <pcap.h>

/* structers */
typedef struct AdapterDevice
{
	int    Err;
	char*  Name;
	char*  Description;
	UINT32 Flags;
}adapterDevice;


/* externs */
extern "C" __declspec(dllexport) adapterDevice*      GetAdapterDevice(int DeviceNumber);
extern "C" __declspec(dllexport) pcap_t*             OpenAdapter(const char* AdapterName,int SnapLen,int Flags,int ReadTimeOut);
extern "C" __declspec(dllexport) pcap_t*             OpenAdapterLive(const char* AdapterName,int SnapLen,int Flags,int ReadTimeOut);
extern "C" __declspec(dllexport) char*               GetLastErrorBuff();
extern "C" __declspec(dllexport) int                 Sendpacket(pcap_t* AdapterHandle,const u_char* packet,int size);
extern "C" __declspec(dllexport) int                 DataLink(pcap_t* LiveAdapterHandle);
extern "C" __declspec(dllexport) u_int               GetAdapterNetMask(const char* AdapterName);
extern "C" __declspec(dllexport) int                 MakeFilter(pcap_t* LiveAdapterHandle,const char* filter,u_int netmask);
extern "C" __declspec(dllexport) void                CaptureLoop(pcap_t* LiveAdapterHandle,int count,DWORD CallBack);

void Packet__Handler(u_char *param,const struct pcap_pkthdr* header,const u_char *pkt_data);
/* error - sucess - info codes defines */

#define ERROR_NO_DEVICE_FOUND   0x000001
#define ERROR_OUT_OF_RANGE      0x000002
#define ERROR_MAKE_FILTER       0x000003


#define NO_ERROR      0x000000
