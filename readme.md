# TDDSample

This repository contains a sample project demonstrating `Test-Driven Development (TDD)` using .Net 7 and Vertical Slice architecture based on Minimal APIs in .NET Core. The project aims to showcase best practices for building maintainable and testable software.

Beside of creating some unit tests for achieving test driven development (TDD), we also covered integration and end-to-end testing here.

Feel free to explore the project, learn from the provided examples, and adapt the testing techniques to your own projects. If you have any questions or need further assistance, please don't hesitate to reach out. Happy testing!

## Test driven development (TDD)

Test driven development is a development process in which you write your unit tests before you write your implementation.

The process looks like this:

- Write a failing unit test. The test should be written in a way that describes what your code should be doing (Red Phase).
- Write just enough code to make the test pass. At this point it’s best not to worry about the quality of the code, just get a passing test (Green Phase).
- Refactor your code to improve readability and performance (Refactor Phase).

This is sometimes referred to as the `Red/Green/Refactor` cycle

You then repeat this development cycle until the software is complete. One of the main reasons TDD is popular is because it removes the fear of change from projects, this is because the tests are always true and no matter what happens to your code, as long is the business logic is still sound.

## Getting Started & Prerequisites

1. This application uses `Https` for hosting apis, to setup a valid certificate on your machine, you can create a [Self-Signed Certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-7.0#macos-or-linux), see more about enforce certificate [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).
2. Install git - [https://git-scm.com/downloads](https://git-scm.com/downloads).
3. Install .NET Core 7.0 - [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/7.0).
4. Install Visual Studio, Rider or VSCode.
5. Open [tdd-sample.sln](./tdd-sample.sln) solution, make sure that's compiling.
6. Navigate to `src/TDDSample.Api` and run `dotnet run` to launch the api (ASP.NET Core Web API)
7. Open web browser https://localhost:5001/swagger Swagger UI

## Contribution

The application is in development status. You are feel free to submit pull request or create the issue.

## License

The project is under [MIT license](https://github.com/mehdihadeli/tdd0sample/blob/master/LICENSE).
