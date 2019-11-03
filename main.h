#pragma once

#include "sleepy_discord/websocketpp_websocket.h"
#include <fstream>

int numIn;
static int64_t channel;

using namespace SleepyDiscord;
class ClientClass : public DiscordClient
{
public:
	using DiscordClient::DiscordClient;

	std::vector<Server> servers = getServers().vector();

	void onMessage(Message message)
	{
		if (message.channelID.number() == channel)
			std::cout << message.content << "\r\n";
	}

	void onHeartbeat()
	{
		std::string chat;
		std::cout << "say \"hatsune_change\" to change servers/channels\r\n";

		while (chat != "hatsune_change")
		{
			std::getline(std::cin, chat);

			//if (GetActiveWindow() && tagINPUT().ki.time)
			//	sendTyping(channel);

			if (GetAsyncKeyState(VK_RETURN) && chat != "hatsune_change")
				sendMessage(channel, chat);
		}
	}
};