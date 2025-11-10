// C:/Users/YourName/ChatClient/Program.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// UDP P2P 클라이언트 (RUDP 순서 보장 포함)
namespace ChatClient
{
    class Client
    {
        // [헤더("서버(중개) 설정")]
        // [툴팁("Google Cloud VM의 공용 IP 주소")]
        private static string serverIP = "34.22.102.159"; // (중요!) 님의 VM IP로 설정
        private static int serverPort = 7777;

        private static UdpClient client;
        private static IPEndPoint serverEndPoint;

        // [헤더("P2P 상대방 설정")]
        // [툴팁("서버가 알려줄 P2P 상대방의 주소입니다.")]
        private static IPEndPoint? peerEndPoint = null; // P2P 상대방 (초기엔 null)

        // [헤더("RUDP (신뢰성) 설정")]
        private static int outgoingSequence = 0; // 내가 보낼 메시지의 순서 번호
        private static int expectedSequence = 0; // 내가 받아야 할 상대방 메시지의 순서 번호
        private static SortedDictionary<int, string> messageBuffer = new SortedDictionary<int, string>(); // 순서가 뒤바뀐 메시지를 임시 저장

        static async Task Main(string[] args)
        {
            client = new UdpClient(); // OS가 임의의 포트 할당
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            Console.WriteLine("========================================");
            Console.WriteLine(" C# UDP P2P 채팅 클라이언트 (RUDP)");
            Console.WriteLine("========================================");
            Console.WriteLine($"[INFO] 중개 서버 {serverIP}:{serverPort}에 등록을 시도합니다...");

            // 1. 서버로부터 메시지(명령)를 수신하는 태스크를 별도로 시작
            Task receiveTask = ReceiveMessagesAsync();

            // 2. (필수!) 서버에 "REGISTER" 메시지를 보내 자신을 등록
            byte[] registerMsg = Encoding.UTF8.GetBytes("REGISTER");
            await client.SendAsync(registerMsg, registerMsg.Length, serverEndPoint);

            Console.WriteLine("[INFO] 서버에 등록 요청을 보냈습니다. 짝을 기다립니다...");

            // 3. 사용자가 메시지를 입력하고 "P2P 상대방"에게 전송하는 루프
            while (true)
            {
                string? message = Console.ReadLine();
                if (string.IsNullOrEmpty(message)) continue;

                if (message.ToLower() == "/exit") break;

                // 4. 짝이 찾아졌는지 확인
                if (peerEndPoint != null)
                {
                    // 5. (RUDP) 순서 번호와 메시지를 조합하여 패킷 생성
                    string packetMessage = $"CHAT:{outgoingSequence}:{message}";
                    byte[] data = Encoding.UTF8.GetBytes(packetMessage);

                    // [중요] 메시지를 서버가 아닌 P2P 상대방(peer)에게 직접 전송!
                    await client.SendAsync(data, data.Length, peerEndPoint);

                    outgoingSequence++; // 보낸 순서 번호 증가
                }
                else
                {
                    Console.WriteLine("[WAIT] 아직 P2P 상대방(짝)이 매칭되지 않았습니다...");
                }
            }

            client.Close();
            Console.WriteLine("[INFO] 클라이언트가 종료되었습니다.");
        }

        /// <summary>
        /// 서버(명령)와 P2P 상대방(채팅) 양쪽으로부터 오는 모든 패킷을 수신합니다.
        /// </summary>
        private static async Task ReceiveMessagesAsync()
        {
            try
            {
                while (true)
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);
                    IPEndPoint remoteEP = result.RemoteEndPoint;

                    // A. 패킷이 "서버"로부터 왔는가? (명령 처리)
                    if (remoteEP.Equals(serverEndPoint))
                    {
                        if (message.StartsWith("PEER:"))
                        {
                            Console.WriteLine($"[SERVER] 짝을 찾았습니다! {message}");
                            string peerAddress = message.Substring(5); // "PEER:" 5글자 자르기

                            // "IP:Port" 문자열을 IPEndPoint 객체로 변환
                            peerEndPoint = IPEndPoint.Parse(peerAddress);

                            // 3. (홀 펀칭) 짝을 찾았으니, 방화벽 구멍을 뚫기 위한 첫 패킷 전송
                            Console.WriteLine($"[INFO] {peerEndPoint} 주소로 홀 펀칭을 시도합니다...");
                            byte[] punchMsg = Encoding.UTF8.GetBytes("PUNCH");
                            await client.SendAsync(punchMsg, punchMsg.Length, peerEndPoint);
                        }
                    }
                    // B. 패킷이 "P2P 상대방"으로부터 왔는가? (채팅/RUDP 처리)
                    else if (remoteEP.Equals(peerEndPoint))
                    {
                        // P2P 상대방으로부터 온 메시지 처리
                        HandlePeerMessage(message);
                    }
                    // C. P2P 상대방이 "설정되기 전"에 PUNCH 패킷이 먼저 도착했는가?
                    else if (peerEndPoint == null && message.StartsWith("PUNCH"))
                    {
                        // 이것은 상대방이 보낸 홀 펀칭 시도입니다.
                        Console.WriteLine($"[INFO] {remoteEP}로부터 홀 펀칭 패킷 수신!");
                        peerEndPoint = remoteEP; // 이 주소가 내 상대방임을 확신
                    }
                }
            }
            catch (ObjectDisposedException) { /* 클라이언트 종료 */ }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 메시지 수신 중 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// P2P 상대방이 보낸 메시지를 RUDP(순서 보장) 로직으로 처리합니다.
        /// </summary>
        private static void HandlePeerMessage(string message)
        {
            // "PUNCH" 메시지는 단순 홀 펀칭용이므로 무시
            if (message.StartsWith("PUNCH")) return;

            // "CHAT:시퀀스:메시지" 형식 파싱
            if (message.StartsWith("CHAT:"))
            {
                try
                {
                    string[] parts = message.Split(':', 3); // CHAT, 시퀀스, 메시지 (최대 3개로 나눔)
                    int sequence = int.Parse(parts[1]);
                    string chatMessage = parts[2];

                    // [RUDP 핵심 로직]
                    // 1. 내가 기대하던(expected) 순서의 메시지가 도착했는가?
                    if (sequence == expectedSequence)
                    {
                        Console.WriteLine($">> {chatMessage}");
                        expectedSequence++; // 다음 기대 순번 증가

                        // 2. 버퍼에 저장해 둔, (이전에 미리 도착했던) 다음 순서의 메시지들이 있는지 확인
                        // (예: #10이 도착해서, 버퍼에 있던 #11, #12, #13을 연달아 처리)
                        while (messageBuffer.ContainsKey(expectedSequence))
                        {
                            Console.WriteLine($">> (Buffered) {messageBuffer[expectedSequence]}");
                            messageBuffer.Remove(expectedSequence);
                            expectedSequence++;
                        }
                    }
                    // 2. 기대했던 것보다 더 나중의 메시지가 "먼저" 도착했는가? (순서 뒤바뀜)
                    else if (sequence > expectedSequence)
                    {
                        Console.WriteLine($"[RUDP] {sequence}번 메시지가 {expectedSequence}번보다 먼저 도착. 버퍼에 저장.");
                        // 버퍼에 임시 저장 (중복 도착 대비 TryAdd 사용)
                        if (!messageBuffer.ContainsKey(sequence))
                        {
                            messageBuffer.Add(sequence, chatMessage);
                        }
                    }
                    // 3. 이미 처리한(기대 순번보다 낮은) 메시지가 또 도착했는가?
                    else // (sequence < expectedSequence)
                    {
                        Console.WriteLine($"[RUDP] 이미 처리한 {sequence}번 메시지 수신 (무시됨).");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARN] P2P 메시지 파싱 실패: {message} ({ex.Message})");
                }
            }
        }
    }
}