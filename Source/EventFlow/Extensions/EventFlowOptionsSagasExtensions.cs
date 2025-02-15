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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventFlow.Sagas;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Extensions
{
    public static class EventFlowOptionsSagasExtensions
    {
        public static IEventFlowOptions AddSagas(
            this IEventFlowOptions eventFlowOptions,
            Assembly fromAssembly,
            Predicate<Type> predicate = null)
        {
            predicate = predicate ?? (t => true);
            var sagaTypes = fromAssembly
                .GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && t.IsAssignableTo<ISaga>())
                .Where(t => predicate(t));

            return eventFlowOptions.AddSagas(sagaTypes);
        }

        public static IEventFlowOptions AddSagas(
            this IEventFlowOptions eventFlowOptions,
            params Type[] sagaTypes)
        {
            return eventFlowOptions.AddSagas(sagaTypes);
        }

        public static IEventFlowOptions AddSagaLocators(
            this IEventFlowOptions eventFlowOptions,
            Assembly fromAssembly,
            Predicate<Type> predicate = null)
        {
            predicate = predicate ?? (t => true);
            var sagaTypes = fromAssembly
                .GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && t.IsAssignableTo<ISagaLocator>())
                .Where(t => !t.HasConstructorParameterOfType(x => x.IsAssignableTo<ISagaLocator>()))
                .Where(t => predicate(t));

            return eventFlowOptions.AddSagaLocators(sagaTypes);
        }

        public static IEventFlowOptions AddSagaLocators(
            this IEventFlowOptions eventFlowOptions,
            params Type[] sagaLocatorTypes)
        {
            return eventFlowOptions.AddSagaLocators((IEnumerable<Type>)sagaLocatorTypes);
        }

        public static IEventFlowOptions AddSagaLocators(
            this IEventFlowOptions eventFlowOptions,
            IEnumerable<Type> sagaLocatorTypes)
        {
            foreach (var sagaLocatorType in sagaLocatorTypes)
            {
                if (!typeof(ISagaLocator).GetTypeInfo().IsAssignableFrom(sagaLocatorType))
                {
                    throw new ArgumentException($"Type '{sagaLocatorType.PrettyPrint()}' is not a '{typeof(ISagaLocator).PrettyPrint()}'");
                }
                eventFlowOptions.ServiceCollection.AddTransient(sagaLocatorType);
            }

            return eventFlowOptions;
        }
    }
}