using KoiCare.Application.Abtractions.Localization;
using KoiCare.Application.Commons;
using MediatR;
using System.Net;

namespace KoiCare.Application.Features.Account
{
    public class CreateUser
    {
        public class Command : IRequest<CommandResult<Result>>
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        public class Result
        {
            public string? Message { get; set; }
        }

        public class Handler(IAppLocalizer localizer) : IRequestHandler<Command, CommandResult<Result>>
        {
            public async Task<CommandResult<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    return CommandResult<Result>.Success(new Result
                    {
                        Message = localizer["User created successfully"],
                    });
                }
                catch (Exception ex)
                {
                    return CommandResult<Result>.Fail(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
