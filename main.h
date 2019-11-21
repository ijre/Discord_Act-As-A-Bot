#pragma once

#include "sleepy_discord/websocketpp_websocket.h"
#include <fstream>
#include <filesystem>

std::string idGet()
{
	std::fstream idFile;
	idFile.open("./id.txt");
	std::string id;
	idFile >> id;
	idFile.close();
	return id;
}

using namespace SleepyDiscord;
class ClientClass : public DiscordClient
{
public:
	using DiscordClient::DiscordClient;
};

ClientClass client(idGet(), 2);

int numIn;
static int64_t channel;
std::vector<Server> servers = client.getServers().vector();
std::string cin;