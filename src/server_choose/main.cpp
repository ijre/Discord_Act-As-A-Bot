#include "main.h"

int askServers(std::vector<Server> servers)
{
	for (int i = 0; i < servers.size(); i++)
		if (i == 0)
			std::cout << "Choose your server: \n" << i + 1 << ". " << servers[i].name << "\n";
		else
			std::cout << i + 1 << ". " << servers[i].name << "\n";

	std::getline(std::cin, cin);

	if (cin != "")
	{
		try
		{
			numIn = (std::stoi(cin));
		}
		catch (std::invalid_argument)
		{
			return -1;
		}
	}
	else return -1;

	if (numIn <= 0 || numIn > servers.size())
		return -1;

	std::ofstream guildID;
	guildID.open("./deps/guild.txt");
	guildID << servers[numIn - 1].ID.string();
	guildID.close();

	return numIn - 1;
}

int64_t askChannels(std::vector<Channel> channels)
{
	for (int i = 0; i < channels.size(); i++)
	{
		std::string doNot = (channels[i].type == 2 || channels[i].type == 4 ? " (INVALID CHANNEL)" : "");

		if (i == 0)
			std::cout << "Choose your channel: \n" << i + 1 << ". " << channels[i].name << doNot << "\n";
		else
			std::cout << i + 1 << ". " << channels[i].name << doNot << "\n";
	}

	std::getline(std::cin, cin);

	if (cin != "")
	{
		try
		{
			numIn = (std::stoi(cin) - 1);
		}
		catch (std::invalid_argument)
		{
			return -1;
		}
	}
	else return -1;

	if (numIn < 0 || numIn >= channels.size())
		return -1;

	if (channels[numIn].type == 2 || channels[numIn].type == 4)
		return -1;
	/*
		enum ChannelType {
		SERVER_TEXT     = 0,
		DM              = 1,
		SERVER_VOICE    = 2,
		GROUP_DM        = 3,
		SERVER_CATEGORY = 4
	*/

	return channels[numIn].ID.number();
}

int main()
{
	int oldNum = askServers(servers);
	std::cout << "\n";
	while (oldNum == -1)
	{
		std::cout << "\n";
		oldNum = askServers(servers);
	}

	while (GetAsyncKeyState(VK_RETURN))
		continue;
	// to prevent accidental rollover into the askChannels function

	channel = askChannels(client.getServerChannels(servers[oldNum].ID));
	std::cout << "\n";
	while (channel == -1)
	{
		std::cout << "\n";
		channel = askChannels(client.getServerChannels(servers[oldNum].ID));
	}

	std::ofstream channelID;
	channelID.open("./deps/channel.txt");
	channelID << std::to_string(channel);
	channelID.close();
}