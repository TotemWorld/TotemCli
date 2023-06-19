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

        public TotemRootCommand(InteractiveOptionHandler interactiveOptionHandler)
        {
            _interactiveOptionHandler = interactiveOptionHandler;

        }
        public async override Task<int> ExecuteAsync(CommandContext context)
        {
            AnsiConsole.Write(new FigletText("Totem cli")
                .LeftJustified()
                .Color(Color.Green)
            );

            //var answerMe = AnsiConsole.Confirm("[gold1]Do you want to enchanced the experience with some music?[/]");

            var selectionPromptStyle = new Style().Foreground(Color.Green);

            var option = AnsiConsole.Prompt<string>
            (
                new SelectionPrompt<string>()
                    .Title("[gold1]Select the[bold] application[/] type:[/]")
                    .PageSize(10)
                    .HighlightStyle(selectionPromptStyle)
                    .MoreChoicesText("Move up and down to reveal more actions")
                    .AddChoices(
                        Enum.GetNames(typeof(ProcessType))
                    )
            );
            if(option == "Interactive")
            {
                try
                {
                    await _interactiveOptionHandler.ShowInterativeOptions();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }

            if(option == "JsonBatch")
            {
                AnsiConsole.Markup("[bold reverse rosybrown]This is not supported yet![/]");
            }
            return 0;
        }
    }
}
