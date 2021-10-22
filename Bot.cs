using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramGameBot
{
    class Bot
    {
        private readonly TelegramBotClient _bot;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        public void StartBot()
        {
            _bot.OnMessage += OnMessageReceived;
            _bot.StartReceiving();
            while (true)
            {
                Console.WriteLine("Bot is worked all right");
                Thread.Sleep(int.MaxValue);
            }
        }

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                Message init = messageEventArgs.Message;
                var markup1 = new ReplyKeyboardMarkup(new[]
{
                   new KeyboardButton("Game"),
                   new KeyboardButton("Help"),
                   new KeyboardButton("Start"),
                })
                {
                    OneTimeKeyboard = true
                };

                await _bot.SendTextMessageAsync(init.Chat.Id, $"Ваш выбор:{init.Text}", replyMarkup: markup1);

                switch (init.Text)
                {
                    case "Game":
                        var markup2 = new ReplyKeyboardMarkup(new[]
                        {
                           new KeyboardButton("rock"),
                           new KeyboardButton("paper"),
                           new KeyboardButton("scissors"),
                        })
                        {
                            OneTimeKeyboard = true
                        };
                        string str = "Выберите фигуру";
                        await _bot.SendTextMessageAsync(init.Chat.Id, str, replyMarkup: markup2);
                        break;
                    case "Start":
                        var markup3 = new ReplyKeyboardMarkup(new[]
                        {
                           new KeyboardButton("Game"),
                           new KeyboardButton("Help"),
                           new KeyboardButton("Start"),
                        })
                        {
                            OneTimeKeyboard = true
                        };

                        string rules = "ВЫ запустили игру КАМЕНЬ-НОЖНИЦЫ-БУМАГА. С правилами игры можете ознакомиться на сайте...";

                        await _bot.SendTextMessageAsync(init.Chat.Id, rules, replyMarkup: markup3);
                        break;
                    case "Help":
                        var markup4 = new ReplyKeyboardMarkup(new[]
                        {
                           new KeyboardButton("Game"),
                           new KeyboardButton("Help"),
                           new KeyboardButton("Start"),
                        })
                        {
                            OneTimeKeyboard = true
                        };
                        string help = "Подробные правила игры и FAQ находятся на сайте...";
                        await _bot.SendTextMessageAsync(init.Chat.Id, help, replyMarkup: markup4);
                        break;
                    case "rock":
                        Process(sender, messageEventArgs, init);
                        break;
                    case "paper":
                        Process(sender, messageEventArgs, init);
                        break;
                    case "scissors":
                        Process(sender, messageEventArgs, init);
                        break;
                    default:
                        await _bot.SendTextMessageAsync(init.Chat.Id, "Начните игру!");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void Process(object sender, MessageEventArgs messageEventArgs, Message init)
        {
            var markup = new ReplyKeyboardMarkup(new[]
                        {
                           new KeyboardButton("rock"),
                           new KeyboardButton("scissors"),
                           new KeyboardButton("paper"),
                        })
            {
                OneTimeKeyboard = true
            };
            Random random = new Random();
            string[] options = { "rock", "scissors", "paper" };
            string first = options[random.Next(0, 3)];
            await _bot.SendTextMessageAsync(init.Chat.Id, first, replyMarkup: markup);

            await _bot.SendTextMessageAsync(init.Chat.Id, RockPaperScissors(first, init.Text));
        }


        public static string RockPaperScissors(string first, string second)
        => (first, second) switch
        {
            ("rock", "paper") => "rock is covered by paper. Paper wins.",
            ("rock", "scissors") => "rock breaks scissors. Rock wins.",
            ("paper", "rock") => "paper covers rock. Paper wins.",
            ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
            ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
            ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
            ("rock", "rock") => "tie",
            ("paper", "paper") => "tie",
            ("scissors", "scissors") => "tie"
        };
    }
}
