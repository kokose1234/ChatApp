//  Copyright 2021 Jonguk Kim
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using ChatApp.Server.Database;
using ChatApp.Server.Net;

namespace ChatApp.Server;

internal static class Program
{
    internal static ChatServer ChatServer
    {
        get => default;
        set
        {
        }
    }

    private static void Main(string[] args)
    {
        Console.Title = "Chat Server";
        var server = new ChatServer();
        PacketHandlers.RegisterPackets();
        DatabaseManager.Setup();
        server.Start();

        while (true)
        {
            //TODO: 서버 명령어 추가하기
            switch (Console.ReadLine() ?? "") { }
        }
    }
}