// The MIT License (MIT)
// 
// Copyright (c) 2015-2024 Rasmus Mikkelsen
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.TestHelpers.Aggregates.ValueObjects;
using Newtonsoft.Json;

namespace EventFlow.TestHelpers.Aggregates.Commands
{
    [CommandVersion("ThingyMaybePing", 1)]
    public class ThingyMaybePingCommand : Command<ThingyAggregate, ThingyId, IExecutionResult>
    {
        public PingId PingId { get; }
        public bool IsSuccess { get; }

        [JsonConstructor]
        public ThingyMaybePingCommand(ThingyId aggregateId, PingId pingId, bool isSuccess)
            : base(aggregateId, CommandId.New)
        {
            PingId = pingId;
            IsSuccess = isSuccess;
        }
    }

    public class ThingyMaybePingCommandHandler :
        CommandHandler<ThingyAggregate, ThingyId, IExecutionResult, ThingyMaybePingCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            ThingyAggregate aggregate,
            ThingyMaybePingCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.PingMaybe(command.PingId, command.IsSuccess);
            return Task.FromResult(executionResult);
        }
    }
}