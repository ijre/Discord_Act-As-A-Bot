#pragma once

#include "sleepy_discord/websocketpp_websocket.h"
#include <fstream>
#include <filesystem>

std::string idGet()
{
	std::fstream idFile;
	if (std::filesystem::exists("./deps/id.txt"))
		idFile.open("./deps/id.txt");
	else
		idFile.open("../deps/id.txt");
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