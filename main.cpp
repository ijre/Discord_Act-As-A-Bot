#include "main.h"

int askServers(std::vector<Server> servers)
{
	for (int i = 0; i < servers.size(); i++)
		if (i == 0)
			std::cout << "Choose your server: \r\n" << i + 1 << ". " << servers[i].name << "\r\n";
		else
			std::cout << i + 1 << ". " << servers[i].name << "\r\n";

	std::getline(std::cin, cin);

	if (cin != "")
		numIn = std::stoi(cin);

	if (numIn <= 0 || numIn > servers.size())
		return -1;

	std::ofstream guildID;
	guildID.open("./guild.txt");
	guildID << servers[numIn - 1].ID.string();
	guildID.close();
	// fed up with the time this is taking, going to take the "easy" and lazy way out

	return numIn - 1;
}

int64_t askChannels(std::vector<Channel> channels)
{
	for (int i = 0; i < channels.size(); i++)
	{
		if (i == 0)
			std::cout << "\r\nChoose your channel: \r\n" << i + 1 << ". " << channels[i].name << "\r\n";
		else
			std::cout << i + 1 << ". " << channels[i].name << "\r\n";
	}

	std::getline(std::cin, cin);

	if (cin != "")
		numIn = std::stoi(cin);

	if (numIn <= 0 || (channels[numIn].type > 1 && channels[numIn].type != 3) || numIn > channels.size())
		return -1;

	return channels[numIn - 1].ID.number();
}

int main()
{
	auto path = std::filesystem::path("./channel.txt");
	if (std::filesystem::exists(path))
		std::filesystem::remove(path);

#define path std::filesystem::path("./guild.txt")
	// im writing this before i test, and if this #define is legal im going to DIE OF LAUGHTER
		// because writing path2 just doesnt work for my puny brain so it autopilots into saying path
			// plus im just curious to see if it works

	// IT WORKS ADIOWHDAIUHWDIUAHBDJBFNA

	if (std::filesystem::exists(path))
		std::filesystem::remove(path);

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
	channelID.open("./channel.txt");
	channelID << std::to_string(channel);
	channelID.close();

	// fed up with the time this is taking, going to take the "easy" and lazy way out
}