﻿using System.Net;

namespace KoiCare.Application.Commons
{
    public class CommandResult<T>
    {
        private CommandResult(string? reason) => FailureReason = reason;
        private CommandResult(HttpStatusCode statusCode, string? reason)
        {
            StatusCode = statusCode;
            FailureReason = reason;
        }
        private CommandResult(T? payload) => Payload = payload;
        public T? Payload { get; }
        public string? FailureReason { get; }
        public object? FailureDetails => new { Reason = FailureReason };
        public HttpStatusCode? StatusCode { get; }
        public bool IsSuccess => FailureReason == null;
        public static CommandResult<T> Fail(string reason) => new(reason);
        public static CommandResult<T> Fail(HttpStatusCode statusCode, string reason) => new(statusCode, reason);
        public static CommandResult<T> Success(T payload) => new(payload);
        public static implicit operator bool(CommandResult<T> result) => result.IsSuccess;
    }
}