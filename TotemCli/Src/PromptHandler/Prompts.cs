using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotemCli.Enums;

namespace TotemCli.PromptHandler
{
    public class Prompts
    {
        private readonly InteractiveOptionHandler _interactiveOptionHandler;

        public Prompts(InteractiveOptionHandler interactiveOptionHandler)
        {
            _interactiveOptionHandler = interactiveOptionHandler;
        }

        public async Task MainMenu()
        {
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
            if (option == "Interactive")
            {
                try
                {
                    await _interactiveOptionHandler.ShowInterativeOptions();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            if (option == "JsonBatch")
            {
                AnsiConsole.Markup("[bold reverse rosybrown]This is not supported yet![/]");
                AnsiConsole.WriteLine();
                await MainMenu();
            }
        }

    }
}
