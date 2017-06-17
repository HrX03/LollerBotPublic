using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loller06_Bot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;

        Random rand;

        string[] freshestMemes;

        string[] randomTexts;

        public MyBot()
        {
            rand = new Random();

            freshestMemes = new string[]
            {
                "mem/mem1.jpg", // 0
                "mem/mem2.jpg", // 1
                "mem/mem3.jpg", // 2
                "mem/mem4.jpg", // 3
                "mem/mem5.jpg", // 4
                "mem/mem6.jpg"  // 5
                //put your meme images on the folder mem/ 
            };

            randomTexts = new string[]
            {
                //put here your own text to be said by the bot. Syntax: "text",    then click enter
            };

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = 'put your prefix here';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();

            commands.CreateCommand("info")
               .Do(async (e) =>
               {
                   await e.Channel.SendMessage(""); //put in the "" the text that the bot will say executing this command
               });

            commands.CreateCommand("help")
               .Do(async (e) =>
               {
                   await e.Channel.SendMessage(""); //put in the "" the text that the bot will say executing this command
               });

            commands.CreateCommand("invite")
               .Do(async (e) =>
               {
                   await e.Channel.SendMessage(""); //put in the "" the text that the bot will say executing this command
               });

            //for creating basilar commands go here (https://pastebin.com/03EdizNh) and paste and change the words on "" with your own text

            RegisterMemeCommand();
            RegisterPurgeCommand();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("put here your bot token", TokenType.Bot);
            });
        }

        private void RegisterMemeCommand()
        {
            commands.CreateCommand("meme")
               .Do(async (e) =>
               {
                   int randomMemeIndex = rand.Next(freshestMemes.Length);
                   string memeToPost = freshestMemes[randomMemeIndex];
                   await e.Channel.SendFile(memeToPost);
               });

            commands.CreateCommand("say")
               .Do(async (e) =>
               {
                   int randomTextIndex = rand.Next(randomTexts.Length);
                   string textToPost = randomTexts[randomTextIndex];
                   await e.Channel.SendMessage(textToPost);
               });
        }

        private void RegisterPurgeCommand()
        {
            commands.CreateCommand("purge")
                .Parameter("purgeAmount", ParameterType.Required)
                .Do(async (e) =>
                {
                    int amnt = Convert.ToInt32(e.GetArg("purgeAmount"));
                    Message[] messagesToDelete;
                    if (amnt < 200)
                    {
                        messagesToDelete = await e.Channel.DownloadMessages(amnt);
                        await e.Channel.DeleteMessages(messagesToDelete);
                        await e.Channel.SendMessage(e.GetArg("purgeAmount") + " messages deleted.");
                    }
                    else
                    {
                        await e.Channel.SendMessage("The maximum amount is 199.");
                    }
                });
        }

        private void Log(object sender, LogMessageEventArgs e) 
        {
            Console.WriteLine(e.Message);
        }
    }
}
