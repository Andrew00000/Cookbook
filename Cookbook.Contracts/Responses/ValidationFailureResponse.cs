﻿namespace Cookbook.Contracts.Responses
{
    public class ValidationFailureResponse
    {
        public required IEnumerable<ValidationResponse> Errors { get; init; }
    }

    public class ValidationResponse
    {
        public required string Message { get; init; }
    }
}
