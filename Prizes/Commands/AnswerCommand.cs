﻿using Auxiliary.Configuration;
using CSF;
using CSF.TShock;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Prizes.Modules
{
    [RequirePermission("tbc.user")]
    internal class AnswerCommand : TSModuleBase<TSCommandContext>
    {
        PrizesSettings config = Configuration<PrizesSettings>.Settings;

        [Command("answer")]
        [Description("The command users use to answer chat games")]
        public IResult Answer(string answer = "")
        {
            var cg = Prizes.cg;
            
            TSPlayer player = Context.Player;

            if (!cg.Occuring)
            {
                return Error("There is no chat game occuring!");
            }
            if(answer == "")
            {
                return Error("Please enter an answer!");
            }


            if (cg.Occuring == true)
            {
                if (answer == cg.answer.ToString())
                {
                    foreach (string cmd in config.CommandsOnChatGameWin)
                    {
                        string newCmd = cmd.Replace("%PLAYER%", '"' + player.Name + '"');
                        newCmd = newCmd.Replace("%ACCOUNT%", '"' + player.Account.Name + '"');

                        Commands.HandleCommand(TSPlayer.Server, newCmd);
                    }
                    cg.Occuring = false;
                    cg.answer = 0;
                    return Announce("[Chat Games] " + player.Name + " won the chat game (answer: " + cg.answer.ToString() + ") and has won 25 minutes of rank playtime! Hooray!", Color.Gold);
                  
                }
                if (answer == cg.wordAnswer.ToString())
                {
                    foreach (string cmd in config.CommandsOnChatGameWin)
                    {
                        string newCmd = cmd.Replace("%PLAYER%", '"' + player.Name + '"');
                        newCmd = newCmd.Replace("%ACCOUNT%", '"' + player.Account.Name + '"');

                        Commands.HandleCommand(TSPlayer.Server, newCmd);
                    }
                    cg.Occuring = false;
                    cg.wordAnswer = null;
                    cg.answer = 0;
                    return Announce("[Chat Games] " + player.Name + " won the chat game (answer: " + cg.wordAnswer.ToString() + ") and has won 25 minutes of rank playtime! Congratz!", Color.Gold);
             
                }

            }
            return Error("Something went wrong!");

        }
    }
}
