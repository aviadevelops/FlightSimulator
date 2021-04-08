#include "pch.h"
#include "stringWrapper.h"

stringWrapper::stringWrapper() {
	str = "rudder";
}

int stringWrapper::length() {
	return str.size();
}
char stringWrapper::getChar(int i) {
	return str[i];
}

void stringWrapper::setString(string s) {
	str = s;
}

string stringWrapper::getString() {
	return str;
}


extern "C" __declspec(dllexport) void* CreatestringWrapper() {
	return (void*) new stringWrapper();
}

extern "C" __declspec(dllexport) int Length(stringWrapper * s) {
	return s->length();
}

extern "C" __declspec(dllexport) char GetChar(stringWrapper * s, int i) {
	return s->getChar(i);
}

extern "C" __declspec(dllexport) void SetString(stringWrapper * s, void* str) {
	s->setString(*(static_cast<std::string*>(str)));
}
