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
		numIn = std::stoi(cin);

	if (numIn <= 0 || numIn > servers.size())
		return -1;

	std::ofstream guildID;
	guildID.open("./deps/guild.txt");
	guildID << servers[numIn - 1].ID.string();
	guildID.close();
	// fed up with the time this is taking, going to take the "easy" and lazy way out

	return numIn - 1;
}

int64_t askChannels(std::vector<Channel> channels)
{
	for (int i = 0; i < channels.size(); i++)
		if (i == 0)
			std::cout << "Choose your channel: \n" << i + 1 << ". " << channels[i].name << "\n";
		else
			std::cout << i + 1 << ". " << channels[i].name << "\n";

	std::getline(std::cin, cin);

	if (cin != "")
		numIn = (std::stoi(cin) - 1);

	if (numIn < 0 || channels[numIn].type != 0)
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
	numIn = askServers(servers);
	while (numIn == -1)
		numIn = askServers(servers);

	while (GetAsyncKeyState(VK_RETURN))
		continue;
	// to prevent accidental rollover into the askChannels function

	channel = askChannels(client.getServerChannels(servers[numIn].ID));
	while (channel == -1)
		channel = askChannels(client.getServerChannels(servers[numIn].ID));

	std::ofstream channelID;
	channelID.open("./deps/channel.txt");
	channelID << std::to_string(channel);
	channelID.close();

	// fed up with the time this is taking, going to take the "easy" and lazy way out
}