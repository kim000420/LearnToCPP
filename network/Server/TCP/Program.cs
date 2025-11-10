// Assets/Scripts/Core/ServerCore.cs (C#에서는 파일 경로보다 네임스페이스가 중요합니다)
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

// TCP 채팅 서버 스크립트
namespace ChatServer
{
    class Server
    {
        // 접속된 클라이언트들을 관리하는 리스트입니다.
        // 스레드로부터 안전한 접근을 위해 lock 객체를 사용합니다.
        private static readonly List<TcpClient> clients = new List<TcpClient>();
        private static readonly object lockObject = new object();

        static async Task Main(string[] args)
        {
            // 1. 서버 리스너 설정
            int port = 7777;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"[INFO] 채팅 서버가 포트 {port}에서 시작되었습니다...");

            // 2. 클라이언트 접속을 비동기로 계속 대기
            while (true)
            {
                try
                {
                    // 비동기로 클라이언트 접속 대기
                    TcpClient client = await server.AcceptTcpClientAsync();

                    // 새 클라이언트가 접속하면 리스트에 추가
                    lock (lockObject)
                    {
                        clients.Add(client);
                    }
                    Console.WriteLine("[INFO] 새 클라이언트가 접속했습니다.");

                    // 3. 해당 클라이언트로부터 메시지를 받는 것을 별도 태스크로 처리
                    // HandleClientAsync가 끝나길 기다리지 않고 바로 다음 접속을 받으러 감 (while 루프 계속)
                    _ = HandleClientAsync(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] 클라이언트 접속 대기 중 오류: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 개별 클라이언트와의 통신을 비동기로 처리합니다. (메시지 수신)
        /// </summary>
        /// <param name="client">접속된 클라이언트 객체</param>
        private static async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024]; // 1KB 버퍼

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        // 클라이언트가 연결을 끊음 (bytesRead == 0)
                        throw new Exception("클라이언트 연결 종료");
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[MSG] 수신: {message}");

                    // 4. 수신된 메시지를 모든 클라이언트에게 전파 (브로드캐스트)
                    await BroadcastMessageAsync(message, client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[INFO] 클라이언트와 연결이 끊어졌습니다: {ex.Message}");
            }
            finally
            {
                // 5. 예외 발생 또는 연결 종료 시 클라이언트 정리
                lock (lockObject)
                {
                    clients.Remove(client);
                }
                client.Close();
                Console.WriteLine("[INFO] 클라이언트 정리 완료.");
            }
        }

        /// <summary>
        /// 모든 클라이언트에게 메시지를 전송합니다 (메시지를 보낸 클라이언트는 제외).
        /// </summary>
        /// <param name="message">전송할 메시지</param>
        /// <param name="sender">메시지를 보낸 클라이언트</param>
        private static async Task BroadcastMessageAsync(string message, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            List<TcpClient> disconnectedClients = new List<TcpClient>();
            TcpClient[] currentClients;

            lock (lockObject)
            {
                currentClients = clients.ToArray(); // 현재 시점의 클라이언트 목록 복사
            }

            foreach (var client in currentClients)
            {
                // 메시지를 보낸 사람은 제외하고 전송 (선택 사항)
                // if (client == sender) continue; 

                try
                {
                    NetworkStream stream = client.GetStream();
                    await stream.WriteAsync(data, 0, data.Length);
                }
                catch (Exception)
                {
                    // 전송 실패 (연결이 끊어진 클라이언트)
                    disconnectedClients.Add(client);
                }
            }
        }
    }
}