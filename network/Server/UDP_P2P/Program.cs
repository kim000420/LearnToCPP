// /home/rsa-key-20251110/ChatServer/Program.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

// UDP P2P 중개 서버 (Rendezvous Server)
namespace ChatServer
{
    class Server
    {
        // [헤더("서버 설정")]
        // [툴팁("클라이언트들이 P2P 상대를 찾기 위해 접속할 중개 포트입니다.")]
        private static int port = 7777;

        // "짝"을 기다리는 클라이언트 목록
        // 중요: TCP와 달리 "연결"이 없으므로, 서버에 한 번이라도 패킷을 보낸 상대를 "클라이언트"로 간주합니다.
        private static readonly List<IPEndPoint> waitingClients = new List<IPEndPoint>();
        private static readonly object lockObject = new object();
        private static UdpClient udpServer;

        static async Task Main(string[] args)
        {
            Console.WriteLine($"[INFO] P2P 중개 서버(Rendezvous Server)가 포트 {port}에서 시작되었습니다...");
            udpServer = new UdpClient(port);

            while (true)
            {
                try
                {
                    // 1. 클라이언트로부터 UDP 패킷을 비동기로 수신
                    UdpReceiveResult result = await udpServer.ReceiveAsync();
                    IPEndPoint clientEndPoint = result.RemoteEndPoint;
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    // 2. "REGISTER" 메시지를 보낸 클라이언트만 처리
                    if (message.StartsWith("REGISTER"))
                    {
                        Console.WriteLine($"[INFO] {clientEndPoint}로부터 등록 요청 수신.");
                        await HandleRegistration(clientEndPoint);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] 수신 대기 중 오류: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 클라이언트 등록 및 매칭을 처리합니다.
        /// </summary>
        private static async Task HandleRegistration(IPEndPoint newClientEP)
        {
            IPEndPoint? peerEP = null;

            lock (lockObject)
            {
                // 이미 리스트에 있는지 확인 (중복 등록 방지)
                if (waitingClients.Contains(newClientEP)) return;

                // 1. 대기자가 있는지 확인
                if (waitingClients.Count > 0)
                {
                    // 2. 대기자가 있으면, 짝(Peer)을 찾음
                    peerEP = waitingClients[0]; // 가장 먼저 온 사람과 매칭
                    waitingClients.RemoveAt(0); // 매칭되었으니 대기 목록에서 제거
                }
                else
                {
                    // 3. 대기자가 없으면, 이 클라이언트를 대기 목록에 추가
                    waitingClients.Add(newClientEP);
                    Console.WriteLine($"[INFO] {newClientEP}가 대기열에 추가됨. (현재 대기: {waitingClients.Count})");
                }
            }

            // 4. 짝을 찾았는지 여부에 따라 처리
            if (peerEP != null)
            {
                // 5. 짝을 찾음! (서로의 주소를 교환해줌)
                Console.WriteLine($"[INFO] 매칭 성공: {newClientEP} <-> {peerEP}");

                // peerEP에게 newClientEP의 주소를 알려줌
                // (주의: IPEndPoint.ToString()은 "IP:Port" 문자열을 반환함)
                byte[] dataForPeer = Encoding.UTF8.GetBytes($"PEER:{newClientEP.ToString()}");
                await udpServer.SendAsync(dataForPeer, dataForPeer.Length, peerEP);

                // newClientEP에게 peerEP의 주소를 알려줌
                byte[] dataForNewClient = Encoding.UTF8.GetBytes($"PEER:{peerEP.ToString()}");
                await udpServer.SendAsync(dataForNewClient, dataForNewClient.Length, newClientEP);
            }
            // else: 짝을 못 찾았으면, 대기 목록에 추가만 하고 아무것도 보내지 않음 (클라이언트는 계속 대기)
        }
    }
}