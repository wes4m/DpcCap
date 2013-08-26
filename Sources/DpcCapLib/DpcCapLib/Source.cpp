
#define WIN32_LEAN_AND_MEAN
#define HAVE_REMOTE

#include "Defines.h"


/* Global Objects */
char ErrBuff[PCAP_ERRBUF_SIZE];
struct bpf_program   fcode; 
DWORD Adrs = 0;
/* Funcs */

// GetAdapterDevice : pcap_findalldevs | pcap_findalldevs_ex
adapterDevice CurrDevice;
adapterDevice* GetAdapterDevice(int DeviceNumber)
{
	pcap_if_t*   AllDevices;
	pcap_if_t*   Adapter;


		if(pcap_findalldevs(&AllDevices,ErrBuff) == -1)
		{
			CurrDevice.Err = ERROR_NO_DEVICE_FOUND;
			return &CurrDevice;
		}

	int i = 0;
    for(Adapter = AllDevices;Adapter != NULL;Adapter = Adapter->next)
		i++;

	if(DeviceNumber < 1 || DeviceNumber > i)
	{
			CurrDevice.Err = ERROR_OUT_OF_RANGE;
			return &CurrDevice;
	}

	Adapter=AllDevices;
	for(i=0;i < DeviceNumber-1 ;i++)
		Adapter=Adapter->next;

	CurrDevice.Err		      = NO_ERROR;
	CurrDevice.Name		      = Adapter->name;
	CurrDevice.Description    = Adapter->description;
	CurrDevice.Flags          = Adapter->flags;

	return &CurrDevice;
	
}

// OpenAdapter : pcap_open
pcap_t* OpenAdapter(const char* AdapterName,int SnapLen,int Flags,int ReadTimeOut)
{
	return pcap_open(AdapterName,SnapLen,Flags,ReadTimeOut,NULL,ErrBuff);
}

// OpedAdapter_Live : pcap_open_live
pcap_t* OpenAdapterLive(const char* AdapterName,int SnapLen,int Flags,int ReadTimeOut)
{
	return pcap_open_live(AdapterName,SnapLen,Flags,ReadTimeOut,ErrBuff);
}

// DataLink : pcap_datalink
int DataLink(pcap_t* LiveAdapterHandle)
{
	return pcap_datalink(LiveAdapterHandle);
}

// MakeFilter : pcap_compile
int MakeFilter(pcap_t* LiveAdapterHandle,const char* filter,u_int netmask)
{
	if(pcap_compile(LiveAdapterHandle,&fcode,filter,1,netmask) < 0)
		return ERROR_MAKE_FILTER;

	if(pcap_setfilter(LiveAdapterHandle,&fcode) < 0)
		return ERROR_MAKE_FILTER;
	
	return NO_ERROR;
}

// CaptureLoop : pcap_loop
void CaptureLoop(pcap_t* LiveAdapterHandle,int count,DWORD Callback)
{
	Adrs = Callback;
	pcap_loop(LiveAdapterHandle,count,Packet__Handler,NULL);
}

			   
// Sendpacket : pcap_sendpacket
int Sendpacket(pcap_t* AdapterHandle,const u_char* packet,int size)
{
	return pcap_sendpacket(AdapterHandle,packet,size);
}



/* Optional Funcs */

u_int GetAdapterNetMask(const char* AdapterName)
{
	pcap_if_t*   AllDevices;
	pcap_if_t*   Adapter;


		if(pcap_findalldevs(&AllDevices,ErrBuff) == -1)
		{
			return 	ERROR_NO_DEVICE_FOUND;
		}


	   for(Adapter = AllDevices;Adapter != NULL;Adapter = Adapter->next)
		{
			if (Adapter->name == AdapterName)
				goto done;
		}

	   return ERROR_NO_DEVICE_FOUND;
	

	done:
		if(Adapter->addresses != NULL)
		{
			return ((sockaddr_in*)(Adapter->addresses->netmask))->sin_addr.S_un.S_addr;
		} 

	return 0xffffff;
}

char* GetLastErrorBuff()
{
	return ErrBuff;
}


int sSize = 0;
void Packet__Handler(u_char *param,const struct pcap_pkthdr* header,const u_char *pkt_data)
{

	sSize = header->len;
	 
	__asm 
	{
		PUSH sSize
		PUSH pkt_data
		CALL Adrs
	}

}