using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;


public class UDP<T>      //receives only T type and can send any type
{
    private UdpClient udpClient;
    public bool connectionSuccessfull = false;
    public readonly Queue<T> incomingQueueT = new Queue<T>();
    Thread receiveThread;
    private bool threadRunning = false;
    private string senderIp;
    private int senderPort;
    ~UDP()
    {
        Stop();
    }
    /**
     * \Starts a connection and binds it to the ports mentioned in the argumenrs
     *
     */
    public void StartConnection(string sendIp, int sendPort, int receivePort)
    {
        try { udpClient = new UdpClient(receivePort); }
        catch (Exception e)
        {
            Debug.Log("Failed to listen for UDP at port " + receivePort + ": " + e.Message);
            return;
        }
        Debug.Log("Created receiving client at ip  and port " + receivePort);
        this.senderIp = sendIp;
        this.senderPort = sendPort;
        Debug.Log("Set sender at ip " + sendIp + " and port " + sendPort);
        StartReceiveThread();
    }
    private void StartReceiveThread()
    {
        receiveThread = new Thread(() => ListenForMessages(udpClient));
        receiveThread.IsBackground = true;
        threadRunning = true;
        receiveThread.Start();
    }
    /**
     * \Function that runs in a thread and tries to keep looking for messages
     *
     */
    private void ListenForMessages(UdpClient client)
    {
        IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0); //IPAddress.Any
        while (threadRunning)
        {
            try
            {
                Byte[] receiveBytes = client.Receive(ref remoteIpEndPoint); // Blocks until a message returns on this socket from a remote host.
                //string returnData = Encoding.UTF8.GetString(receiveBytes);
                //Debug.Log("len of recvd bytes is "+receiveBytes.Length+" T size is "+ System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
                var template_data = FromByteConverter.fromBytes<T>(receiveBytes, System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
                //fromBytes(receiveBytes);
                connectionSuccessfull = true;
                lock (incomingQueueT)
                {
                    incomingQueueT.Enqueue(template_data);
                }
            }
            catch (SocketException e)
            {
                // 10004 thrown when socket is closed
                if (e.ErrorCode != 10004) Debug.Log("Socket exception while receiving data from udp client: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.Log("Error receiving data from udp client: " + e.Message);
            }
            Thread.Sleep(1);
        }
    }
    public T[] getMessagesT()
    {
        T[] pendingMessages = new T[0];
        lock (incomingQueueT)
        {
            pendingMessages = new T[incomingQueueT.Count];
            int i = 0;
            while (incomingQueueT.Count != 0)
            {
                //Debug.Log("Getting messages");
                pendingMessages[i] = incomingQueueT.Dequeue();
                i++;
            }
        }
        return pendingMessages;
    }
    /**
     * \function to send a message to the SenderPort and IP
     *
     */
    public void Send<T1>(T1 message)
    {
        //Debug.Log(String.Format("Send msg to ip:{0} port:{1} msg:{2}", senderIp, senderPort, message));
        IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(senderIp), senderPort);
        Byte[] sendBytes = FromByteConverter.toBytes<T1>(message);
        udpClient.Send(sendBytes, sendBytes.Length, serverEndpoint);
    }
    public void Stop()
    {
        threadRunning = false;
        receiveThread.Abort();
        if (udpClient != null)
            udpClient.Close();
        Debug.Log("Stop");
    }
}
/**
     * \converts the received bytes into T
     *
     */
public static class FromByteConverter
{
    //https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
    public static T fromBytes<T>(byte[] arr, int size)   //size is size of T
    {
        T str;//= new T();
        //int size = System.Runtime.InteropServices.Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);                     //arr.Len was size
        //Debug.Log("size is " + size+" arr size is "+arr.Length);
        Marshal.Copy(arr, 0, ptr, arr.Length);                           //arr.Len was size
        Type typeParameterType = typeof(T);
        str = (T)Marshal.PtrToStructure(ptr, typeParameterType);
        Marshal.FreeHGlobal(ptr);
        return str;
    }
    /**
     * \Converts struct present in memory to bytes
     *
     */
    public static byte[] toBytes<T1>(T1 str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
}
