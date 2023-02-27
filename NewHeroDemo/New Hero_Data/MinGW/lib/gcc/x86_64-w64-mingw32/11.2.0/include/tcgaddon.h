// COPYRIGHT TCG - new Hero 
// https://newhero-project.web.app

#include<vector>
#include<iostream>

void readArray(int argc, char *argv[], int output[]){
    vector<int> numbers;

    for(int i = 1; i < argc; i++){
        int num = std::stoi(argv[i]);
        numbers.push_back(num);
    }
    
    for(int i = 0; i < numbers.size(); i++)
        output[i] = numbers[i];
}
