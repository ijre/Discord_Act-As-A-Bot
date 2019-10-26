#include "sleepy_discord/websocketpp_websocket.h"
#include <fstream>

std::string idGet()
{
	std::fstream idFile;
	idFile.open("./id.txt");
	std::string id;
	idFile >> id;
	idFile.close();
	return id;
}

bool find(std::string string)
{
	std::size_t find = string.find("hydro");
	std::size_t find2 = string.find("hydrate");
	std::size_t find3 = string.find("water");

	return (find != std::string::npos || find2 != std::string::npos || find3 != std::string::npos);
}


using namespace SleepyDiscord;
class ClientClass : public DiscordClient
{
public:
	using DiscordClient::DiscordClient;
	void onMessage(Message message)
	{
		ClientClass client(idGet(), 2);
		if (message.author.bot)
			return;

		if (find(message.content))
			sendMessage(message.channelID, "Stay hydrated! uwu");

		if (message.content == "~!control")
		{
			std::string output;
			std::cout << "\nready, type quit to exit\n";
			client.sendTyping(message.channelID);

			while (output != "quit")
			{
				std::getline(std::cin, output);

				if (output == "quit")
					break;
				if (GetAsyncKeyState(VK_RETURN) && output != "quit")
				{
					sendMessage(message.channelID, output);
					client.sendTyping(message.channelID);
				}
			}
		}
	}
};

int main()
{
	ClientClass client(idGet(), 2);
	client.run();
}