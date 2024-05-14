// See https://aka.ms/new-console-template for more information
using Test;

new HttpServer().Start<WebHookReceive>();
Console.ReadLine();
