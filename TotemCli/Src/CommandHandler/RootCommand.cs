using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using TotemCli.Enums;
using TotemCli.PromptHandler;
using System.ComponentModel;

namespace TotemCli.CommandHandler
{
    public class TotemRootCommand : AsyncCommand
    {
        private readonly InteractiveOptionHandler _interactiveOptionHandler;
        private readonly Prompts _prompts;


        public TotemRootCommand
        (
            InteractiveOptionHandler interactiveOptionHandler, 
            Prompts prompts
        )
        {
            _interactiveOptionHandler = interactiveOptionHandler;
            _prompts = prompts;
        }
        public async override Task<int> ExecuteAsync(CommandContext context)
        {
            AnsiConsole.Write(new FigletText("Totem cli")
                .LeftJustified()
                .Color(Color.Green)
            );
            //AnsiConsole.Markup("[green]To go to the Main Menu press [underline]CTRL+M[/][/]");
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[green]To exit the app press [underline green]CTRL+C[/][/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();


            //var answerMe = AnsiConsole.Confirm("[gold1]Do you want to enchanced the experience with some music?[/]");

            await _prompts.MainMenu();
            return 0;
        }
    }
}
