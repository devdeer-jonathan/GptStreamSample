# GptStreamSample

A simple proof of concept project demonstrating the streaming capabilities of GPT models, where the assistant responds in real-time as data is generated.

**This project is meant for local development only. The README is written accordingly.**

Prerequisites

- .NET 8
- An OpenAI API Key (For DEVDEER members this can be found in the 1Password password manager)
- The dotnet user-secrets CLI tool

## Features

- Console Streaming: Stream responses from GPT models directly to the console as they are generated.
- API Streaming: Stream GPT responses through an API.

## Setup

1.  Set Up OpenAI API Key for Local Development
    To securely store your OpenAI API key for local development, use the dotnet user-secrets tool:
    To initialize the use of dotnet user-secrets run the following commands in the root folder of the project:
    For the API:

        `dotnet user-secrets init --project ./src/Services/Services.CoreApi`

        `dotnet user-secrets set 'OpenAi:ApiKey' '[YourApiKey]' --project .\src\Services\Services.CoreApi`

2.  API Streaming

    1. Add the Services.CoreApi to the startup projects in your IDE (such as Visual Studio or Rider).
    2. Or, run the following command in the root folder of the project:

       `dotnet run --project .\src\Services\Services.CoreApi\Services.CoreApi.csproj`

    3. Follow the swagger documentation to test the endpoint.

3.  Running the Console Application
    To run the application and see the streaming in the console:

    1. Add the Ui.Console to the startup projects in your IDE (such as Visual Studio or Rider).
    2. Or, run the following command in the root folder of the project:

       `dotnet run --project .\src\Ui\Ui.Console\Ui.Console.csproj`

    3. With both projects running you can chat with a GPT-4 model. To properly display the streaming capabilities aim for long answer with prompts such as `Tell me a short story.`
       You can also ask follow-up questions within a session.
