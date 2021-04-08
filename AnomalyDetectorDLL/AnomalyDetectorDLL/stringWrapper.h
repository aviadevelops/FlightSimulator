#include <string>
using namespace std;

class stringWrapper {
	string str;
public:
	stringWrapper();

	int length();

	char getChar(int i);

	string getString();

	void setString(string s);
};

