#include "main.h"

int main()
{
	for (int i = 0; i < servers.size(); i++)
		std::cout << servers[i].name << " [" << servers[i].ID.string() << "]" << "\n";
}