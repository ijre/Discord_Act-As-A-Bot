#include "main.h"

std::string idGet()
{
	std::fstream idFile;
	idFile.open("./id.txt");
	std::string id;
	idFile >> id;
	idFile.close();
	return id;
}

ClientClass client(idGet(), 2);

int askServers(std::vector<Server> servers)
{
	for (int i = 0; i < servers.size(); i++)
		if (i == 0)
			std::cout << "Choose your server: \r\n" << i + 1 << ". " << servers[i].name << "\r\n";
		else
			std::cout << i + 1 << ". " << servers[i].name << "\r\n";

	std::string cin;
	std::getline(std::cin, cin);

	numIn = 0;
	if (cin != "")
		numIn = std::stoi(cin);

	if (numIn <= 0)
		return -1;

	return numIn - 1;
}

int64_t askChannels(std::vector<Channel> channels)
{
	for (int i = 0; i < channels.size(); i++)
	{
		if (i == 0)
			std::cout << "Choose your channel: \r\n" << i + 1 << ". " << channels[i].name << "\r\n";
		else
			std::cout << i + 1 << ". " << channels[i].name << "\r\n";
	}

	std::string cin;
	std::getline(std::cin, cin);

	numIn = 0;
	if (cin != "")
		numIn = std::stoi(cin);

	if (numIn < 0)
		return -1;

	return channels[numIn - 1].ID.number();
}

int main()
{
	numIn = askServers(client.servers);
	while (numIn == -1)
		numIn = askServers(client.servers);

	//while (GetAsyncKeyState(VK_RETURN))
	//	continue;
	// old check from debugging days to prevent accidental rollover into the askChannels function

	channel = askChannels(client.getServerChannels(client.servers[numIn].ID));

	client.run();
}